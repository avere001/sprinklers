using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley
{
	public struct InputButton
	{
		public Keys key;

		public bool mouseLeft;

		public bool mouseRight;

		public InputButton(Keys key)
		{
			this.key = key;
			this.mouseLeft = false;
			this.mouseRight = false;
		}

		public InputButton(bool mouseLeft)
		{
			this.key = Keys.None;
			this.mouseLeft = mouseLeft;
			this.mouseRight = !mouseLeft;
		}

		public override string ToString()
		{
			if (this.mouseLeft)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Left-Click", new object[0]);
			}
			if (this.mouseRight)
			{
				return Game1.content.LoadString("Strings\\StringsFromCSFiles:Right-Click", new object[0]);
			}
			switch (this.key)
			{
			case Keys.D0:
				return "0";
			case Keys.D1:
				return "1";
			case Keys.D2:
				return "2";
			case Keys.D3:
				return "3";
			case Keys.D4:
				return "4";
			case Keys.D5:
				return "5";
			case Keys.D6:
				return "6";
			case Keys.D7:
				return "7";
			case Keys.D8:
				return "8";
			case Keys.D9:
				return "9";
			default:
			{
				string result = this.key.ToString().Replace("Oem", "");
				if (Game1.content.LoadString("Strings\\StringsFromCSFiles:" + this.key.ToString().Replace("Oem", ""), new object[0]) != "Strings\\StringsFromCSFiles:" + this.key.ToString().Replace("Oem", ""))
				{
					result = Game1.content.LoadString("Strings\\StringsFromCSFiles:" + this.key.ToString().Replace("Oem", ""), new object[0]);
				}
				return result;
			}
			}
		}
	}
}
