using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using Microsoft.Xna.Framework;

namespace ConfigurableSprinklers
{
    static class Utils
    {
        public static IMonitor Monitor { get; set; }
        public static IModHelper Helper { get; set; }
        public static ModEntry Mod { get; set; }
        public static IManifest Manifest { get; set; }

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <returns>The mouse position in pixels</returns>
        public static Vector2 GetMousePos()
        {
            /** BEGIN: Copyright of Stardew Valley author **/
            int x = Game1.getOldMouseX() + Game1.viewport.X;
            int y = Game1.getOldMouseY() + Game1.viewport.Y;
            if (Game1.mouseCursorTransparency == 0.0)
            {
                x = (int)Game1.player.GetGrabTile().X * Game1.tileSize;
                y = (int)Game1.player.GetGrabTile().Y * Game1.tileSize;
            }
            if (Game1.player.GetGrabTile().Equals(Game1.player.getTileLocation()) && Game1.mouseCursorTransparency == 0.0)
            {
                Vector2 translatedVector2 = Utility.getTranslatedVector2(Game1.player.GetGrabTile(), Game1.player.facingDirection, 1f);
                x = (int)translatedVector2.X * Game1.tileSize;
                y = (int)translatedVector2.Y * Game1.tileSize;
            }
            /** END: Copyright of Stardew Valley author **/

            return new Vector2(x, y);
        }

        /// <summary>
        /// Get the current tile that the mouse is over.
        /// </summary>
        /// <returns>The current tile position</returns>
        public static Vector2 GetMouseTile()
        {
            return GetTilePos(GetMousePos());
        }

        /// <summary>
        /// Get the tile that contains <paramref name="pixelPos"/>.
        /// </summary>
        /// <param name="pixelPos">The position in pixels</param>
        /// <returns>The position in tiles</returns>
        public static Vector2 GetTilePos(Vector2 pixelPos)
        {
            return new Vector2((int)pixelPos.X / Game1.tileSize, (int)pixelPos.Y / Game1.tileSize);
        }

        public static List<Vector2> GetTilesBetween(Vector2 startTile, Vector2 endTile)
        {

            var intermediates = new Queue<Vector2>();
            var currTile = startTile;

            int dx = (int)Math.Abs(endTile.X - startTile.X);
            int dy = (int)Math.Abs(endTile.Y - startTile.Y);
            
            // represents next collision
            // -: horizontal, +: vertical, 0: corner 
            int error = dx - dy; 
            int initialError = error;

            // Ensures we go in the correct direction
            int xIncrement = (endTile.X > startTile.X) ? 1 : -1;
            int yIncrement = (endTile.Y > startTile.Y) ? 1 : -1;

            while (currTile != endTile)
            {
                if (currTile != startTile)
                    intermediates.Enqueue(currTile);
                
                if (error > 0 && initialError > 0)
                {
                    currTile.X += xIncrement;
                    error -= dy;
                }
                else if (error < 0 && initialError < 0)
                {
                    currTile.Y += yIncrement;
                    error += dx;
                }
                else
                {
                    currTile.X += xIncrement;
                    currTile.Y += yIncrement;
                    error += dx - dy;
                }
            }

            return intermediates.ToList();
        }

        /// <summary>
        /// Gets the subset of <paramref name="waterTiles"/> containing only tiles that are valid tiles to for the sprinkler to water.
        /// A tile is valid if all tiles between it and the sprinkler are in <paramref name="waterTiles"/>.
        /// </summary>
        /// <param name="sprinklerTile">The sprinkler position.</param>
        /// <param name="waterTiles">The set of tiles that a sprinkler intends to later water.</param>
        /// <returns>
        /// A subset of <paramref name="waterTiles"/> containing only tiles that are valid tiles to for the sprinkler to water. 
        /// This is not a "proper" subset; That is, the return may contain exactly the same tiles as <paramref name="waterTiles"/>.
        /// </returns> 
        public static HashSet<Vector2> GetValidWaterTiles(Vector2 sprinklerTile, IEnumerable<Vector2> waterTiles)
        {
            var validTiles = new HashSet<Vector2>(waterTiles);
            var invalidTiles = new HashSet<Vector2>();
            var loopCount = 0;
            do
            {
                validTiles.ExceptWith(invalidTiles);
                invalidTiles.Clear();

                foreach (var tile in validTiles)
                {
                    loopCount++;
                    if (!IsValidWaterTile(sprinklerTile, tile, validTiles))
                    {
                        invalidTiles.Add(tile);
                    }
                }

            } while (invalidTiles.Count > 0);

            return validTiles;
        }

        /// <summary>
        /// Checks if <paramref name="testTile"/> can be added as a tile to be watered
        /// </summary>
        /// <param name="sprinklerTile">The sprinkler position.</param>
        /// <param name="testTile">The tile we are interested in adding to the set of tiles to water.</param>
        /// <param name="waterTiles">the tiles that the sprinkler already waters</param>
        /// <param name="considerExistingAsNew">if set to <c>true</c>, checks whether a tile is valid to add ignoring whether it already exists.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="testTile"/> can be added to the set of tiles the sprinkler waters; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///   This is an adaption of the "All-Integer Math" section of "Raytracing on a grid" by James McNeill
        ///   http://playtechs.blogspot.ca/2007/03/raytracing-on-grid.html
        /// </remarks>
        public static bool IsValidWaterTile(Vector2 sprinklerTile, Vector2 testTile, IEnumerable<Vector2> waterTiles)
        {
            // TODO using this method may make it easier to accidently delete squares unintentionally, so color squares that will be removed, too.
            // TODO perhaps autofill the space between where the user clicks and the sprinkler?

            return waterTiles.Contains(testTile) && sprinklerTile != testTile &&
                   Utils.GetTilesBetween(sprinklerTile, testTile).All(tile => waterTiles.Contains(tile));
        }
    }


}
