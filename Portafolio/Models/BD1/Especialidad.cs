using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class Especialidad
    {
        [Key]
        public int ID_Especialidad { get; set; }
        public string? NombreEspecialidad { get; set; }
    }
}
