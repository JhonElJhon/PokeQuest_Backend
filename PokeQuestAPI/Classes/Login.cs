using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PokeQuestAPI.Classes
{
    public class Login
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
