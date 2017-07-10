using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;

namespace StardewValley
{
	public class Chunk
	{
		public Vector2 position;

		[XmlIgnore]
		public float xVelocity;

		[XmlIgnore]
		public float yVelocity;

		[XmlIgnore]
		public bool hasPassedRestingLineOnce;

		[XmlIgnore]
		public int bounces;

		public int debrisType;

		[XmlIgnore]
		public bool hitWall;

		public int xSpriteSheet;

		public int ySpriteSheet;

		[XmlIgnore]
		public float rotation;

		[XmlIgnore]
		public float rotationVelocity;

		public float scale;

		public float alpha;

		public Chunk()
		{
		}

		public Chunk(Vector2 position, float xVelocity, float yVelocity, int debrisType)
		{
			this.position = position;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.debrisType = debrisType;
			this.alpha = 1f;
		}
	}
}
