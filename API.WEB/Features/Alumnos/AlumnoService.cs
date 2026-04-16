using Dapper;
using FluentValidation;
using API.WEB.Common;
using API.WEB.Data;
using API.WEB.Features.Alumnos.Dominio.Dto;
using API.WEB.Features.Alumnos.Dominio.Entidad;

namespace API.WEB.Features.Alumnos;

public class AlumnoService
{
    private readonly DbContext _db;
    private readonly IValidator<CrearAlumnoDTO> _crearValidator;
    private readonly IValidator<ActualizarAlumnoDTO> _actualizarValidator;

    public AlumnoService(
        DbContext db,
        IValidator<CrearAlumnoDTO> crearValidator,
        IValidator<ActualizarAlumnoDTO> actualizarValidator)
    {
        _db = db;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<ApiResponse<IEnumerable<Alumno>>> ObtenerTodos()
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT Matricula, Nombre, Apellidos, Tel, Correo, IsActivo, FecRegistro
                    FROM ALUMNO
                    ORDER BY Matricula";
        var alumnos = await connection.QueryAsync<Alumno>(sql);
        return ApiResponse<IEnumerable<Alumno>>.Ok(alumnos);
    }

    public async Task<ApiResponse<Alumno>> ObtenerPorMatricula(int matricula)
    {
        using var connection = _db.CreateConnection();
        var sql = @"SELECT Matricula, Nombre, Apellidos, Tel, Correo, IsActivo, FecRegistro
                    FROM ALUMNO
                    WHERE Matricula = @matricula";
        var alumno = await connection.QueryFirstOrDefaultAsync<Alumno>(sql, new { matricula });

        if (alumno is null)
            return ApiResponse<Alumno>.Error($"No se encontro el alumno con matricula {matricula}");

        return ApiResponse<Alumno>.Ok(alumno);
    }

    public async Task<ApiResponse<Alumno>> Crear(CrearAlumnoDTO dto)
    {
        var validacion = await _crearValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<Alumno>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();

        // Verificar duplicados ignorando acentos, espacios y caracteres especiales (´ ')
        var nombreNorm = dto.Nombre.Replace("'", "").Replace("´", "").Replace(" ", "");
        var apellidosNorm = dto.Apellidos.Replace("'", "").Replace("´", "").Replace(" ", "");

        var sqlDuplicado = @"
            SELECT COUNT(1) FROM ALUMNO
            WHERE REPLACE(REPLACE(REPLACE(Nombre, ' ', ''), '''', ''), '´', '')
                  COLLATE Latin1_General_CI_AI = @NombreNorm
              AND REPLACE(REPLACE(REPLACE(Apellidos, ' ', ''), '''', ''), '´', '')
                  COLLATE Latin1_General_CI_AI = @ApellidosNorm";

        var existe = await connection.QuerySingleAsync<int>(sqlDuplicado, new { NombreNorm = nombreNorm, ApellidosNorm = apellidosNorm });

        if (existe > 0)
            return ApiResponse<Alumno>.Error("Ya existe un alumno con el mismo nombre y apellidos");

        var sql = @"INSERT INTO ALUMNO (Nombre, Apellidos, Tel, Correo, IsActivo, FecRegistro)
                    VALUES (@Nombre, @Apellidos, @Tel, @Correo, 1, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

        var matricula = await connection.QuerySingleAsync<int>(sql, new
        {
            dto.Nombre,
            dto.Apellidos,
            dto.Tel,
            dto.Correo
        });

        return ApiResponse<Alumno>.Ok(new Alumno
        {
            Matricula = matricula,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Tel = dto.Tel,
            Correo = dto.Correo,
            IsActivo = true,
            FecRegistro = DateTime.Now
        }, "Alumno creado correctamente");
    }

    public async Task<ApiResponse<Alumno>> Actualizar(int matricula, ActualizarAlumnoDTO dto)
    {
        var validacion = await _actualizarValidator.ValidateAsync(dto);
        if (!validacion.IsValid)
            return ApiResponse<Alumno>.Error(validacion.Errors.First().ErrorMessage);

        using var connection = _db.CreateConnection();

        // Verificar duplicados ignorando acentos, espacios y caracteres especiales (´ ')
        var nombreNorm = dto.Nombre.Replace("'", "").Replace("´", "").Replace(" ", "");
        var apellidosNorm = dto.Apellidos.Replace("'", "").Replace("´", "").Replace(" ", "");

        var sqlDuplicado = @"
            SELECT COUNT(1) FROM ALUMNO
            WHERE REPLACE(REPLACE(REPLACE(Nombre, ' ', ''), '''', ''), '´', '')
                  COLLATE Latin1_General_CI_AI = @NombreNorm
              AND REPLACE(REPLACE(REPLACE(Apellidos, ' ', ''), '''', ''), '´', '')
                  COLLATE Latin1_General_CI_AI = @ApellidosNorm
              AND Matricula != @Matricula";

        var existe = await connection.QuerySingleAsync<int>(sqlDuplicado, new { NombreNorm = nombreNorm, ApellidosNorm = apellidosNorm, Matricula = matricula });

        if (existe > 0)
            return ApiResponse<Alumno>.Error("Ya existe un alumno con el mismo nombre y apellidos");

        var sql = @"UPDATE ALUMNO
                    SET Nombre = @Nombre,
                        Apellidos = @Apellidos,
                        Tel = @Tel,
                        Correo = @Correo,
                        IsActivo = @IsActivo
                    WHERE Matricula = @Matricula";

        var filas = await connection.ExecuteAsync(sql, new
        {
            Matricula = matricula,
            dto.Nombre,
            dto.Apellidos,
            dto.Tel,
            dto.Correo,
            dto.IsActivo
        });

        if (filas == 0)
            return ApiResponse<Alumno>.Error($"No se encontro el alumno con matricula {matricula}");

        return await ObtenerPorMatricula(matricula);
    }

    public async Task<ApiResponse<bool>> Eliminar(int matricula)
    {
        using var connection = _db.CreateConnection();
        var sql = "DELETE FROM ALUMNO WHERE Matricula = @matricula";
        var filas = await connection.ExecuteAsync(sql, new { matricula });

        if (filas == 0)
            return ApiResponse<bool>.Error($"No se encontro el alumno con matricula {matricula}");

        return ApiResponse<bool>.Ok(true, "Alumno eliminado correctamente");
    }
}
