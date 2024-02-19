using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class InfHome
    {
        [Key]
        public int ID_InfHome { get; set; }
        public string? TituloDeBienvenida { get; set; }
        public string? DescripcionHome { get; set; }

        public int? ID_Usuario { get; set; }
    }
}
