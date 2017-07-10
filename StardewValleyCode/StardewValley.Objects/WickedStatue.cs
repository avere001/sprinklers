using Microsoft.Xna.Framework;
using StardewValley.Projectiles;
using StardewValley.Tools;
using System;

namespace StardewValley.Objects
{
	public class WickedStatue : StardewValley.Object
	{
		public WickedStatue()
		{
		}

		public WickedStatue(Vector2 tileLocation) : base(tileLocation, 84, false)
		{
			this.fragility = 2;
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		public override bool performToolAction(Tool t)
		{
			if (t is Pickaxe)
			{
				Game1.createRadialDebris(Game1.currentLocation, 14, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.random.Next(8, 16), false, -1, false, -1);
				Game1.createMultipleObjectDebris(390, (int)this.tileLocation.X, (int)this.tileLocation.Y, (int)this.tileLocation.X % 4 + 3, t.getLastFarmerToUse().uniqueMultiplayerID);
				if (Game1.currentLocation.objects.ContainsKey(this.tileLocation))
				{
					Game1.currentLocation.objects.Remove(this.tileLocation);
				}
				Game1.playSound("hammer");
				return false;
			}
			return false;
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
			if (Game1.random.NextDouble() < 0.001 && Utility.isThereAFarmerWithinDistance(this.tileLocation, 12) != null)
			{
				Farmer nearestFarmerInCurrentLocation = Utility.getNearestFarmerInCurrentLocation(this.tileLocation);
				Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize), 12f, nearestFarmerInCurrentLocation);
				Game1.currentLocation.projectiles.Add(new BasicProjectile(8, 10, 2, 2, 0.196349546f, velocityTowardPlayer.X, velocityTowardPlayer.Y, this.tileLocation * (float)Game1.tileSize));
			}
		}
	}
}
