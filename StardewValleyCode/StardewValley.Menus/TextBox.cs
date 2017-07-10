using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace StardewValley.Menus
{
	public class TextBox : IKeyboardSubscriber
	{
		private Texture2D _textBoxTexture;

		private Texture2D _caretTexture;

		private SpriteFont _font;

		private Color _textColor;

		public bool numbersOnly;

		public int textLimit = -1;

		public bool limitWidth = true;

		private string _text = "";

		private bool _showKeyboard;

		private bool _selected;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event TextBoxEvent OnEnterPressed;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event TextBoxEvent OnTabPressed;

		public SpriteFont Font
		{
			get
			{
				return this._font;
			}
		}

		public Color TextColor
		{
			get
			{
				return this._textColor;
			}
		}

		public int X
		{
			get;
			set;
		}

		public int Y
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public bool PasswordBox
		{
			get;
			set;
		}

		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				if (this._text == null)
				{
					this._text = "";
				}
				if (this._text != "")
				{
					string text = "";
					for (int i = 0; i < value.Length; i++)
					{
						char value2 = value[i];
						if (this._font.Characters.Contains(value2))
						{
							text += value2.ToString();
						}
					}
					this._text = Program.sdk.FilterDirtyWords(text);
					if (this.limitWidth && this._font.MeasureString(this._text).X > (float)(this.Width - Game1.tileSize / 3))
					{
						this.Text = this._text.Substring(0, this._text.Length - 1);
					}
				}
			}
		}

		public string TitleText
		{
			get;
			set;
		}

		public bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				if (this._selected == value)
				{
					return;
				}
				Console.WriteLine("TextBox.Selected is now '{0}'.", value);
				this._selected = value;
				if (this._selected)
				{
					Game1.keyboardDispatcher.Subscriber = this;
					this._showKeyboard = true;
					return;
				}
				this._showKeyboard = false;
			}
		}

		public TextBox(Texture2D textBoxTexture, Texture2D caretTexture, SpriteFont font, Color textColor)
		{
			this._textBoxTexture = textBoxTexture;
			if (textBoxTexture != null)
			{
				this.Width = textBoxTexture.Width;
				this.Height = textBoxTexture.Height;
			}
			this._caretTexture = caretTexture;
			this._font = font;
			this._textColor = textColor;
		}

		public void SelectMe()
		{
			this.Selected = true;
		}

		public void Update()
		{
			Mouse.GetState();
			Point value = new Point(Game1.getMouseX(), Game1.getMouseY());
			Rectangle rectangle = new Rectangle(this.X, this.Y, this.Width, this.Height);
			if (rectangle.Contains(value))
			{
				this.Selected = true;
			}
			else
			{
				this.Selected = false;
			}
			if (this._showKeyboard)
			{
				this._showKeyboard = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			bool flag = DateTime.Now.Millisecond % 1000 >= 500;
			string text = this.Text;
			if (this.PasswordBox)
			{
				text = "";
				for (int i = 0; i < this.Text.Length; i++)
				{
					text += "•";
				}
			}
			if (this._textBoxTexture != null)
			{
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X, this.Y, Game1.tileSize / 4, this.Height), new Rectangle?(new Rectangle(0, 0, Game1.tileSize / 4, this.Height)), Color.White);
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X + Game1.tileSize / 4, this.Y, this.Width - Game1.tileSize / 2, this.Height), new Rectangle?(new Rectangle(Game1.tileSize / 4, 0, 4, this.Height)), Color.White);
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X + this.Width - Game1.tileSize / 4, this.Y, Game1.tileSize / 4, this.Height), new Rectangle?(new Rectangle(this._textBoxTexture.Bounds.Width - Game1.tileSize / 4, 0, Game1.tileSize / 4, this.Height)), Color.White);
			}
			else
			{
				Game1.drawDialogueBox(this.X - Game1.tileSize / 2, this.Y - Game1.tileSize * 7 / 4 + 10, this.Width + Game1.tileSize * 5 / 4, this.Height, false, true, null, false);
			}
			Vector2 vector = this._font.MeasureString(text);
			while (vector.X > (float)this.Width)
			{
				text = text.Substring(1);
				vector = this._font.MeasureString(text);
			}
			if (flag && this.Selected)
			{
				spriteBatch.Draw(Game1.staminaRect, new Rectangle(this.X + Game1.tileSize / 4 + (int)vector.X + 2, this.Y + 8, 4, 32), this._textColor);
			}
			Utility.drawTextWithShadow(spriteBatch, text, this._font, new Vector2((float)(this.X + Game1.tileSize / 4), (float)(this.Y + ((this._textBoxTexture != null) ? (Game1.tileSize / 4 - Game1.pixelZoom) : (Game1.pixelZoom * 2)))), this._textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		public void RecieveTextInput(char inputChar)
		{
			if (this.Selected && (!this.numbersOnly || char.IsDigit(inputChar)) && (this.textLimit == -1 || this.Text.Length < this.textLimit))
			{
				if (Game1.gameMode != 3)
				{
					if (inputChar <= '*')
					{
						if (inputChar == '"')
						{
							return;
						}
						if (inputChar == '$')
						{
							Game1.playSound("money");
							goto IL_B3;
						}
						if (inputChar == '*')
						{
							Game1.playSound("hammer");
							goto IL_B3;
						}
					}
					else
					{
						if (inputChar == '+')
						{
							Game1.playSound("slimeHit");
							goto IL_B3;
						}
						if (inputChar == '<')
						{
							Game1.playSound("crystal");
							goto IL_B3;
						}
						if (inputChar == '=')
						{
							Game1.playSound("coin");
							goto IL_B3;
						}
					}
					Game1.playSound("cowboy_monsterhit");
				}
				IL_B3:
				this.Text += inputChar.ToString();
			}
		}

		public void RecieveTextInput(string text)
		{
			int num = -1;
			if (this.Selected && (!this.numbersOnly || int.TryParse(text, out num)) && (this.textLimit == -1 || this.Text.Length < this.textLimit))
			{
				this.Text += text;
			}
		}

		public void RecieveCommandInput(char command)
		{
			if (this.Selected)
			{
				if (command != '\b')
				{
					if (command != '\t')
					{
						if (command != '\r')
						{
							return;
						}
						if (this.OnEnterPressed != null)
						{
							this.OnEnterPressed(this);
							return;
						}
					}
					else if (this.OnTabPressed != null)
					{
						this.OnTabPressed(this);
					}
				}
				else if (this.Text.Length > 0)
				{
					this.Text = this.Text.Substring(0, this.Text.Length - 1);
					if (Game1.gameMode != 3)
					{
						Game1.playSound("tinyWhip");
						return;
					}
				}
			}
		}

		public void RecieveSpecialInput(Keys key)
		{
		}

		public void Hover(int x, int y)
		{
			if (x > this.X && x < this.X + this.Width && y > this.Y && y < this.Y + this.Height)
			{
				Game1.SetFreeCursorDrag();
			}
		}
	}
}
