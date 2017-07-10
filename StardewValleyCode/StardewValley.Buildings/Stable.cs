using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using System;

namespace StardewValley.Buildings
{
	public class Stable : Building
	{
		public Stable(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
		}

		public Stable()
		{
		}

		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height);
		}

		public override void load()
		{
			base.load();
			this.grabHorse();
		}

		public void grabHorse()
		{
			Horse horse = Utility.findHorse();
			if (horse == null)
			{
				Game1.getFarm().characters.Add(new Horse(this.tileX + 1, this.tileY + 1));
				return;
			}
			Game1.warpCharacter(horse, "Farm", new Point(this.tileX + 1, this.tileY + 1), false, true);
		}

		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft <= 0)
			{
				this.grabHorse();
			}
		}

		public override bool intersects(Rectangle boundingBox)
		{
			return base.intersects(boundingBox) && (boundingBox.X < (this.tileX + 1) * Game1.tileSize || boundingBox.Right >= (this.tileX + 3) * Game1.tileSize || boundingBox.Y <= (this.tileY + 1) * Game1.tileSize);
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
		}

		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
		}
	}
}
