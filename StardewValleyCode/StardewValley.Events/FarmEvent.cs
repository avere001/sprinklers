using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Events
{
	public interface FarmEvent
	{
		bool setUp();

		bool tickUpdate(GameTime time);

		void draw(SpriteBatch b);

		void drawAboveEverything(SpriteBatch b);

		void makeChangesToLocation();
	}
}
