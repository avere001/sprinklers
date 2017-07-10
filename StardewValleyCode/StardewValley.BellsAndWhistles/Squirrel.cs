using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Squirrel : Critter
	{
		private int nextNibbleTimer = 1000;

		private int treeRunTimer;

		private int characterCheckTimer = 200;

		private bool running;

		private Tree climbed;

		private Vector2 treeTile;

		public Squirrel(Vector2 position, bool flip)
		{
			this.position = position * (float)Game1.tileSize;
			this.flip = flip;
			this.baseFrame = 60;
			this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 32, 32);
			this.sprite.loop = false;
			this.startingPosition = position;
		}

		private void doneNibbling(Farmer who)
		{
			this.nextNibbleTimer = Game1.random.Next(2000);
		}

		public override void draw(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(-64 + ((this.treeRunTimer > 0) ? (this.flip ? (Game1.tileSize * 3 + Game1.tileSize / 2) : (-Game1.tileSize / 4)) : 0)), (float)(-32 * Game1.pixelZoom / 2) + this.yJumpOffset + this.yOffset + (float)((this.treeRunTimer > 0) ? (this.flip ? 0 : (Game1.tileSize * 2)) : 0))), (this.position.Y + (float)Game1.tileSize + (float)((this.treeRunTimer > 0) ? (Game1.tileSize * 2) : 0)) / 10000f + this.position.X / 1000000f, 0, 0, Color.White, this.flip, 4f, (this.treeRunTimer > 0) ? ((float)((double)(this.flip ? 1 : -1) * 3.1415926535897931 / 2.0)) : 0f, false);
			if (this.treeRunTimer <= 0)
			{
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, (float)(Game1.tileSize - 4))), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 16f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
			}
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			this.nextNibbleTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.sprite.currentAnimation == null && this.nextNibbleTimer <= 0)
			{
				int num = Game1.random.Next(2, 8);
				List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
				for (int i = 0; i < num; i++)
				{
					list.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200));
					list.Add(new FarmerSprite.AnimationFrame(this.baseFrame + 1, 200));
				}
				list.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneNibbling), false));
				this.sprite.setCurrentAnimation(list);
			}
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer <= 0 && !this.running)
			{
				if (Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 12, environment) != null)
				{
					this.running = true;
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame(this.baseFrame + 2, 50),
						new FarmerSprite.AnimationFrame(this.baseFrame + 3, 50),
						new FarmerSprite.AnimationFrame(this.baseFrame + 4, 50),
						new FarmerSprite.AnimationFrame(this.baseFrame + 5, 120),
						new FarmerSprite.AnimationFrame(this.baseFrame + 6, 80),
						new FarmerSprite.AnimationFrame(this.baseFrame + 7, 50)
					});
					this.sprite.loop = true;
				}
				this.characterCheckTimer = 200;
			}
			if (this.running)
			{
				if (this.treeRunTimer > 0)
				{
					this.position.Y = this.position.Y - 4f;
				}
				else
				{
					this.position.X = this.position.X + (float)(this.flip ? -4 : 4);
				}
			}
			if (this.running && this.characterCheckTimer <= 0 && this.treeRunTimer <= 0)
			{
				this.characterCheckTimer = 100;
				Vector2 vector = new Vector2((float)((int)(this.position.X / (float)Game1.tileSize)), (float)((int)this.position.Y / Game1.tileSize));
				if (environment.terrainFeatures.ContainsKey(vector) && environment.terrainFeatures[vector] is Tree)
				{
					this.treeRunTimer = 700;
					this.climbed = (environment.terrainFeatures[vector] as Tree);
					this.treeTile = vector;
					this.position = vector * (float)Game1.tileSize;
					return false;
				}
				vector = new Vector2((float)((int)((this.position.X + (float)Game1.tileSize + 1f) / (float)Game1.tileSize)), (float)((int)this.position.Y / Game1.tileSize));
			}
			if (this.treeRunTimer > 0)
			{
				this.treeRunTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.treeRunTimer <= 0)
				{
					this.climbed.performUseAction(this.treeTile);
					return true;
				}
			}
			return base.update(time, environment);
		}
	}
}
