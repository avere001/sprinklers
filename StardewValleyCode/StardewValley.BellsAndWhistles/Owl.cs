using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
	public class Owl : Critter
	{
		public Owl()
		{
		}

		public Owl(Vector2 position)
		{
			this.baseFrame = 83;
			this.position = position;
			this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 32, 32);
			this.startingPosition = position;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(83, 100),
				new FarmerSprite.AnimationFrame(84, 100),
				new FarmerSprite.AnimationFrame(85, 100),
				new FarmerSprite.AnimationFrame(86, 100)
			});
		}

		public override bool update(GameTime time, GameLocation environment)
		{
			Vector2 value = new Vector2((float)Game1.viewport.X - Game1.previousViewportPosition.X, (float)Game1.viewport.Y - Game1.previousViewportPosition.Y) * 0.15f;
			this.position.Y = this.position.Y + (float)time.ElapsedGameTime.TotalMilliseconds * 0.2f;
			this.position.X = this.position.X + (float)time.ElapsedGameTime.TotalMilliseconds * 0.05f;
			this.position -= value;
			return base.update(time, environment);
		}

		public override void draw(SpriteBatch b)
		{
		}

		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f + this.position.X / 100000f, 0, 0, Color.MediumBlue, this.flip, 4f, 0f, false);
		}
	}
}
