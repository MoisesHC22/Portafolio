using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models.BD1
{
    public class TipRedSocial
    {
        [Key]
        public int ID_TipoRedSocial { get; set; }
        public string? NombreTipoRedSocial { get; set; }
    }
}
