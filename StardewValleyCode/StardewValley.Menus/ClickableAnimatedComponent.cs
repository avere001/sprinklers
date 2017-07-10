using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
	public class ClickableAnimatedComponent : ClickableComponent
	{
		public TemporaryAnimatedSprite sprite;

		public Rectangle sourceRect;

		public float baseScale;

		public string hoverText = "";

		private bool drawLabel;

		public ClickableAnimatedComponent(Rectangle bounds, string name, string hoverText, TemporaryAnimatedSprite sprite, bool drawLabel) : base(bounds, name)
		{
			this.sprite = sprite;
			this.sprite.position = new Vector2((float)bounds.X, (float)bounds.Y);
			this.baseScale = sprite.scale;
			this.hoverText = hoverText;
			this.drawLabel = drawLabel;
		}

		public ClickableAnimatedComponent(Rectangle bounds, string name, string hoverText, TemporaryAnimatedSprite sprite) : this(bounds, name, hoverText, sprite, true)
		{
		}

		public void update(GameTime time)
		{
			this.sprite.update(time);
		}

		public string tryHover(int x, int y)
		{
			if (this.bounds.Contains(x, y))
			{
				this.sprite.scale = Math.Min(this.sprite.scale + 0.02f, this.baseScale + 0.1f);
				return this.hoverText;
			}
			this.sprite.scale = Math.Max(this.sprite.scale - 0.02f, this.baseScale);
			return null;
		}

		public void draw(SpriteBatch b)
		{
			this.sprite.draw(b, true, 0, 0);
		}
	}
}
