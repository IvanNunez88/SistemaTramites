namespace API.WEB.Features.Alumnos.Dominio.Dto;

public class CrearAlumnoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
}

public class ActualizarAlumnoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool IsActivo { get; set; } = true;
}
