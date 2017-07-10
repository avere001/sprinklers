using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableSprinklers.Configuration
{
    class ListConfigModel
    {
        public Dictionary<String, ConfigModel> Sprinklers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListConfigModel"/> class with default water patterns.
        /// </summary>
        public ListConfigModel()
        {
            Sprinklers = new Dictionary<string, ConfigModel> {
                { ConfigModel.BASIC_SPRINKLER, new ConfigModel(ConfigModel.BASIC_SPRINKLER) },
                { ConfigModel.QUALITY_SPRINKLER, new ConfigModel(ConfigModel.QUALITY_SPRINKLER) },
                { ConfigModel.IRIDIUM_SPRINKLER, new ConfigModel(ConfigModel.IRIDIUM_SPRINKLER) },
            };
        }
    }
}
