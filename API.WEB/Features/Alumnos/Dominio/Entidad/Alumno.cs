namespace API.WEB.Features.Alumnos.Dominio.Entidad;

public class Alumno
{
    public int Matricula { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool IsActivo { get; set; }
    public DateTime FecRegistro { get; set; }
}
