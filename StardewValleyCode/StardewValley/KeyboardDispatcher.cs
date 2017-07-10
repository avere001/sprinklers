using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Windows;

namespace StardewValley
{
	public class KeyboardDispatcher
	{
		private IKeyboardSubscriber _subscriber;

		private string _pasteResult = "";

		public IKeyboardSubscriber Subscriber
		{
			get
			{
				return this._subscriber;
			}
			set
			{
				if (this._subscriber == value)
				{
					return;
				}
				if (this._subscriber != null)
				{
					this._subscriber.Selected = false;
				}
				this._subscriber = value;
				if (this._subscriber != null)
				{
					this._subscriber.Selected = true;
				}
			}
		}

		public KeyboardDispatcher(GameWindow window)
		{
			KeyboardInput.Initialize(window);
			KeyboardInput.CharEntered += new CharEnteredHandler(this.EventInput_CharEntered);
			KeyboardInput.KeyDown += new KeyEventHandler(this.EventInput_KeyDown);
		}

		private void Event_KeyDown(object sender, Keys key)
		{
			if (this._subscriber == null)
			{
				return;
			}
			if (key == Keys.Back)
			{
				this._subscriber.RecieveCommandInput('\b');
			}
			if (key == Keys.Enter)
			{
				this._subscriber.RecieveCommandInput('\r');
			}
			if (key == Keys.Tab)
			{
				this._subscriber.RecieveCommandInput('\t');
			}
			this._subscriber.RecieveSpecialInput(key);
		}

		private void EventInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (this._subscriber == null)
			{
				return;
			}
			this._subscriber.RecieveSpecialInput(e.KeyCode);
		}

		private void EventInput_CharEntered(object sender, CharacterEventArgs e)
		{
			if (this._subscriber == null)
			{
				return;
			}
			if (!char.IsControl(e.Character))
			{
				this._subscriber.RecieveTextInput(e.Character);
				return;
			}
			if (e.Character == '\u0016')
			{
				Thread expr_31 = new Thread(new ThreadStart(this.PasteThread));
				expr_31.SetApartmentState(ApartmentState.STA);
				expr_31.Start();
				expr_31.Join();
				this._subscriber.RecieveTextInput(this._pasteResult);
				return;
			}
			this._subscriber.RecieveCommandInput(e.Character);
		}

		[STAThread]
		private void PasteThread()
		{
			if (Clipboard.ContainsText())
			{
				this._pasteResult = Clipboard.GetText();
				return;
			}
			this._pasteResult = "";
		}
	}
}
