using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Projectiles
{
	public class DebuffingProjectile : Projectile
	{
		private Buff debuff;

		public DebuffingProjectile(Buff debuff, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition, Character owner = null)
		{
			this.theOneWhoFiredMe = owner;
			this.debuff = debuff;
			this.currentTileSheetIndex = parentSheetIndex;
			this.bouncesLeft = bouncesTillDestruct;
			this.tailLength = tailLength;
			this.rotationVelocity = rotationVelocity;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.position = startingPosition;
			Game1.playSound("debuffSpell");
		}

		public override void updatePosition(GameTime time)
		{
			this.position.X = this.position.X + this.xVelocity;
			this.position.Y = this.position.Y + this.yVelocity;
			this.position.X = this.position.X + (float)Math.Sin((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 128.0) * 8f;
			this.position.Y = this.position.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 128.0) * 8f;
		}

		public override void behaviorOnCollisionWithPlayer(GameLocation location)
		{
			if (Game1.random.Next(10) >= Game1.player.immunity)
			{
				Game1.buffsDisplay.addOtherBuff(this.debuff);
				this.explosionAnimation(location);
				Game1.playSound("debuffHit");
			}
		}

		public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
		{
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

		private void explosionAnimation(GameLocation location)
		{
			location.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(100, 150), 2, 1, this.position, false, false));
		}

		public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
		{
		}
	}
}
