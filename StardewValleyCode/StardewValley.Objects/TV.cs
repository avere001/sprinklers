using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Objects
{
	public class TV : Furniture
	{
		public const int customChannel = 1;

		public const int weatherChannel = 2;

		public const int fortuneTellerChannel = 3;

		public const int tipsChannel = 4;

		public const int cookingChannel = 5;

		private int currentChannel;

		private TemporaryAnimatedSprite screen;

		private TemporaryAnimatedSprite screenOverlay;

		public TV()
		{
		}

		public TV(int which, Vector2 tile) : base(which, tile)
		{
		}

		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			List<Response> list = new List<Response>();
			list.Add(new Response("Weather", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13105", new object[0])));
			list.Add(new Response("Fortune", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13107", new object[0])));
			string text = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
			if (text.Equals("Mon") || text.Equals("Thu"))
			{
				list.Add(new Response("Livin'", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13111", new object[0])));
			}
			if (text.Equals("Sun"))
			{
				list.Add(new Response("The", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13114", new object[0])));
			}
			if (text.Equals("Wed") && Game1.stats.DaysPlayed > 7u)
			{
				list.Add(new Response("The", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13117", new object[0])));
			}
			list.Add(new Response("(Leave)", Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13118", new object[0])));
			Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13120", new object[0]), list.ToArray(), new GameLocation.afterQuestionBehavior(this.selectChannel), null);
			Game1.player.Halt();
			return true;
		}

		public override Item getOne()
		{
			TV expr_11 = new TV(this.parentSheetIndex, this.tileLocation);
			expr_11.drawPosition = this.drawPosition;
			expr_11.defaultBoundingBox = this.defaultBoundingBox;
			expr_11.boundingBox = this.boundingBox;
			expr_11.currentRotation = this.currentRotation - 1;
			expr_11.rotations = this.rotations;
			expr_11.rotate();
			return expr_11;
		}

		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
		}

		public void selectChannel(Farmer who, string answer)
		{
			string a = answer.Split(new char[]
			{
				' '
			})[0];
			if (a == "Weather")
			{
				this.currentChannel = 2;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText(this.getWeatherChannelOpening()));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (a == "Fortune")
			{
				this.currentChannel = 3;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText(this.getFortuneTellerOpening()));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (a == "Livin'")
			{
				this.currentChannel = 4;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(517, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13124", new object[0])));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (!(a == "The"))
			{
				return;
			}
			this.currentChannel = 5;
			this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(602, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
			Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13127", new object[0])));
			Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
		}

		private string getFortuneTellerOpening()
		{
			switch (Game1.random.Next(5))
			{
			case 0:
				if (!Game1.player.isMale)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13130", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13128", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13132", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13133", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13134", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13135", new object[0]);
			default:
				return "";
			}
		}

		private string getWeatherChannelOpening()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13136", new object[0]);
		}

		public float getScreenSizeModifier()
		{
			if (this.parentSheetIndex != 1468)
			{
				return (float)Game1.pixelZoom / 2f;
			}
			return (float)Game1.pixelZoom;
		}

		public Vector2 getScreenPosition()
		{
			if (this.parentSheetIndex == 1466)
			{
				return new Vector2((float)(this.boundingBox.X + 6 * Game1.pixelZoom), (float)this.boundingBox.Y);
			}
			if (this.parentSheetIndex == 1468)
			{
				return new Vector2((float)(this.boundingBox.X + 3 * Game1.pixelZoom), (float)(this.boundingBox.Y - Game1.tileSize * 2 + Game1.pixelZoom * 8));
			}
			if (this.parentSheetIndex == 1680)
			{
				return new Vector2((float)(this.boundingBox.X + 6 * Game1.pixelZoom), (float)(this.boundingBox.Y - 3 * Game1.pixelZoom));
			}
			return Vector2.Zero;
		}

		public void proceedToNextScene()
		{
			if (this.currentChannel == 2)
			{
				if (this.screenOverlay == null)
				{
					this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(497, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
					Game1.drawObjectDialogue(Game1.parseText(this.getWeatherForecast()));
					this.setWeatherOverlay();
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					return;
				}
				this.turnOffTV();
				return;
			}
			else if (this.currentChannel == 3)
			{
				if (this.screenOverlay == null)
				{
					this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(624, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
					Game1.drawObjectDialogue(Game1.parseText(this.getFortuneForecast()));
					this.setFortuneOverlay();
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					return;
				}
				this.turnOffTV();
				return;
			}
			else
			{
				if (this.currentChannel != 4)
				{
					if (this.currentChannel == 5)
					{
						if (this.screenOverlay == null)
						{
							Game1.multipleDialogues(this.getWeeklyRecipe());
							Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
							this.screenOverlay = new TemporaryAnimatedSprite
							{
								alpha = 1E-07f
							};
							return;
						}
						this.turnOffTV();
					}
					return;
				}
				if (this.screenOverlay == null)
				{
					Game1.drawObjectDialogue(Game1.parseText(this.getTodaysTip()));
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					this.screenOverlay = new TemporaryAnimatedSprite
					{
						alpha = 1E-07f
					};
					return;
				}
				this.turnOffTV();
				return;
			}
		}

		public void turnOffTV()
		{
			this.screen = null;
			this.screenOverlay = null;
		}

		private void setWeatherOverlay()
		{
			switch (Game1.weatherForTomorrow)
			{
			case 0:
			case 6:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 1:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 333, 13, 13), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 2:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, Game1.currentSeason.Equals("spring") ? new Rectangle(465, 359, 13, 13) : (Game1.currentSeason.Equals("fall") ? new Rectangle(413, 359, 13, 13) : new Rectangle(465, 346, 13, 13)), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 3:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 346, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 4:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 372, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 5:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			default:
				return;
			}
		}

		private string getTodaysTip()
		{
			Dictionary<string, string> dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\TipChannel");
			if (!dictionary.ContainsKey(string.Concat(Game1.stats.DaysPlayed % 224u)))
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13148", new object[0]);
			}
			return dictionary[string.Concat(Game1.stats.DaysPlayed % 224u)];
		}

		private string[] getWeeklyRecipe()
		{
			string[] array = new string[2];
			int num = (int)(Game1.stats.DaysPlayed % 224u / 7u);
			Dictionary<string, string> dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\CookingChannel");
			if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Wed"))
			{
				Random random = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
				num = Math.Max(1, 1 + random.Next((int)(Game1.stats.DaysPlayed % 224u)) / 7);
			}
			try
			{
				string text = dictionary[string.Concat(num)].Split(new char[]
				{
					'/'
				})[0];
				array[0] = dictionary[string.Concat(num)].Split(new char[]
				{
					'/'
				})[1];
				if (CraftingRecipe.cookingRecipes.ContainsKey(text))
				{
					string[] array2 = CraftingRecipe.cookingRecipes[text].Split(new char[]
					{
						'/'
					});
					array[1] = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) ? (Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat(num)].Split(new char[]
					{
						'/'
					})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
					{
						text
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
					{
						text
					})) : (Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat(num)].Split(new char[]
					{
						'/'
					})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
					{
						array2[array2.Length - 1]
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
					{
						array2[array2.Length - 1]
					})));
				}
				else
				{
					array[1] = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) ? (Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat(num)].Split(new char[]
					{
						'/'
					})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
					{
						text
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
					{
						text
					})) : (Game1.player.cookingRecipes.ContainsKey(dictionary[string.Concat(num)].Split(new char[]
					{
						'/'
					})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
					{
						dictionary[string.Concat(num)].Split(new char[]
						{
							'/'
						}).Last<string>()
					}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
					{
						dictionary[string.Concat(num)].Split(new char[]
						{
							'/'
						}).Last<string>()
					})));
				}
				if (!Game1.player.cookingRecipes.ContainsKey(text))
				{
					Game1.player.cookingRecipes.Add(text, 0);
				}
			}
			catch (Exception)
			{
				string text2 = dictionary["1"].Split(new char[]
				{
					'/'
				})[0];
				array[0] = dictionary["1"].Split(new char[]
				{
					'/'
				})[1];
				array[1] = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en) ? (Game1.player.cookingRecipes.ContainsKey(dictionary["1"].Split(new char[]
				{
					'/'
				})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
				{
					text2
				}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
				{
					text2
				})) : (Game1.player.cookingRecipes.ContainsKey(dictionary["1"].Split(new char[]
				{
					'/'
				})[0]) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13151", new object[]
				{
					dictionary["1"].Split(new char[]
					{
						'/'
					}).Last<string>()
				}) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13153", new object[]
				{
					dictionary["1"].Split(new char[]
					{
						'/'
					}).Last<string>()
				})));
				if (!Game1.player.cookingRecipes.ContainsKey(text2))
				{
					Game1.player.cookingRecipes.Add(text2, 0);
				}
			}
			return array;
		}

		private string getWeatherForecast()
		{
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth % 12 == 0)
			{
				Game1.weatherForTomorrow = 3;
			}
			if (Game1.stats.DaysPlayed == 2u)
			{
				Game1.weatherForTomorrow = 1;
			}
			if (Game1.dayOfMonth == 28)
			{
				Game1.weatherForTomorrow = 0;
			}
			switch (Game1.weatherForTomorrow)
			{
			case 0:
			case 6:
				if (Game1.random.NextDouble() >= 0.5)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13183", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13182", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13184", new object[0]);
			case 2:
				if (Game1.currentSeason.Equals("spring"))
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13187", new object[0]);
				}
				if (!Game1.currentSeason.Equals("fall"))
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13190", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13189", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13185", new object[0]);
			case 4:
			{
				Dictionary<string, string> dictionary;
				try
				{
					dictionary = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (Game1.dayOfMonth + 1));
				}
				catch (Exception)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13164", new object[0]);
				}
				string text = dictionary["name"];
				string a = dictionary["conditions"].Split(new char[]
				{
					'/'
				})[0];
				int time = Convert.ToInt32(dictionary["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[0]);
				int time2 = Convert.ToInt32(dictionary["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[1]);
				string text2 = "";
				if (!(a == "Town"))
				{
					if (!(a == "Beach"))
					{
						if (a == "Forest")
						{
							text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13174", new object[0]);
						}
					}
					else
					{
						text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13172", new object[0]);
					}
				}
				else
				{
					text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13170", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13175", new object[]
				{
					text,
					text2,
					Game1.getTimeOfDayString(time),
					Game1.getTimeOfDayString(time2)
				});
			}
			case 5:
				if (Game1.random.NextDouble() >= 0.5)
				{
					return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13181", new object[0]);
				}
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13180", new object[0]);
			default:
				return "";
			}
		}

		private void setFortuneOverlay()
		{
			if (Game1.dailyLuck < -0.07)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck < -0.02)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck > 0.07)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(644, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck > 0.02)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
		}

		private string getFortuneForecast()
		{
			string result;
			if (Game1.dailyLuck == -0.12)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13191", new object[0]);
			}
			else if (Game1.dailyLuck < -0.07)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13192", new object[0]);
			}
			else if (Game1.dailyLuck < -0.02)
			{
				result = ((Game1.random.NextDouble() < 0.5) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13193", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13195", new object[0]));
			}
			else if (Game1.dailyLuck == 0.12)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13197", new object[0]);
			}
			else if (Game1.dailyLuck > 0.07)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13198", new object[0]);
			}
			else if (Game1.dailyLuck > 0.02)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13199", new object[0]);
			}
			else
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13200", new object[0]);
			}
			if (Game1.dailyLuck == 0.0)
			{
				result = Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13201", new object[0]);
			}
			return result;
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			base.draw(spriteBatch, x, y, alpha);
			if (this.screen != null)
			{
				this.screen.update(Game1.currentGameTime);
				this.screen.draw(spriteBatch, false, 0, 0);
				if (this.screenOverlay != null)
				{
					this.screenOverlay.update(Game1.currentGameTime);
					this.screenOverlay.draw(spriteBatch, false, 0, 0);
				}
			}
		}
	}
}
