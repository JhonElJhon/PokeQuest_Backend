using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuestAPI.Classes
{
    public class Trivia
    {
        public string Pokemon { get; set; }
        public string Pregunta { get; set; }
        public List<string> Respuestas { get; set; }
        public string RespuestaCorrecta { get; set; }
        public string SpriteURL { get; set; }
    }
}
