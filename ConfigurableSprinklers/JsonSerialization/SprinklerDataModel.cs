using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using Microsoft.Xna.Framework;

namespace ConfigurableSprinklers.JsonSerialization
{
    class SprinklerModel
    {
        public String LocationName { get; set; }
        public Vector2[] SpotsToWater { get; set; }
        public Vector2 Position { get; set; }
    }
}
