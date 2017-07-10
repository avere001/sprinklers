using System;

namespace StardewValley
{
	public class CharacterEventArgs : EventArgs
	{
		private readonly char character;

		private readonly int lParam;

		public char Character
		{
			get
			{
				return this.character;
			}
		}

		public int Param
		{
			get
			{
				return this.lParam;
			}
		}

		public int RepeatCount
		{
			get
			{
				return this.lParam & 65535;
			}
		}

		public bool ExtendedKey
		{
			get
			{
				return (this.lParam & 16777216) > 0;
			}
		}

		public bool AltPressed
		{
			get
			{
				return (this.lParam & 536870912) > 0;
			}
		}

		public bool PreviousState
		{
			get
			{
				return (this.lParam & 1073741824) > 0;
			}
		}

		public bool TransitionState
		{
			get
			{
				return (this.lParam & -2147483648) > 0;
			}
		}

		public CharacterEventArgs(char character, int lParam)
		{
			this.character = character;
			this.lParam = lParam;
		}
	}
}
