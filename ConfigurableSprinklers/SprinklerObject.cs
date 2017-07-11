using System;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ConfigurableSprinklers.Configuration;

namespace ConfigurableSprinklers
{

    public class SprinklerObject : StardewValley.Object
    {
        public long ConfigStartTime { get => Utils.Mod.LastPlacementTime; }

        internal HashSet<Vector2> _spotsToWater;
        public HashSet<Vector2> WaterTiles {

            // set to the default pattern if _spotsToWater is unset by JSON loader
            get
            {
                if (_spotsToWater == null) 
                    _spotsToWater = new HashSet<Vector2>(from spot in Config.DefaultSpotsToWater[Name]
                                                         select spot + tileLocation);
                return _spotsToWater;
            }

            set => _spotsToWater = value; }

        internal int SpareSpotsToWater { get => Config.MaxSpotsToWater[name] - WaterTiles.Count; }
        
        // Note: sprinklers are determined by their names in the config, not their id.
        // These ids only used to ensure that strange objects added to the config (like soda machines) call their original DayUpdate method as well
        const int BASIC_SPRINKLER_ID = 599;
        const int QUALITY_SPRINKLER_ID = 621;
        const int IRIDIUM_SPRINKLER_ID = 645;
        readonly int[] realSprinklerIds = { BASIC_SPRINKLER_ID, QUALITY_SPRINKLER_ID, IRIDIUM_SPRINKLER_ID };

        /**
         * Drawing methods
         **/
        /// <summary>
        /// Draws the configuration squares.
        /// </summary>
        public void DrawConfigurationSquares()
        {
            Vector2 mouseTile = Utils.GetMouseTile();

            // Cause spinkler square to blink
            // TODO: use the timespan object directly instead of always making a new one
            if (new TimeSpan(Game1.currentGameTime.TotalGameTime.Ticks - ConfigStartTime).Milliseconds < 500 || mouseTile == TileLocation)
            {
                DrawUtils.DrawSprite(TileLocation, SpriteInfo.GREEN_HIGHLIGHT);
            }
            
            // Draw the appropriate colored highlight under the mouse cursor
            if (WaterTiles.Contains(mouseTile))
            {

                var newWaterTiles = GetValidWaterTilesWithoutTile(mouseTile);
                foreach (var tile in WaterTiles)
                {
                    if (newWaterTiles.Contains(tile)) {
                        DrawUtils.DrawSprite(tile, SpriteInfo.GREEN_HIGHLIGHT);
                    }
                    else
                    {
                        DrawUtils.DrawSprite(tile, SpriteInfo.GREY_HIGHLIGHT);
                    }
                } 
            }
            else
            {
                var allTilesNeeded = GetNewWaterTilesNeeded(mouseTile);
                var addableTilesNeeded = allTilesNeeded.Take(SpareSpotsToWater);
                var unaddableTilesNeeded = allTilesNeeded.Skip(SpareSpotsToWater);

                foreach (var tile in addableTilesNeeded) DrawUtils.DrawSprite(tile, SpriteInfo.BLUE_HIGHLIGHT);
                foreach (var tile in unaddableTilesNeeded) DrawUtils.DrawSprite(tile, SpriteInfo.RED_HIGHLIGHT);
                foreach (var tile in WaterTiles) DrawUtils.DrawSprite(tile, SpriteInfo.GREEN_HIGHLIGHT);
            }
        }

        /**
         * Sprinkler configuration mode methods
         **/

        public HashSet<Vector2> GetValidWaterTilesWithoutTile(Vector2 tile)
        {
            return Utils.GetValidWaterTiles(TileLocation, WaterTiles.Where(e => e != tile));
        }

        /// <summary>
        /// Called when the mouse is clicked while this sprinkler is being configured.
        /// Adds/Removes tiles from WaterTiles as appropriate
        /// </summary>
        public virtual void OnClickDuringConfig()
        {
            Vector2 mouseTile = Utils.GetMouseTile();
            
            if (WaterTiles.Remove(mouseTile))
            {
                var validSpots = Utils.GetValidWaterTiles(TileLocation, WaterTiles);
                WaterTiles = validSpots;
            }
            // TODO: add all tiles that can be added, not just the one on the cursor
            else if (CanAddWaterTile(mouseTile))
            {
                var newTiles = GetNewWaterTilesNeeded(mouseTile);
                WaterTiles.UnionWith(newTiles);
            }
        }

        /// <summary>
        /// Determines whether this spot can be added to WaterTiles.
        /// </summary>
        /// <param name="newTile">The tile we are interested in</param>
        /// <returns>
        ///   <c>true</c> if this instance can add the specified spot; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAddWaterTile(Vector2 newTile)
        {
            if (newTile.Equals(tileLocation) || WaterTiles.Contains(newTile))
                return false;

            var newTilesNeeded = GetNewWaterTilesNeeded(newTile);
            return SpareSpotsToWater >= newTilesNeeded.Count;
            
        }

