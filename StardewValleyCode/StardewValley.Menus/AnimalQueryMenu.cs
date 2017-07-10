using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.Menus
{
	public class AnimalQueryMenu : IClickableMenu
	{
		public const int region_okButton = 101;

		public const int region_love = 102;

		public const int region_sellButton = 103;

		public const int region_moveHomeButton = 104;

		public const int region_noButton = 105;

		public const int region_allowReproductionButton = 106;

		public const int region_fullnessHover = 107;

		public const int region_happinessHover = 108;

		public const int region_loveHover = 109;

		public const int region_textBoxCC = 110;

		public new static int width = Game1.tileSize * 6;

		public new static int height = Game1.tileSize * 8;

		private FarmAnimal animal;

		private TextBox textBox;

		private TextBoxEvent e;

		public ClickableTextureComponent okButton;

		public ClickableTextureComponent love;

		public ClickableTextureComponent sellButton;

		public ClickableTextureComponent moveHomeButton;

		public ClickableTextureComponent yesButton;

		public ClickableTextureComponent noButton;

		public ClickableTextureComponent allowReproductionButton;

		public ClickableComponent fullnessHover;

		public ClickableComponent happinessHover;

		public ClickableComponent loveHover;

		public ClickableComponent textBoxCC;

		private double fullnessLevel;

		private double happinessLevel;

		private double loveLevel;

		private bool confirmingSell;

		private bool movingAnimal;

		private string hoverText = "";

		private string parentName;

		public AnimalQueryMenu(FarmAnimal animal) : base(Game1.viewport.Width / 2 - AnimalQueryMenu.width / 2, Game1.viewport.Height / 2 - AnimalQueryMenu.height / 2, AnimalQueryMenu.width, AnimalQueryMenu.height, false)
		{
			Game1.player.Halt();
			Game1.player.faceGeneralDirection(animal.position, 0);
			AnimalQueryMenu.width = Game1.tileSize * 6;
			AnimalQueryMenu.height = Game1.tileSize * 8;
			this.animal = animal;
			this.textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor);
			this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 2 - 12;
			this.textBox.Y = this.yPositionOnScreen - 4 + Game1.tileSize * 2;
			this.textBox.Width = Game1.tileSize * 4;
			this.textBox.Height = Game1.tileSize * 3;
			this.textBoxCC = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X, this.textBox.Y, this.textBox.Width, Game1.tileSize), "")
			{
				myID = 110,
				downNeighborID = 104
			};
			this.textBox.Text = animal.displayName;
			Game1.keyboardDispatcher.Subscriber = this.textBox;
			this.textBox.Selected = false;
			if (animal.parentId != -1L)
			{
				FarmAnimal farmAnimal = Utility.getAnimal(animal.parentId);
				if (farmAnimal != null)
				{
					this.parentName = farmAnimal.displayName;
				}
			}
			if (animal.sound != null && Game1.soundBank != null)
			{
				Cue expr_1B2 = Game1.soundBank.GetCue(animal.sound);
				expr_1B2.SetVariable("Pitch", (float)(1200 + Game1.random.Next(-200, 201)));
				expr_1B2.Play();
			}
			this.okButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
			{
				myID = 101,
				upNeighborID = 103
			};
			this.sellButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 384, 16, 16), 4f, false)
			{
				myID = 103,
				downNeighborID = 101,
				upNeighborID = 104
			};
			this.moveHomeButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 4 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(16, 384, 16, 16), 4f, false)
			{
				myID = 104,
				downNeighborID = 103,
				upNeighborID = 110
			};
			if (!animal.isBaby() && !animal.isCoopDweller())
			{
				this.allowReproductionButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + Game1.pixelZoom * 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 2 - IClickableMenu.borderWidth + Game1.pixelZoom * 2, Game1.pixelZoom * 9, Game1.pixelZoom * 9), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(animal.allowReproduction ? 128 : 137, 393, 9, 9), 4f, false)
				{
					myID = 106
				};
			}
			this.love = new ClickableTextureComponent(Math.Round((double)animal.friendshipTowardFarmer, 0) / 10.0 + "<", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2 + 16, this.yPositionOnScreen - Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 2, AnimalQueryMenu.width - Game1.tileSize * 2, Game1.tileSize), null, "Friendship", Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(172, 512, 16, 16), 4f, false)
			{
				myID = 102
			};
			this.loveHover = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 2, AnimalQueryMenu.width, Game1.tileSize), "Friendship")
			{
				myID = 109
			};
			this.fullnessLevel = (double)((float)animal.fullness / 255f);
			if (animal.home != null && animal.home.indoors != null)
			{
				int num = animal.home.indoors.numberOfObjectsWithName("Hay");
				if (num > 0)
				{
					int count = (animal.home.indoors as AnimalHouse).animalsThatLiveHere.Count;
					this.fullnessLevel = Math.Min(1.0, this.fullnessLevel + (double)num / (double)count);
				}
			}
			else
			{
				Utility.fixAllAnimals();
			}
			this.happinessLevel = (double)((float)animal.happiness / 255f);
			this.loveLevel = (double)((float)animal.friendshipTowardFarmer / 1000f);
			if (Game1.options.SnappyMenus)
			{
				base.populateClickableComponentList();
				this.snapToDefaultClickableComponent();
			}
		}

		public override void snapToDefaultClickableComponent()
		{
			this.currentlySnappedComponent = base.getComponentWithID(101);
			this.snapCursorToCurrentSnappedComponent();
		}

		public void textBoxEnter(TextBox sender)
		{
		}

		public override void receiveKeyPress(Keys key)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (Game1.options.menuButton.Contains(new InputButton(key)) && (this.textBox == null || !this.textBox.Selected))
			{
				Game1.playSound("smallSelect");
				if (this.readyToClose())
				{
					Game1.exitActiveMenu();
					if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
					{
						this.animal.displayName = this.textBox.Text;
						this.animal.name = this.textBox.Text;
						return;
					}
				}
				else if (this.movingAnimal)
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
					return;
				}
			}
			else if (Game1.options.SnappyMenus && (!Game1.options.menuButton.Contains(new InputButton(key)) || this.textBox == null || !this.textBox.Selected))
			{
				base.receiveKeyPress(key);
			}
		}

		public override void update(GameTime time)
		{
			base.update(time);
			if (this.movingAnimal)
			{
				int num = Game1.getOldMouseX() + Game1.viewport.X;
				int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
				if (num - Game1.viewport.X < Game1.tileSize)
				{
					Game1.panScreen(-8, 0);
				}
				else if (num - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
				{
					Game1.panScreen(8, 0);
				}
				if (num2 - Game1.viewport.Y < Game1.tileSize)
				{
					Game1.panScreen(0, -8);
				}
				else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
				{
					Game1.panScreen(0, 8);
				}
				Keys[] pressedKeys = Game1.oldKBState.GetPressedKeys();
				for (int i = 0; i < pressedKeys.Length; i++)
				{
					Keys key = pressedKeys[i];
					this.receiveKeyPress(key);
				}
			}
		}

		public void finishedPlacingAnimal()
		{
			Game1.exitActiveMenu();
			Game1.currentLocation = Game1.player.currentLocation;
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.displayFarmer = true;
			Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_HomeChanged", new object[0]), Color.LimeGreen, 3500f));
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (this.movingAnimal)
			{
				if (this.okButton != null && this.okButton.containsPoint(x, y))
				{
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
					Game1.playSound("smallSelect");
				}
				Vector2 tile = new Vector2((float)((x + Game1.viewport.X) / Game1.tileSize), (float)((y + Game1.viewport.Y) / Game1.tileSize));
				Building buildingAt = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(tile);
				if (buildingAt != null)
				{
					if (!buildingAt.buildingType.Contains(this.animal.buildingTypeILiveIn))
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_CantLiveThere", new object[]
						{
							this.animal.shortDisplayType()
						}));
						return;
					}
					if ((buildingAt.indoors as AnimalHouse).isFull())
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_BuildingFull", new object[0]));
						return;
					}
					if (buildingAt.Equals(this.animal.home))
					{
						Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_AlreadyHome", new object[0]));
						return;
					}
					(this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
					if ((this.animal.home.indoors as AnimalHouse).animals.ContainsKey(this.animal.myID))
					{
						(buildingAt.indoors as AnimalHouse).animals.Add(this.animal.myID, this.animal);
						(this.animal.home.indoors as AnimalHouse).animals.Remove(this.animal.myID);
					}
					this.animal.home = buildingAt;
					this.animal.homeLocation = new Vector2((float)buildingAt.tileX, (float)buildingAt.tileY);
					(buildingAt.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animal.myID);
					this.animal.makeSound();
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.finishedPlacingAnimal), 0.02f);
					return;
				}
			}
			else if (this.confirmingSell)
			{
				if (this.yesButton.containsPoint(x, y))
				{
					Game1.player.money += this.animal.getSellPrice();
					(this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
					this.animal.health = -1;
					int num = this.animal.frontBackSourceRect.Width / 2;
					for (int i = 0; i < num; i++)
					{
						int num2 = Game1.random.Next(25, 200);
						Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, this.animal.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Width * 3), (float)Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Height * 3)), new Color(255 - num2, 255, 255 - num2), 8, false, (float)((Game1.random.NextDouble() < 0.5) ? 50 : Game1.random.Next(30, 200)), 0, Game1.tileSize, -1f, Game1.tileSize, (Game1.random.NextDouble() < 0.5) ? 0 : Game1.random.Next(0, 600))
						{
							scale = (float)Game1.random.Next(2, 5) * 0.25f,
							alpha = (float)Game1.random.Next(2, 5) * 0.25f,
							motion = new Vector2(0f, (float)(-(float)Game1.random.NextDouble()))
						});
					}
					Game1.playSound("newRecipe");
					Game1.playSound("money");
					Game1.exitActiveMenu();
					return;
				}
				if (this.noButton.containsPoint(x, y))
				{
					this.confirmingSell = false;
					Game1.playSound("smallSelect");
					if (Game1.options.SnappyMenus)
					{
						this.currentlySnappedComponent = base.getComponentWithID(103);
						this.snapCursorToCurrentSnappedComponent();
						return;
					}
				}
			}
			else
			{
				if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
				{
					Game1.exitActiveMenu();
					if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
					{
						this.animal.displayName = this.textBox.Text;
						this.animal.name = this.textBox.Text;
					}
					Game1.playSound("smallSelect");
				}
				if (this.sellButton.containsPoint(x, y))
				{
					this.confirmingSell = true;
					this.yesButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 - Game1.tileSize - 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
					{
						myID = 111,
						rightNeighborID = 105
					};
					this.noButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 + 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false)
					{
						myID = 105,
						leftNeighborID = 111
					};
					Game1.playSound("smallSelect");
					if (Game1.options.SnappyMenus)
					{
						base.populateClickableComponentList();
						this.currentlySnappedComponent = this.noButton;
						this.snapCursorToCurrentSnappedComponent();
					}
				}
				if (this.moveHomeButton.containsPoint(x, y))
				{
					Game1.playSound("smallSelect");
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForAnimalPlacement), 0.02f);
				}
				if (this.allowReproductionButton != null && this.allowReproductionButton.containsPoint(x, y))
				{
					Game1.playSound("drumkit6");
					this.animal.allowReproduction = !this.animal.allowReproduction;
					if (this.animal.allowReproduction)
					{
						this.allowReproductionButton.sourceRect.X = 128;
					}
					else
					{
						this.allowReproductionButton.sourceRect.X = 137;
					}
				}
				this.textBox.Update();
			}
		}

		public override bool overrideSnappyMenuCursorMovementBan()
		{
			return this.movingAnimal;
		}

		public void prepareForAnimalPlacement()
		{
			this.movingAnimal = true;
			Game1.currentLocation = Game1.getLocationFromName("Farm");
			Game1.globalFadeToClear(null, 0.02f);
			this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
			this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
			Game1.displayHUD = false;
			Game1.viewportFreeze = true;
			Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
			Game1.panScreen(0, 0);
			Game1.currentLocation.resetForPlayerEntry();
			Game1.displayFarmer = false;
		}

		public void prepareForReturnFromPlacement()
		{
			Game1.currentLocation = Game1.player.currentLocation;
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(null, 0.02f);
			this.okButton.bounds.X = this.xPositionOnScreen + AnimalQueryMenu.width + 4;
			this.okButton.bounds.Y = this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth;
			Game1.displayHUD = true;
			Game1.viewportFreeze = false;
			Game1.displayFarmer = true;
			this.movingAnimal = false;
		}

		public override bool readyToClose()
		{
			this.textBox.Selected = false;
			return base.readyToClose() && !this.movingAnimal && !Game1.globalFade;
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (Game1.globalFade)
			{
				return;
			}
			if (this.readyToClose())
			{
				Game1.exitActiveMenu();
				if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
				{
					this.animal.displayName = this.textBox.Text;
					this.animal.name = this.textBox.Text;
				}
				Game1.playSound("smallSelect");
				return;
			}
			if (this.movingAnimal)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
			}
		}

		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			if (this.movingAnimal)
			{
				Vector2 tile = new Vector2((float)((x + Game1.viewport.X) / Game1.tileSize), (float)((y + Game1.viewport.Y) / Game1.tileSize));
				Farm farm = Game1.getLocationFromName("Farm") as Farm;
				using (List<Building>.Enumerator enumerator = farm.buildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.color = Color.White;
					}
				}
				Building buildingAt = farm.getBuildingAt(tile);
				if (buildingAt != null)
				{
					if (buildingAt.buildingType.Contains(this.animal.buildingTypeILiveIn) && !(buildingAt.indoors as AnimalHouse).isFull() && !buildingAt.Equals(this.animal.home))
					{
						buildingAt.color = Color.LightGreen * 0.8f;
					}
					else
					{
						buildingAt.color = Color.Red * 0.8f;
					}
				}
			}
			if (this.okButton != null)
			{
				if (this.okButton.containsPoint(x, y))
				{
					this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
				}
				else
				{
					this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
				}
			}
			if (this.sellButton != null)
			{
				if (this.sellButton.containsPoint(x, y))
				{
					this.sellButton.scale = Math.Min(4.1f, this.sellButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Sell", new object[]
					{
						this.animal.getSellPrice()
					});
				}
				else
				{
					this.sellButton.scale = Math.Max(4f, this.sellButton.scale - 0.05f);
				}
			}
			if (this.moveHomeButton != null)
			{
				if (this.moveHomeButton.containsPoint(x, y))
				{
					this.moveHomeButton.scale = Math.Min(4.1f, this.moveHomeButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Move", new object[0]);
				}
				else
				{
					this.moveHomeButton.scale = Math.Max(4f, this.moveHomeButton.scale - 0.05f);
				}
			}
			if (this.allowReproductionButton != null)
			{
				if (this.allowReproductionButton.containsPoint(x, y))
				{
					this.allowReproductionButton.scale = Math.Min(4.1f, this.allowReproductionButton.scale + 0.05f);
					this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_AllowReproduction", new object[0]);
				}
				else
				{
					this.allowReproductionButton.scale = Math.Max(4f, this.allowReproductionButton.scale - 0.05f);
				}
			}
			if (this.yesButton != null)
			{
				if (this.yesButton.containsPoint(x, y))
				{
					this.yesButton.scale = Math.Min(1.1f, this.yesButton.scale + 0.05f);
				}
				else
				{
					this.yesButton.scale = Math.Max(1f, this.yesButton.scale - 0.05f);
				}
			}
			if (this.noButton != null)
			{
				if (this.noButton.containsPoint(x, y))
				{
					this.noButton.scale = Math.Min(1.1f, this.noButton.scale + 0.05f);
					return;
				}
				this.noButton.scale = Math.Max(1f, this.noButton.scale - 0.05f);
			}
		}

		public override void draw(SpriteBatch b)
		{
			if (!this.movingAnimal && !Game1.globalFade)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + Game1.tileSize * 2, AnimalQueryMenu.width, AnimalQueryMenu.height - Game1.tileSize * 2, false, true, null, false);
				if (this.animal.harvestType != 2)
				{
					this.textBox.Draw(b);
				}
				int num = (this.animal.age + 1) / 28 + 1;
				string text;
				if (num > 1)
				{
					text = Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeN", new object[]
					{
						num
					});
				}
				else
				{
					text = Game1.content.LoadString("Strings\\UI:AnimalQuery_Age1", new object[0]);
				}
				if (this.animal.age < (int)this.animal.ageWhenMature)
				{
					text += Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeBaby", new object[0]);
				}
				Utility.drawTextWithShadow(b, text, Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				int num2 = 0;
				if (this.parentName != null)
				{
					num2 = Game1.tileSize / 3;
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AnimalQuery_Parent", new object[]
					{
						this.parentName
					}), Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(Game1.tileSize / 2 + this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
				int num3 = (int)((this.loveLevel * 1000.0 % 200.0 >= 100.0) ? (this.loveLevel * 1000.0 / 200.0) : -100.0);
				for (int i = 0; i < 5; i++)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * i), (float)(num2 + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211 + ((this.loveLevel * 1000.0 <= (double)((i + 1) * 195)) ? 7 : 0), 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
					if (num3 == i)
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * i), (float)(num2 + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211, 428, 4, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.891f);
					}
				}
				Utility.drawTextWithShadow(b, Game1.parseText(this.animal.getMoodMessage(), Game1.smallFont, AnimalQueryMenu.width - IClickableMenu.spaceToClearSideBorder * 2 - Game1.tileSize), Game1.smallFont, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float)(num2 + this.yPositionOnScreen + Game1.tileSize * 6 - Game1.tileSize + Game1.pixelZoom)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				this.okButton.draw(b);
				this.sellButton.draw(b);
				this.moveHomeButton.draw(b);
				if (this.allowReproductionButton != null)
				{
					this.allowReproductionButton.draw(b);
				}
				if (this.confirmingSell)
				{
					b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
					Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 5 / 2, Game1.viewport.Height / 2 - Game1.tileSize * 3, Game1.tileSize * 5, Game1.tileSize * 4, false, true, null, false);
					string text2 = Game1.content.LoadString("Strings\\UI:AnimalQuery_ConfirmSell", new object[0]);
					b.DrawString(Game1.dialogueFont, text2, new Vector2((float)(Game1.viewport.Width / 2) - Game1.dialogueFont.MeasureString(text2).X / 2f, (float)(Game1.viewport.Height / 2 - Game1.tileSize * 3 / 2 + 8)), Game1.textColor);
					this.yesButton.draw(b);
					this.noButton.draw(b);
				}
				else if (this.hoverText != null && this.hoverText.Length > 0)
				{
					IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
				}
			}
			else if (!Game1.globalFade)
			{
				string text3 = Game1.content.LoadString("Strings\\UI:AnimalQuery_ChooseBuilding", new object[]
				{
					this.animal.displayHouse,
					this.animal.displayType
				});
				Game1.drawDialogueBox(Game1.tileSize / 2, -Game1.tileSize, (int)Game1.dialogueFont.MeasureString(text3).X + IClickableMenu.borderWidth * 2 + Game1.tileSize / 4, Game1.tileSize * 2 + IClickableMenu.borderWidth * 2, false, true, null, false);
				b.DrawString(Game1.dialogueFont, text3, new Vector2((float)(Game1.tileSize / 2 + IClickableMenu.spaceToClearSideBorder * 2 + 8), (float)(Game1.tileSize / 2 + Game1.pixelZoom * 3)), Game1.textColor);
				this.okButton.draw(b);
			}
			base.drawMouse(b);
		}
	}
}
