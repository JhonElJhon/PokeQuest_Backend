using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuestAPI.Classes
{
    public class User
    {
        public int ID { get; set; }
        public int Avatar { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public byte[] Contraseña { get; set; }
        public byte[] Sal { get; set; }
        public DateTime FechaInicio { get; set; }
        public int Puntaje { get; set; }
        public int Trivias { get; set; }
    }
}
