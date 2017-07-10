using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.Events
{
	public class SoundInTheNightEvent : FarmEvent
	{
		public const int cropCircle = 0;

		public const int meteorite = 1;

		public const int dogs = 2;

		public const int owl = 3;

		public const int earthquake = 4;

		private int behavior;

		private int timer;

		private string soundName;

		private string message;

		private bool playedSound;

		private bool showedMessage;

		private Vector2 targetLocation;

		private Building targetBuilding;

		public SoundInTheNightEvent(int which)
		{
			this.behavior = which;
		}

		public bool setUp()
		{
			Random random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			Farm farm = Game1.getLocationFromName("Farm") as Farm;
			switch (this.behavior)
			{
			case 0:
				this.soundName = "UFO";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_UFO", new object[0]);
				for (int i = 50; i > 0; i--)
				{
					this.targetLocation = new Vector2((float)random.Next(5, farm.map.GetLayer("Back").TileWidth - 4), (float)random.Next(5, farm.map.GetLayer("Back").TileHeight - 4));
					if (!farm.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
					{
						return true;
					}
				}
				break;
			case 1:
			{
				this.soundName = "Meteorite";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Meteorite", new object[0]);
				this.targetLocation = new Vector2((float)random.Next(5, farm.map.GetLayer("Back").TileWidth - 20), (float)random.Next(5, farm.map.GetLayer("Back").TileHeight - 4));
				int num = (int)this.targetLocation.X;
				while ((float)num <= this.targetLocation.X + 1f)
				{
					int num2 = (int)this.targetLocation.Y;
					while ((float)num2 <= this.targetLocation.Y + 1f)
					{
						Vector2 vector = new Vector2((float)num, (float)num2);
						if (!farm.isTileOpenBesidesTerrainFeatures(vector) || !farm.isTileOpenBesidesTerrainFeatures(new Vector2(vector.X + 1f, vector.Y)) || !farm.isTileOpenBesidesTerrainFeatures(new Vector2(vector.X + 1f, vector.Y - 1f)) || !farm.isTileOpenBesidesTerrainFeatures(new Vector2(vector.X, vector.Y - 1f)) || farm.doesTileHaveProperty((int)vector.X, (int)vector.Y, "Water", "Back") != null || farm.doesTileHaveProperty((int)vector.X + 1, (int)vector.Y, "Water", "Back") != null)
						{
							return true;
						}
						num2++;
					}
					num++;
				}
				break;
			}
			case 2:
				this.soundName = "dogs";
				if (random.NextDouble() < 0.5)
				{
					return true;
				}
				foreach (Building current in farm.buildings)
				{
					if (current.indoors != null && current.indoors is AnimalHouse && !current.animalDoorOpen && (current.indoors as AnimalHouse).animalsThatLiveHere.Count > (current.indoors as AnimalHouse).animals.Count && random.NextDouble() < (double)(1f / (float)farm.buildings.Count))
					{
						this.targetBuilding = current;
						break;
					}
				}
				return this.targetBuilding == null;
			case 3:
				this.soundName = "owl";
				for (int i = 50; i > 0; i--)
				{
					this.targetLocation = new Vector2((float)random.Next(5, farm.map.GetLayer("Back").TileWidth - 4), (float)random.Next(5, farm.map.GetLayer("Back").TileHeight - 4));
					if (!farm.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
					{
						return true;
					}
				}
				break;
			case 4:
				this.soundName = "thunder_small";
				this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Earthquake", new object[0]);
				break;
			}
			Game1.freezeControls = true;
			return false;
		}

		public bool tickUpdate(GameTime time)
		{
			this.timer += time.ElapsedGameTime.Milliseconds;
			if (this.timer > 1500 && !this.playedSound)
			{
				if (this.soundName != null && !this.soundName.Equals(""))
				{
					Game1.playSound(this.soundName);
					this.playedSound = true;
				}
				if (!this.playedSound && this.message != null)
				{
					Game1.drawObjectDialogue(this.message);
					Game1.globalFadeToClear(null, 0.02f);
					this.showedMessage = true;
				}
			}
			if (this.timer > 7000 && !this.showedMessage)
			{
				Game1.pauseThenMessage(10, this.message, false);
				this.showedMessage = true;
			}
			if (this.showedMessage && this.playedSound)
			{
				Game1.freezeControls = false;
				return true;
			}
			return false;
		}

		public void draw(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black);
		}

		public void makeChangesToLocation()
		{
			Farm farm = Game1.getLocationFromName("Farm") as Farm;
			switch (this.behavior)
			{
			case 0:
			{
				StardewValley.Object @object = new StardewValley.Object(this.targetLocation, 96, false);
				@object.minutesUntilReady = 24000 - Game1.timeOfDay;
				farm.objects.Add(this.targetLocation, @object);
				return;
			}
			case 1:
				if (farm.terrainFeatures.ContainsKey(this.targetLocation))
				{
					farm.terrainFeatures.Remove(this.targetLocation);
				}
				if (farm.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 0f)))
				{
					farm.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 0f));
				}
				if (farm.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 1f)))
				{
					farm.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 1f));
				}
				if (farm.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(0f, 1f)))
				{
					farm.terrainFeatures.Remove(this.targetLocation + new Vector2(0f, 1f));
				}
				farm.resourceClumps.Add(new ResourceClump(622, 2, 2, this.targetLocation));
				return;
			case 2:
			{
				AnimalHouse animalHouse = this.targetBuilding.indoors as AnimalHouse;
				long num = 0L;
				foreach (long current in animalHouse.animalsThatLiveHere)
				{
					if (!animalHouse.animals.ContainsKey(current))
					{
						num = current;
						break;
					}
				}
				if (!Game1.getFarm().animals.ContainsKey(num))
				{
					return;
				}
				Game1.getFarm().animals.Remove(num);
				animalHouse.animalsThatLiveHere.Remove(num);
				using (Dictionary<long, FarmAnimal>.Enumerator enumerator2 = Game1.getFarm().animals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<long, FarmAnimal> current2 = enumerator2.Current;
						current2.Value.moodMessage = 5;
					}
					return;
				}
				break;
			}
			case 3:
				break;
			default:
				return;
			}
			farm.objects.Add(this.targetLocation, new StardewValley.Object(this.targetLocation, 95, false));
		}

		public void drawAboveEverything(SpriteBatch b)
		{
		}
	}
}
