using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley
{
	public class CosmeticDebris : TemporaryAnimatedSprite
	{
		public const float gravity = 0.3f;

		public const float bounciness = 0.45f;

		private new Vector2 position;

		private new float rotation;

		private float rotationSpeed;

		private float xVelocity;

		private float yVelocity;

		private new Rectangle sourceRect;

		private int groundYLevel;

		private int disappearTimer;

		private int lightTailLength;

		private int timeToDisappearAfterReachingGround;

		private new Color color;

		private Cue tapSound;

		private new LightSource light;

		private Queue<Vector2> lightTail;

		private Texture2D texture;

		public CosmeticDebris(Texture2D texture, Vector2 startingPosition, float rotationSpeed, float xVelocity, float yVelocity, int groundYLevel, Rectangle sourceRect, Color color, Cue tapSound, LightSource light, int lightTailLength, int disappearTime)
		{
			this.timeToDisappearAfterReachingGround = disappearTime;
			this.disappearTimer = this.timeToDisappearAfterReachingGround;
			this.texture = texture;
			this.position = startingPosition;
			this.rotationSpeed = rotationSpeed;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.sourceRect = sourceRect;
			this.groundYLevel = groundYLevel;
			this.color = color;
			this.tapSound = tapSound;
			this.light = light;
			if (lightTailLength > 0)
			{
				this.lightTail = new Queue<Vector2>();
				this.lightTailLength = lightTailLength;
			}
		}

		public override bool update(GameTime time)
		{
			this.yVelocity += 0.3f;
			this.position += new Vector2(this.xVelocity, this.yVelocity);
			this.rotation += this.rotationSpeed;
			if (this.position.Y >= (float)this.groundYLevel)
			{
				this.position.Y = (float)(this.groundYLevel - 1);
				this.yVelocity = -this.yVelocity;
				this.yVelocity *= 0.45f;
				this.xVelocity *= 0.45f;
				this.rotationSpeed *= 0.225f;
				if (Game1.soundBank != null && !this.tapSound.IsPlaying)
				{
					this.tapSound = Game1.soundBank.GetCue(this.tapSound.Name);
					this.tapSound.Play();
				}
				this.disappearTimer--;
			}
			if (this.disappearTimer < this.timeToDisappearAfterReachingGround)
			{
				this.disappearTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.disappearTimer <= 0)
				{
					return true;
				}
			}
			return false;
		}

		public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
		{
			spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, this.position), new Rectangle?(this.sourceRect), this.color, this.rotation, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.groundYLevel + 1) / 10000f);
		}
	}
}
