using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Projectiles
{
	public class BasicProjectile : Projectile
	{
		public delegate void onCollisionBehavior(GameLocation location, int xPosition, int yPosition, Character who);

		public int damageToFarmer;

		private string collisionSound;

		private bool explode;

		private BasicProjectile.onCollisionBehavior collisionBehavior;

		public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition, string collisionSound, string firingSound, bool explode, bool damagesMonsters = false, Character firer = null, bool spriteFromObjectSheet = false, BasicProjectile.onCollisionBehavior collisionBehavior = null)
		{
			this.damageToFarmer = damageToFarmer;
			this.currentTileSheetIndex = parentSheetIndex;
			this.bouncesLeft = bouncesTillDestruct;
			this.tailLength = tailLength;
			this.rotationVelocity = rotationVelocity;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.position = startingPosition;
			if (firingSound != null && !firingSound.Equals(""))
			{
				Game1.playSound(firingSound);
			}
			this.explode = explode;
			this.collisionSound = collisionSound;
			this.damagesMonsters = damagesMonsters;
			this.theOneWhoFiredMe = firer;
			this.spriteFromObjectSheet = spriteFromObjectSheet;
			this.collisionBehavior = collisionBehavior;
		}

		public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition) : this(damageToFarmer, parentSheetIndex, bouncesTillDestruct, tailLength, rotationVelocity, xVelocity, yVelocity, startingPosition, "flameSpellHit", "flameSpell", true, false, null, false, null)
		{
		}

		public override void updatePosition(GameTime time)
		{
			this.position.X = this.position.X + this.xVelocity;
			this.position.Y = this.position.Y + this.yVelocity;
		}

		public override void behaviorOnCollisionWithPlayer(GameLocation location)
		{
			if (!this.damagesMonsters)
			{
				Game1.farmerTakeDamage(this.damageToFarmer, false, null);
				this.explosionAnimation(location);
			}
		}

		public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
		{
			t.performUseAction(tileLocation);
			this.explosionAnimation(location);
		}

		public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
		{
			this.explosionAnimation(Game1.mine);
		}

		public override void behaviorOnCollisionWithOther(GameLocation location)
		{
			this.explosionAnimation(location);
		}

		public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
		{
			if (this.damagesMonsters)
			{
				this.explosionAnimation(location);
				if (n is Monster)
				{
					location.damageMonster(n.GetBoundingBox(), this.damageToFarmer, this.damageToFarmer + 1, false, (this.theOneWhoFiredMe is Farmer) ? (this.theOneWhoFiredMe as Farmer) : Game1.player);
					return;
				}
				n.getHitByPlayer((this.theOneWhoFiredMe == null || !(this.theOneWhoFiredMe is Farmer)) ? Game1.player : (this.theOneWhoFiredMe as Farmer), location);
			}
		}

		private void explosionAnimation(GameLocation location)
		{
			Rectangle sourceRectForStandardTileSheet = Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, -1, -1);
			sourceRectForStandardTileSheet.X += Game1.tileSize / 2 - 4;
			sourceRectForStandardTileSheet.Y += Game1.tileSize / 2 - 4;
			sourceRectForStandardTileSheet.Width = 8;
			sourceRectForStandardTileSheet.Height = 8;
			int debrisType = 12;
			int currentTileSheetIndex = this.currentTileSheetIndex;
			switch (currentTileSheetIndex)
			{
			case 378:
				debrisType = 0;
				break;
			case 379:
			case 381:
			case 383:
			case 385:
				break;
			case 380:
				debrisType = 2;
				break;
			case 382:
				debrisType = 4;
				break;
			case 384:
				debrisType = 6;
				break;
			case 386:
				debrisType = 10;
				break;
			default:
				if (currentTileSheetIndex == 390)
				{
					debrisType = 14;
				}
				break;
			}
			if (this.spriteFromObjectSheet)
			{
				Game1.createRadialDebris(location, debrisType, (int)(this.position.X + (float)(Game1.tileSize / 2)) / Game1.tileSize, (int)(this.position.Y + (float)(Game1.tileSize / 2)) / Game1.tileSize, 6, false, -1, false, -1);
			}
			else
			{
				Game1.createRadialDebris(location, Projectile.projectileSheet, sourceRectForStandardTileSheet, 4, (int)this.position.X + Game1.tileSize / 2, (int)this.position.Y + Game1.tileSize / 2, 12, (int)(this.position.Y / (float)Game1.tileSize) + 1);
			}
			if (this.collisionSound != null && !this.collisionSound.Equals(""))
			{
				Game1.playSound(this.collisionSound);
			}
			if (this.explode)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, this.position, false, Game1.random.NextDouble() < 0.5));
			}
			if (this.collisionBehavior != null)
			{
				this.collisionBehavior(location, this.getBoundingBox().Center.X, this.getBoundingBox().Center.Y, this.theOneWhoFiredMe);
			}
			this.destroyMe = true;
		}

		public static void explodeOnImpact(GameLocation location, int x, int y, Character who)
		{
			location.explode(new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize)), 3, (who is Farmer) ? ((Farmer)who) : null);
		}
	}
}
