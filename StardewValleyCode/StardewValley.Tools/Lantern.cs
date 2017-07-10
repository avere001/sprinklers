using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace StardewValley.Tools
{
	public class Lantern : Tool
	{
		public const float baseRadius = 10f;

		public const int millisecondsPerFuelUnit = 6000;

		public const int maxFuel = 100;

		public int fuelLeft;

		private int fuelTimer;

		public bool on;

		public Lantern() : base("Lantern", 0, 74, 74, false, 0)
		{
			this.upgradeLevel = 0;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			this.instantUse = true;
		}

		protected override string loadDescription()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Lantern.cs.14115", new object[0]);
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Lantern.cs.14114", new object[0]);
		}

		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			this.on = !this.on;
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			if (this.on)
			{
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 2.5f + (float)this.fuelLeft / 100f * 10f * 0.75f, new Color(0, 131, 255), -85736));
				return;
			}
			for (int i = Game1.currentLightSources.Count - 1; i >= 0; i--)
			{
				if (Game1.currentLightSources.ElementAt(i).identifier == -85736)
				{
					Game1.currentLightSources.Remove(Game1.currentLightSources.ElementAt(i));
					return;
				}
			}
		}

		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (this.on && this.fuelLeft > 0 && Game1.drawLighting)
			{
				this.fuelTimer += time.ElapsedGameTime.Milliseconds;
				if (this.fuelTimer > 6000)
				{
					this.fuelLeft--;
					this.fuelTimer = 0;
				}
				bool flag = false;
				foreach (LightSource current in Game1.currentLightSources)
				{
					if (current.identifier == -85736)
					{
						current.position = new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 2.5f + (float)this.fuelLeft / 100f * 10f * 0.75f, new Color(0, 131, 255), -85736));
				}
			}
			if (this.on && this.fuelLeft <= 0)
			{
				Utility.removeLightSource(1);
			}
		}
	}
}
