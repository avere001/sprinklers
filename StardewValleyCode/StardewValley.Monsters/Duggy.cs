using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	public class Duggy : Monster
	{
		private double chanceToDisappear = 0.03;

		private bool hasDugForTreasure;

		public Duggy()
		{
			this.hideShadow = true;
		}

		public Duggy(Vector2 position) : base("Duggy", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.isInvisible = true;
			this.damageToFarmer = 0;
			this.sprite.loop = false;
			this.sprite.CurrentFrame = 0;
			this.hideShadow = true;
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int num = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				num = -1;
			}
			else
			{
				this.health -= num;
				Game1.playSound("hitEnemy");
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return num;
		}

		public override void deathAnimation()
		{
			Game1.playSound("monsterdead");
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.DarkRed, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				holdLastFrame = true,
				alphaFade = 0.01f,
				interval = 70f
			}, Game1.currentLocation, 4, 64, 64);
		}

		public override void update(GameTime time, GameLocation location)
		{
			if (this.invincibleCountdown > 0)
			{
				this.glowingColor = Color.Cyan;
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			this.behaviorAtGameTick(time);
			if (this.position.X < 0f || this.position.X > (float)(location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || this.position.Y < 0f || this.position.Y > (float)(location.map.GetLayer("Back").LayerHeight * Game1.tileSize))
			{
				location.characters.Remove(this);
			}
			base.updateGlow();
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.isEmoting = false;
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			if (this.sprite.currentFrame < 4)
			{
				boundingBox.Inflate(Game1.tileSize * 2, Game1.tileSize * 2);
				if (!this.isInvisible || boundingBox.Contains(Game1.player.getStandingX(), Game1.player.getStandingY()))
				{
					if (this.isInvisible)
					{
						if (Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier") || (!Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].TileIndex != 0))
						{
							return;
						}
						this.position = new Vector2(Game1.player.position.X, Game1.player.position.Y + (float)Game1.player.sprite.spriteHeight - (float)this.sprite.spriteHeight);
						Game1.playSound("Duggy");
						this.sprite.interval = 100f;
						this.position = Game1.player.getTileLocation() * (float)Game1.tileSize;
					}
					this.isInvisible = false;
					this.sprite.AnimateDown(time, 0, "");
				}
			}
			if (this.sprite.currentFrame >= 4 && this.sprite.CurrentFrame < 8)
			{
				if (!this.hasDugForTreasure)
				{
					base.getTileLocation();
				}
				boundingBox.Inflate(-Game1.tileSize * 2, -Game1.tileSize * 2);
				Game1.currentLocation.isCollidingPosition(boundingBox, Game1.viewport, false, 8, false, this);
				this.sprite.AnimateRight(time, 0, "");
				this.sprite.interval = 220f;
			}
			if (this.sprite.currentFrame >= 8)
			{
				this.sprite.AnimateUp(time, 0, "");
			}
			if (this.sprite.currentFrame >= 10)
			{
				this.isInvisible = true;
				this.sprite.currentFrame = 0;
				this.hasDugForTreasure = false;
				int num = 0;
				Vector2 tileLocation = base.getTileLocation();
				Game1.currentLocation.map.GetLayer("Back").Tiles[(int)tileLocation.X, (int)tileLocation.Y].TileIndex = 0;
				Game1.currentLocation.removeEverythingExceptCharactersFromThisTile((int)tileLocation.X, (int)tileLocation.Y);
				Vector2 vector = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
				while (num < 4 && (vector.X <= 0f || vector.X >= (float)Game1.currentLocation.map.Layers[0].LayerWidth || vector.Y <= 0f || vector.Y >= (float)Game1.currentLocation.map.Layers[0].LayerHeight || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)vector.X, (int)vector.Y] == null || Game1.currentLocation.isTileOccupied(vector, "") || !Game1.currentLocation.isTilePassable(new Location((int)vector.X, (int)vector.Y), Game1.viewport) || vector.Equals(new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize))) || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)vector.X, (int)vector.Y].Properties.ContainsKey("NPCBarrier") || (!Game1.currentLocation.map.GetLayer("Back").Tiles[(int)vector.X, (int)vector.Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int)vector.X, (int)vector.Y].TileIndex != 0)))
				{
					vector = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-2, 2)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-2, 2)));
					num++;
				}
			}
		}
	}
}
