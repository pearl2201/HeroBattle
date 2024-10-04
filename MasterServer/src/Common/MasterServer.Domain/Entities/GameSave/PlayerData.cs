using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.GameSave
{
    public class PlayerData
    {
        public string Key { get; set; }

        public Dictionary<string,object> Value { get; set; }
    }
}
