using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Objects
{
	public class SwitchFloor : StardewValley.Object
	{
		public static Color successColor = Color.LightBlue;

		public Color onColor;

		public Color offColor;

		private bool readyToflip;

		public bool finished;

		private int ticksToSuccess = -1;

		private float glow;

		public SwitchFloor()
		{
		}

		public SwitchFloor(Vector2 tileLocation, Color onColor, Color offColor, bool on)
		{
			this.tileLocation = tileLocation;
			this.onColor = onColor;
			this.offColor = offColor;
			this.isOn = on;
			this.fragility = 2;
			this.name = "Switch Floor";
		}

		protected override string loadDisplayName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:SwitchFloor.cs.13097", new object[0]);
		}

		public void flip(GameLocation environment)
		{
			this.isOn = !this.isOn;
			this.glow = 0.65f;
			foreach (Vector2 current in Utility.getAdjacentTileLocations(this.tileLocation))
			{
				if (environment.objects.ContainsKey(current) && environment.objects[current] is SwitchFloor)
				{
					environment.objects[current].isOn = !environment.objects[current].isOn;
					(environment.objects[current] as SwitchFloor).glow = 0.3f;
				}
			}
			Game1.playSound("shiny4");
		}

		public void setSuccessCountdown(int ticks)
		{
			this.ticksToSuccess = ticks;
			this.glow = 0.5f;
		}

		public void checkForCompleteness()
		{
			Queue<Vector2> queue = new Queue<Vector2>();
			HashSet<Vector2> hashSet = new HashSet<Vector2>();
			queue.Enqueue(this.tileLocation);
			Vector2 vector = default(Vector2);
			List<Vector2> list = new List<Vector2>();
			while (queue.Count > 0)
			{
				vector = queue.Dequeue();
				if (Game1.currentLocation.objects.ContainsKey(vector) && Game1.currentLocation.objects[vector] is SwitchFloor && (Game1.currentLocation.objects[vector] as SwitchFloor).isOn != this.isOn)
				{
					return;
				}
				hashSet.Add(vector);
				list = Utility.getAdjacentTileLocations(vector);
				for (int i = 0; i < list.Count; i++)
				{
					if (!hashSet.Contains(list[i]) && Game1.currentLocation.objects.ContainsKey(vector) && Game1.currentLocation.objects[vector] is SwitchFloor)
					{
						queue.Enqueue(list[i]);
					}
				}
				list.Clear();
			}
			int num = 5;
			foreach (Vector2 current in hashSet)
			{
				if (Game1.currentLocation.objects.ContainsKey(current) && Game1.currentLocation.objects[current] is SwitchFloor)
				{
					(Game1.currentLocation.objects[current] as SwitchFloor).setSuccessCountdown(num);
				}
				num += 2;
			}
			int coins = (int)Math.Sqrt((double)hashSet.Count) * 2;
			Vector2 vector2 = hashSet.Last<Vector2>();
			while (Game1.currentLocation.isTileOccupiedByFarmer(vector2) != null)
			{
				hashSet.Remove(vector2);
				if (hashSet.Count > 0)
				{
					vector2 = hashSet.Last<Vector2>();
				}
			}
			Game1.currentLocation.objects[vector2] = new Chest(coins, null, vector2, false);
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, vector2 * (float)Game1.tileSize, false, false));
			Game1.playSound("coin");
		}

		public override bool isPassable()
		{
			return true;
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), this.finished ? SwitchFloor.successColor : (this.isOn ? this.onColor : this.offColor), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1E-08f);
			if (this.glow > 0f)
			{
				spriteBatch.Draw(Flooring.floorsTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 1280, Game1.tileSize, Game1.tileSize)), Color.White * this.glow, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 2E-08f);
			}
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.glow > 0f)
			{
				this.glow -= 0.04f;
			}
			if (this.ticksToSuccess > 0)
			{
				this.ticksToSuccess--;
				if (this.ticksToSuccess == 0)
				{
					this.finished = true;
					this.glow += 0.2f;
					Game1.playSound("boulderCrack");
					return;
				}
			}
			else if (!this.finished)
			{
				using (List<Farmer>.Enumerator enumerator = Game1.currentLocation.getFarmers().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.getTileLocation().Equals(this.tileLocation))
						{
							if (this.readyToflip)
							{
								this.flip(Game1.currentLocation);
								this.checkForCompleteness();
							}
							this.readyToflip = false;
							return;
						}
					}
				}
				this.readyToflip = true;
			}
		}
	}
}
