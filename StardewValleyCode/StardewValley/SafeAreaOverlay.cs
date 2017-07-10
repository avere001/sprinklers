using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class SafeAreaOverlay : DrawableGameComponent
	{
		private SpriteBatch spriteBatch;

		private Texture2D dummyTexture;

		public SafeAreaOverlay(Game game) : base(game)
		{
			base.DrawOrder = 1000;
		}

		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(Game1.graphics.GraphicsDevice);
			this.dummyTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
			this.dummyTexture.SetData<Color>(new Color[]
			{
				Color.White
			});
		}

		public override void Draw(GameTime gameTime)
		{
			Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
			Rectangle titleSafeArea = viewport.TitleSafeArea;
			int num = viewport.X + viewport.Width;
			int num2 = viewport.Y + viewport.Height;
			Rectangle destinationRectangle = new Rectangle(viewport.X, viewport.Y, titleSafeArea.X - viewport.X, viewport.Height);
			Rectangle destinationRectangle2 = new Rectangle(titleSafeArea.Right, viewport.Y, num - titleSafeArea.Right, viewport.Height);
			Rectangle destinationRectangle3 = new Rectangle(titleSafeArea.Left, viewport.Y, titleSafeArea.Width, titleSafeArea.Top - viewport.Y);
			Rectangle destinationRectangle4 = new Rectangle(titleSafeArea.Left, titleSafeArea.Bottom, titleSafeArea.Width, num2 - titleSafeArea.Bottom);
			Color red = Color.Red;
			this.spriteBatch.Begin();
			this.spriteBatch.Draw(this.dummyTexture, destinationRectangle, red);
			this.spriteBatch.Draw(this.dummyTexture, destinationRectangle2, red);
			this.spriteBatch.Draw(this.dummyTexture, destinationRectangle3, red);
			this.spriteBatch.Draw(this.dummyTexture, destinationRectangle4, red);
			this.spriteBatch.End();
		}
	}
}
