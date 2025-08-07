using Microsoft.Extensions.Options;
using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Models.Comunicacion;
using SistemaDeCalidad.API.Models.Soportes;
using SistemaDeCalidad.API.Services;
using System.Data.Odbc;

namespace SistemaDeCalidad.API.Repositories
{
    public class SoportesRepository : ISoportesRepository
    {
        private readonly SistemaDeCalidadConfiguration _sgcConfigurations;
        public SoportesRepository(IOptions<SistemaDeCalidadConfiguration> sgcConfigurations)
        {
            _sgcConfigurations = sgcConfigurations.Value;
        }

        public bool FinalizarSoporte(ClaveSoporte claveSoporte)
        {
            var parametros = new List<(string tabla, List<(string, object)> parametros, List<(string, object)> filtros)>();
            parametros.Add(NonQueries.Soportes.ParametrosUpdateClaveSoporte(claveSoporte));

            var statements = EiffelService.ArmarStatementUpdate(parametros, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);
            var resultado = EiffelService.SendTransactionToDBFDatabase(statements, _sgcConfigurations.SistemaDeCalidadQueryExecutorURL, _sgcConfigurations.SistemaDeCalidadDSNs);

            return resultado;
        }

        public Soporte RecuperarSoporte(string id)
        {
            var queryExecutor = new QueryExecutor()
            {
                Consulta = Queries.Soportes.SoporteSegunId(id),
                DSNs = _sgcConfigurations.SistemaDeCalidadDSNs,
                URL = _sgcConfigurations.SistemaDeCalidadQueryExecutorURL,
                Parametros = new List<(string, object, OdbcType)>()
            };
            var soporte = EiffelService.SendToDBFDatabase<Soporte>(queryExecutor);
            return soporte;
        }
    }
}
