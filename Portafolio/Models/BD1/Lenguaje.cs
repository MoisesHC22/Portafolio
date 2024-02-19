using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class Lenguaje
    {
        [Key]
        public int ID_Lenguaje { get; set; }
        public string? NombreLenguaje { get; set; }
    }
}
