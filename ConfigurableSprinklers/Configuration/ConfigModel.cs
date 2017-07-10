using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StardewValley;

namespace ConfigurableSprinklers.Configuration
{
    internal class ConfigModel
    {
        /// <summary>The maximum number of tiles this sprinkler can water</summary>
        /// <example>
        /// <code language="js" title="JSON">
        /// {
        ///     "Quality Sprinkler":
        ///     {
        ///         "MaxWaterableTiles": 8
        ///         "WaterPattern": 
        ///         [
        ///             "--X--",
        ///             "--X--",
        ///             "XX*XX",
        ///             "--X--",
        ///             "--X--"
        ///         ]
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// defaults:
        /// <list type="bullet">
        ///     <item>
        ///         <term>Sprinkler</term>
        ///         <description>4</description>
        ///     </item>
        ///     <item>
        ///         <term>Quality Sprinkler</term>
        ///         <description>8</description>
        ///     </item>
        ///     <item>
        ///         <term>Iridium Sprinkler</term>
        ///         <description>24</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public int MaxWaterableTiles { get; set; }

        /// <summary>A user friendly representation of the spots that this sprinkler type waters by default</summary>
        /// <example>
        /// <code language="js" title="JSON">
        /// {
        ///     "Quality Sprinkler":
        ///     {
        ///         "MaxWaterableTiles": 8
        ///         "WaterPattern": 
        ///         [
        ///             "--X--",
        ///             "--X--",
        ///             "XX*XX",
        ///             "--X--",
        ///             "--X--"
        ///         ]
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// <para>Each string in the list must be the same length as the others</para>
        /// <list type="table">
        ///     <listheader><term>Valid Characters</term><description>Description</description></listheader>
        ///     <item>
        ///         <term>*</term>
        ///         <description>Represents the sprinkler. There must be exactly one sprinkler.</description>
        ///     </item>
        ///     <item>
        ///         <term>X</term>
        ///         <description>Represents a spot to be watered. In order to be valid, it must be next to the sprinkler or another valid water spot.</description>
        ///     </item>
        ///     <item>
        ///         <term>-</term>
        ///         <description>Represents a spot that won't be watered and is not the sprinkler.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public String[] WaterPattern { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <remarks>
        /// Ignored by Json since it is error prone and confusing to have the name as both
        /// the key to this json entry and as a field of the entry.
        /// </remarks>
        /// <value>
        /// The name of the sprinkler. <see cref="StardewValley.Object.Name"/>.
        /// </value>
        [JsonIgnore]
        public string Name { get; set; }


        /// <summary>
        /// Gets the spots to water as vectors relative to a sprinkler at the origin.
        /// </summary>
        /// <remarks>
        /// The first access of this property initializes the field from WaterPattern.
        /// Ignored by Json serializer since this is a poor way to represent the spots to be watered to the user.
        /// </remarks>
        /// <value>
        /// The spots to water.
        /// </value>
        [JsonIgnore]
        public HashSet<Vector2> SpotsToWater {
            get
            {
                if (_spotsToWater == null)
                {
                    _spotsToWater = GetWaterPatternAsVectors();
                }
                return _spotsToWater;
            }
        }

        /// <summary>
        /// The spots to water. <see cref="SpotsToWater"/>
        /// </summary>
        [JsonIgnore]
        private HashSet<Vector2> _spotsToWater = null;
        
        public static readonly String BASIC_SPRINKLER = "Sprinkler";
        public static readonly String QUALITY_SPRINKLER = "Quality Sprinkler";
        public static readonly String IRIDIUM_SPRINKLER = "Iridium Sprinkler";
        public static readonly String[] DEFAULT_SPRINKLERS = new[] { BASIC_SPRINKLER, QUALITY_SPRINKLER, IRIDIUM_SPRINKLER };

        public static readonly Vector2[] adjVectors = new[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1) };


        /// <summary>
        /// Do not use this constructor. Needed only for JSON deserialization.
        /// </summary>
        public ConfigModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigModel"/> class.
        /// Uses the original Stardew Valley pattern for a sprinkler of type <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the sprinkler. Must be a valid Stardew Valley sprinkler name (an element of <see cref="DEFAULT_SPRINKLERS"/>).</param>
        public ConfigModel(String name)
        {
            Name = name;
            if (Name == BASIC_SPRINKLER)
            {
                MaxWaterableTiles = 4;
                WaterPattern = new String[] {
                    "-X-",
                    "X*X",
                    "-X-"
                };
            }
            else if (Name == QUALITY_SPRINKLER)
            {
                MaxWaterableTiles = 8;
                WaterPattern = new String[]
                {
                    "XXX",
                    "X*X",
                    "XXX"
                };
            }
            else if (Name == IRIDIUM_SPRINKLER)
            {
                MaxWaterableTiles = 24;
                WaterPattern = new String[]
                {
                    "XXXXX",
                    "XXXXX",
                    "XX*XX",
                    "XXXXX",
                    "XXXXX"
                };
            }
            else
            {
                MaxWaterableTiles = 0;
                WaterPattern = new String[] { };
            }
        }

