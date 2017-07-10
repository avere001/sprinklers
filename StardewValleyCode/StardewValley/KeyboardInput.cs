using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StardewValley
{
	public static class KeyboardInput
	{
		private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		private static bool initialized;

		private static IntPtr prevWndProc;

		private static KeyboardInput.WndProc hookProcDelegate;

		private static IntPtr hIMC;

		private const int GWL_WNDPROC = -4;

		private const int WM_KEYDOWN = 256;

		private const int WM_KEYUP = 257;

		private const int WM_CHAR = 258;

		private const int WM_IME_SETCONTEXT = 641;

		private const int WM_INPUTLANGCHANGE = 81;

		private const int WM_GETDLGCODE = 135;

		private const int WM_IME_COMPOSITION = 271;

		private const int DLGC_WANTALLKEYS = 4;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event CharEnteredHandler CharEntered;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event KeyEventHandler KeyDown;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event KeyEventHandler KeyUp;

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		public static void Initialize(GameWindow window)
		{
			if (KeyboardInput.initialized)
			{
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");
			}
			KeyboardInput.hookProcDelegate = new KeyboardInput.WndProc(KeyboardInput.HookProc);
			KeyboardInput.prevWndProc = (IntPtr)KeyboardInput.SetWindowLong(window.Handle, -4, (int)Marshal.GetFunctionPointerForDelegate(KeyboardInput.hookProcDelegate));
			KeyboardInput.hIMC = KeyboardInput.ImmGetContext(window.Handle);
			KeyboardInput.initialized = true;
		}

		private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = KeyboardInput.CallWindowProc(KeyboardInput.prevWndProc, hWnd, msg, wParam, lParam);
			if (msg <= 135u)
			{
				if (msg != 81u)
				{
					if (msg == 135u)
					{
						result = (IntPtr)(result.ToInt32() | 4);
					}
				}
				else
				{
					KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
					result = (IntPtr)1;
				}
			}
			else
			{
				switch (msg)
				{
				case 256u:
					if (KeyboardInput.KeyDown != null)
					{
						KeyboardInput.KeyDown(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 257u:
					if (KeyboardInput.KeyUp != null)
					{
						KeyboardInput.KeyUp(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 258u:
					if (KeyboardInput.CharEntered != null)
					{
						KeyboardInput.CharEntered(null, new CharacterEventArgs((char)((int)wParam), lParam.ToInt32()));
					}
					break;
				default:
					if (msg == 641u)
					{
						if (wParam.ToInt32() == 1)
						{
							KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
						}
					}
					break;
				}
			}
			return result;
		}
	}
}
