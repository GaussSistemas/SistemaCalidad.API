using System.Data;
using System.Data.Odbc;

namespace SistemaDeCalidad.API.Models.Comunicacion
{
    public class QueryExecutor
    {
        public string URL { get; set; }
        public List<string> DSNs { get; set; }
        public List<(string, object, OdbcType)> Parametros { get; set; }
        public string Consulta { get; set; }
    }
}
