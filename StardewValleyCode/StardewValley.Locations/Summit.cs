using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;

namespace StardewValley.Locations
{
	public class Summit : GameLocation
	{
		public Summit()
		{
		}

		public Summit(Map map, string name) : base(map, name)
		{
		}

		public override void checkForMusic(GameTime time)
		{
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.temporarySprites.Count == 0 && Game1.random.NextDouble() < ((Game1.timeOfDay >= 1800) ? ((Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20) ? 1.0 : 0.001) : 0.0006))
			{
				Rectangle empty = Rectangle.Empty;
				Vector2 vector = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(0, 200));
				float x = -4f;
				int numberOfLoops = 100;
				float animationInterval = 100f;
				if (Game1.timeOfDay < 1800)
				{
					if (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("fall"))
					{
						empty = new Rectangle(640, 736, 16, 16);
						int num = Game1.random.Next(1, 4);
						x = -1f;
						for (int i = 0; i < num; i++)
						{
							TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, empty, (float)Game1.random.Next(80, 121), 4, 100, vector + new Vector2((float)((i + 1) * Game1.random.Next(15, 18)), (float)((i + 1) * -20)), false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							temporaryAnimatedSprite.motion = new Vector2(-1f, 0f);
							this.temporarySprites.Add(temporaryAnimatedSprite);
							temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, empty, (float)Game1.random.Next(80, 121), 4, 100, vector + new Vector2((float)((i + 1) * Game1.random.Next(15, 18)), (float)((i + 1) * 20)), false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							temporaryAnimatedSprite.motion = new Vector2(-1f, 0f);
							this.temporarySprites.Add(temporaryAnimatedSprite);
						}
					}
					else if (Game1.currentSeason.Equals("summer"))
					{
						empty = new Rectangle(640, 752 + ((Game1.random.NextDouble() < 0.5) ? 16 : 0), 16, 16);
						x = -0.5f;
						animationInterval = 150f;
					}
				}
				else if (Game1.timeOfDay >= 1900)
				{
					empty = new Rectangle(640, 816, 16, 16);
					x = -2f;
					numberOfLoops = 0;
					vector.X -= (float)Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
					if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20)
					{
						int num2 = Game1.random.Next(3);
						for (int j = 0; j < num2; j++)
						{
							TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, empty, (float)Game1.random.Next(80, 121), Game1.currentSeason.Equals("winter") ? 2 : 4, numberOfLoops, vector, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							temporaryAnimatedSprite2.motion = new Vector2(x, 0f);
							this.temporarySprites.Add(temporaryAnimatedSprite2);
							vector.X -= (float)Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
							vector.Y = (float)Game1.random.Next(0, 200);
						}
					}
					else if (Game1.currentSeason.Equals("winter") && Game1.timeOfDay >= 1700 && Game1.random.NextDouble() < 0.1)
					{
						empty = new Rectangle(640, 800, 32, 16);
						numberOfLoops = 1000;
						vector.X = (float)Game1.viewport.Width;
					}
					else if (Game1.currentSeason.Equals("winter"))
					{
						empty = Rectangle.Empty;
					}
				}
				if (Game1.timeOfDay >= 2200 && !Game1.currentSeason.Equals("winter") && Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20 && Game1.random.NextDouble() < 0.05)
				{
					empty = new Rectangle(640, 784, 16, 16);
					numberOfLoops = 100;
					vector.X = (float)Game1.viewport.Width;
					x = -3f;
				}
				if (!empty.Equals(Rectangle.Empty))
				{
					TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, empty, animationInterval, Game1.currentSeason.Equals("winter") ? 2 : 4, numberOfLoops, vector, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
					temporaryAnimatedSprite3.motion = new Vector2(x, 0f);
					this.temporarySprites.Add(temporaryAnimatedSprite3);
				}
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.background = null;
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.background = new Background();
			this.temporarySprites.Clear();
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
		}
	}
}
