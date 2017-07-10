using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
	public class ChatBox : IClickableMenu
	{
		public const int errorMessage = 0;

		public const int userNotificationMessage = -1;

		public const int blankMessage = -2;

		public const int defaultMaxMessages = 10;

		public const int timeToDisplayMessages = 600;

		public TextBox chatBox;

		private TextBoxEvent e;

		private KeyboardDispatcher keyboardDispatcher;

		private List<ChatMessage> messages = new List<ChatMessage>();

		public int maxMessages = 10;

		public ChatBox() : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height - Game1.tileSize * 2 - Game1.tileSize / 2, Game1.tileSize * 14, 56, false)
		{
			this.chatBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\chatBox"), null, Game1.smallFont, Color.White);
			this.e = new TextBoxEvent(this.textBoxEnter);
			this.chatBox.OnEnterPressed += this.e;
			this.keyboardDispatcher = Game1.keyboardDispatcher;
			this.keyboardDispatcher.Subscriber = this.chatBox;
			this.chatBox.X = this.xPositionOnScreen;
			this.chatBox.Y = this.yPositionOnScreen;
			this.chatBox.Width = this.width;
			this.chatBox.Height = 56;
			this.chatBox.Selected = false;
		}

		public void textBoxEnter(TextBox sender)
		{
			if (Game1.IsMultiplayer && sender.Text.Length > 0)
			{
				string text = sender.Text.Trim();
				if (text.Length < 1)
				{
					return;
				}
				if (text[0] == '/' && text.Split(new char[]
				{
					' '
				})[0].Length > 1)
				{
					try
					{
						string text2 = text.Split(new char[]
						{
							' '
						})[0].Substring(1);
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 1013213428u)
						{
							if (num != 355814093u)
							{
								if (num != 405908334u)
								{
									if (num != 1013213428u)
									{
										goto IL_214;
									}
									if (!(text2 == "texture"))
									{
										goto IL_214;
									}
									Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + text.Split(new char[]
									{
										' '
									})[1]);
									goto IL_231;
								}
								else if (!(text2 == "nick"))
								{
									goto IL_214;
								}
							}
							else if (!(text2 == "nickname"))
							{
								goto IL_214;
							}
						}
						else if (num <= 2180167635u)
						{
							if (num != 1158129075u)
							{
								if (num != 2180167635u)
								{
									goto IL_214;
								}
								if (!(text2 == "rename"))
								{
									goto IL_214;
								}
							}
							else
							{
								if (!(text2 == "othergirl"))
								{
									goto IL_214;
								}
								Game1.otherFarmers.Values.ElementAt(0).Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
								goto IL_231;
							}
						}
						else if (num != 2369371622u)
						{
							if (num != 2723493283u)
							{
								goto IL_214;
							}
							if (!(text2 == "girl"))
							{
								goto IL_214;
							}
							Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
							Game1.player.isMale = false;
							goto IL_231;
						}
						else if (!(text2 == "name"))
						{
							goto IL_214;
						}
						MultiplayerUtility.sendNameChange(text.Substring(text.IndexOf(' ') + 1), Game1.player.uniqueMultiplayerID);
						goto IL_231;
						IL_214:
						this.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:ChatBox.cs.10261", new object[0]), 0L);
						IL_231:
						goto IL_27B;
					}
					catch (Exception)
					{
						this.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:ChatBox.cs.10262", new object[0]), 0L);
						goto IL_27B;
					}
				}
				MultiplayerUtility.sendChatMessage(text, Game1.player.uniqueMultiplayerID);
				if (Game1.IsServer)
				{
					this.receiveChatMessage(text, Game1.player.uniqueMultiplayerID);
				}
			}
			IL_27B:
			sender.Text = "";
			this.clickAway();
		}

		public override void clickAway()
		{
			base.clickAway();
			this.chatBox.Selected = false;
			Game1.isChatting = false;
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public void receiveChatMessage(string message, long who)
		{
			string str = Game1.player.name;
			using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Farmer current = enumerator.Current;
					if (current.uniqueMultiplayerID == who)
					{
						str = current.name;
					}
				}
			}
			str += ":";
			if (who == 0L)
			{
				str = "::";
			}
			else if (who == -1L)
			{
				str = ">";
			}
			else if (who == -2L)
			{
				str = "";
			}
			ChatMessage chatMessage = new ChatMessage();
			chatMessage.message = Game1.parseText(str + " " + message, this.chatBox.Font, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize);
			chatMessage.timeLeftToDisplay = 600;
			chatMessage.verticalSize = (int)this.chatBox.Font.MeasureString(chatMessage.message).Y;
			this.messages.Add(chatMessage);
			if (this.messages.Count > this.maxMessages)
			{
				this.messages.RemoveAt(0);
			}
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
		}

		public void update()
		{
			for (int i = 0; i < this.messages.Count; i++)
			{
				if (this.messages[i].timeLeftToDisplay > 0)
				{
					this.messages[i].timeLeftToDisplay--;
				}
				if (this.messages[i].timeLeftToDisplay < 75)
				{
					this.messages[i].alpha = (float)this.messages[i].timeLeftToDisplay / 75f;
				}
			}
			if (this.chatBox.Selected)
			{
				using (List<ChatMessage>.Enumerator enumerator = this.messages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.alpha = 1f;
					}
				}
			}
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.yPositionOnScreen = newBounds.Height - Game1.tileSize * 2 - Game1.tileSize / 2;
			this.chatBox.Y = this.yPositionOnScreen;
			this.chatBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize;
			this.chatBox.Width = Game1.tileSize * 14;
			this.width = Game1.tileSize * 14;
			this.chatBox.Height = 56;
		}

		public override void draw(SpriteBatch b)
		{
			int num = 0;
			for (int i = this.messages.Count - 1; i >= 0; i--)
			{
				num += this.messages[i].verticalSize;
				b.DrawString(this.chatBox.Font, this.messages[i].message, new Vector2(4f, (float)(Game1.viewport.Height - num - 8)), this.chatBox.TextColor * this.messages[i].alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
			}
			if (this.chatBox.Selected)
			{
				this.chatBox.Draw(b);
			}
			this.update();
		}
	}
}
