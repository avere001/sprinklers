using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableSprinklers.Configuration
{
    class Config
    {
        public static List<String> ValidSprinklers { get; set; }
        public static Dictionary<String, int> MaxSpotsToWater { get; set; }
        public static Dictionary<String, HashSet<Vector2>> DefaultSpotsToWater { get; set; }

        internal static void Load()
        {
            ListConfigModel config = Utils.Helper.ReadConfig<ListConfigModel>();
            ValidSprinklers = config.Sprinklers.Keys.ToList();
            MaxSpotsToWater = config.Sprinklers.ToDictionary(e => e.Key, e => e.Value.MaxWaterableTiles);
            foreach (var e in config.Sprinklers) e.Value.Name = e.Key;
            DefaultSpotsToWater = config.Sprinklers.ToDictionary(e => e.Key, e => e.Value.SpotsToWater);
        }

        internal static bool IsValidSprinkler(string sprinkler)
        {
            if (sprinkler == null)
                return false;

            return ValidSprinklers.Contains(sprinkler) && MaxSpotsToWater[sprinkler] > 0;
        }

        internal static void UpdateDefaultSpotsToWater(SprinklerObject value)
        {
            DefaultSpotsToWater[value.name] = new HashSet<Vector2>(from spot in value.WaterTiles
                                                                   select Vector2.Subtract(spot, value.TileLocation));
        }
    }
}
