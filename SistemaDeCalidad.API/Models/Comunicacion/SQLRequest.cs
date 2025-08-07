using System.Data.Odbc;

namespace SistemaDeCalidad.API.Models.Comunicacion
{
    public class SQLRequest
    {
        public string Query { get; set; }
        public List<(string, object, OdbcType)> Parametros { get; set; }
        public string Table { get; set; }
        public List<string> Dsns { get; set; }
    }
}
