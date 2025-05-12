using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuestAPI.Classes
{
    public class Register
    {
        [Required(ErrorMessage = "El avatar es obligatorio")]
        public int Avatar { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
       
    }
}
