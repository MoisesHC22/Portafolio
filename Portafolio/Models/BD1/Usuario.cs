using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public int? ID_Especialidad { get; set; }
        public string? ImgUsuario { get; set; }

    }
}
