using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Woodpecker : Critter
	{
		public const int flyingSpeed = 6;

		private bool flyingAway;

		private Tree tree;

		private int peckTimer;

		private int characterCheckTimer = 200;

		public Woodpecker(Tree tree, Vector2 position)
		{
			this.tree = tree;
			position *= (float)Game1.tileSize;
			this.position = position;
			this.position.X = this.position.X + (float)(Game1.tileSize / 2);
			this.position.Y = this.position.Y + 0f;
			this.startingPosition = position;
			this.baseFrame = 320;
			this.sprite = new AnimatedSprite(Critter.critterTexture, 320, 16, 16);
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(-64 - Game1.tileSize / 4), (float)(-128 + Game1.tileSize) + this.yJumpOffset + this.yOffset)), 1f, 0, 0, Color.White, this.flip, 4f, 0f, false);
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0f, -4f)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 3f + Math.Max(-3f, (this.yJumpOffset + this.yOffset) / 16f), SpriteEffects.None, (this.position.Y - 1f) / 10000f);
		}

		private void donePecking(Farmer who)
		{
			this.peckTimer = Game1.random.Next(1000, 3000);
		}

		private void playFlap(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("batFlap");
			}
		}

		private void playPeck(Farmer who)
		{
			if (Utility.isOnScreen(this.position, Game1.tileSize))
			{
				Game1.playSound("Cowboy_gunshot");
			}
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			if (environment == null || this.sprite == null || this.tree == null)
			{
				return true;
			}
			if (this.yJumpOffset < 0f && !this.flyingAway)
			{
				if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X - 2f;
				}
				else if (!environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, null, false, false, true))
				{
					this.position.X = this.position.X + 2f;
				}
			}
			this.peckTimer -= time.ElapsedGameTime.Milliseconds;
			if (!this.flyingAway && this.peckTimer <= 0 && this.sprite.currentAnimation == null)
			{
				int num = Game1.random.Next(2, 8);
				List<FarmerSprite.AnimationFrame> list = new List<FarmerSprite.AnimationFrame>();
				for (int i = 0; i < num; i++)
				{
					list.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 100));
					list.Add(new FarmerSprite.AnimationFrame(this.baseFrame + 1, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(this.playPeck), false));
				}
				list.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.donePecking), false));
				this.sprite.setCurrentAnimation(list);
				this.sprite.loop = false;
			}
			this.characterCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.characterCheckTimer < 0)
			{
				Character character = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float)Game1.tileSize, 6, environment);
				this.characterCheckTimer = 200;
				if ((character != null || this.tree.stump) && !this.flyingAway)
				{
					this.flyingAway = true;
					if (character != null && character.position.X > this.position.X)
					{
						this.flip = false;
					}
					else
					{
						this.flip = true;
					}
					this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
					{
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 2)), 70),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 60, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 4)), 70),
						new FarmerSprite.AnimationFrame((int)((short)(this.baseFrame + 3)), 60)
					});
					this.sprite.loop = true;
				}
			}
			if (this.flyingAway)
			{
				if (!this.flip)
				{
					this.position.X = this.position.X - 6f;
				}
				else
				{
					this.position.X = this.position.X + 6f;
				}
				this.yOffset -= 1f;
			}
			return base.update(time, environment);
		}
	}
}
