using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley
{
	public class KeyEventArgs : EventArgs
	{
		private Keys keyCode;

		public Keys KeyCode
		{
			get
			{
				return this.keyCode;
			}
		}

		public KeyEventArgs(Keys keyCode)
		{
			this.keyCode = keyCode;
		}
	}
}