        public List<Vector2> GetNewWaterTilesNeeded(Vector2 newTile)
        {
            List<Vector2> newSpotsNeeded = Utils.GetTilesBetween(tileLocation, newTile);
            newSpotsNeeded = newSpotsNeeded.Except(WaterTiles).ToList();
            newSpotsNeeded.Add(newTile);

            return newSpotsNeeded;
        }

        /// <summary>
        /// Convert <paramref name="objFrom"/> to type <typeparamref name="TObjTo"/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <typeparam name="TObjTo">The type of the new object. Either <c>SprinklerObject</c> or <c>StardewValley.Object</c></typeparam>
        /// <param name="objFrom">The object to be converted.</param>
        /// <returns>A new instance of type <typeparamref name="TObjTo"/> that has the fields of <paramref name="objFrom"/>.</returns>
        private static StardewValley.Object ConvertTo<TObjTo>(StardewValley.Object objFrom) where TObjTo : StardewValley.Object, new()
        {
            Type tObject = typeof(StardewValley.Object);
            Type[] validTypes = { tObject, typeof(SprinklerObject) };

            // note: using "objFrom is TObjTo" would prevent converting SprinklerObject back to StardewValley.Object
            if (objFrom.GetType() == typeof(TObjTo))
            {
                // No need to convert if it's already the right type
                return objFrom;
            }
            if (!validTypes.Contains(objFrom.GetType()))
            {
                // FIXME: Users may want to use objects from mods as their sprinkler. Currently not possible.
                Utils.Monitor.Log($"As of this time, {objFrom.Name} cannot be used as a sprinkler.\n" +
                    $"Please contact the mod author of {Utils.Mod.ModManifest.Name} " +
                    $"(or preferably submit a feature request on this projects github) " +
                    $"if you would like this object to be supported", StardewModdingAPI.LogLevel.Warn);

                return objFrom;
            }

            // This following code was adapted from https://stackoverflow.com/questions/8631898/c-sharp-inheritance-derived-class-from-base-class.
            TObjTo objTo = new TObjTo();
            foreach (FieldInfo fieldInf in tObject.GetFields())
            {
                if (!(fieldInf.IsLiteral && !fieldInf.IsInitOnly))
                    fieldInf.SetValue(objTo, fieldInf.GetValue(objFrom));
            }
            foreach (PropertyInfo propInf in tObject.GetProperties())
            {
                if (propInf.CanWrite)
                    propInf.SetValue(objTo, propInf.GetValue(objFrom));
            }
            return objTo;
        }

        /// <summary>
        /// Water the tiles based on <see cref="SpotsToWater"/> and animate it.
        /// Also resets the health of the sprinkler back to 10.
        /// </summary>
        /// <remarks>
        /// We override the DayUpdate method so that the game uses our code on sprinklers instead of <see cref="StardewValley.Object.DayUpdate(GameLocation)"/>
        /// Honestly not sure why the author of Stardew Valley doesn't already do this. He must really like switch statements.
        /// </remarks>
        /// <param name="location">The location (e.g. Farm) that this sprinkler is in</param>
        public override void DayUpdate(GameLocation location)
        {

            if (!realSprinklerIds.Contains(parentSheetIndex))
            {
                // reset the health of this sprinkler
                health = 10;
            }
            else
            {
                // This object is not traditionally a sprinkler. Call it's original DayUpdate, too.
                // TODO: FIXME currently will break any object that is a subtype of SprinklerObject.
                base.DayUpdate(location);
            }

            // If the sprinkler is inside then rain can't water it, so water whether it is raining or not
            // And if it's not raining, then we need to water it no matter what.
            if (!Game1.isRaining || !location.isOutdoors)
                
            {
                
                foreach (Vector2 spotToWater in WaterTiles)
                {
                    location.terrainFeatures.TryGetValue(spotToWater, out TerrainFeature terrainFeature);

                    if (terrainFeature is HoeDirt hoeDirt)
                        hoeDirt.state = 1;
#if DEBUG
                    int sprinklerDelay = 0;
#else
                    int sprinklerDelay = Game1.random.Next(1000);
#endif

                    // https://stackoverflow.com/questions/2276855/xna-2d-vector-angles-whats-the-correct-way-to-calculate
                    Vector2 spotFromSprinkler = Vector2.Subtract(spotToWater, tileLocation);
                    float degree = (float) Math.Atan2(spotFromSprinkler.X, -spotFromSprinkler.Y);

                    // TODO make animation look proper
                    DrawUtils.SpawnWaterAnimation(location.temporarySprites, spotToWater, 0.0f, 0.0f, sprinklerDelay, rotation: degree);
                }

                
            }
        }

        public static StardewValley.Object GetSprinklerObjectAs<TReturn>(StardewValley.Object obj) where TReturn : StardewValley.Object, new ()
        {
            if (Config.IsValidSprinkler(obj.name))
            {
                return ConvertTo<TReturn>(obj);
            }
            return obj;
        }
    }
}
