using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuestAPI.Enums
{
    /// <summary>
    /// Enum de los posibles resultados al intentar registrar un usuario
    /// </summary>
    public enum RegisterUserResultEnum
    {
        Created = 1,
        UserAlreadyExists = 2,
        EmailAlreadyExists = 3,
        InvalidPassword = 4,
        Error = 5,
    }
}
