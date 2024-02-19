using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class RedSocial
    {
        [Key]
        public int ID_RedSocial { get; set; }
        public string? UrlRedSocial { get; set; }


        public int? ID_Usuario { get; set; }
        public int? ID_TipoRedSocial { get; set; }
        public Usuario? Usuarios { get; set; }

    }
}
