using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class LightSource
	{
		public const int lantern = 1;

		public const int windowLight = 2;

		public const int tableLight = 3;

		public const int sconceLight = 4;

		public const int cauldronLight = 5;

		public const int indoorWindowLight = 6;

		public const int bigIndoorLight = 9;

		public const int maxLightsOnScreenBeforeReduction = 8;

		public const float reductionPerExtraLightSource = 0.03f;

		public const int playerLantern = -85736;

		public Texture2D lightTexture;

		public Vector2 position;

		public Color color;

		public float radius;

		public int identifier;

		public LightSource()
		{
		}

		public LightSource(Texture2D texture, Vector2 position, float radius, Color color)
		{
			this.lightTexture = texture;
			this.position = position;
			this.radius = radius;
			this.color = color;
		}

		public LightSource(int lightSource, Vector2 position, float radius, Color color)
		{
			this.loadTextureFromConstantValue(lightSource);
			this.position = position;
			this.radius = radius;
			this.color = color;
		}

		public LightSource(Texture2D texture, Vector2 position, float radius) : this(texture, position, radius, new Color(0, 131, 255), -1)
		{
		}

		public LightSource(Texture2D texture, Vector2 position, float radius, Color color, int identifier)
		{
			this.lightTexture = texture;
			this.position = position;
			this.radius = radius;
			this.color = color;
			this.identifier = identifier;
		}

		public LightSource(int texture, Vector2 position, float radius, Color color, int identifier)
		{
			this.loadTextureFromConstantValue(texture);
			this.position = position;
			this.radius = radius;
			this.color = color;
			this.identifier = identifier;
		}

		private void loadTextureFromConstantValue(int value)
		{
			switch (value)
			{
			case 1:
				this.lightTexture = Game1.lantern;
				return;
			case 2:
				this.lightTexture = Game1.windowLight;
				return;
			case 3:
				this.lightTexture = Game1.sconceLight;
				this.position.X = this.position.X + (float)(Game1.tileSize / 2);
				return;
			case 4:
				this.lightTexture = Game1.sconceLight;
				return;
			case 5:
				this.lightTexture = Game1.cauldronLight;
				return;
			case 6:
				this.lightTexture = Game1.indoorWindowLight;
				return;
			case 7:
			case 8:
				break;
			case 9:
				this.lightTexture = Game1.sconceLight;
				this.radius = 3f;
				break;
			default:
				return;
			}
		}

		public LightSource(int lightSource, Vector2 position, float radius)
		{
			this.loadTextureFromConstantValue(lightSource);
			this.position = position;
			this.radius = radius;
			this.color = Color.Black;
		}
	}
}
