using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableSprinklers.JsonSerialization
{
    static class DataManager
    {
        /// <summary>
        /// Gets the data file associated with the current save.
        /// </summary>
        /// <param name="isTmp">Whether we want the .tmp file or the .json file</param>
        /// <returns>The data file associated with the current save</returns>
        public static String GetDataFile(bool isTmp = false)
        {
            /* BEGIN: From StardewValley.SaveGame.ensureFolderStructExists */
            string pathSafeName = Game1.player.Name;
            foreach (char c in pathSafeName)
            {
                if (!char.IsLetterOrDigit(c))
                    pathSafeName = pathSafeName.Replace(c.ToString() ?? "", "");
            }

            string dataFolder = pathSafeName + "_" + Game1.uniqueIDForThisGame;
            /* END: From StardewValley.SaveGame */

            return Path.Combine(dataFolder, "data" + (isTmp ? ".tmp" : ".json"));
        }


        public static void LoadSprinklers()
        {
            ConvertAllSprinklersTo<SprinklerObject>();

            var model = Utils.Helper.ReadJsonFile<SprinklerListDataModel>(GetDataFile()) ?? new SprinklerListDataModel();

            foreach (var sprinklerModel in model.Sprinklers)
            {
                GameLocation location = Game1.getLocationFromName(sprinklerModel.LocationName);

                location.objects.TryGetValue(sprinklerModel.Position, out StardewValley.Object obj);
                if (obj != null && obj is SprinklerObject sprinkler)
                {
                    sprinkler.WaterTiles = new HashSet<Vector2>(sprinklerModel.SpotsToWater);
                }

            }
        }

        public static void TentativelySaveSprinklers()
        {
            var sprinklerModels = from loc in Game1.locations
                                  from obj in loc.objects
                                  let sprinkler = obj.Value as SprinklerObject
                                  where sprinkler != null
                                  select new SprinklerModel()
                                  {
                                      LocationName = loc.name,

                                      SpotsToWater = sprinkler.WaterTiles.ToArray(),
                                      Position = sprinkler.tileLocation,
                                  }; ;

            var sprinklerListModel = new SprinklerListDataModel(sprinklerModels);

            // saves to a .tmp file so that if stardew valley saving fails our mod won't have mismatched data
            Utils.Helper.WriteJsonFile(GetDataFile(isTmp: true), sprinklerListModel);

            ConvertAllSprinklersTo<StardewValley.Object>();
        }

        public static void ConfirmSaveSprinklers()
        {
            string datafile = Path.Combine(Utils.Helper.DirectoryPath, GetDataFile());
            string tmpDataFile = Path.Combine(Utils.Helper.DirectoryPath, GetDataFile(isTmp: true));

            // delete the old data file if it exists
            try { File.Delete(datafile); }
            catch (IOException) { }

            // The tmp file should always exist here
            File.Move(tmpDataFile, datafile);
            
        }

        /// <summary>
        /// Convert all sprinklers to <typeparamref name="TObject"/>
        /// </summary>
        /// <remarks>
        /// Checks all locations in case a mod allows someone to place sprinklers somewhere invalid.
        /// </remarks>
        /// <typeparam name="TObject">SprinklerObject or StardewValley.Object</typeparam>
        private static void ConvertAllSprinklersTo<TObject>() where TObject : StardewValley.Object, new()
        {
            // Convert all sprinklers back to generic Stardew Valley objects for saving
            // Checks all locations in case a mod allows someone to place them somewhere invalid
            foreach (var loc in Game1.locations)
            {
                var objs = loc.objects;
                foreach (var key in objs.Keys.ToList())
                {
                    objs[key] = SprinklerObject.GetSprinklerObjectAs<TObject>(objs[key]);
                }
            }
        }
    }
}
