using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class CollectionsPage : IClickableMenu
	{
		public const int region_sideTabShipped = 7001;

		public const int region_sideTabFish = 7002;

		public const int region_sideTabArtifacts = 7003;

		public const int region_sideTabMinerals = 7004;

		public const int region_sideTabCooking = 7005;

		public const int region_sideTabAchivements = 7006;

		public const int region_forwardButton = 707;

		public const int region_backButton = 706;

		public static int widthToMoveActiveTab = Game1.tileSize / 8;

		public const int organicsTab = 0;

		public const int fishTab = 1;

		public const int archaeologyTab = 2;

		public const int mineralsTab = 3;

		public const int cookingTab = 4;

		public const int achievementsTab = 5;

		public const int distanceFromMenuBottomBeforeNewPage = 128;

		private string descriptionText = "";

		private string hoverText = "";

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent forwardButton;

		public List<ClickableTextureComponent> sideTabs = new List<ClickableTextureComponent>();

		private int currentTab;

		private int currentPage;

		public Dictionary<int, List<List<ClickableTextureComponent>>> collections = new Dictionary<int, List<List<ClickableTextureComponent>>>();

		private int value;

		public CollectionsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4 + CollectionsPage.widthToMoveActiveTab, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Shipped", new object[0]), Game1.mouseCursors, new Rectangle(640, 80, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7001,
				downNeighborID = 7002,
				rightNeighborID = 0
			});
			this.collections.Add(0, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Fish", new object[0]), Game1.mouseCursors, new Rectangle(640, 64, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7002,
				upNeighborID = 7001,
				downNeighborID = 7003,
				rightNeighborID = 0
			});
			this.collections.Add(1, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 4, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Artifacts", new object[0]), Game1.mouseCursors, new Rectangle(656, 64, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7003,
				upNeighborID = 7002,
				downNeighborID = 7004,
				rightNeighborID = 0
			});
			this.collections.Add(2, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 5, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Minerals", new object[0]), Game1.mouseCursors, new Rectangle(672, 64, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7004,
				upNeighborID = 7003,
				downNeighborID = 7005,
				rightNeighborID = 0
			});
			this.collections.Add(3, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 6, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Cooking", new object[0]), Game1.mouseCursors, new Rectangle(688, 64, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7005,
				upNeighborID = 7004,
				downNeighborID = 7006,
				rightNeighborID = 0
			});
			this.collections.Add(4, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 7, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Achievements", new object[0]), Game1.mouseCursors, new Rectangle(656, 80, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 7006,
				upNeighborID = 7005,
				rightNeighborID = 0
			});
			this.collections.Add(5, new List<List<ClickableTextureComponent>>());
			CollectionsPage.widthToMoveActiveTab = Game1.tileSize / 8;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 706,
				rightNeighborID = -7777
			};
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2 - 15 * Game1.pixelZoom, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				myID = 707,
				leftNeighborID = -7777
			};
			int[] array = new int[this.sideTabs.Count];
			int num = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder;
			int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4;
			int num3 = 10;
			foreach (KeyValuePair<int, string> current in Game1.objectInformation)
			{
				string text = current.Value.Split(new char[]
				{
					'/'
				})[3];
				bool drawShadow = false;
				int num4;
				if (text.Contains("Arch"))
				{
					num4 = 2;
					if (Game1.player.archaeologyFound.ContainsKey(current.Key))
					{
						drawShadow = true;
					}
				}
				else if (text.Contains("Fish"))
				{
					if (current.Key >= 167 && current.Key < 173)
					{
						continue;
					}
					num4 = 1;
					if (Game1.player.fishCaught.ContainsKey(current.Key))
					{
						drawShadow = true;
					}
				}
				else if (text.Contains("Mineral") || text.Substring(text.Length - 3).Equals("-2"))
				{
					num4 = 3;
					if (Game1.player.mineralsFound.ContainsKey(current.Key))
					{
						drawShadow = true;
					}
				}
				else if (text.Contains("Cooking") || text.Substring(text.Length - 3).Equals("-7"))
				{
					num4 = 4;
					if (Game1.player.recipesCooked.ContainsKey(current.Key))
					{
						drawShadow = true;
					}
					if (current.Key == 217 || current.Key == 772)
					{
						continue;
					}
					if (current.Key == 773)
					{
						continue;
					}
				}
				else
				{
					if (!StardewValley.Object.isPotentialBasicShippedCategory(current.Key, text.Substring(text.Length - 3)))
					{
						continue;
					}
					num4 = 0;
					if (Game1.player.basicShipped.ContainsKey(current.Key))
					{
						drawShadow = true;
					}
				}
				int x2 = num + array[num4] % num3 * (Game1.tileSize + 4);
				int num5 = num2 + array[num4] / num3 * (Game1.tileSize + 4);
				if (num5 > this.yPositionOnScreen + height - 128)
				{
					this.collections[num4].Add(new List<ClickableTextureComponent>());
					array[num4] = 0;
					x2 = num;
					num5 = num2;
				}
				if (this.collections[num4].Count == 0)
				{
					this.collections[num4].Add(new List<ClickableTextureComponent>());
				}
				this.collections[num4].Last<List<ClickableTextureComponent>>().Add(new ClickableTextureComponent(current.Key + " " + drawShadow.ToString(), new Rectangle(x2, num5, Game1.tileSize, Game1.tileSize), null, "", Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, current.Key, 16, 16), (float)Game1.pixelZoom, drawShadow)
				{
					myID = this.collections[num4].Last<List<ClickableTextureComponent>>().Count,
					rightNeighborID = (((this.collections[num4].Last<List<ClickableTextureComponent>>().Count + 1) % num3 == 0) ? -1 : (this.collections[num4].Last<List<ClickableTextureComponent>>().Count + 1)),
					leftNeighborID = ((this.collections[num4].Last<List<ClickableTextureComponent>>().Count % num3 == 0) ? 7001 : (this.collections[num4].Last<List<ClickableTextureComponent>>().Count - 1)),
					downNeighborID = ((num5 + (Game1.tileSize + 4) > this.yPositionOnScreen + height - 128) ? -7777 : (this.collections[num4].Last<List<ClickableTextureComponent>>().Count + num3)),
					upNeighborID = ((this.collections[num4].Last<List<ClickableTextureComponent>>().Count < num3) ? 12345 : (this.collections[num4].Last<List<ClickableTextureComponent>>().Count - num3)),
					fullyImmutable = true
				});
				array[num4]++;
			}
			if (this.collections[5].Count == 0)
			{
				this.collections[5].Add(new List<ClickableTextureComponent>());
			}
			foreach (KeyValuePair<int, string> current2 in Game1.achievements)
			{
				bool flag = Game1.player.achievements.Contains(current2.Key);
				string[] array2 = current2.Value.Split(new char[]
				{
					'^'
				});
				if (flag || (array2[2].Equals("true") && (array2[3].Equals("-1") || this.farmerHasAchievements(array2[3]))))
				{
					int x3 = num + array[5] % num3 * (Game1.tileSize + 4);
					int y2 = num2 + array[5] / num3 * (Game1.tileSize + 4);
					this.collections[5][0].Add(new ClickableTextureComponent(current2.Key + " " + flag.ToString(), new Rectangle(x3, y2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 25, -1, -1), 1f, false));
					array[5]++;
				}
			}
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			base.customSnapBehavior(direction, oldRegion, oldID);
			if (direction == 2)
			{
				if (this.currentPage > 0)
				{
					this.currentlySnappedComponent = base.getComponentWithID(706);
				}
				else if (this.currentPage == 0 && this.collections[this.currentTab].Count > 1)
				{
					this.currentlySnappedComponent = base.getComponentWithID(707);
				}
				this.backButton.upNeighborID = oldID;
				this.forwardButton.upNeighborID = oldID;
				return;
			}
			if (direction == 3)
			{
				if (oldID == 707 && this.currentPage > 0)
				{
					this.currentlySnappedComponent = base.getComponentWithID(706);
					return;
				}
			}
			else if (direction == 1 && oldID == 706 && this.collections[this.currentTab].Count > this.currentPage + 1)
			{
				this.currentlySnappedComponent = base.getComponentWithID(707);
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			base.snapToDefaultClickableComponent();
			this.currentlySnappedComponent = base.getComponentWithID(0);
			this.snapCursorToCurrentSnappedComponent();
		}

		private bool farmerHasAchievements(string listOfAchievementNumbers)
		{
			string[] array = listOfAchievementNumbers.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (!Game1.player.achievements.Contains(Convert.ToInt32(text)))
				{
					return false;
				}
			}
			return true;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			for (int i = 0; i < this.sideTabs.Count; i++)
			{
				if (this.sideTabs[i].containsPoint(x, y) && this.currentTab != i)
				{
					Game1.playSound("smallSelect");
					ClickableTextureComponent expr_47_cp_0_cp_0 = this.sideTabs[this.currentTab];
					expr_47_cp_0_cp_0.bounds.X = expr_47_cp_0_cp_0.bounds.X - CollectionsPage.widthToMoveActiveTab;
					this.currentTab = i;
					this.currentPage = 0;
					ClickableTextureComponent expr_74_cp_0_cp_0 = this.sideTabs[i];
					expr_74_cp_0_cp_0.bounds.X = expr_74_cp_0_cp_0.bounds.X + CollectionsPage.widthToMoveActiveTab;
				}
			}
			if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
			{
				this.currentPage--;
				Game1.playSound("shwip");
				this.backButton.scale = this.backButton.baseScale;
				if (Game1.options.snappyMenus && Game1.options.gamepadControls && this.currentPage == 0)
				{
					this.currentlySnappedComponent = this.forwardButton;
					Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
				}
			}
			if (this.currentPage < this.collections[this.currentTab].Count - 1 && this.forwardButton.containsPoint(x, y))
			{
				this.currentPage++;
				Game1.playSound("shwip");
				this.forwardButton.scale = this.forwardButton.baseScale;
				if (Game1.options.snappyMenus && Game1.options.gamepadControls && this.currentPage == this.collections[this.currentTab].Count - 1)
				{
					this.currentlySnappedComponent = this.backButton;
					Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			this.value = -1;
			foreach (ClickableTextureComponent current in this.sideTabs)
			{
				if (current.containsPoint(x, y))
				{
					this.hoverText = current.hoverText;
					return;
				}
			}
			foreach (ClickableTextureComponent current2 in this.collections[this.currentTab][this.currentPage])
			{
				if (current2.containsPoint(x, y))
				{
					current2.scale = Math.Min(current2.scale + 0.02f, current2.baseScale + 0.1f);
					if (Convert.ToBoolean(current2.name.Split(new char[]
					{
						' '
					})[1]) || this.currentTab == 5)
					{
						this.hoverText = this.createDescription(Convert.ToInt32(current2.name.Split(new char[]
						{
							' '
						})[0]));
					}
					else
					{
						this.hoverText = "???";
					}
				}
				else
				{
					current2.scale = Math.Max(current2.scale - 0.02f, current2.baseScale);
				}
			}
			this.forwardButton.tryHover(x, y, 0.5f);
			this.backButton.tryHover(x, y, 0.5f);
		}

		public string createDescription(int index)
		{
			string text = "";
			if (this.currentTab == 5)
			{
				string[] array = Game1.achievements[index].Split(new char[]
				{
					'^'
				});
				text = text + array[0] + Environment.NewLine + Environment.NewLine;
				text += array[1];
			}
			else
			{
				string[] array2 = Game1.objectInformation[index].Split(new char[]
				{
					'/'
				});
				string text2 = array2[4];
				text = string.Concat(new string[]
				{
					text,
					text2,
					Environment.NewLine,
					Environment.NewLine,
					Game1.parseText(array2[5], Game1.smallFont, Game1.tileSize * 4),
					Environment.NewLine,
					Environment.NewLine
				});
				if (array2[3].Contains("Arch"))
				{
					text += (Game1.player.archaeologyFound.ContainsKey(index) ? Game1.content.LoadString("Strings\\UI:Collections_Description_ArtifactsFound", new object[]
					{
						Game1.player.archaeologyFound[index][0]
					}) : "");
				}
				else if (array2[3].Contains("Cooking"))
				{
					text += (Game1.player.recipesCooked.ContainsKey(index) ? Game1.content.LoadString("Strings\\UI:Collections_Description_RecipesCooked", new object[]
					{
						Game1.player.recipesCooked[index]
					}) : "");
				}
				else if (array2[3].Contains("Fish"))
				{
					text += Game1.content.LoadString("Strings\\UI:Collections_Description_FishCaught", new object[]
					{
						Game1.player.fishCaught.ContainsKey(index) ? Game1.player.fishCaught[index][0] : 0
					});
					if (Game1.player.fishCaught.ContainsKey(index) && Game1.player.fishCaught[index][1] > 0)
					{
						text = text + Environment.NewLine + Game1.content.LoadString("Strings\\UI:Collections_Description_BiggestCatch", new object[]
						{
							(LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en) ? Math.Round((double)Game1.player.fishCaught[index][1] * 2.54) : ((double)Game1.player.fishCaught[index][1])
						});
					}
				}
				else if (array2[3].Contains("Minerals") || array2[3].Substring(array2[3].Length - 3).Equals("-2"))
				{
					text += Game1.content.LoadString("Strings\\UI:Collections_Description_MineralsFound", new object[]
					{
						Game1.player.mineralsFound.ContainsKey(index) ? Game1.player.mineralsFound[index] : 0
					});
				}
				else
				{
					text += Game1.content.LoadString("Strings\\UI:Collections_Description_NumberShipped", new object[]
					{
						Game1.player.basicShipped.ContainsKey(index) ? Game1.player.basicShipped[index] : 0
					});
				}
				this.value = Convert.ToInt32(array2[1]);
			}
			return text;
		}

		public override void draw(SpriteBatch b)
		{
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.sideTabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			if (this.currentPage > 0)
			{
				this.backButton.draw(b);
			}
			if (this.currentPage < this.collections[this.currentTab].Count - 1)
			{
				this.forwardButton.draw(b);
			}
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
			foreach (ClickableTextureComponent current in this.collections[this.currentTab][this.currentPage])
			{
				bool flag = Convert.ToBoolean(current.name.Split(new char[]
				{
					' '
				})[1]);
				current.draw(b, flag ? Color.White : (Color.Black * 0.2f), 0.86f);
				if (this.currentTab == 5 & flag)
				{
					int num = new Random(Convert.ToInt32(current.name.Split(new char[]
					{
						' '
					})[0])).Next(12);
					b.Draw(Game1.mouseCursors, new Vector2((float)(current.bounds.X + 16 + Game1.tileSize / 4), (float)(current.bounds.Y + 20 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(256 + num % 6 * Game1.tileSize / 2, 128 + num / 6 * Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), current.scale, SpriteEffects.None, 0.88f);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, this.value, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
