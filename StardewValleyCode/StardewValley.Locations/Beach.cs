using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	public class Beach : GameLocation
	{
		private NPC oldMariner;

		public bool bridgeFixed;

		public Beach()
		{
		}

		public Beach(Map m, string name) : base(m, name)
		{
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			if (this.oldMariner != null)
			{
				this.oldMariner.update(time, this);
			}
			if (!Game1.eventUp && Game1.random.NextDouble() < 1E-06)
			{
				Vector2 vector = new Vector2((float)(Game1.random.Next(15, 47) * Game1.tileSize), (float)(Game1.random.Next(29, 42) * Game1.tileSize));
				bool flag = true;
				for (float num = vector.Y / (float)Game1.tileSize; num < (float)this.map.GetLayer("Back").LayerHeight; num += 1f)
				{
					if (base.doesTileHaveProperty((int)vector.X / Game1.tileSize, (int)num, "Water", "Back") == null || base.doesTileHaveProperty((int)vector.X / Game1.tileSize - 1, (int)num, "Water", "Back") == null || base.doesTileHaveProperty((int)vector.X / Game1.tileSize + 1, (int)num, "Water", "Back") == null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.temporarySprites.Add(new SeaMonsterTemporarySprite(250f, 4, Game1.random.Next(7), vector));
				}
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (!Game1.isRaining && !Game1.isFestival())
			{
				Game1.changeMusicTrack("none");
			}
			this.oldMariner = null;
		}

		public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("summer") && who.getTileX() >= 82 && who.FishingLevel >= 5 && !who.fishCaught.ContainsKey(159) && waterDepth >= 3 && Game1.random.NextDouble() < 0.18)
			{
				return new StardewValley.Object(159, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(65, 11, 25, 12);
			float num = 1f;
			while (Game1.random.NextDouble() < (double)num)
			{
				int parentSheetIndex = 393;
				if (Game1.random.NextDouble() < 0.2)
				{
					parentSheetIndex = 397;
				}
				Vector2 vector = new Vector2((float)Game1.random.Next(rectangle.X, rectangle.Right), (float)Game1.random.Next(rectangle.Y, rectangle.Bottom));
				if (this.isTileLocationTotallyClearAndPlaceable(vector))
				{
					this.dropObject(new StardewValley.Object(parentSheetIndex, 1, false, -1, 0), vector * (float)Game1.tileSize, Game1.viewport, true, null);
				}
				num /= 2f;
			}
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth >= 12 && Game1.dayOfMonth <= 14)
			{
				for (int i = 0; i < 5; i++)
				{
					this.spawnObjects();
				}
				num = 1.5f;
				while (Game1.random.NextDouble() < (double)num)
				{
					int parentSheetIndex2 = 393;
					if (Game1.random.NextDouble() < 0.2)
					{
						parentSheetIndex2 = 397;
					}
					Vector2 randomTile = base.getRandomTile();
					randomTile.Y /= 2f;
					string text = base.doesTileHaveProperty((int)randomTile.X, (int)randomTile.Y, "Type", "Back");
					if (this.isTileLocationTotallyClearAndPlaceable(randomTile) && (text == null || !text.Equals("Wood")))
					{
						this.dropObject(new StardewValley.Object(parentSheetIndex2, 1, false, -1, 0), randomTile * (float)Game1.tileSize, Game1.viewport, true, null);
					}
					num /= 1.1f;
				}
			}
		}

		public void doneWithBridgeFix()
		{
			Game1.globalFadeToClear(null, 0.02f);
			Game1.viewportFreeze = false;
		}

		public void fadedForBridgeFix()
		{
			DelayedAction.playSoundAfterDelay("crafting", 1000);
			DelayedAction.playSoundAfterDelay("crafting", 1500);
			DelayedAction.playSoundAfterDelay("crafting", 2000);
			DelayedAction.playSoundAfterDelay("crafting", 2500);
			DelayedAction.playSoundAfterDelay("axchop", 3000);
			DelayedAction.playSoundAfterDelay("Ship", 3200);
			Game1.viewportFreeze = true;
			Game1.viewport.X = -10000;
			this.bridgeFixed = true;
			Game1.pauseThenDoFunction(4000, new Game1.afterFadeFunction(this.doneWithBridgeFix));
			this.fixBridge();
		}

		public override bool answerDialogueAction(string questionAndAnswer, string[] questionParams)
		{
			if (questionAndAnswer != null && questionAndAnswer.Equals("BeachBridge_Yes"))
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.fadedForBridgeFix), 0.02f);
				Game1.player.removeItemsFromInventory(388, 300);
				return true;
			}
			return base.answerDialogueAction(questionAndAnswer, questionParams);
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int num = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (num != 284)
			{
				if (num == 496)
				{
					if (Game1.stats.DaysPlayed <= 1u)
					{
						Game1.drawLetterMessage(Game1.content.LoadString("Strings\\Locations:Beach_GoneFishingMessage", new object[0]).Replace('\n', '^'));
						return false;
					}
				}
			}
			else if (who.hasItemInInventory(388, 300, 0))
			{
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Beach_FixBridge_Question", new object[0]), base.createYesNoResponses(), "BeachBridge");
			}
			else
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Beach_FixBridge_Hint", new object[0]));
			}
			if (this.oldMariner != null && this.oldMariner.getTileX() == tileLocation.X && this.oldMariner.getTileY() == tileLocation.Y)
			{
				string text = Game1.content.LoadString("Strings\\Locations:Beach_Mariner_Player_" + (who.isMale ? "Male" : "Female"), new object[0]);
				if (!who.isMarried() && who.specialItems.Contains(460) && !Utility.doesItemWithThisIndexExistAnywhere(460, false))
				{
					for (int i = who.specialItems.Count - 1; i >= 0; i--)
					{
						if (who.specialItems[i] == 460)
						{
							who.specialItems.RemoveAt(i);
						}
					}
				}
				if (who.isMarried())
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerMarried", new object[]
					{
						text
					})));
				}
				else if (who.specialItems.Contains(460))
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerHasItem", new object[]
					{
						text
					})));
				}
				else if (who.hasAFriendWithHeartLevel(10, true) && who.houseUpgradeLevel == 0)
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerNotUpgradedHouse", new object[]
					{
						text
					})));
				}
				else if (who.hasAFriendWithHeartLevel(10, true))
				{
					Response[] answerChoices = new Response[]
					{
						new Response("Buy", Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_AnswerYes", new object[0])),
						new Response("Not", Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_AnswerNo", new object[0]))
					};
					base.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_Question", new object[]
					{
						text
					})), answerChoices, "mariner");
				}
				else
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerNoRelationship", new object[]
					{
						text
					})));
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return (this.oldMariner != null && position.Intersects(this.oldMariner.GetBoundingBox())) || base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public override void checkForMusic(GameTime time)
		{
			if (Game1.random.NextDouble() < 0.003 && Game1.timeOfDay < 1900)
			{
				Game1.playSound("seagulls");
			}
		}

		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (!Game1.isRaining && !Game1.isFestival())
			{
				Game1.changeMusicTrack("ocean");
			}
			int number = Game1.random.Next(6);
			foreach (Vector2 current in Utility.getPositionsInClusterAroundThisTile(new Vector2((float)Game1.random.Next(this.map.DisplayWidth / Game1.tileSize), (float)Game1.random.Next(12, this.map.DisplayHeight / Game1.tileSize)), number))
			{
				if (base.isTileOnMap(current) && (this.isTileLocationTotallyClearAndPlaceable(current) || base.doesTileHaveProperty((int)current.X, (int)current.Y, "Water", "Back") != null))
				{
					int startingState = 3;
					if (base.doesTileHaveProperty((int)current.X, (int)current.Y, "Water", "Back") != null)
					{
						startingState = 2;
						if (Game1.random.NextDouble() < 0.5)
						{
							continue;
						}
					}
					this.critters.Add(new Seagull(current * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), startingState));
				}
			}
			if (Game1.isRaining && Game1.timeOfDay < 1900)
			{
				this.oldMariner = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Mariner"), 0, 16, 32), new Vector2(80f, 5f) * (float)Game1.tileSize, 2, "Old Mariner", null);
			}
			if (this.bridgeFixed)
			{
				this.fixBridge();
			}
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth >= 12 && Game1.dayOfMonth <= 14)
			{
				this.waterColor = new Color(0, 255, 0) * 0.4f;
			}
		}

		public void fixBridge()
		{
			base.setMapTile(58, 13, 301, "Buildings", null, 1);
			base.setMapTile(59, 13, 301, "Buildings", null, 1);
			base.setMapTile(60, 13, 301, "Buildings", null, 1);
			base.setMapTile(61, 13, 301, "Buildings", null, 1);
			base.setMapTile(58, 14, 336, "Back", null, 1);
			base.setMapTile(59, 14, 336, "Back", null, 1);
			base.setMapTile(60, 14, 336, "Back", null, 1);
			base.setMapTile(61, 14, 336, "Back", null, 1);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.oldMariner != null)
			{
				this.oldMariner.draw(b);
			}
			base.draw(b);
			if (!this.bridgeFixed)
			{
				float num = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(58 * Game1.tileSize - 8), (float)(13 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(14 * Game1.tileSize) / 10000f + 1E-06f + 0.0058f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(58 * Game1.tileSize + Game1.tileSize / 2), (float)(13 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(14 * Game1.tileSize) / 10000f + 1E-05f + 0.0058f);
			}
		}
	}
}
