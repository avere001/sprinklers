using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using System;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Town : GameLocation
	{
		private TemporaryAnimatedSprite minecartSteam;

		private bool ccRefurbished;

		private bool ccJoja;

		private bool playerCheckedBoard;

		private bool isShowingDestroyedJoja;

		private bool[] garbageChecked = new bool[7];

		private Vector2 clockCenter = new Vector2((float)(53 * Game1.tileSize), (float)(16 * Game1.tileSize + Game1.tileSize / 2));

		private Vector2 ccFacadePosition = new Vector2((float)(47 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(11 * Game1.tileSize + 59 * Game1.pixelZoom));

		private Vector2 ccFacadePositionBottom = new Vector2((float)(47 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(11 * Game1.tileSize + 109 * Game1.pixelZoom));

		public static Microsoft.Xna.Framework.Rectangle minuteHandSource = new Microsoft.Xna.Framework.Rectangle(363, 395, 5, 13);

		public static Microsoft.Xna.Framework.Rectangle hourHandSource = new Microsoft.Xna.Framework.Rectangle(369, 399, 5, 9);

		public static Microsoft.Xna.Framework.Rectangle clockNub = new Microsoft.Xna.Framework.Rectangle(375, 404, 4, 4);

		public static Microsoft.Xna.Framework.Rectangle jojaFacadeTop = new Microsoft.Xna.Framework.Rectangle(424, 1275, 174, 50);

		public static Microsoft.Xna.Framework.Rectangle jojaFacadeBottom = new Microsoft.Xna.Framework.Rectangle(424, 1325, 174, 51);

		public static Microsoft.Xna.Framework.Rectangle jojaFacadeWinterOverlay = new Microsoft.Xna.Framework.Rectangle(66, 1678, 174, 25);

		public Town()
		{
		}

		public Town(Map map, string name) : base(map, name)
		{
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (!Game1.isStartingToGetDarkOut())
			{
				this.addClintMachineGraphics();
				return;
			}
			AmbientLocationSounds.removeSound(new Vector2(100f, 79f));
		}

		public void checkedBoard()
		{
			this.playerCheckedBoard = true;
		}

		private void addClintMachineGraphics()
		{
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(302, 1946, 15, 16), (float)(7000 - Game1.gameTimeInterval), 1, 1, new Vector2(100f, 79f) * (float)Game1.tileSize + new Vector2(9f, 6f) * (float)Game1.pixelZoom, false, false, (float)(81 * Game1.tileSize + Game1.pixelZoom) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				shakeIntensity = 1f
			});
			for (int i = 0; i < 10; i++)
			{
				Utility.addSmokePuff(this, new Vector2(101f, 78f) * (float)Game1.tileSize + new Vector2(4f, 4f) * (float)Game1.pixelZoom, i * ((7000 - Game1.gameTimeInterval) / 16));
			}
			for (int j = 0; j < Game1.random.Next(1, 4); j++)
			{
				for (int k = 0; k < 16; k++)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(643, 1305, 5, 18), 50f, 4, 1, new Vector2(100f, 78f) * (float)Game1.tileSize + new Vector2((float)(-5 - k * 4), 0f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = j * 1500 + 100 * k
					});
				}
				Utility.addSmokePuff(this, new Vector2(100f, 78f) * (float)Game1.tileSize + new Vector2(-70f, -6f) * (float)Game1.pixelZoom, j * 1500 + 1600);
			}
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			for (int i = 0; i < this.garbageChecked.Length; i++)
			{
				this.garbageChecked[i] = false;
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex <= 1080)
				{
					if (tileIndex <= 620)
					{
						if (tileIndex != 78)
						{
							if (tileIndex != 620)
							{
								goto IL_6C7;
							}
							if (Game1.player.eventsSeen.Contains(191393))
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_SeedShopSign", new object[0]).Replace('\n', '^'));
							}
							else
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_SeedShopSign", new object[0]).Split(new char[]
								{
									'\n'
								}).First<string>() + "^" + Game1.content.LoadString("Strings\\Locations:SeedShop_LockedWed", new object[0]));
							}
							return true;
						}
						else
						{
							string text = base.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
							int num = (text != null) ? Convert.ToInt32(text.Split(new char[]
							{
								' '
							})[1]) : -1;
							if (num < 0 || num >= this.garbageChecked.Length || this.garbageChecked[num])
							{
								goto IL_6C7;
							}
							this.garbageChecked[num] = true;
							Game1.playSound("trashcan");
							Character character = Utility.isThereAFarmerOrCharacterWithinDistance(new Vector2((float)tileLocation.X, (float)tileLocation.Y), 7, this);
							if (character != null && character is NPC && !character.name.Equals("Linus") && !(character is Horse))
							{
								if ((character as NPC).age == 2)
								{
									character.doEmote(28, true);
									(character as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Child", new object[0]), true, true);
								}
								else if ((character as NPC).age == 1)
								{
									character.doEmote(8, true);
									(character as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Teen", new object[0]), true, true);
								}
								else
								{
									character.doEmote(12, true);
									(character as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Adult", new object[0]), true, true);
								}
								who.changeFriendship(-25, character as NPC);
								Game1.drawDialogue(character as NPC);
							}
							Random random = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + 777 + num);
							if (random.NextDouble() < 0.2 + Game1.dailyLuck)
							{
								int parentSheetIndex = 168;
								switch (random.Next(10))
								{
								case 0:
									parentSheetIndex = 168;
									break;
								case 1:
									parentSheetIndex = 167;
									break;
								case 2:
									parentSheetIndex = 170;
									break;
								case 3:
									parentSheetIndex = 171;
									break;
								case 4:
									parentSheetIndex = 172;
									break;
								case 5:
									parentSheetIndex = 216;
									break;
								case 6:
									parentSheetIndex = Utility.getRandomItemFromSeason(Game1.currentSeason, tileLocation.X * 653 + tileLocation.Y * 777, false);
									break;
								case 7:
									parentSheetIndex = 403;
									break;
								case 8:
									parentSheetIndex = 309 + random.Next(3);
									break;
								case 9:
									parentSheetIndex = 153;
									break;
								}
								if (num == 3 && random.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									parentSheetIndex = 535;
									if (random.NextDouble() < 0.05)
									{
										parentSheetIndex = 749;
									}
								}
								if (num == 4 && random.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									parentSheetIndex = 378 + random.Next(3) * 2;
									random.Next(1, 5);
								}
								if (num == 5 && random.NextDouble() < 0.2 + Game1.dailyLuck && Game1.dishOfTheDay != null)
								{
									if (Game1.dishOfTheDay.parentSheetIndex == 217)
									{
										parentSheetIndex = 216;
									}
									else
									{
										parentSheetIndex = Game1.dishOfTheDay.parentSheetIndex;
									}
								}
								if (num == 6 && random.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									parentSheetIndex = 223;
								}
								who.addItemByMenuIfNecessary(new StardewValley.Object(parentSheetIndex, 1, false, -1, 0), null);
								goto IL_6C7;
							}
							goto IL_6C7;
						}
					}
					else if (tileIndex != 958 && tileIndex != 1080)
					{
						goto IL_6C7;
					}
				}
				else
				{
					if (tileIndex <= 1925)
					{
						if (tileIndex == 1081)
						{
							goto IL_4EA;
						}
						if (tileIndex != 1925)
						{
							goto IL_6C7;
						}
					}
					else if (tileIndex != 1926 && tileIndex != 1945 && tileIndex != 1946)
					{
						goto IL_6C7;
					}
					if (this.isShowingDestroyedJoja)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_JojaSign_Destroyed", new object[0]));
						return true;
					}
					goto IL_6C7;
				}
				IL_4EA:
				if (Game1.player.getMount() != null)
				{
					return true;
				}
				if (Game1.player.getTileX() <= 70)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_PickupTruck", new object[0]));
					return true;
				}
				if (!Game1.player.mailReceived.Contains("ccBoilerRoom"))
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder", new object[0]));
					return true;
				}
				if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
				{
					Game1.player.getMount().checkAction(Game1.player, this);
				}
				else
				{
					Response[] answerChoices;
					if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
					{
						answerChoices = new Response[]
						{
							new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
							new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
							new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry", new object[0])),
							new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
						};
					}
					else
					{
						answerChoices = new Response[]
						{
							new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
							new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
							new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
						};
					}
					base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), answerChoices, "Minecart");
				}
			}
			IL_6C7:
			return base.checkAction(tileLocation, viewport, who);
		}

		private void refurbishCommunityCenter()
		{
			if (this.ccRefurbished)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(47, 11, 11, 9);
			for (int i = rectangle.X; i <= rectangle.Right; i++)
			{
				for (int j = rectangle.Y; j <= rectangle.Bottom; j++)
				{
					if (this.map.GetLayer("Back").Tiles[i, j] != null && this.map.GetLayer("Back").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Back").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Back").Tiles[i, j].TileIndex += 12;
					}
					if (this.map.GetLayer("Buildings").Tiles[i, j] != null && this.map.GetLayer("Buildings").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Buildings").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Buildings").Tiles[i, j].TileIndex += 12;
					}
					if (this.map.GetLayer("Front").Tiles[i, j] != null && this.map.GetLayer("Front").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Front").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Front").Tiles[i, j].TileIndex += 12;
					}
					if (this.map.GetLayer("AlwaysFront").Tiles[i, j] != null && this.map.GetLayer("AlwaysFront").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("AlwaysFront").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("AlwaysFront").Tiles[i, j].TileIndex += 12;
					}
				}
			}
			this.ccRefurbished = true;
			if (Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.ccJoja = true;
			}
		}

		private void showDestroyedJoja()
		{
			if (this.isShowingDestroyedJoja)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(90, 42, 11, 9);
			for (int i = rectangle.X; i <= rectangle.Right; i++)
			{
				for (int j = rectangle.Y; j <= rectangle.Bottom; j++)
				{
					bool flag = false;
					if (i > rectangle.X + 6 || j < rectangle.Y + 9)
					{
						flag = true;
					}
					if (flag && this.map.GetLayer("Back").Tiles[i, j] != null && this.map.GetLayer("Back").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Back").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Back").Tiles[i, j].TileIndex += 20;
					}
					if (flag && this.map.GetLayer("Buildings").Tiles[i, j] != null && this.map.GetLayer("Buildings").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Buildings").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Buildings").Tiles[i, j].TileIndex += 20;
					}
					if (flag && ((i != 93 && j != 50) || (i != 94 && j != 50)) && this.map.GetLayer("Front").Tiles[i, j] != null && this.map.GetLayer("Front").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("Front").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("Front").Tiles[i, j].TileIndex += 20;
					}
					if (flag && this.map.GetLayer("AlwaysFront").Tiles[i, j] != null && this.map.GetLayer("AlwaysFront").Tiles[i, j].TileSheet.Id.Equals("Town") && this.map.GetLayer("AlwaysFront").Tiles[i, j].TileIndex > 1200)
					{
						this.map.GetLayer("AlwaysFront").Tiles[i, j].TileIndex += 20;
					}
				}
			}
			this.isShowingDestroyedJoja = true;
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float)(107 * Game1.tileSize + Game1.pixelZoom * 2), (float)(79 * Game1.tileSize) - (float)Game1.tileSize * 3f / 4f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					totalNumberOfLoops = 999999,
					interval = 60f,
					flipped = true
				};
			}
			if (Game1.player.mailReceived.Contains("ccIsComplete") || Game1.player.mailReceived.Contains("JojaMember") || Game1.player.hasCompletedCommunityCenter())
			{
				this.refurbishCommunityCenter();
			}
			if (Game1.player.eventsSeen.Contains(191393))
			{
				this.showDestroyedJoja();
			}
			if (!Game1.currentSeason.Equals("winter"))
			{
				AmbientLocationSounds.addSound(new Vector2(26f, 26f), 0);
				AmbientLocationSounds.addSound(new Vector2(26f, 28f), 0);
			}
			if (!Game1.isStartingToGetDarkOut())
			{
				AmbientLocationSounds.addSound(new Vector2(100f, 79f), 2);
				this.addClintMachineGraphics();
			}
			if (Game1.player.mailReceived.Contains("checkedBulletinOnce"))
			{
				this.playerCheckedBoard = true;
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.minecartSteam = null;
			if ((Game1.currentSong != null && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors) && Game1.currentSong.Name.ToLower().Contains("town")) || (Game1.nextMusicTrack != null && Game1.nextMusicTrack.ToLower().Contains("town")))
			{
				Game1.changeMusicTrack("none");
			}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.update(time);
			}
		}

		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.draw(spriteBatch, false, 0, 0);
			}
			if (this.ccJoja)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePositionBottom), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeBottom), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize) / 10000f);
			}
			if (!this.playerCheckedBoard)
			{
				float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(41 * Game1.tileSize - 8), (float)(56 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(41 * Game1.tileSize + Game1.tileSize / 2), (float)(56 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			if (Game1.questOfTheDay != null && !Game1.questOfTheDay.accepted && Game1.questOfTheDay.questDescription.Length > 0)
			{
				float num2 = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(42 * Game1.tileSize + Game1.pixelZoom), (float)(56 * Game1.tileSize - Game1.tileSize + Game1.tileSize / 8) + num2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(395, 497, 3, 8)), Color.White, 0f, new Vector2(1f, 4f), (float)Game1.pixelZoom + Math.Max(0f, 0.25f - num2 / 16f), SpriteEffects.None, 1f);
			}
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("fall") && who.getTileLocation().Y < 15f && who.FishingLevel >= 3 && !who.fishCaught.ContainsKey(160) && Game1.random.NextDouble() < 0.2)
			{
				return new StardewValley.Object(160, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (this.ccJoja)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePosition), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeTop), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize) / 10000f);
				if (Game1.IsWinter)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePosition), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeWinterOverlay), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize + 1) / 10000f);
				}
			}
			else if (this.ccRefurbished)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.hourHandSource), Color.White, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1200) / 1200f) + (double)((float)Game1.gameTimeInterval / 7000f / 23f)), new Vector2(2.5f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.98f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.minuteHandSource), Color.White, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1000 % 100 % 60) / 60f) + (double)((float)Game1.gameTimeInterval / 7000f * 1.02f)), new Vector2(2.5f, 12f), (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.clockNub), Color.White, 0f, new Vector2(2f, 2f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			base.drawAboveAlwaysFrontLayer(b);
		}
	}
}
