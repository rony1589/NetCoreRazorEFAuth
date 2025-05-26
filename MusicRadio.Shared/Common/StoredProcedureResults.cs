using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Shared.Common
{

    /// <summary>
    /// Clase base para resultados de procedimientos almacenados
    /// </summary>
    public class SpResult
    {
        /// <summary>
        /// Número de filas afectadas (positivo = éxito, negativo = código de error)
        /// </summary>
        public int RowsAffected { get; set; }

        /// <summary>
        /// Mensaje de error en caso de fallo
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Severidad del error (SQL Server)
        /// </summary>
        public int ErrorSeverity { get; set; }
    }

    /// <summary>
    /// Resultado con ID generado (para operaciones INSERT)
    /// </summary>
    public class SpResultWithId : SpResult
    {
        /// <summary>
        /// ID generado por la operación
        /// </summary>
        public int? Id { get; set; }
    }

    /// <summary>
    /// Códigos de error estándar para procedimientos almacenados
    /// </summary>
    public static class SpErrorCodes
    {
        public const int DuplicateRecord = -1;       // Registro duplicado
        public const int RecordNotFound = -2;        // Registro no encontrado
        public const int RelatedRecordNotFound = -3; // Registro relacionado no existe
        public const int DependentRecord = -4;       // Tiene registros dependientes
        public const int UnexpectedError = -99;      // Error inesperado

    }
}