        /// <summary>
        /// Gets the water pattern as vectors.
        /// </summary>
        /// <returns>A set of vectors representing the spots to be watered by a sprinkler at the origin</returns>
        private HashSet<Vector2> GetWaterPatternAsVectors()
        {
            // Ensure pattern has consistent string lengths
            if (!WaterPattern.All(s => s.Length == WaterPattern.First().Length))
            {
                // The user must fix the configuration file
                Utils.Monitor.Log($"All strings in default WaterPattern of '{Name}' must be the same length." +
                    $"You can use '-' to represent a spot that should not be watered by the sprinkler", LogLevel.Alert);
                MaxWaterableTiles = 0;
                return new HashSet<Vector2>();
            }

            var pattern = new HashSet<Vector2>();
            Vector2 sprinkler = new Vector2();
            bool sprinklerExists = false;

            // Creates a set of vectors assuming the sprinkler is in the upper left corner of WaterPattern.
            // Also finds the location of the sprinkler.
            int x = 0;
            foreach (var row in WaterPattern)
            {
                int y = 0;
                foreach (var c in row)
                {
                    if (c == 'X')
                        pattern.Add(new Vector2(x, y));
                    else if (c == '*')
                    {
                        if (sprinklerExists)
                        { 
                            Utils.Monitor.Log($"Multiple sprinkler characters ('*') in default WaterPattern of '{Name}'. Treating addtion sprinkler as '-'", LogLevel.Warn);
                        }
                        else
                        {
                            sprinkler = new Vector2(x, y);
                            sprinklerExists = true;
                        }
                    }
                    else if (c == '-')
                    {
                        // represents spot to NOT water, so do nothing
                    }
                    else
                    {
                        // unexpected character
                        Utils.Monitor.Log($"Unexpected character '{c}' in default WaterPattern of '{Name}'. Treating it as '-'", LogLevel.Warn);
                    }

                    y++;
                }
                x++;
            }

            // Make sure a sprinkler was found in the WaterPattern
            if (!sprinklerExists)
            {

                ConfigModel defaultModel = new ConfigModel(Name);
                if (defaultModel.MaxWaterableTiles > 0)
                {
                    // This sprinkler is a sprinkler in vanilla, so use the default vanilla pattern
                    // We should still warn the user to fix their config. But at least their sprinklers Will still work
                    Utils.Monitor.Log($"No sprinkler defined ('*') in default WaterPattern of '{Name}'. Using default vanilla configuration for {Name}", LogLevel.Warn);
                    MaxWaterableTiles = defaultModel.MaxWaterableTiles;
                    WaterPattern = defaultModel.WaterPattern;
                    return defaultModel.SpotsToWater;
                }
                else
                {
                    // There is no "default" pattern for this sprinkler.
                    // The user must fix the configuration before they can use the sprinkler
                    Utils.Monitor.Log($"No sprinkler defined ('*') in default WaterPattern of '{Name}'. " +
                                      $"This must be fixed before you can use this sprinkler", LogLevel.Alert);

                    MaxWaterableTiles = 0;
                    return new HashSet<Vector2>();
                }

            }

            if (pattern.Count < MaxWaterableTiles)
            {
                Utils.Monitor.Log($"Number of spots to water (X) in WaterPattern of '{Name}' was fewer than MaxWaterableTiles", LogLevel.Warn);
            }
            else if (pattern.Count > MaxWaterableTiles)
            {
                Utils.Monitor.Log($"Number of spots to water (X) in WaterPattern of '{Name}' was greater than MaxWaterableTiles", LogLevel.Warn);
            }

            // Get the spots to water relative to the sprinklers position
            // TODO: test to make sure subtraction happens in the expected order
            var candidatePattern = new HashSet<Vector2> (pattern.Select(spot => new Vector2(spot.X - sprinkler.X, spot.Y - sprinkler.Y)));
            sprinkler = new Vector2(0, 0);
            
            pattern = Utils.GetValidWaterTiles(sprinkler, candidatePattern);
            if (pattern.Count < candidatePattern.Count)
            {
                Utils.Monitor.Log($"The WaterPattern for '{Name}' was not valid. All water (i.e. the pattern can't skip over spaces)", LogLevel.Warn);
            }

            MaxWaterableTiles = Math.Max(pattern.Count, MaxWaterableTiles);

            return pattern;
        }
    }
}