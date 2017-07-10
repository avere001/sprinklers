using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StardewValley.Menus
{
	public class SocialPage : IClickableMenu
	{
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			public static readonly SocialPage.<>c <>9 = new SocialPage.<>c();

			public static Func<ClickableTextureComponent, int> <>9__12_0;

			internal int ctor>b__12_0(ClickableTextureComponent i)
			{
				return -Game1.player.getFriendshipLevelForNPC(i.name);
			}
		}

		public const int slotsOnPage = 5;

		private string descriptionText = "";

		private string hoverText = "";

		private ClickableTextureComponent upButton;

		private ClickableTextureComponent downButton;

		private ClickableTextureComponent scrollBar;

		private Rectangle scrollBarRunner;

		private List<ClickableTextureComponent> friendNames;

		private int slotPosition;

		private List<string> kidsNames = new List<string>();

		private Dictionary<string, string> npcNames;

		private bool scrolling;

		public SocialPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.friendNames = new List<ClickableTextureComponent>();
			this.npcNames = new Dictionary<string, string>();
			foreach (NPC current in Utility.getAllCharacters())
			{
				if ((!current.name.Equals("Sandy") || Game1.player.mailReceived.Contains("ccVault")) && !current.name.Equals("???") && !current.name.Equals("Bouncer") && !current.name.Equals("Marlon") && !current.name.Equals("Gil") && !current.name.Equals("Gunther") && !current.name.Equals("Henchman") && !current.IsMonster && !(current is Horse) && !(current is Pet) && !(current is JunimoHarvester))
				{
					if (Game1.player.friendships.ContainsKey(current.name))
					{
						string str = current.datingFarmer ? "true" : "false";
						str += (current.datable ? "_true" : "_false");
						this.friendNames.Add(new ClickableTextureComponent(current.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), null, str, current.sprite.Texture, current.getMugShotSourceRect(), (float)Game1.pixelZoom, false));
						this.npcNames[current.name] = current.getName();
						if (current is Child)
						{
							this.kidsNames.Add(current.name);
						}
					}
					else if (!current.name.Equals("Dwarf") && !current.name.Contains("Qi") && !(current is Pet) && !(current is Horse) && !(current is Junimo) && !(current is Child))
					{
						this.friendNames.Add(new ClickableTextureComponent(current.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), null, "false_false", current.sprite.Texture, current.getMugShotSourceRect(), (float)Game1.pixelZoom, false));
						this.npcNames[current.name] = "???";
						if (current is Child)
						{
							this.kidsNames.Add(current.name);
						}
					}
				}
			}
			IEnumerable<ClickableTextureComponent> arg_30C_0 = this.friendNames;
			Func<ClickableTextureComponent, int> arg_30C_1;
			if ((arg_30C_1 = SocialPage.<>c.<>9__12_0) == null)
			{
				arg_30C_1 = (SocialPage.<>c.<>9__12_0 = new Func<ClickableTextureComponent, int>(SocialPage.<>c.<>9.<.ctor>b__12_0));
			}
			this.friendNames = arg_30C_0.OrderBy(arg_30C_1).ToList<ClickableTextureComponent>();
			this.upButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upButton.bounds.X + Game1.pixelZoom * 3, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upButton.bounds.Height - Game1.pixelZoom * 2);
			this.updateSlots();
		}

		public void updateSlots()
		{
			int num = 0;
			for (int i = this.slotPosition; i < this.slotPosition + 5; i++)
			{
				if (this.friendNames.Count > i)
				{
					int y = this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize / 2 + (Game1.tileSize + Game1.tileSize * 3 / 4) * num + Game1.pixelZoom * 8;
					this.friendNames[i].bounds.Y = y;
				}
				num++;
			}
		}

		public override void applyMovementKey(int direction)
		{
			if (direction == 0 && this.slotPosition > 0)
			{
				this.upArrowPressed();
				return;
			}
			if (direction == 2 && this.slotPosition < this.friendNames.Count - 5)
			{
				this.downArrowPressed();
				return;
			}
			if (direction == 3 || direction == 1)
			{
				base.applyMovementKey(direction);
			}
		}

		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_E8_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upButton.bounds.Height + Game1.pixelZoom * 5));
				float num = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.slotPosition = Math.Min(this.friendNames.Count - 5, Math.Max(0, (int)((float)this.friendNames.Count * num)));
				this.setScrollBarToCurrentIndex();
				if (arg_E8_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
				}
			}
		}

		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.scrolling = false;
		}

		private void setScrollBarToCurrentIndex()
		{
			if (this.friendNames.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.friendNames.Count - 5 + 1) * this.slotPosition + this.upButton.bounds.Bottom + Game1.pixelZoom;
				if (this.slotPosition == this.friendNames.Count - 5)
				{
					this.scrollBar.bounds.Y = this.downButton.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
			this.updateSlots();
		}

		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.slotPosition > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.slotPosition < Math.Max(0, this.friendNames.Count - 5))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		public void upArrowPressed()
		{
			this.slotPosition--;
			this.updateSlots();
			this.upButton.scale = 3.5f;
			this.setScrollBarToCurrentIndex();
		}

		public void downArrowPressed()
		{
			this.slotPosition++;
			this.updateSlots();
			this.downButton.scale = 3.5f;
			this.setScrollBarToCurrentIndex();
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.upButton.containsPoint(x, y) && this.slotPosition > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.downButton.containsPoint(x, y) && this.slotPosition < this.friendNames.Count - 5)
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downButton.containsPoint(x, y) && x > this.xPositionOnScreen + this.width - Game1.tileSize * 3 / 2 && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.slotPosition = Math.Max(0, Math.Min(this.friendNames.Count - 5, this.slotPosition));
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			this.upButton.tryHover(x, y, 0.1f);
			this.downButton.tryHover(x, y, 0.1f);
		}

		public override void draw(SpriteBatch b)
		{
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 2 + Game1.pixelZoom, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom * 5, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 9, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 6 + Game1.tileSize / 2 + Game1.pixelZoom * 13, true);
			base.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3, true);
			base.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3 + 85 * Game1.pixelZoom, true);
			for (int i = this.slotPosition; i < this.slotPosition + 5; i++)
			{
				if (i < this.friendNames.Count)
				{
					this.friendNames[i].draw(b);
					int friendshipHeartLevelForNPC = Game1.player.getFriendshipHeartLevelForNPC(this.friendNames[i].name);
					bool flag = this.friendNames[i].hoverText.Split(new char[]
					{
						'_'
					})[1].Equals("true");
					bool flag2 = Game1.player.spouse != null && Game1.player.spouse.Equals(this.friendNames[i].name);
					b.DrawString(Game1.dialogueFont, this.npcNames[this.friendNames[i].name], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize - Game1.pixelZoom * 5 + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.npcNames[this.friendNames[i].name]).X / 2f, (float)(this.friendNames[i].bounds.Y + Game1.tileSize * 3 / 4 - (flag ? (Game1.pixelZoom * 6) : (Game1.pixelZoom * 5)))), Game1.textColor);
					for (int j = 0; j < 10 + (this.friendNames[i].name.Equals(Game1.player.spouse) ? 2 : 0); j++)
					{
						int x = (j < friendshipHeartLevelForNPC) ? 211 : 218;
						if (flag && !this.friendNames[i].hoverText.Split(new char[]
						{
							'_'
						})[0].Equals("true") && !flag2 && j >= 8)
						{
							x = 211;
						}
						if (j < 10)
						{
							b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + j * (8 * Game1.pixelZoom)), (float)(this.friendNames[i].bounds.Y + Game1.tileSize - Game1.pixelZoom * 7)), new Rectangle?(new Rectangle(x, 428, 7, 6)), (flag && !this.friendNames[i].hoverText.Split(new char[]
							{
								'_'
							})[0].Equals("true") && !flag2 && j >= 8) ? (Color.Black * 0.35f) : Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
						else
						{
							b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + (j - 10) * (8 * Game1.pixelZoom)), (float)(this.friendNames[i].bounds.Y + Game1.tileSize)), new Rectangle?(new Rectangle(x, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
					}
					if (flag)
					{
						string text = (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.pt) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635", new object[0]) : ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635", new object[0]).Split(new char[]
						{
							'/'
						}).First<string>() : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635", new object[0]).Split(new char[]
						{
							'/'
						}).Last<string>());
						if (flag2 && Game1.getCharacterFromName(this.friendNames[i].name, false) != null)
						{
							text = ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11636", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11637", new object[0]));
						}
						else if (!Game1.player.isMarried() && this.friendNames[i].hoverText.Split(new char[]
						{
							'_'
						})[0].Equals("true") && Game1.getCharacterFromName(this.friendNames[i].name, false) != null)
						{
							text = ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11639", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11640", new object[0]));
						}
						else if (Game1.getCharacterFromName(this.friendNames[i].name, false).divorcedFromFarmer)
						{
							text = ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11642", new object[0]) : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11643", new object[0]));
						}
						b.DrawString(Game1.smallFont, text, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 + Game1.pixelZoom * 2) - Game1.smallFont.MeasureString(text).X / 2f, (float)this.friendNames[i].bounds.Bottom), Game1.textColor);
					}
					if (Game1.player.friendships.ContainsKey(this.friendNames[i].name) && (Game1.player.spouse == null || !this.friendNames[i].name.Equals(Game1.player.spouse)) && !this.kidsNames.Contains(this.friendNames[i].name))
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 66 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2 - Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(229, 410, 14, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 81 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendships[this.friendNames[i].name][1] == 2) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 91 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendships[this.friendNames[i].name][1] >= 1) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					}
					if (Game1.player.spouse != null && this.friendNames[i].name.Equals(Game1.player.spouse))
					{
						b.Draw(Game1.objectSpriteSheet, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize * 3), (float)this.friendNames[i].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 460, 16, 16)), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
					}
					else if (this.friendNames[i].hoverText.Split(new char[]
					{
						'_'
					})[0].Equals("true"))
					{
						b.Draw(Game1.objectSpriteSheet, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize * 3), (float)this.friendNames[i].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 458, 16, 16)), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
					}
				}
			}
			this.upButton.draw(b);
			this.downButton.draw(b);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, true);
			this.scrollBar.draw(b);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}
	}
}
