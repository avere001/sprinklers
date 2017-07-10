using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class CharacterCustomization : IClickableMenu
	{
		public const int region_okbutton = 505;

		public const int region_skipIntroButton = 506;

		public const int region_randomButton = 507;

		public const int region_male = 508;

		public const int region_female = 509;

		public const int region_dog = 510;

		public const int region_cat = 511;

		public const int region_shirtLeft = 512;

		public const int region_shirtRight = 513;

		public const int region_hairLeft = 514;

		public const int region_hairRight = 515;

		public const int region_accLeft = 516;

		public const int region_accRight = 517;

		public const int region_skinLeft = 518;

		public const int region_skinRight = 519;

		public const int region_directionLeft = 520;

		public const int region_directionRight = 521;

		public const int region_colorPicker1 = 522;

		public const int region_colorPicker2 = 523;

		public const int region_colorPicker3 = 524;

		public const int region_colorPicker4 = 525;

		public const int region_colorPicker5 = 526;

		public const int region_colorPicker6 = 527;

		public const int region_colorPicker7 = 528;

		public const int region_colorPicker8 = 529;

		public const int region_colorPicker9 = 530;

		public const int region_farmSelection1 = 531;

		public const int region_farmSelection2 = 532;

		public const int region_farmSelection3 = 533;

		public const int region_farmSelection4 = 534;

		public const int region_farmSelection5 = 535;

		public const int region_nameBox = 536;

		public const int region_farmNameBox = 537;

		public const int region_favThingBox = 538;

		public const int colorPickerTimerDelay = 100;

		private List<int> shirtOptions;

		private List<int> hairStyleOptions;

		private List<int> accessoryOptions;

		private int currentShirt;

		private int currentHair;

		private int currentAccessory;

		private int colorPickerTimer;

		public ColorPicker pantsColorPicker;

		public ColorPicker hairColorPicker;

		public ColorPicker eyeColorPicker;

		public List<ClickableComponent> labels = new List<ClickableComponent>();

		public List<ClickableComponent> leftSelectionButtons = new List<ClickableComponent>();

		public List<ClickableComponent> rightSelectionButtons = new List<ClickableComponent>();

		public List<ClickableComponent> genderButtons = new List<ClickableComponent>();

		public List<ClickableComponent> petButtons = new List<ClickableComponent>();

		public List<ClickableTextureComponent> farmTypeButtons = new List<ClickableTextureComponent>();

		public List<ClickableComponent> colorPickerCCs = new List<ClickableComponent>();

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent skipIntroButton;

		public ClickableTextureComponent randomButton;

		private TextBox nameBox;

		private TextBox farmnameBox;

		private TextBox favThingBox;

		private bool skipIntro;

		private bool wizardSource;

		private string hoverText;

		private string hoverTitle;

		public ClickableComponent nameBoxCC;

		public ClickableComponent farmnameBoxCC;

		public ClickableComponent favThingBoxCC;

		public ClickableComponent backButton;

		private ClickableComponent nameLabel;

		private ClickableComponent farmLabel;

		private ClickableComponent favoriteLabel;

		private ClickableComponent shirtLabel;

		private ClickableComponent skinLabel;

		private ClickableComponent hairLabel;

		private ClickableComponent accLabel;

		private ColorPicker lastHeldColorPicker;

		private int timesRandom;

		public CharacterCustomization(List<int> shirtOptions, List<int> hairStyleOptions, List<int> accessoryOptions, bool wizardSource = false) : base(Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize, 632 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize, false)
		{
			this.shirtOptions = shirtOptions;
			this.hairStyleOptions = hairStyleOptions;
			this.accessoryOptions = accessoryOptions;
			this.wizardSource = wizardSource;
			this.setUpPositions();
			Game1.player.faceDirection(2);
			Game1.player.FarmerSprite.StopAnimation();
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - (632 + IClickableMenu.borderWidth * 2) / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize;
			this.setUpPositions();
		}

		private void setUpPositions()
		{
			this.labels.Clear();
			this.petButtons.Clear();
			this.genderButtons.Clear();
			this.leftSelectionButtons.Clear();
			this.rightSelectionButtons.Clear();
			this.farmTypeButtons.Clear();
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 505,
				upNeighborID = 530,
				leftNeighborID = 506,
				rightNeighborID = 535,
				downNeighborID = (this.wizardSource ? -1 : 81114)
			};
			this.nameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4,
				Text = Game1.player.name
			};
			this.backButton = new ClickableComponent(new Rectangle(Game1.viewport.Width + -198 - 48, Game1.viewport.Height - 81 - 24, 198, 81), "")
			{
				myID = 81114,
				leftNeighborID = 535
			};
			this.nameBoxCC = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4, 192, 48), "")
			{
				myID = 536,
				leftNeighborID = 507,
				downNeighborID = 537,
				rightNeighborID = 531
			};
			int num = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt) ? (-Game1.pixelZoom) : 0;
			this.labels.Add(this.nameLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + num + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Name", new object[0])));
			this.farmnameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize,
				Text = Game1.player.farmName
			};
			this.farmnameBoxCC = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize, 192, 48), "")
			{
				myID = 537,
				leftNeighborID = 507,
				downNeighborID = 538,
				upNeighborID = 536,
				rightNeighborID = 531
			};
			this.labels.Add(this.farmLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + num * 3 + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Farm", new object[0])));
			this.favThingBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4,
				Y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize * 2,
				Text = Game1.player.favoriteThing
			};
			this.favThingBoxCC = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize * 2, 192, 48), "")
			{
				myID = 538,
				leftNeighborID = 521,
				downNeighborID = 511,
				upNeighborID = 537,
				rightNeighborID = 531
			};
			this.labels.Add(this.favoriteLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + num + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4 + Game1.tileSize * 2, 1, 1), Game1.content.LoadString("Strings\\UI:Character_FavoriteThing", new object[0])));
			this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 12, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 14, Game1.pixelZoom * 10, Game1.pixelZoom * 10), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float)Game1.pixelZoom, false)
			{
				myID = 507,
				rightNeighborID = 536,
				downNeighborID = 520
			};
			int num2 = Game1.tileSize * 2;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Direction", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 520,
				rightNeighborID = 521,
				upNeighborID = 507,
				downNeighborID = 508
			});
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Direction", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false)
			{
				myID = 521,
				leftNeighborID = 520,
				downNeighborID = 509,
				upNeighborID = 507,
				rightNeighborID = 538
			});
			if (!this.wizardSource)
			{
				this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8 + num, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 8 + Game1.tileSize * 3, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Animal", new object[0])));
			}
			int num3 = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru) ? (Game1.pixelZoom * 15) : 0;
			this.petButtons.Add(new ClickableTextureComponent("Cat", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 6 - Game1.tileSize / 4 + num3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, "Cat", Game1.mouseCursors, new Rectangle(160, 192, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 511,
				rightNeighborID = 510,
				leftNeighborID = 509,
				downNeighborID = 522,
				upNeighborID = 538
			});
			this.petButtons.Add(new ClickableTextureComponent("Dog", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 7 - Game1.tileSize / 4 + num3, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, "Dog", Game1.mouseCursors, new Rectangle(176, 192, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 510,
				leftNeighborID = 511,
				downNeighborID = 522,
				rightNeighborID = 532,
				upNeighborID = 538
			});
			this.genderButtons.Add(new ClickableTextureComponent("Male", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 2 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), null, "Male", Game1.mouseCursors, new Rectangle(128, 192, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 508,
				rightNeighborID = 509,
				downNeighborID = 518,
				upNeighborID = 520
			});
			this.genderButtons.Add(new ClickableTextureComponent("Female", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 2 + Game1.tileSize + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), null, "Female", Game1.mouseCursors, new Rectangle(144, 192, 16, 16), (float)Game1.pixelZoom, false)
			{
				myID = 509,
				leftNeighborID = 508,
				downNeighborID = 519,
				rightNeighborID = 511,
				upNeighborID = 521
			});
			num2 = Game1.tileSize * 4 + 8;
			int num4 = (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt) ? (-Game1.pixelZoom * 5) : 0;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Skin", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4 + num4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 518,
				rightNeighborID = 519,
				downNeighborID = 514,
				upNeighborID = 508
			});
			this.labels.Add(this.skinLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize / 4 + Game1.tileSize + 8 + num4 / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Skin", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Skin", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false)
			{
				myID = 519,
				leftNeighborID = 518,
				downNeighborID = 515,
				upNeighborID = 509,
				rightNeighborID = 522
			});
			if (!this.wizardSource)
			{
				Point point = new Point(this.xPositionOnScreen + this.width + Game1.pixelZoom + Game1.tileSize / 8, this.yPositionOnScreen + IClickableMenu.borderWidth * 2);
				this.farmTypeButtons.Add(new ClickableTextureComponent("Standard", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmStandard", new object[0]), Game1.mouseCursors, new Rectangle(0, 324, 22, 20), (float)Game1.pixelZoom, false)
				{
					myID = 531,
					downNeighborID = 532,
					leftNeighborID = 537
				});
				this.farmTypeButtons.Add(new ClickableTextureComponent("Riverland", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 2, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmFishing", new object[0]), Game1.mouseCursors, new Rectangle(22, 324, 22, 20), (float)Game1.pixelZoom, false)
				{
					myID = 532,
					downNeighborID = 533,
					upNeighborID = 531,
					leftNeighborID = 510,
					rightNeighborID = 81114
				});
				this.farmTypeButtons.Add(new ClickableTextureComponent("Forest", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 3, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmForaging", new object[0]), Game1.mouseCursors, new Rectangle(44, 324, 22, 20), (float)Game1.pixelZoom, false)
				{
					myID = 533,
					downNeighborID = 534,
					upNeighborID = 532,
					leftNeighborID = 522,
					rightNeighborID = 81114
				});
				this.farmTypeButtons.Add(new ClickableTextureComponent("Hills", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 4, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmMining", new object[0]), Game1.mouseCursors, new Rectangle(66, 324, 22, 20), (float)Game1.pixelZoom, false)
				{
					myID = 534,
					downNeighborID = 535,
					upNeighborID = 533,
					leftNeighborID = 525,
					rightNeighborID = 81114
				});
				this.farmTypeButtons.Add(new ClickableTextureComponent("Wilderness", new Rectangle(point.X, point.Y + 22 * Game1.pixelZoom * 5, 22 * Game1.pixelZoom, 20 * Game1.pixelZoom), null, Game1.content.LoadString("Strings\\UI:Character_FarmCombat", new object[0]), Game1.mouseCursors, new Rectangle(88, 324, 22, 20), (float)Game1.pixelZoom, false)
				{
					myID = 535,
					upNeighborID = 534,
					leftNeighborID = 528,
					downNeighborID = 505,
					rightNeighborID = 81114
				});
			}
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_EyeColor", new object[0])));
			Point point2 = new Point(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2);
			this.eyeColorPicker = new ColorPicker(point2.X, point2.Y);
			this.eyeColorPicker.setColor(Game1.player.newEyeColor);
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y, Game1.tileSize * 2, 20), "")
			{
				myID = 522,
				downNeighborID = 523,
				upNeighborID = 511,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 20, Game1.tileSize * 2, 20), "")
			{
				myID = 523,
				upNeighborID = 522,
				downNeighborID = 524,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 40, Game1.tileSize * 2, 20), "")
			{
				myID = 524,
				upNeighborID = 523,
				downNeighborID = 525,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			num2 += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Hair", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder + num4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 514,
				rightNeighborID = 515,
				downNeighborID = 512,
				upNeighborID = 518
			});
			this.labels.Add(this.hairLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 8 + num4 / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Hair", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Hair", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false)
			{
				myID = 515,
				leftNeighborID = 514,
				downNeighborID = 513,
				upNeighborID = 519,
				rightNeighborID = 525
			});
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_HairColor", new object[0])));
			point2 = new Point(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2);
			this.hairColorPicker = new ColorPicker(point2.X, point2.Y);
			this.hairColorPicker.setColor(Game1.player.hairstyleColor);
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y, Game1.tileSize * 2, 20), "")
			{
				myID = 525,
				downNeighborID = 526,
				upNeighborID = 524,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 20, Game1.tileSize * 2, 20), "")
			{
				myID = 526,
				upNeighborID = 525,
				downNeighborID = 527,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 40, Game1.tileSize * 2, 20), "")
			{
				myID = 527,
				upNeighborID = 526,
				downNeighborID = 528,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			num2 += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Shirt", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + num4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 512,
				rightNeighborID = 513,
				downNeighborID = 516,
				upNeighborID = 514
			});
			this.labels.Add(this.shirtLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 8 + num4 / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Shirt", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Shirt", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false)
			{
				myID = 513,
				leftNeighborID = 512,
				downNeighborID = 517,
				upNeighborID = 515,
				rightNeighborID = 528
			});
			this.labels.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 + 8, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_PantsColor", new object[0])));
			point2 = new Point(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 + Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2);
			this.pantsColorPicker = new ColorPicker(point2.X, point2.Y);
			this.pantsColorPicker.setColor(Game1.player.pantsColor);
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y, Game1.tileSize * 2, 20), "")
			{
				myID = 528,
				downNeighborID = 529,
				upNeighborID = 527,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 20, Game1.tileSize * 2, 20), "")
			{
				myID = 529,
				upNeighborID = 528,
				downNeighborID = 530,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.colorPickerCCs.Add(new ClickableComponent(new Rectangle(point2.X, point2.Y + 40, Game1.tileSize * 2, 20), "")
			{
				myID = 530,
				upNeighborID = 529,
				downNeighborID = 506,
				leftNeighborImmutable = true,
				rightNeighborImmutable = true
			});
			this.skipIntroButton = new ClickableTextureComponent("Skip Intro", new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 - Game1.tileSize * 3 / 4 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + Game1.tileSize * 5 / 4, Game1.pixelZoom * 9, Game1.pixelZoom * 9), null, Game1.content.LoadString("Strings\\UI:Character_SkipIntro", new object[0]), Game1.mouseCursors, new Rectangle(227, 425, 9, 9), (float)Game1.pixelZoom, false)
			{
				myID = 506,
				upNeighborID = 530,
				leftNeighborID = 517,
				rightNeighborID = 505
			};
			num2 += Game1.tileSize + 8;
			this.leftSelectionButtons.Add(new ClickableTextureComponent("Acc", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + num4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false)
			{
				myID = 516,
				rightNeighborID = 517,
				upNeighborID = 512
			});
			this.labels.Add(this.accLabel = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth + Game1.tileSize + 8 + num4 / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2 + 16, 1, 1), Game1.content.LoadString("Strings\\UI:Character_Accessory", new object[0])));
			this.rightSelectionButtons.Add(new ClickableTextureComponent("Acc", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4 + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + num2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 33, -1, -1), 1f, false)
			{
				myID = 517,
				leftNeighborID = 516,
				upNeighborID = 513,
				rightNeighborID = 528
			});
			if (Game1.options.snappyMenus && Game1.options.gamepadControls)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(521);
			this.snapCursorToCurrentSnappedComponent();
		}

		public override void gamePadButtonHeld(Buttons b)
		{
			base.gamePadButtonHeld(b);
			if (this.currentlySnappedComponent != null)
			{
				if (b == Buttons.LeftThumbstickRight || b == Buttons.DPadRight)
				{
					switch (this.currentlySnappedComponent.myID)
					{
					case 522:
						this.eyeColorPicker.changeHue(1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 523:
						this.eyeColorPicker.changeSaturation(1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 524:
						this.eyeColorPicker.changeValue(1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 525:
						this.hairColorPicker.changeHue(1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 526:
						this.hairColorPicker.changeSaturation(1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 527:
						this.hairColorPicker.changeValue(1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 528:
						this.pantsColorPicker.changeHue(1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						return;
					case 529:
						this.pantsColorPicker.changeSaturation(1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						return;
					case 530:
						this.pantsColorPicker.changeValue(1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						return;
					default:
						return;
					}
				}
				else if (b == Buttons.LeftThumbstickLeft || b == Buttons.DPadLeft)
				{
					switch (this.currentlySnappedComponent.myID)
					{
					case 522:
						this.eyeColorPicker.changeHue(-1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 523:
						this.eyeColorPicker.changeSaturation(-1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 524:
						this.eyeColorPicker.changeValue(-1);
						Game1.player.changeEyeColor(this.eyeColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.eyeColorPicker;
						return;
					case 525:
						this.hairColorPicker.changeHue(-1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 526:
						this.hairColorPicker.changeSaturation(-1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 527:
						this.hairColorPicker.changeValue(-1);
						Game1.player.changeHairColor(this.hairColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.hairColorPicker;
						return;
					case 528:
						this.pantsColorPicker.changeHue(-1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						return;
					case 529:
						this.pantsColorPicker.changeSaturation(-1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						return;
					case 530:
						this.pantsColorPicker.changeValue(-1);
						Game1.player.changePants(this.pantsColorPicker.getSelectedColor());
						this.lastHeldColorPicker = this.pantsColorPicker;
						break;
					default:
						return;
					}
				}
			}
		}

		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (this.currentlySnappedComponent != null)
			{
				if (b == Buttons.RightTrigger)
				{
					switch (this.currentlySnappedComponent.myID)
					{
					case 512:
					case 513:
					case 514:
					case 515:
					case 516:
					case 517:
					case 518:
					case 519:
					case 520:
					case 521:
						this.selectionClick(this.currentlySnappedComponent.name, 1);
						return;
					default:
						return;
					}
				}
				else if (b == Buttons.LeftTrigger)
				{
					switch (this.currentlySnappedComponent.myID)
					{
					case 512:
					case 513:
					case 514:
					case 515:
					case 516:
					case 517:
					case 518:
					case 519:
					case 520:
					case 521:
						this.selectionClick(this.currentlySnappedComponent.name, -1);
						break;
					default:
						return;
					}
				}
			}
		}

		private void optionButtonClick(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1367651536u)
			{
				if (num <= 989237149u)
				{
					if (num != 1485672u)
					{
						if (num == 989237149u)
						{
							if (name == "Wilderness")
							{
								if (!this.wizardSource)
								{
									Game1.whichFarm = 4;
									Game1.spawnMonstersAtNight = true;
								}
							}
						}
					}
					else if (name == "Standard")
					{
						if (!this.wizardSource)
						{
							Game1.whichFarm = 0;
							Game1.spawnMonstersAtNight = false;
						}
					}
				}
				else if (num != 1216165616u)
				{
					if (num != 1265483177u)
					{
						if (num == 1367651536u)
						{
							if (name == "Riverland")
							{
								if (!this.wizardSource)
								{
									Game1.whichFarm = 1;
									Game1.spawnMonstersAtNight = false;
								}
							}
						}
					}
					else if (name == "Dog")
					{
						if (!this.wizardSource)
						{
							Game1.player.catPerson = false;
						}
					}
				}
				else if (name == "Male")
				{
					if (!this.wizardSource)
					{
						Game1.player.changeGender(true);
						Game1.player.changeHairStyle(0);
					}
				}
			}
			else if (num <= 2246359087u)
			{
				if (num != 1761538983u)
				{
					if (num == 2246359087u)
					{
						if (name == "OK")
						{
							if (!this.canLeaveMenu())
							{
								return;
							}
							Game1.player.Name = this.nameBox.Text.Trim();
							Game1.player.displayName = Game1.player.Name;
							Game1.player.favoriteThing = this.favThingBox.Text.Trim();
							if (Game1.activeClickableMenu is TitleMenu)
							{
								(Game1.activeClickableMenu as TitleMenu).createdNewCharacter(this.skipIntro);
							}
							else
							{
								Game1.exitActiveMenu();
								if (Game1.currentMinigame != null && Game1.currentMinigame is Intro)
								{
									(Game1.currentMinigame as Intro).doneCreatingCharacter();
								}
								else if (this.wizardSource)
								{
									Game1.flashAlpha = 1f;
									Game1.playSound("yoba");
								}
							}
						}
					}
				}
				else if (name == "Cat")
				{
					if (!this.wizardSource)
					{
						Game1.player.catPerson = true;
					}
				}
			}
			else if (num != 2503779456u)
			{
				if (num != 2508411131u)
				{
					if (num == 3634523321u)
					{
						if (name == "Female")
						{
							if (!this.wizardSource)
							{
								Game1.player.changeGender(false);
								Game1.player.changeHairStyle(16);
							}
						}
					}
				}
				else if (name == "Hills")
				{
					if (!this.wizardSource)
					{
						Game1.whichFarm = 3;
						Game1.spawnMonstersAtNight = false;
					}
				}
			}
			else if (name == "Forest")
			{
				if (!this.wizardSource)
				{
					Game1.whichFarm = 2;
					Game1.spawnMonstersAtNight = false;
				}
			}
			Game1.playSound("coin");
		}

		private void selectionClick(string name, int change)
		{
			if (name == "Skin")
			{
				Game1.player.changeSkinColor(Game1.player.skin + change);
				Game1.playSound("skeletonStep");
				return;
			}
			if (name == "Hair")
			{
				Game1.player.changeHairStyle(Game1.player.hair + change);
				Game1.playSound("grassyStep");
				return;
			}
			if (name == "Shirt")
			{
				Game1.player.changeShirt(Game1.player.shirt + change);
				Game1.playSound("coin");
				return;
			}
			if (name == "Acc")
			{
				Game1.player.changeAccessory(Game1.player.accessory + change);
				Game1.playSound("purchase");
				return;
			}
			if (!(name == "Direction"))
			{
				return;
			}
			Game1.player.faceDirection((Game1.player.facingDirection - change + 4) % 4);
			Game1.player.FarmerSprite.StopAnimation();
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.playSound("pickUpItem");
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			foreach (ClickableComponent current in this.genderButtons)
			{
				if (current.containsPoint(x, y))
				{
					this.optionButtonClick(current.name);
					current.scale -= 0.5f;
					current.scale = Math.Max(3.5f, current.scale);
				}
			}
			foreach (ClickableComponent current2 in this.farmTypeButtons)
			{
				if (current2.containsPoint(x, y) && !current2.name.Contains("Gray"))
				{
					this.optionButtonClick(current2.name);
					current2.scale -= 0.5f;
					current2.scale = Math.Max(3.5f, current2.scale);
				}
			}
			foreach (ClickableComponent current3 in this.petButtons)
			{
				if (current3.containsPoint(x, y))
				{
					this.optionButtonClick(current3.name);
					current3.scale -= 0.5f;
					current3.scale = Math.Max(3.5f, current3.scale);
				}
			}
			foreach (ClickableComponent current4 in this.leftSelectionButtons)
			{
				if (current4.containsPoint(x, y))
				{
					this.selectionClick(current4.name, -1);
					current4.scale -= 0.25f;
					current4.scale = Math.Max(0.75f, current4.scale);
				}
			}
			foreach (ClickableComponent current5 in this.rightSelectionButtons)
			{
				if (current5.containsPoint(x, y))
				{
					this.selectionClick(current5.name, 1);
					current5.scale -= 0.25f;
					current5.scale = Math.Max(0.75f, current5.scale);
				}
			}
			if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
			{
				this.optionButtonClick(this.okButton.name);
				this.okButton.scale -= 0.25f;
				this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
			}
			if (this.hairColorPicker.containsPoint(x, y))
			{
				Game1.player.changeHairColor(this.hairColorPicker.click(x, y));
				this.lastHeldColorPicker = this.hairColorPicker;
			}
			else if (this.pantsColorPicker.containsPoint(x, y))
			{
				Game1.player.changePants(this.pantsColorPicker.click(x, y));
				this.lastHeldColorPicker = this.pantsColorPicker;
			}
			else if (this.eyeColorPicker.containsPoint(x, y))
			{
				Game1.player.changeEyeColor(this.eyeColorPicker.click(x, y));
				this.lastHeldColorPicker = this.eyeColorPicker;
			}
			if (!this.wizardSource)
			{
				this.nameBox.Update();
				this.farmnameBox.Update();
				this.favThingBox.Update();
				if (this.skipIntroButton.containsPoint(x, y))
				{
					Game1.playSound("drumkit6");
					this.skipIntroButton.sourceRect.X = ((this.skipIntroButton.sourceRect.X == 227) ? 236 : 227);
					this.skipIntro = !this.skipIntro;
				}
			}
			if (this.randomButton.containsPoint(x, y))
			{
				string cueName = "drumkit6";
				if (this.timesRandom > 0)
				{
					switch (Game1.random.Next(15))
					{
					case 0:
						cueName = "drumkit1";
						break;
					case 1:
						cueName = "dirtyHit";
						break;
					case 2:
						cueName = "axchop";
						break;
					case 3:
						cueName = "hoeHit";
						break;
					case 4:
						cueName = "fishSlap";
						break;
					case 5:
						cueName = "drumkit6";
						break;
					case 6:
						cueName = "drumkit5";
						break;
					case 7:
						cueName = "drumkit6";
						break;
					case 8:
						cueName = "junimoMeep1";
						break;
					case 9:
						cueName = "coin";
						break;
					case 10:
						cueName = "axe";
						break;
					case 11:
						cueName = "hammer";
						break;
					case 12:
						cueName = "drumkit2";
						break;
					case 13:
						cueName = "drumkit4";
						break;
					case 14:
						cueName = "drumkit3";
						break;
					}
				}
				Game1.playSound(cueName);
				this.timesRandom++;
				if (Game1.random.NextDouble() < 0.33)
				{
					if (Game1.player.isMale)
					{
						Game1.player.changeAccessory(Game1.random.Next(19));
					}
					else
					{
						Game1.player.changeAccessory(Game1.random.Next(6, 19));
					}
				}
				else
				{
					Game1.player.changeAccessory(-1);
				}
				if (Game1.player.isMale)
				{
					Game1.player.changeHairStyle(Game1.random.Next(16));
				}
				else
				{
					Game1.player.changeHairStyle(Game1.random.Next(16, 32));
				}
				Color c = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				if (Game1.random.NextDouble() < 0.5)
				{
					c.R /= 2;
					c.G /= 2;
					c.B /= 2;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					c.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					c.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					c.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changeHairColor(c);
				Game1.player.changeShirt(Game1.random.Next(112));
				Game1.player.changeSkinColor(Game1.random.Next(6));
				if (Game1.random.NextDouble() < 0.25)
				{
					Game1.player.changeSkinColor(Game1.random.Next(24));
				}
				Color color = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				if (Game1.random.NextDouble() < 0.5)
				{
					color.R /= 2;
					color.G /= 2;
					color.B /= 2;
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					color.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					color.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					color.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changePants(color);
				Color c2 = new Color(Game1.random.Next(25, 254), Game1.random.Next(25, 254), Game1.random.Next(25, 254));
				c2.R /= 2;
				c2.G /= 2;
				c2.B /= 2;
				if (Game1.random.NextDouble() < 0.5)
				{
					c2.R = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					c2.G = (byte)Game1.random.Next(15, 50);
				}
				if (Game1.random.NextDouble() < 0.5)
				{
					c2.B = (byte)Game1.random.Next(15, 50);
				}
				Game1.player.changeEyeColor(c2);
				this.randomButton.scale = (float)Game1.pixelZoom - 0.5f;
				this.pantsColorPicker.setColor(Game1.player.pantsColor);
				this.eyeColorPicker.setColor(Game1.player.newEyeColor);
				this.hairColorPicker.setColor(Game1.player.hairstyleColor);
			}
		}

		public override void leftClickHeld(int x, int y)
		{
			this.colorPickerTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
			if (this.colorPickerTimer <= 0)
			{
				if (this.lastHeldColorPicker != null && !Game1.options.SnappyMenus)
				{
					if (this.lastHeldColorPicker.Equals(this.hairColorPicker))
					{
						Game1.player.changeHairColor(this.hairColorPicker.clickHeld(x, y));
					}
					if (this.lastHeldColorPicker.Equals(this.pantsColorPicker))
					{
						Game1.player.changePants(this.pantsColorPicker.clickHeld(x, y));
					}
					if (this.lastHeldColorPicker.Equals(this.eyeColorPicker))
					{
						Game1.player.changeEyeColor(this.eyeColorPicker.clickHeld(x, y));
					}
				}
				this.colorPickerTimer = 100;
			}
		}

		public override void releaseLeftClick(int x, int y)
		{
			this.hairColorPicker.releaseClick();
			this.pantsColorPicker.releaseClick();
			this.eyeColorPicker.releaseClick();
			this.lastHeldColorPicker = null;
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveKeyPress(Keys key)
		{
			if (!this.wizardSource && key == Keys.Tab)
			{
				if (this.nameBox.Selected)
				{
					this.farmnameBox.SelectMe();
					this.nameBox.Selected = false;
				}
				else if (this.farmnameBox.Selected)
				{
					this.farmnameBox.Selected = false;
					this.favThingBox.SelectMe();
				}
				else
				{
					this.favThingBox.Selected = false;
					this.nameBox.SelectMe();
				}
			}
			if (Game1.options.SnappyMenus && !Game1.options.doesInputListContain(Game1.options.menuButton, key) && Keyboard.GetState().GetPressedKeys().Count<Keys>() == 0)
			{
				base.receiveKeyPress(key);
			}
		}

		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			this.hoverTitle = "";
			using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClickableTextureComponent clickableTextureComponent = (ClickableTextureComponent)enumerator.Current;
					if (clickableTextureComponent.containsPoint(x, y))
					{
						clickableTextureComponent.scale = Math.Min(clickableTextureComponent.scale + 0.02f, clickableTextureComponent.baseScale + 0.1f);
					}
					else
					{
						clickableTextureComponent.scale = Math.Max(clickableTextureComponent.scale - 0.02f, clickableTextureComponent.baseScale);
					}
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.rightSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClickableTextureComponent clickableTextureComponent2 = (ClickableTextureComponent)enumerator.Current;
					if (clickableTextureComponent2.containsPoint(x, y))
					{
						clickableTextureComponent2.scale = Math.Min(clickableTextureComponent2.scale + 0.02f, clickableTextureComponent2.baseScale + 0.1f);
					}
					else
					{
						clickableTextureComponent2.scale = Math.Max(clickableTextureComponent2.scale - 0.02f, clickableTextureComponent2.baseScale);
					}
				}
			}
			if (!this.wizardSource)
			{
				foreach (ClickableTextureComponent current in this.farmTypeButtons)
				{
					if (current.containsPoint(x, y) && !current.name.Contains("Gray"))
					{
						current.scale = Math.Min(current.scale + 0.02f, current.baseScale + 0.1f);
						this.hoverTitle = current.hoverText.Split(new char[]
						{
							'_'
						})[0];
						this.hoverText = current.hoverText.Split(new char[]
						{
							'_'
						})[1];
					}
					else
					{
						current.scale = Math.Max(current.scale - 0.02f, current.baseScale);
						if (current.name.Contains("Gray") && current.containsPoint(x, y))
						{
							this.hoverText = "Reach level 10 " + Game1.content.LoadString("Strings\\UI:Character_" + current.name.Split(new char[]
							{
								'_'
							})[1], new object[0]) + " to unlock.";
						}
					}
				}
			}
			if (!this.wizardSource)
			{
				using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent clickableTextureComponent3 = (ClickableTextureComponent)enumerator.Current;
						if (clickableTextureComponent3.containsPoint(x, y))
						{
							clickableTextureComponent3.scale = Math.Min(clickableTextureComponent3.scale + 0.02f, clickableTextureComponent3.baseScale + 0.1f);
						}
						else
						{
							clickableTextureComponent3.scale = Math.Max(clickableTextureComponent3.scale - 0.02f, clickableTextureComponent3.baseScale);
						}
					}
				}
				using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent clickableTextureComponent4 = (ClickableTextureComponent)enumerator.Current;
						if (clickableTextureComponent4.containsPoint(x, y))
						{
							clickableTextureComponent4.scale = Math.Min(clickableTextureComponent4.scale + 0.02f, clickableTextureComponent4.baseScale + 0.1f);
						}
						else
						{
							clickableTextureComponent4.scale = Math.Max(clickableTextureComponent4.scale - 0.02f, clickableTextureComponent4.baseScale);
						}
					}
				}
			}
			if (this.okButton.containsPoint(x, y) && this.canLeaveMenu())
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			this.randomButton.tryHover(x, y, 0.25f);
			this.randomButton.tryHover(x, y, 0.25f);
			if (this.hairColorPicker.containsPoint(x, y) || this.pantsColorPicker.containsPoint(x, y) || this.eyeColorPicker.containsPoint(x, y))
			{
				Game1.SetFreeCursorDrag();
			}
			this.nameBox.Hover(x, y);
			this.farmnameBox.Hover(x, y);
			this.favThingBox.Hover(x, y);
			this.skipIntroButton.tryHover(x, y, 0.1f);
		}

		public bool canLeaveMenu()
		{
			return this.wizardSource || (Game1.player.name.Length > 0 && Game1.player.farmName.Length > 0 && Game1.player.favoriteThing.Length > 0);
		}

		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			b.Draw(Game1.daybg, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), Color.White);
			Game1.player.FarmerRenderer.draw(b, Game1.player.FarmerSprite.CurrentAnimationFrame, Game1.player.FarmerSprite.CurrentFrame, Game1.player.FarmerSprite.SourceRect, new Vector2((float)(this.xPositionOnScreen - 2 + Game1.tileSize * 2 / 3 + Game1.tileSize * 2 - Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth - Game1.tileSize / 4 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2)), Vector2.Zero, 0.8f, Color.White, 0f, 1f, Game1.player);
			if (!this.wizardSource)
			{
				using (List<ClickableComponent>.Enumerator enumerator = this.genderButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent clickableTextureComponent = (ClickableTextureComponent)enumerator.Current;
						clickableTextureComponent.draw(b);
						if ((clickableTextureComponent.name.Equals("Male") && Game1.player.isMale) || (clickableTextureComponent.name.Equals("Female") && !Game1.player.isMale))
						{
							b.Draw(Game1.mouseCursors, clickableTextureComponent.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
						}
					}
				}
				using (List<ClickableComponent>.Enumerator enumerator = this.petButtons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent clickableTextureComponent2 = (ClickableTextureComponent)enumerator.Current;
						clickableTextureComponent2.draw(b);
						if ((clickableTextureComponent2.name.Equals("Cat") && Game1.player.catPerson) || (clickableTextureComponent2.name.Equals("Dog") && !Game1.player.catPerson))
						{
							b.Draw(Game1.mouseCursors, clickableTextureComponent2.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 34, -1, -1)), Color.White);
						}
					}
				}
				Game1.player.name = this.nameBox.Text;
				Game1.player.favoriteThing = this.favThingBox.Text;
				Game1.player.farmName = this.farmnameBox.Text;
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.leftSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					((ClickableTextureComponent)enumerator.Current).draw(b);
				}
			}
			foreach (ClickableComponent current in this.labels)
			{
				string text = "";
				float num = 0f;
				Color color = Game1.textColor;
				if (current == this.nameLabel)
				{
					color = ((Game1.player.name.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (current == this.farmLabel)
				{
					color = ((Game1.player.farmName.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (current == this.favoriteLabel)
				{
					color = ((Game1.player.favoriteThing.Length < 1) ? Color.Red : Game1.textColor);
					if (this.wizardSource)
					{
						continue;
					}
				}
				else if (current == this.shirtLabel)
				{
					num = (float)(Game1.tileSize / 3) - Game1.smallFont.MeasureString(current.name).X / 2f;
					text = string.Concat(Game1.player.shirt + 1);
				}
				else if (current == this.skinLabel)
				{
					num = (float)(Game1.tileSize / 3) - Game1.smallFont.MeasureString(current.name).X / 2f;
					text = string.Concat(Game1.player.skin + 1);
				}
				else if (current == this.hairLabel)
				{
					num = (float)(Game1.tileSize / 3) - Game1.smallFont.MeasureString(current.name).X / 2f;
					if (!current.name.Contains("Color"))
					{
						text = string.Concat(Game1.player.hair + 1);
					}
				}
				else if (current == this.accLabel)
				{
					num = (float)(Game1.tileSize / 3) - Game1.smallFont.MeasureString(current.name).X / 2f;
					text = string.Concat(Game1.player.accessory + 2);
				}
				else
				{
					color = Game1.textColor;
				}
				Utility.drawTextWithShadow(b, current.name, Game1.smallFont, new Vector2((float)current.bounds.X + num, (float)current.bounds.Y), color, 1f, -1f, -1, -1, 1f, 3);
				if (text.Length > 0)
				{
					Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2((float)(current.bounds.X + Game1.tileSize / 3) - Game1.smallFont.MeasureString(text).X / 2f, (float)(current.bounds.Y + Game1.tileSize / 2)), color, 1f, -1f, -1, -1, 1f, 3);
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.rightSelectionButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					((ClickableTextureComponent)enumerator.Current).draw(b);
				}
			}
			if (!this.wizardSource)
			{
				IClickableMenu.drawTextureBox(b, this.farmTypeButtons[0].bounds.X - Game1.pixelZoom * 4, this.farmTypeButtons[0].bounds.Y - Game1.pixelZoom * 5, 30 * Game1.pixelZoom, 110 * Game1.pixelZoom + Game1.pixelZoom * 9, Color.White);
				for (int i = 0; i < this.farmTypeButtons.Count; i++)
				{
					this.farmTypeButtons[i].draw(b, this.farmTypeButtons[i].name.Contains("Gray") ? (Color.Black * 0.5f) : Color.White, 0.88f);
					if (this.farmTypeButtons[i].name.Contains("Gray"))
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.farmTypeButtons[i].bounds.Center.X - Game1.pixelZoom * 3), (float)(this.farmTypeButtons[i].bounds.Center.Y - Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(107, 442, 7, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
					}
					if (i == Game1.whichFarm)
					{
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.farmTypeButtons[i].bounds.X, this.farmTypeButtons[i].bounds.Y - Game1.pixelZoom, this.farmTypeButtons[i].bounds.Width, this.farmTypeButtons[i].bounds.Height + Game1.pixelZoom * 2, Color.White, (float)Game1.pixelZoom, false);
					}
				}
			}
			if (this.canLeaveMenu())
			{
				this.okButton.draw(b, Color.White, 0.75f);
			}
			else
			{
				this.okButton.draw(b, Color.White, 0.75f);
				this.okButton.draw(b, Color.Black * 0.5f, 0.751f);
			}
			this.hairColorPicker.draw(b);
			this.pantsColorPicker.draw(b);
			this.eyeColorPicker.draw(b);
			if (!this.wizardSource)
			{
				this.nameBox.Draw(b);
				this.farmnameBox.Draw(b);
				if (this.skipIntroButton != null)
				{
					this.skipIntroButton.draw(b);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:Character_SkipIntro", new object[0]), Game1.smallFont, new Vector2((float)(this.skipIntroButton.bounds.X + this.skipIntroButton.bounds.Width + Game1.pixelZoom * 2), (float)(this.skipIntroButton.bounds.Y + Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:Character_FarmNameSuffix", new object[0]), Game1.smallFont, new Vector2((float)(this.farmnameBox.X + this.farmnameBox.Width + Game1.pixelZoom * 2), (float)(this.farmnameBox.Y + Game1.pixelZoom * 3)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				this.favThingBox.Draw(b);
			}
			if (this.hoverText != null && this.hoverTitle != null && this.hoverText.Count<char>() > 0)
			{
				IClickableMenu.drawHoverText(b, Game1.parseText(this.hoverText, Game1.smallFont, Game1.tileSize * 4), Game1.smallFont, 0, 0, -1, this.hoverTitle, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			this.randomButton.draw(b);
			base.drawMouse(b);
		}
	}
}
