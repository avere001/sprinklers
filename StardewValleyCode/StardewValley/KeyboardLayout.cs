using System;
using System.Runtime.InteropServices;
using System.Text;

namespace StardewValley
{
	public class KeyboardLayout
	{
		private const uint KLF_ACTIVATE = 1u;

		private const int KL_NAMELENGTH = 9;

		private const string LANG_EN_US = "00000409";

		private const string LANG_HE_IL = "0001101A";

		[DllImport("user32.dll")]
		private static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);

		[DllImport("user32.dll")]
		private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

		public static string getName()
		{
			StringBuilder expr_07 = new StringBuilder(9);
			KeyboardLayout.GetKeyboardLayoutName(expr_07);
			return expr_07.ToString();
		}
	}
}
