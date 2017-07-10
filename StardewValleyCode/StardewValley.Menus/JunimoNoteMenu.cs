using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class JunimoNoteMenu : IClickableMenu
	{
		public const int region_ingredientSlotModifier = 250;

		public const int region_bundleModifier = 505;

		public const int region_areaNextButton = 101;

		public const int region_areaBackButton = 102;

		public const int region_backButton = 103;

		public const int region_purchaseButton = 104;

		public const int region_presentButton = 105;

		public Texture2D noteTexture;

		private bool specificBundlePage;

		public const int baseWidth = 320;

		public const int baseHeight = 180;

		public InventoryMenu inventory;

		private Item heldItem;

		private Item hoveredItem;

		public static bool canClick = true;

		private int whichArea;

		public static ScreenSwipe screenSwipe;

		public static string hoverText = "";

		public List<Bundle> bundles = new List<Bundle>();

		public static List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();

		public List<ClickableTextureComponent> ingredientSlots = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> ingredientList = new List<ClickableTextureComponent>();

		public List<ClickableTextureComponent> otherClickableComponents = new List<ClickableTextureComponent>();

		public bool fromGameMenu;

		public bool scrambledText;

		public ClickableTextureComponent backButton;

		public ClickableTextureComponent purchaseButton;

		public ClickableTextureComponent areaNextButton;

		public ClickableTextureComponent areaBackButton;

		public ClickableAnimatedComponent presentButton;

		private Bundle currentPageBundle;

		public JunimoNoteMenu(bool fromGameMenu, int area = 1, bool fromThisMenu = false) : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			CommunityCenter communityCenter = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
			if (fromGameMenu && !fromThisMenu)
			{
				for (int i = 0; i < communityCenter.areasComplete.Length; i++)
				{
					if (communityCenter.shouldNoteAppearInArea(i) && !communityCenter.areasComplete[i])
					{
						area = i;
						this.whichArea = area;
						break;
					}
				}
			}
			this.setUpMenu(area, communityCenter.bundles);
			Game1.player.forceCanMove();
			this.areaNextButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				visible = false,
				myID = 101,
				leftNeighborID = 102
			};
			this.areaBackButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				visible = false,
				myID = 102,
				rightNeighborID = 101
			};
			for (int j = 0; j < 6; j++)
			{
				if (communityCenter.shouldNoteAppearInArea((area + j) % 6))
				{
					this.areaNextButton.visible = true;
				}
			}
			for (int k = 0; k < 6; k++)
			{
				int num = area - k;
				if (num == -1)
				{
					num = 5;
				}
				if (communityCenter.shouldNoteAppearInArea(num))
				{
					this.areaBackButton.visible = true;
				}
			}
			this.fromGameMenu = fromGameMenu;
			using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.depositsAllowed = false;
				}
			}
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public JunimoNoteMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete) : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			this.setUpMenu(whichArea, bundlesComplete);
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(505);
			this.snapCursorToCurrentSnappedComponent();
		}

		protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
		{
			if (oldID - 505 >= 0 && oldID - 505 < 10 && this.currentlySnappedComponent != null)
			{
				int num = -1;
				int num2 = 999999;
				Point center = this.currentlySnappedComponent.bounds.Center;
				for (int i = 0; i < this.bundles.Count; i++)
				{
					if (this.bundles[i].myID != oldID)
					{
						int num3 = 999999;
						Point center2 = this.bundles[i].bounds.Center;
						switch (direction)
						{
						case 0:
							if (center2.Y < center.Y)
							{
								num3 = center.Y - center2.Y + Math.Abs(center.X - center2.X) * 3;
							}
							break;
						case 1:
							if (center2.X > center.X)
							{
								num3 = center2.X - center.X + Math.Abs(center.Y - center2.Y) * 3;
							}
							break;
						case 2:
							if (center2.Y > center.Y)
							{
								num3 = center2.Y - center.Y + Math.Abs(center.X - center2.X) * 3;
							}
							break;
						case 3:
							if (center2.X < center.X)
							{
								num3 = center.X - center2.X + Math.Abs(center.Y - center2.Y) * 3;
							}
							break;
						}
						if (num3 < 10000 && num3 < num2)
						{
							num2 = num3;
							num = i;
						}
					}
				}
				if (num != -1)
				{
					this.currentlySnappedComponent = base.getComponentWithID(num + 505);
					this.snapCursorToCurrentSnappedComponent();
					return;
				}
				switch (direction)
				{
				case 1:
					if (this.areaNextButton != null)
					{
						this.currentlySnappedComponent = this.areaNextButton;
						this.snapCursorToCurrentSnappedComponent();
						this.areaNextButton.leftNeighborID = oldID;
					}
					break;
				case 2:
					if (this.presentButton != null)
					{
						this.currentlySnappedComponent = this.presentButton;
						this.snapCursorToCurrentSnappedComponent();
						this.presentButton.upNeighborID = oldID;
						return;
					}
					break;
				case 3:
					if (this.areaBackButton != null)
					{
						this.currentlySnappedComponent = this.areaBackButton;
						this.snapCursorToCurrentSnappedComponent();
						this.areaBackButton.rightNeighborID = oldID;
						return;
					}
					break;
				default:
					return;
				}
			}
		}

		public void setUpMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete)
		{
			this.noteTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\JunimoNote");
			if (!Game1.player.hasOrWillReceiveMail("seenJunimoNote"))
			{
				Game1.player.removeQuest(26);
				Game1.player.mailReceived.Add("seenJunimoNote");
			}
			if (!Game1.player.hasOrWillReceiveMail("wizardJunimoNote"))
			{
				Game1.addMailForTomorrow("wizardJunimoNote", false, false);
			}
			this.scrambledText = !Game1.player.hasOrWillReceiveMail("canReadJunimoText");
			JunimoNoteMenu.tempSprites.Clear();
			this.whichArea = whichArea;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), 36, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false)
			{
				capacity = 36
			};
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			string areaNameFromNumber = CommunityCenter.getAreaNameFromNumber(whichArea);
			int num = 0;
			foreach (string current in dictionary.Keys)
			{
				if (current.Contains(areaNameFromNumber))
				{
					int num2 = Convert.ToInt32(current.Split(new char[]
					{
						'/'
					})[1]);
					this.bundles.Add(new Bundle(num2, dictionary[current], bundlesComplete[num2], this.getBundleLocationFromNumber(num), this.noteTexture, this)
					{
						myID = num + 505,
						rightNeighborID = -7777,
						leftNeighborID = -7777,
						upNeighborID = -7777,
						downNeighborID = -7777,
						fullyImmutable = true
					});
					num++;
				}
			}
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 103
			};
			this.checkForRewards();
			JunimoNoteMenu.canClick = true;
			Game1.playSound("shwip");
			bool flag = false;
			foreach (Bundle current2 in this.bundles)
			{
				if (!current2.complete && !current2.Equals(this.currentPageBundle))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[whichArea] = true;
				this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(whichArea);
			}
		}

		public override bool readyToClose()
		{
			return this.heldItem == null && !this.specificBundlePage;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!JunimoNoteMenu.canClick)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (this.scrambledText)
			{
				return;
			}
			if (this.specificBundlePage)
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				if (this.backButton.containsPoint(x, y) && this.heldItem == null)
				{
					this.takeDownBundleSpecificPage(this.currentPageBundle);
					Game1.playSound("shwip");
				}
				if (this.heldItem != null)
				{
					if (Game1.oldKBState.IsKeyDown(Keys.LeftShift))
					{
						for (int i = 0; i < this.ingredientSlots.Count; i++)
						{
							if (this.ingredientSlots[i].item == null)
							{
								this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[i], this.noteTexture);
								this.checkIfBundleIsComplete();
								return;
							}
						}
					}
					for (int j = 0; j < this.ingredientSlots.Count; j++)
					{
						if (this.ingredientSlots[j].containsPoint(x, y))
						{
							this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[j], this.noteTexture);
							this.checkIfBundleIsComplete();
						}
					}
				}
				if (this.purchaseButton != null && this.purchaseButton.containsPoint(x, y))
				{
					int stack = this.currentPageBundle.ingredients.Last<BundleIngredientDescription>().stack;
					if (Game1.player.Money >= stack)
					{
						Game1.player.Money -= stack;
						Game1.playSound("select");
						this.currentPageBundle.completionAnimation(this, true, 0);
						if (this.purchaseButton != null)
						{
							this.purchaseButton.scale = this.purchaseButton.baseScale * 0.75f;
						}
						((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
						(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[this.currentPageBundle.bundleIndex][0] = true;
						this.checkForRewards();
						bool flag = false;
						foreach (Bundle current in this.bundles)
						{
							if (!current.complete && !current.Equals(this.currentPageBundle))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
							this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
							((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
						}
						else
						{
							Junimo junimoForArea = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
							if (junimoForArea != null)
							{
								junimoForArea.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
							}
						}
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 600;
					}
				}
				if (this.upperRightCloseButton != null && !this.readyToClose() && this.upperRightCloseButton.containsPoint(x, y))
				{
					this.closeBundlePage();
				}
			}
			else
			{
				foreach (Bundle current2 in this.bundles)
				{
					if (current2.canBeClicked() && current2.containsPoint(x, y))
					{
						this.setUpBundleSpecificPage(current2);
						Game1.playSound("shwip");
						return;
					}
				}
				if (this.presentButton != null && this.presentButton.containsPoint(x, y))
				{
					this.openRewardsMenu();
				}
				if (this.fromGameMenu)
				{
					CommunityCenter communityCenter = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
					if (this.areaNextButton.containsPoint(x, y))
					{
						for (int k = 1; k < 7; k++)
						{
							if (communityCenter.shouldNoteAppearInArea((this.whichArea + k) % 6))
							{
								Game1.activeClickableMenu = new JunimoNoteMenu(true, (this.whichArea + k) % 6, true);
								return;
							}
						}
					}
					else if (this.areaBackButton.containsPoint(x, y))
					{
						int num = this.whichArea;
						for (int l = 1; l < 7; l++)
						{
							num--;
							if (num == -1)
							{
								num = 5;
							}
							if (communityCenter.shouldNoteAppearInArea(num))
							{
								Game1.activeClickableMenu = new JunimoNoteMenu(true, num, true);
								return;
							}
						}
					}
				}
			}
			if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
			{
				Game1.playSound("throwDownITem");
				Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, null);
				this.heldItem = null;
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (this.fromGameMenu)
			{
				CommunityCenter communityCenter = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
				if (b == Buttons.RightTrigger)
				{
					for (int i = 1; i < 7; i++)
					{
						if (communityCenter.shouldNoteAppearInArea((this.whichArea + i) % 6))
						{
							Game1.activeClickableMenu = new JunimoNoteMenu(true, (this.whichArea + i) % 6, true);
							return;
						}
					}
					return;
				}
				if (b == Buttons.LeftTrigger)
				{
					int num = this.whichArea;
					for (int j = 1; j < 7; j++)
					{
						num--;
						if (num == -1)
						{
							num = 5;
						}
						if (communityCenter.shouldNoteAppearInArea(num))
						{
							Game1.activeClickableMenu = new JunimoNoteMenu(true, num, true);
							return;
						}
					}
				}
			}
		}

		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (key.Equals(Keys.Delete) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && !this.readyToClose())
			{
				this.closeBundlePage();
			}
		}

		private void closeBundlePage()
		{
			if (this.specificBundlePage)
			{
				if (this.heldItem == null)
				{
					this.takeDownBundleSpecificPage(this.currentPageBundle);
					Game1.playSound("shwip");
					return;
				}
				this.heldItem = this.inventory.tryToAddItem(this.heldItem, "coin");
			}
		}

		private void reOpenThisMenu()
		{
			Game1.activeClickableMenu = new JunimoNoteMenu(this.whichArea, ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles);
		}

		private void updateIngredientSlots()
		{
			int num = 0;
			for (int i = 0; i < this.currentPageBundle.ingredients.Count; i++)
			{
				if (this.currentPageBundle.ingredients[i].completed)
				{
					this.ingredientSlots[num].item = new StardewValley.Object(this.currentPageBundle.ingredients[i].index, this.currentPageBundle.ingredients[i].stack, false, -1, this.currentPageBundle.ingredients[i].quality);
					this.currentPageBundle.ingredientDepositAnimation(this.ingredientSlots[num], this.noteTexture, true);
					num++;
				}
			}
		}

		private void openRewardsMenu()
		{
			Game1.playSound("smallSelect");
			List<Item> list = new List<Item>();
			Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			foreach (string current in dictionary.Keys)
			{
				if (current.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
				{
					int num = Convert.ToInt32(current.Split(new char[]
					{
						'/'
					})[1]);
					if (((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[num])
					{
						Item itemFromStandardTextDescription = Utility.getItemFromStandardTextDescription(dictionary[current].Split(new char[]
						{
							'/'
						})[1], Game1.player, ' ');
						itemFromStandardTextDescription.specialVariable = num;
						list.Add(itemFromStandardTextDescription);
					}
				}
			}
			Game1.activeClickableMenu = new ItemGrabMenu(list, false, true, null, null, null, new ItemGrabMenu.behaviorOnItemSelect(this.rewardGrabbed), false, false, true, true, false, 0, null, -1, null);
			Game1.activeClickableMenu.exitFunction = ((this.exitFunction != null) ? this.exitFunction : new IClickableMenu.onExit(this.reOpenThisMenu));
		}

		private void rewardGrabbed(Item item, Farmer who)
		{
			((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[item.specialVariable] = false;
		}

		private void checkIfBundleIsComplete()
		{
			if (!this.specificBundlePage || this.currentPageBundle == null)
			{
				return;
			}
			int num = 0;
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.ingredientSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item != null)
					{
						num++;
					}
				}
			}
			if (num >= this.currentPageBundle.numberOfIngredientSlots)
			{
				if (this.heldItem != null)
				{
					Game1.player.addItemToInventory(this.heldItem);
					this.heldItem = null;
				}
				for (int i = 0; i < ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex].Length; i++)
				{
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex][i] = true;
				}
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).checkForNewJunimoNotes();
				JunimoNoteMenu.screenSwipe = new ScreenSwipe(0, -1f, -1);
				this.currentPageBundle.completionAnimation(this, true, 400);
				JunimoNoteMenu.canClick = false;
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
				bool flag = false;
				foreach (Bundle current in this.bundles)
				{
					if (!current.complete && !current.Equals(this.currentPageBundle))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
					this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
				}
				else
				{
					Junimo junimoForArea = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
					if (junimoForArea != null)
					{
						junimoForArea.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
					}
				}
				this.checkForRewards();
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.sendMessageToEveryone(6, string.Concat(this.currentPageBundle.bundleIndex), Game1.player.uniqueMultiplayerID);
				}
			}
		}

		private void restoreAreaOnExit()
		{
			if (!this.fromGameMenu)
			{
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).restoreAreaCutscene(this.whichArea);
			}
		}

		public void checkForRewards()
		{
			foreach (string current in Game1.content.Load<Dictionary<string, string>>("Data\\Bundles").Keys)
			{
				if (current.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
				{
					int key = Convert.ToInt32(current.Split(new char[]
					{
						'/'
					})[1]);
					if (((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[key])
					{
						this.presentButton = new ClickableAnimatedComponent(new Rectangle(this.xPositionOnScreen + 148 * Game1.pixelZoom, this.yPositionOnScreen + 128 * Game1.pixelZoom, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10783", new object[0]), new TemporaryAnimatedSprite(this.noteTexture, new Rectangle(548, 262, 18, 20), 70f, 4, 99999, new Vector2((float)(-(float)Game1.tileSize), (float)(-(float)Game1.tileSize)), false, false, 0.5f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true));
						break;
					}
				}
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (!JunimoNoteMenu.canClick)
			{
				return;
			}
			if (this.specificBundlePage)
			{
				this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
			}
			if (!this.specificBundlePage && this.readyToClose())
			{
				base.exitThisMenu(true);
			}
		}

		public override void update(GameTime time)
		{
			using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.update(time);
				}
			}
			for (int i = JunimoNoteMenu.tempSprites.Count - 1; i >= 0; i--)
			{
				if (JunimoNoteMenu.tempSprites[i].update(time))
				{
					JunimoNoteMenu.tempSprites.RemoveAt(i);
				}
			}
			if (this.presentButton != null)
			{
				this.presentButton.update(time);
			}
			if (JunimoNoteMenu.screenSwipe != null)
			{
				JunimoNoteMenu.canClick = false;
				if (JunimoNoteMenu.screenSwipe.update(time))
				{
					JunimoNoteMenu.screenSwipe = null;
					JunimoNoteMenu.canClick = true;
				}
			}
		}

		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			if (this.scrambledText)
			{
				return;
			}
			JunimoNoteMenu.hoverText = "";
			if (this.specificBundlePage)
			{
				this.backButton.tryHover(x, y, 0.1f);
				this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
				foreach (ClickableTextureComponent current in this.ingredientList)
				{
					if (current.bounds.Contains(x, y))
					{
						JunimoNoteMenu.hoverText = current.hoverText;
						break;
					}
				}
				if (this.heldItem != null)
				{
					foreach (ClickableTextureComponent current2 in this.ingredientSlots)
					{
						if (current2.bounds.Contains(x, y) && this.currentPageBundle.canAcceptThisItem(this.heldItem, current2))
						{
							current2.sourceRect.X = 530;
							current2.sourceRect.Y = 262;
						}
						else
						{
							current2.sourceRect.X = 512;
							current2.sourceRect.Y = 244;
						}
					}
				}
				if (this.purchaseButton != null)
				{
					this.purchaseButton.tryHover(x, y, 0.1f);
					return;
				}
			}
			else
			{
				if (this.presentButton != null)
				{
					JunimoNoteMenu.hoverText = this.presentButton.tryHover(x, y);
				}
				using (List<Bundle>.Enumerator enumerator2 = this.bundles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.tryHoverAction(x, y);
					}
				}
				if (this.fromGameMenu)
				{
					Game1.getLocationFromName("CommunityCenter");
					this.areaNextButton.tryHover(x, y, 0.1f);
					this.areaBackButton.tryHover(x, y, 0.1f);
				}
			}
		}

		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			if (!this.specificBundlePage)
			{
				b.Draw(this.noteTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
				SpriteText.drawStringHorizontallyCenteredAt(b, this.scrambledText ? CommunityCenter.getAreaEnglishDisplayNameFromNumber(this.whichArea) : CommunityCenter.getAreaDisplayNameFromNumber(this.whichArea), this.xPositionOnScreen + this.width / 2 + Game1.pixelZoom * 4, this.yPositionOnScreen + Game1.pixelZoom * 3, 999999, -1, 99999, 0.88f, 0.88f, this.scrambledText, -1);
				if (this.scrambledText)
				{
					SpriteText.drawString(b, LocalizedContentManager.CurrentLanguageLatin ? Game1.content.LoadString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10786", new object[0]) : Game1.content.LoadBaseString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10786", new object[0]), this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen + Game1.tileSize * 3 / 2, 999999, this.width - Game1.tileSize * 3, 99999, 0.88f, 0.88f, true, -1, "", -1);
					base.draw(b);
					if (JunimoNoteMenu.canClick)
					{
						base.drawMouse(b);
					}
					return;
				}
				using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				if (this.presentButton != null)
				{
					this.presentButton.draw(b);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = JunimoNoteMenu.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, true, 0, 0);
					}
				}
				if (this.fromGameMenu)
				{
					if (this.areaNextButton.visible)
					{
						this.areaNextButton.draw(b);
					}
					if (this.areaBackButton.visible)
					{
						this.areaBackButton.draw(b);
					}
				}
			}
			else
			{
				b.Draw(this.noteTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(320, 0, 320, 180)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
				if (this.currentPageBundle != null)
				{
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 218 * Game1.pixelZoom), (float)(this.yPositionOnScreen + 22 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.currentPageBundle.bundleIndex * 16 * 2 % this.noteTexture.Width, 180 + 32 * (this.currentPageBundle.bundleIndex * 16 * 2 / this.noteTexture.Width), 32, 32)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.15f);
					float x = Game1.dialogueFont.MeasureString((!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					})).X;
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom - (int)x / 2 - Game1.pixelZoom * 4), (float)(this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(517, 266, 4, 17)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					b.Draw(this.noteTexture, new Rectangle(this.xPositionOnScreen + 234 * Game1.pixelZoom - (int)x / 2, this.yPositionOnScreen + 57 * Game1.pixelZoom, (int)x, 17 * Game1.pixelZoom), new Rectangle?(new Rectangle(520, 266, 1, 17)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom + (int)x / 2), (float)(this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(524, 266, 4, 17)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					b.DrawString(Game1.dialogueFont, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					}), new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float)(this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(2f, 2f), Game1.textShadowColor);
					b.DrawString(Game1.dialogueFont, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					}), new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float)(this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(0f, 2f), Game1.textShadowColor);
					b.DrawString(Game1.dialogueFont, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					}), new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float)(this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(2f, 0f), Game1.textShadowColor);
					b.DrawString(Game1.dialogueFont, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					}), new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float)(this.yPositionOnScreen + 61 * Game1.pixelZoom)), Game1.textColor * 0.9f);
				}
				this.backButton.draw(b);
				if (this.purchaseButton != null)
				{
					this.purchaseButton.draw(b);
					Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = JunimoNoteMenu.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, true, 0, 0);
					}
				}
				foreach (ClickableTextureComponent current in this.ingredientSlots)
				{
					if (current.item == null)
					{
						current.draw(b, this.fromGameMenu ? (Color.LightGray * 0.5f) : Color.White, 0.89f);
					}
					current.drawItem(b, Game1.pixelZoom, Game1.pixelZoom);
				}
				foreach (ClickableTextureComponent current2 in this.ingredientList)
				{
					b.Draw(Game1.shadowTexture, new Vector2((float)(current2.bounds.Center.X - Game1.shadowTexture.Bounds.Width * Game1.pixelZoom / 2 - Game1.pixelZoom), (float)(current2.bounds.Center.Y + Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					current2.drawItem(b, 0, 0);
				}
				this.inventory.draw(b);
			}
			SpriteText.drawStringWithScrollCenteredAt(b, this.getRewardNameForArea(this.whichArea), this.xPositionOnScreen + this.width / 2, Math.Min(this.yPositionOnScreen + this.height + Game1.pixelZoom * 5, Game1.viewport.Height - Game1.tileSize - Game1.pixelZoom * 2), "", 1f, -1, 0, 0.88f, false);
			base.draw(b);
			Game1.mouseCursorTransparency = 1f;
			if (JunimoNoteMenu.canClick)
			{
				base.drawMouse(b);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 16), (float)(Game1.getOldMouseY() + 16)), 1f);
			}
			if (this.inventory.descriptionText.Length > 0)
			{
				if (this.hoveredItem != null)
				{
					IClickableMenu.drawToolTip(b, this.hoveredItem.getDescription(), this.hoveredItem.DisplayName, this.hoveredItem, false, -1, 0, -1, -1, null, -1);
				}
			}
			else
			{
				IClickableMenu.drawHoverText(b, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText") && JunimoNoteMenu.hoverText.Length > 0) ? "???" : JunimoNoteMenu.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (JunimoNoteMenu.screenSwipe != null)
			{
				JunimoNoteMenu.screenSwipe.draw(b);
			}
		}

		public string getRewardNameForArea(int whichArea)
		{
			switch (whichArea)
			{
			case 0:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardPantry", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardCrafts", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardFishTank", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBoiler", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardVault", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBulletin", new object[0]);
			default:
				return "???";
			}
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2;
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
			if (this.fromGameMenu)
			{
				this.areaNextButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
				{
					visible = false
				};
				this.areaBackButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
				{
					visible = false
				};
			}
			this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), Game1.player.maxItems, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false);
			for (int i = 0; i < this.bundles.Count; i++)
			{
				Point bundleLocationFromNumber = this.getBundleLocationFromNumber(i);
				this.bundles[i].bounds.X = bundleLocationFromNumber.X;
				this.bundles[i].bounds.Y = bundleLocationFromNumber.Y;
				this.bundles[i].sprite.position = new Vector2((float)bundleLocationFromNumber.X, (float)bundleLocationFromNumber.Y);
			}
			if (this.specificBundlePage)
			{
				int numberOfIngredientSlots = this.currentPageBundle.numberOfIngredientSlots;
				List<Rectangle> list = new List<Rectangle>();
				this.addRectangleRowsToList(list, numberOfIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
				this.ingredientSlots.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					this.ingredientSlots.Add(new ClickableTextureComponent(list[j], this.noteTexture, new Rectangle(512, 244, 18, 18), (float)Game1.pixelZoom, false));
				}
				List<Rectangle> list2 = new List<Rectangle>();
				this.ingredientList.Clear();
				this.addRectangleRowsToList(list2, this.currentPageBundle.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Game1.objectInformation.ContainsKey(this.currentPageBundle.ingredients[k].index))
					{
						this.ingredientList.Add(new ClickableTextureComponent("", list2[k], "", Game1.objectInformation[this.currentPageBundle.ingredients[k].index].Split(new char[]
						{
							'/'
						})[0], Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.currentPageBundle.ingredients[k].index, 16, 16), (float)Game1.pixelZoom, false)
						{
							item = new StardewValley.Object(this.currentPageBundle.ingredients[k].index, this.currentPageBundle.ingredients[k].stack, false, -1, this.currentPageBundle.ingredients[k].quality)
						});
					}
				}
				this.updateIngredientSlots();
			}
		}

		private void setUpBundleSpecificPage(Bundle b)
		{
			JunimoNoteMenu.tempSprites.Clear();
			this.currentPageBundle = b;
			this.specificBundlePage = true;
			if (this.whichArea == 4)
			{
				if (!this.fromGameMenu)
				{
					this.purchaseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 200 * Game1.pixelZoom, this.yPositionOnScreen + 126 * Game1.pixelZoom, 65 * Game1.pixelZoom, 18 * Game1.pixelZoom), this.noteTexture, new Rectangle(517, 286, 65, 20), (float)Game1.pixelZoom, false)
					{
						myID = 797,
						leftNeighborID = 103
					};
					if (Game1.options.SnappyMenus)
					{
						this.currentlySnappedComponent = this.purchaseButton;
						this.snapCursorToCurrentSnappedComponent();
						return;
					}
				}
			}
			else
			{
				int numberOfIngredientSlots = b.numberOfIngredientSlots;
				List<Rectangle> list = new List<Rectangle>();
				this.addRectangleRowsToList(list, numberOfIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
				for (int i = 0; i < list.Count; i++)
				{
					this.ingredientSlots.Add(new ClickableTextureComponent(list[i], this.noteTexture, new Rectangle(512, 244, 18, 18), (float)Game1.pixelZoom, false)
					{
						myID = i + 250,
						rightNeighborID = ((i < list.Count - 1) ? (i + 250 + 1) : -1),
						leftNeighborID = ((i > 0) ? (i + 250 - 1) : -1)
					});
				}
				List<Rectangle> list2 = new List<Rectangle>();
				this.addRectangleRowsToList(list2, b.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
				for (int j = 0; j < list2.Count; j++)
				{
					if (Game1.objectInformation.ContainsKey(b.ingredients[j].index))
					{
						string text = Game1.objectInformation[b.ingredients[j].index].Split(new char[]
						{
							'/'
						})[4];
						this.ingredientList.Add(new ClickableTextureComponent("", list2[j], "", text, Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, b.ingredients[j].index, 16, 16), (float)Game1.pixelZoom, false)
						{
							item = new StardewValley.Object(b.ingredients[j].index, b.ingredients[j].stack, false, -1, b.ingredients[j].quality)
						});
					}
				}
				this.updateIngredientSlots();
				if (Game1.options.SnappyMenus)
				{
					base.populateClickableComponentList();
					if (this.inventory != null && this.inventory.inventory != null)
					{
						for (int k = 0; k < this.inventory.inventory.Count; k++)
						{
							if (this.inventory.inventory[k] != null)
							{
								if (this.inventory.inventory[k].downNeighborID == 101)
								{
									this.inventory.inventory[k].downNeighborID = -1;
								}
								if (this.inventory.inventory[k].rightNeighborID == 106)
								{
									this.inventory.inventory[k].rightNeighborID = 250;
								}
								if (this.inventory.inventory[k].leftNeighborID == -1)
								{
									this.inventory.inventory[k].leftNeighborID = 103;
								}
								if (this.inventory.inventory[k].upNeighborID >= 1000)
								{
									this.inventory.inventory[k].upNeighborID = 103;
								}
							}
						}
					}
					this.currentlySnappedComponent = base.getComponentWithID(0);
					this.snapCursorToCurrentSnappedComponent();
				}
			}
		}

		private void addRectangleRowsToList(List<Rectangle> toAddTo, int numberOfItems, int centerX, int centerY)
		{
			switch (numberOfItems)
			{
			case 1:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 1, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 2:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 3:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 4:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 5:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 6:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 7:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 8:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 9:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 10:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 11:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 12:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			default:
				return;
			}
		}

		private List<Rectangle> createRowOfBoxesCenteredAt(int xStart, int yStart, int numBoxes, int boxWidth, int boxHeight, int horizontalGap)
		{
			List<Rectangle> list = new List<Rectangle>();
			int num = xStart - numBoxes * (boxWidth + horizontalGap) / 2;
			int y = yStart - boxHeight / 2;
			for (int i = 0; i < numBoxes; i++)
			{
				list.Add(new Rectangle(num + i * (boxWidth + horizontalGap), y, boxWidth, boxHeight));
			}
			return list;
		}

		public void takeDownBundleSpecificPage(Bundle b = null)
		{
			if (!this.specificBundlePage)
			{
				return;
			}
			if (b == null)
			{
				b = this.currentPageBundle;
			}
			this.specificBundlePage = false;
			this.ingredientSlots.Clear();
			this.ingredientList.Clear();
			JunimoNoteMenu.tempSprites.Clear();
			this.purchaseButton = null;
			if (Game1.options.SnappyMenus)
			{
				this.snapToDefaultClickableComponent();
			}
		}

		private Point getBundleLocationFromNumber(int whichBundle)
		{
			Point result = new Point(this.xPositionOnScreen, this.yPositionOnScreen);
			switch (whichBundle)
			{
			case 0:
				result.X += 148 * Game1.pixelZoom;
				result.Y += 34 * Game1.pixelZoom;
				break;
			case 1:
				result.X += 98 * Game1.pixelZoom;
				result.Y += 96 * Game1.pixelZoom;
				break;
			case 2:
				result.X += 196 * Game1.pixelZoom;
				result.Y += 97 * Game1.pixelZoom;
				break;
			case 3:
				result.X += 76 * Game1.pixelZoom;
				result.Y += 63 * Game1.pixelZoom;
				break;
			case 4:
				result.X += 223 * Game1.pixelZoom;
				result.Y += 63 * Game1.pixelZoom;
				break;
			case 5:
				result.X += 147 * Game1.pixelZoom;
				result.Y += 69 * Game1.pixelZoom;
				break;
			case 6:
				result.X += 147 * Game1.pixelZoom;
				result.Y += 95 * Game1.pixelZoom;
				break;
			case 7:
				result.X += 110 * Game1.pixelZoom;
				result.Y += 41 * Game1.pixelZoom;
				break;
			case 8:
				result.X += 194 * Game1.pixelZoom;
				result.Y += 41 * Game1.pixelZoom;
				break;
			}
			return result;
		}
	}
}
