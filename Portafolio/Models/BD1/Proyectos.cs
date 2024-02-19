using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class Proyectos
    {
        [Key]
        public int ID_Proyectos { get; set; }
        public string? TituloProy { get; set; }
        public string? DescripcionProy { get; set; }
        public string? LinkProy { get; set; }
        public string? ImgProy { get; set; }

        public int? ID_Usuario { get; set; }
        public int? ID_Lenguaje { get; set; }
    }
}
