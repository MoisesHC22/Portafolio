namespace Portafolio.ViewModel
{
    public class CuentaVM
    {
        public int ID_Cuenta { get; set; }

        public string? Correo { get; set; }
        public string? Contrasena { get; set; }

        public int? ID_Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? ImgUsuario { get; set; }
        public int? ID_Especialidad { get; set; }
    }
}
