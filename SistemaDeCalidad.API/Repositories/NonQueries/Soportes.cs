using Microsoft.Extensions.Options;
using SistemaDeCalidad.API.Models.Soportes;
using System.Data.Odbc;

namespace SistemaDeCalidad.API.Repositories.NonQueries
{
    public static class Soportes
    {
        public static (string tabla, List<(string, object)> parametros, List<(string, object)> filtros) ParametrosUpdateClaveSoporte(ClaveSoporte claveSoporte)
        {
            var parametros = new List<(string, object)>()
            {
                ("id_estado", $"'{claveSoporte.EstadoId}'"),
            };

            var filtros = new List<(string, object)>()
            {
                ("Hash", $"'{claveSoporte.Hash}'"),
            };

            return (claveSoporte.NombreTabla(), parametros, filtros);
        }
    }
}
