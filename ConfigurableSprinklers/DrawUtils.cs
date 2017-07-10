using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableSprinklers
{
    public struct SpriteInfo
    {
        public Texture2D spriteSheet;
        public Rectangle spriteBounds;
        public Color tint;

        public SpriteInfo(Texture2D spriteSheet, int spriteXIndex, int spriteYIndex, Color tint)
        {
            this.spriteSheet = spriteSheet;
            this.spriteBounds = new Rectangle(spriteXIndex, spriteYIndex, 16, 16);
            this.tint = tint;
        }

        public static SpriteInfo RED_HIGHLIGHT;
        public static SpriteInfo GREEN_HIGHLIGHT;
        public static SpriteInfo BLUE_HIGHLIGHT;
        public static SpriteInfo GREY_HIGHLIGHT;

        const int GREEN_HIGHLIGHT_X_INDEX = 194;
        const int RED_HIGHLIGHT_X_INDEX = 210;
        const int HIGHLIGHT_Y_INDEX = 388;

        static SpriteInfo()
        {
            RED_HIGHLIGHT = new SpriteInfo(Game1.mouseCursors, RED_HIGHLIGHT_X_INDEX, HIGHLIGHT_Y_INDEX, Color.White);
            GREEN_HIGHLIGHT = new SpriteInfo(Game1.mouseCursors, GREEN_HIGHLIGHT_X_INDEX, HIGHLIGHT_Y_INDEX, Color.White);
            BLUE_HIGHLIGHT = new SpriteInfo(Game1.mouseCursors, GREEN_HIGHLIGHT_X_INDEX, HIGHLIGHT_Y_INDEX, Color.Blue);
            GREY_HIGHLIGHT = new SpriteInfo(Game1.mouseCursors, RED_HIGHLIGHT_X_INDEX, HIGHLIGHT_Y_INDEX, Color.Green);
        }
    }

    static class DrawUtils
    {


        /// <summary>
        /// Draws a highlight on the tile at the specified location.
        /// Adapted from <see cref="StardewValley.Object.drawPlacementBounds(SpriteBatch, GameLocation)"/>
        /// </summary>
        /// <param name="x">The x position of the tile.</param>
        /// <param name="y">The y position of the tile.</param>
        /// <param name="spriteIndex">Index of the sprite.</param>
        /// <param name="tint">The color to shade the texture with.</param>
        public static void DrawSprite(Vector2 tile, SpriteInfo sprite)
        {
            Vector2 viewportLocation = new Vector2(Game1.viewport.X, Game1.viewport.Y);
            Vector2 tileScreenLocation = (tile * Game1.tileSize) - viewportLocation;

            Game1.spriteBatch.Draw(
                Game1.mouseCursors,
                tileScreenLocation,
                sprite.spriteBounds,
                sprite.tint, 0.0f, Vector2.Zero,
                Game1.pixelZoom, SpriteEffects.None,
                0.01f);
        }

        /// <summary>
        /// Spawns a water animation.
        /// Adapted from <see cref="StardewValley.Object.DayUpdate(GameLocation)"/>
        /// </summary>
        /// <param name="temporarySprites">The temporary sprites.</param>
        /// <param name="tileLocation">The tile location.</param>
        /// <param name="offsetX">The x offset from <paramref name="tileLocation"/>>.</param>
        /// <param name="offsetY">The y offset from <paramref name="tileLocation"/>>.</param>
        /// <param name="animationDelay">The animation delay.</param>
        /// <param name="rotation">The rotation.</param>
        public static void SpawnWaterAnimation(List<TemporaryAnimatedSprite> temporarySprites, Vector2 tileLocation, float offsetX, float offsetY, int animationDelay, float rotation)
        {
            temporarySprites.Add(
                new TemporaryAnimatedSprite(
                    rowInAnimationTexture: 29,
                    position: tileLocation * Game1.tileSize + new Vector2(Game1.tileSize * offsetX, Game1.tileSize * offsetY),
                    color: Color.White * 0.5f,
                    animationLength: 4,
                    flipped: false,
                    animationInterval: 60f,
                    numberOfLoops: 100,
                    sourceRectWidth: -1, sourceRectHeight: -1,
                    layerDepth: -1f,
                    delay: animationDelay)
                {
                    id = (float)(tileLocation.X * 4000.0 + tileLocation.Y),
                    rotation = rotation
                }
            );
        }
    }
}
