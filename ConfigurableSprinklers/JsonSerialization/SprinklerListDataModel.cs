using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableSprinklers.JsonSerialization
{
    class SprinklerListDataModel
    {
        public SprinklerModel[] Sprinklers { get; set; }
        public SprinklerListDataModel(IEnumerable<SprinklerModel> sprinklerModels)
        {
            Sprinklers = sprinklerModels.ToArray();
        }

        public SprinklerListDataModel()
        {
            Sprinklers = new List<SprinklerModel>().ToArray();
        }
    }
}
