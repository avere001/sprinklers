using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class SkillsPage : IClickableMenu
	{
		public const int region_special1 = 10201;

		public const int region_special2 = 10202;

		public const int region_special3 = 10203;

		public const int region_special4 = 10204;

		public const int region_special5 = 10205;

		public const int region_special6 = 10206;

		public const int region_special7 = 10207;

		public const int region_skillArea1 = 0;

		public const int region_skillArea2 = 1;

		public const int region_skillArea3 = 2;

		public const int region_skillArea4 = 3;

		public const int region_skillArea5 = 4;

		public List<ClickableTextureComponent> skillBars = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> skillAreas = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> specialItems = new List<ClickableTextureComponent>();

		private string hoverText = "";

		private string hoverTitle = "";

		private int professionImage = -1;

		private int playerPanelIndex;

		private int playerPanelTimer;

		private Rectangle playerPanel;

		private int[] playerPanelFrames = new int[]
		{
			0,
			1,
			0,
			2
		};

		public SkillsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			int num = this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 / 4;
			int y2 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)height / 2f) + Game1.tileSize * 5 / 4;
			this.playerPanel = new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder, Game1.tileSize * 2, Game1.tileSize * 3);
			if (Game1.player.canUnderstandDwarves)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num, y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11587", new object[0]), Game1.mouseCursors, new Rectangle(129, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10201,
					rightNeighborID = 10202,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasRustyKey)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11588", new object[0]), Game1.mouseCursors, new Rectangle(145, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10202,
					rightNeighborID = 10203,
					leftNeighborID = 10201,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasClubCard)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + 2 * (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11589", new object[0]), Game1.mouseCursors, new Rectangle(161, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10203,
					rightNeighborID = 10204,
					leftNeighborID = 10202,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasSpecialCharm)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + 3 * (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11590", new object[0]), Game1.mouseCursors, new Rectangle(177, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10204,
					rightNeighborID = 10205,
					leftNeighborID = 10203,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasSkullKey)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + 4 * (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11591", new object[0]), Game1.mouseCursors, new Rectangle(193, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10205,
					rightNeighborID = 10206,
					leftNeighborID = 10204,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasDarkTalisman)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + 5 * (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]), Game1.mouseCursors, new Rectangle(225, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10206,
					rightNeighborID = 10207,
					leftNeighborID = 10205,
					upNeighborID = 4
				});
			}
			if (Game1.player.hasMagicInk)
			{
				this.specialItems.Add(new ClickableTextureComponent("", new Rectangle(num + 6 * (Game1.tileSize + Game1.pixelZoom), y2, Game1.tileSize, Game1.tileSize), null, Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]), Game1.mouseCursors, new Rectangle(241, 320, 16, 16), (float)Game1.pixelZoom, true)
				{
					myID = 10207,
					leftNeighborID = 10206,
					upNeighborID = 4
				});
			}
			int num2 = 0;
			int num3 = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? (this.xPositionOnScreen + width - Game1.tileSize * 7 - Game1.tileSize * 3 / 4 + Game1.pixelZoom) : (this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - Game1.pixelZoom);
			int num4 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 3;
			for (int i = 4; i < 10; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					string text = "";
					string text2 = "";
					bool flag = false;
					int num5 = -1;
					switch (j)
					{
					case 0:
						flag = (Game1.player.FarmingLevel > i);
						num5 = Game1.player.getProfessionForSkill(0, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					case 1:
						flag = (Game1.player.MiningLevel > i);
						num5 = Game1.player.getProfessionForSkill(3, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					case 2:
						flag = (Game1.player.ForagingLevel > i);
						num5 = Game1.player.getProfessionForSkill(2, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					case 3:
						flag = (Game1.player.FishingLevel > i);
						num5 = Game1.player.getProfessionForSkill(1, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					case 4:
						flag = (Game1.player.CombatLevel > i);
						num5 = Game1.player.getProfessionForSkill(4, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					case 5:
						flag = (Game1.player.LuckLevel > i);
						num5 = Game1.player.getProfessionForSkill(5, i + 1);
						this.parseProfessionDescription(ref text, ref text2, LevelUpMenu.getProfessionDescription(num5));
						break;
					}
					if (flag && (i + 1) % 5 == 0)
					{
						this.skillBars.Add(new ClickableTextureComponent(string.Concat(num5), new Rectangle(num2 + num3 - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom), num4 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6), 14 * Game1.pixelZoom, 9 * Game1.pixelZoom), null, text, Game1.mouseCursors, new Rectangle(159, 338, 14, 9), (float)Game1.pixelZoom, true)
						{
							myID = ((i + 1 == 5) ? (100 + j) : (200 + j)),
							leftNeighborID = ((i + 1 == 5) ? j : (100 + j)),
							rightNeighborID = ((i + 1 == 5) ? (200 + j) : -1),
							downNeighborID = 10201
						});
					}
				}
				num2 += Game1.pixelZoom * 6;
			}
			for (int k = 0; k < this.skillBars.Count; k++)
			{
				if (k < this.skillBars.Count - 1 && Math.Abs(this.skillBars[k + 1].myID - this.skillBars[k].myID) < 50)
				{
					this.skillBars[k].downNeighborID = this.skillBars[k + 1].myID;
					this.skillBars[k + 1].upNeighborID = this.skillBars[k].myID;
				}
			}
			if (this.skillBars.Count > 1 && this.skillBars.Last<ClickableTextureComponent>().myID >= 200 && this.skillBars[this.skillBars.Count - 2].myID >= 200)
			{
				this.skillBars.Last<ClickableTextureComponent>().upNeighborID = this.skillBars[this.skillBars.Count - 2].myID;
			}
			for (int l = 0; l < 5; l++)
			{
				int num6 = l;
				if (num6 == 1)
				{
					num6 = 3;
				}
				else if (num6 == 3)
				{
					num6 = 1;
				}
				string text3 = "";
				switch (num6)
				{
				case 0:
					if (Game1.player.FarmingLevel > 0)
					{
						text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11592", new object[]
						{
							Game1.player.FarmingLevel
						}) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11594", new object[]
						{
							Game1.player.FarmingLevel
						});
					}
					break;
				case 1:
					if (Game1.player.FishingLevel > 0)
					{
						text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11598", new object[]
						{
							Game1.player.FishingLevel
						});
					}
					break;
				case 2:
					if (Game1.player.ForagingLevel > 0)
					{
						text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11596", new object[]
						{
							Game1.player.ForagingLevel
						});
					}
					break;
				case 3:
					if (Game1.player.MiningLevel > 0)
					{
						text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11600", new object[]
						{
							Game1.player.MiningLevel
						});
					}
					break;
				case 4:
					if (Game1.player.CombatLevel > 0)
					{
						text3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11602", new object[]
						{
							Game1.player.CombatLevel * 5
						});
					}
					break;
				}
				this.skillAreas.Add(new ClickableTextureComponent(string.Concat(num6), new Rectangle(num3 - Game1.tileSize * 2 - Game1.tileSize * 3 / 4, num4 + l * (Game1.tileSize / 2 + Game1.pixelZoom * 6), Game1.tileSize * 2 + Game1.pixelZoom * 5, 9 * Game1.pixelZoom), string.Concat(num6), text3, null, Rectangle.Empty, 1f, false)
				{
					myID = l,
					downNeighborID = ((l < 4) ? (l + 1) : 10201),
					upNeighborID = ((l > 0) ? (l - 1) : 12341),
					rightNeighborID = 100 + l
				});
			}
		}

		private void parseProfessionDescription(ref string professionBlurb, ref string professionTitle, List<string> professionDescription)
		{
			if (professionDescription.Count > 0)
			{
				professionTitle = professionDescription[0];
				for (int i = 1; i < professionDescription.Count; i++)
				{
					professionBlurb += professionDescription[i];
					if (i < professionDescription.Count - 1)
					{
						professionBlurb += Environment.NewLine;
					}
				}
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = ((this.skillAreas.Count > 0) ? base.getComponentWithID(0) : null);
			if (this.currentlySnappedComponent != null && Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				this.currentlySnappedComponent.snapMouseCursorToCenter();
			}
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			this.hoverTitle = "";
			this.professionImage = -1;
			foreach (ClickableTextureComponent current in this.specialItems)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.hoverText;
					break;
				}
			}
			foreach (ClickableTextureComponent current2 in this.skillBars)
			{
				current2.scale = (float)Game1.pixelZoom;
				if (current2.containsPoint(x, y) && current2.hoverText.Length > 0 && !current2.name.Equals("-1"))
				{
					this.hoverText = current2.hoverText;
					this.hoverTitle = LevelUpMenu.getProfessionTitleFromNumber(Convert.ToInt32(current2.name));
					this.professionImage = Convert.ToInt32(current2.name);
					current2.scale = 0f;
				}
			}
			foreach (ClickableTextureComponent current3 in this.skillAreas)
			{
				if (current3.containsPoint(x, y) && current3.hoverText.Length > 0)
				{
					this.hoverText = current3.hoverText;
					this.hoverTitle = Farmer.getSkillDisplayNameFromIndex(Convert.ToInt32(current3.name));
					break;
				}
			}
			if (this.playerPanel.Contains(x, y))
			{
				this.playerPanelTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (this.playerPanelTimer <= 0)
				{
					this.playerPanelIndex = (this.playerPanelIndex + 1) % 4;
					this.playerPanelTimer = 150;
					return;
				}
			}
			else
			{
				this.playerPanelIndex = 0;
			}
		}

		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			int num = this.xPositionOnScreen + Game1.tileSize - Game1.pixelZoom * 3;
			int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
			b.Draw((Game1.timeOfDay >= 1900) ? Game1.nightbg : Game1.daybg, new Vector2((float)num, (float)num2), Color.White);
			Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], 0, false, false, null, false), Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, Game1.player.bathingClothes ? 576 : 0, 16, 32), new Vector2((float)(num + Game1.tileSize / 2), (float)(num2 + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.White, 0f, 1f, Game1.player);
			if (Game1.timeOfDay >= 1900)
			{
				Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(this.playerPanelFrames[this.playerPanelIndex], 0, false, false, null, false), this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, 0, 16, 32), new Vector2((float)(num + Game1.tileSize / 2), (float)(num2 + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.DarkBlue * 0.3f, 0f, 1f, Game1.player);
			}
			b.DrawString(Game1.smallFont, Game1.player.name, new Vector2((float)(num + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.name).X / 2f, (float)(num2 + 3 * Game1.tileSize + 4)), Game1.textColor);
			b.DrawString(Game1.smallFont, Game1.player.getTitle(), new Vector2((float)(num + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.getTitle()).X / 2f, (float)(num2 + 4 * Game1.tileSize - Game1.tileSize / 2)), Game1.textColor);
			num = ((LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? (this.xPositionOnScreen + this.width - Game1.tileSize * 7 - Game1.tileSize * 3 / 4) : (this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8));
			num2 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 2;
			int num3 = 0;
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					bool flag = false;
					bool flag2 = false;
					string text = "";
					int num4 = 0;
					Rectangle empty = Rectangle.Empty;
					switch (j)
					{
					case 0:
						flag = (Game1.player.FarmingLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11604", new object[0]);
						}
						num4 = Game1.player.FarmingLevel;
						flag2 = (Game1.player.addedFarmingLevel > 0);
						empty = new Rectangle(10, 428, 10, 10);
						break;
					case 1:
						flag = (Game1.player.MiningLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11605", new object[0]);
						}
						num4 = Game1.player.MiningLevel;
						flag2 = (Game1.player.addedMiningLevel > 0);
						empty = new Rectangle(30, 428, 10, 10);
						break;
					case 2:
						flag = (Game1.player.ForagingLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11606", new object[0]);
						}
						num4 = Game1.player.ForagingLevel;
						flag2 = (Game1.player.addedForagingLevel > 0);
						empty = new Rectangle(60, 428, 10, 10);
						break;
					case 3:
						flag = (Game1.player.FishingLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11607", new object[0]);
						}
						num4 = Game1.player.FishingLevel;
						flag2 = (Game1.player.addedFishingLevel > 0);
						empty = new Rectangle(20, 428, 10, 10);
						break;
					case 4:
						flag = (Game1.player.CombatLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11608", new object[0]);
						}
						num4 = Game1.player.CombatLevel;
						flag2 = (Game1.player.addedCombatLevel > 0);
						empty = new Rectangle(120, 428, 10, 10);
						break;
					case 5:
						flag = (Game1.player.LuckLevel > i);
						if (i == 0)
						{
							text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11609", new object[0]);
						}
						num4 = Game1.player.LuckLevel;
						flag2 = (Game1.player.addedLuckLevel > 0);
						empty = new Rectangle(50, 428, 10, 10);
						break;
					}
					if (!text.Equals(""))
					{
						b.DrawString(Game1.smallFont, text, new Vector2((float)num - Game1.smallFont.MeasureString(text).X + (float)Game1.pixelZoom - (float)Game1.tileSize, (float)(num2 + Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Game1.textColor);
						b.Draw(Game1.mouseCursors, new Vector2((float)(num - Game1.pixelZoom * 14), (float)(num2 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(empty), Color.Black * 0.3f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(num - Game1.pixelZoom * 13), (float)(num2 - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(empty), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					if (!flag && (i + 1) % 5 == 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(num3 + num - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(num2 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145, 338, 14, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(num3 + num + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(num2 - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145 + (flag ? 14 : 0), 338, 14, 9)), Color.White * (flag ? 1f : 0.65f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					else if ((i + 1) % 5 != 0)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(num3 + num - Game1.pixelZoom + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(num2 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.Black * 0.35f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.85f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(num3 + num + i * (Game1.tileSize / 2 + Game1.pixelZoom)), (float)(num2 - Game1.pixelZoom + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129 + (flag ? 8 : 0), 338, 8, 9)), Color.White * (flag ? 1f : 0.65f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
					}
					if (i == 9)
					{
						NumberSprite.draw(num4, b, new Vector2((float)(num3 + num + (i + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 3 + ((num4 >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(num2 + Game1.pixelZoom * 4 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Color.Black * 0.35f, 1f, 0.85f, 1f, 0, 0);
						NumberSprite.draw(num4, b, new Vector2((float)(num3 + num + (i + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 4 + ((num4 >= 10) ? (Game1.pixelZoom * 3) : 0)), (float)(num2 + Game1.pixelZoom * 3 + j * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), (flag2 ? Color.LightGreen : Color.SandyBrown) * ((num4 == 0) ? 0.75f : 1f), 1f, 0.87f, 1f, 0, 0);
					}
				}
				if ((i + 1) % 5 == 0)
				{
					num3 += Game1.pixelZoom * 6;
				}
			}
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.skillBars.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			foreach (ClickableTextureComponent current in this.skillBars)
			{
				if (current.scale == 0f)
				{
					IClickableMenu.drawTextureBox(b, current.bounds.X - Game1.tileSize / 4 - Game1.pixelZoom * 2, current.bounds.Y - Game1.tileSize / 4 - Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Color.White);
					b.Draw(Game1.mouseCursors, new Vector2((float)(current.bounds.X - Game1.pixelZoom * 2), (float)(current.bounds.Y - Game1.tileSize / 2 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionImage % 6 * 16, 624 + this.professionImage / 6 * 16, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
			Game1.drawDialogueBox(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)this.height / 2f) - Game1.tileSize / 2, this.width - Game1.tileSize - IClickableMenu.spaceToClearSideBorder * 2, this.height / 4 + Game1.tileSize, false, true, null, false);
			base.drawBorderLabel(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11610", new object[0]), Game1.smallFont, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 3 / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int)((float)this.height / 2f) - Game1.tileSize / 2);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.specialItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			if (this.hoverText.Length > 0)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (this.hoverTitle.Length > 0) ? this.hoverTitle : null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
