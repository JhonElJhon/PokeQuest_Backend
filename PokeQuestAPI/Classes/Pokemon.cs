using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuestAPI.Classes
{
    public class Pokemon
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string[] Tipos { get; set; }
        public string SpriteURL { get; set; }
    }
}
