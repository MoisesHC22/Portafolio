using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class Cuenta
    {
        [Key]
        public int ID_Cuenta { get; set; }

        public string? Correo { get; set; }
        public string? Contrasena { get; set; }

        public int? ID_Usuario { get; set; }
    }
}
