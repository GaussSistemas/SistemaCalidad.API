using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models.Comunicacion;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using static SistemaDeCalidad.API.Models.Comunicacion.HTTPResponse;

namespace SistemaDeCalidad.API.Services
{
    public static class EiffelService
    {
        public static T SendToDBFDatabase<T>(QueryExecutor queryExecutor) where T : class
        {

            var request = new SQLRequest()
            {
                Dsns = queryExecutor.DSNs,
                Parametros = queryExecutor.Parametros,
                Query = queryExecutor.Consulta
            };

            var url = $"{queryExecutor.URL}/QueryExecutor/Unitario";
            var response = Post(request, url);

            if (response != null && response.Success)
            {
                if (!typeof(T).IsConstructedGenericType)
                {
                    var deserializedData = JsonConvert.DeserializeObject<List<T>>(response.Response);
                    return deserializedData.FirstOrDefault();
                }
                else
                    return JsonConvert.DeserializeObject<T>(response.Response);
            }
            else
                throw new ArgumentException($"Status: {response.Status} mensaje: {response.Message} query: {request.Query}");
        }

        public static bool SendTransactionToDBFDatabase(List<(string statement, List<(String, Object, OdbcType)> parametros)> queries, string url, List<string> dsns)
        {
            var endpoint = $"{url}/QueryExecutor/EnTransaction";
            var requests = new List<SQLRequest>();
            foreach (var query in queries)
                requests.Add(new SQLRequest() { Query = query.statement, Dsns = dsns, Parametros = query.parametros });

            var response = Post(requests, endpoint);
            return response.Status == 200;
        }

        public static List<Column> RequestColumnsToDBFDatabase(string tabla, string url, List<string> dsns)
        {
            var endpoint = $"{url}/QueryExecutor/Columns";
            var request = new SQLRequest() { Dsns = dsns, Table = tabla };

            var response = Post(request, endpoint);

            if (response != null && response.Success && response.Estructura != null)
                return response.Estructura;
            else
                throw new ArgumentException($"Status: {response.Status} mensaje: {response.Message}");
        }

        private static HTTPResponse Post(SQLRequest request, string url)
        {
            try
            {
                var client = new RestClient(url);
                var postRequest = new RestRequest()
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(JsonConvert.SerializeObject(request));

                var apiResponse = client.Post(postRequest);
                if (apiResponse != null && apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(apiResponse.Content))
                    {
                        var res = Newtonsoft.Json.JsonConvert.DeserializeObject<HTTPResponse>(apiResponse.Content);
                        res.Response = Newtonsoft.Json.JsonConvert.SerializeObject(res.Data);
                        res.Success = true;
                        res.Status = 200;
                        return res;
                    }
                    else
                        return new HTTPResponse() { Response = "Empty data", Success = true, Status = 200 };
                }
                else
                    return new HTTPResponse() { Status = (int)apiResponse.StatusCode, Success = false, Message = string.IsNullOrEmpty(apiResponse.ErrorMessage) ? apiResponse.Content : apiResponse.ErrorMessage };
            }
            catch (HttpRequestException ex)
            {
                return new HTTPResponse() { Success = false, Message = "Ocurrió un error al comunicarse con la base del Eiffel. Inténtelo nuevamente, si el error persiste, comuníquese con Gauss Sistemas.", Status = 500 };
            }
        }

        private static HTTPResponse Post(List<SQLRequest> requests, string url)
        {
            try
            {
                var client = new RestClient(url);
                var postRequest = new RestRequest()
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(JsonConvert.SerializeObject(requests));

                var apiResponse = client.Post(postRequest);
                if (apiResponse != null && apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(apiResponse.Content))
                    {
                        var res = Newtonsoft.Json.JsonConvert.DeserializeObject<HTTPResponse>(apiResponse.Content);
                        return new HTTPResponse() { Response = Newtonsoft.Json.JsonConvert.SerializeObject(res.Data), Success = true, Status = 200 };
                    }
                    else
                        return new HTTPResponse() { Response = "Empty data", Success = true, Status = 200 };
                }
                else
                    return new HTTPResponse() { Status = (int)apiResponse.StatusCode, Success = false, Message = string.IsNullOrEmpty(apiResponse.ErrorMessage) ? apiResponse.Content : apiResponse.ErrorMessage };
            }
            catch (Exception ex)
            {
                return new HTTPResponse() { Success = false, Message = "Ocurrió un error al grabar la transacción. Comuníquese con Gauss Sistemas.", Status = 500 };
            }
        }

        public static (string statement, List<(String, Object, OdbcType)> parametros) InsertConCamposFaltantesConValoresNeutros(string tabla, List<(string column, object value, OdbcType type)> parametrosCargados, string apiTraductorUrl, List<string> dsns)
        {
            var columnasTabla = EiffelService.RequestColumnsToDBFDatabase(tabla, apiTraductorUrl, dsns);

            var parametros = new List<(string column, object value, OdbcType type)>();
            foreach (var columna in columnasTabla)
            {
                if (parametrosCargados.Any(parametro => parametro.column.ToLower() == columna.Name.ToLower()))
                    parametros.Add(parametrosCargados.First(parametro => parametro.column.ToLower() == columna.Name.ToLower()));
                else
                {
                    var type = new OdbcType();
                    object valorNeutro = new object();
                    switch (columna.DataType)
                    {
                        case Type decimalType when decimalType == typeof(decimal):
                            type = OdbcType.Decimal;
                            valorNeutro = 0;
                            break;
                        case Type stringType when stringType == typeof(string):
                            type = OdbcType.Text;
                            valorNeutro = columna.ValorNeutro;
                            break;
                        case Type boolType when boolType == typeof(bool):
                            type = OdbcType.Bit;
                            valorNeutro = ".F.";
                            break;
                        case Type dateType when dateType == typeof(DateTime):
                            type = OdbcType.Text;
                            valorNeutro = columna.ValorNeutro;
                            break;
                        default:
                            type = OdbcType.Text;
                            valorNeutro = columna.ValorNeutro;
                            break;
                    }
                    parametros.Add((columna.Name, valorNeutro, type));
                }
            }

            var columnas = string.Join(", ", parametros.Select(par => par.column).ToList());
            var values = string.Join(", ", parametros.Select(par => par.value).ToList());
            values = Regex.Replace(values, @"(\d+),(\d+)", "$1.$2");

            var statement = $"INSERT INTO {tabla} ({columnas}) VALUES ({values})";

            return (statement, new List<(string column, object value, OdbcType type)>());
        }

        public static List<(string statement, List<(string, object, OdbcType)> parametros)> ArmarStatements(List<(string tabla, List<(String, Object, OdbcType)> parametros)> parametros, string url, List<string> dsns)
        {
            var statements = new List<(string statement, List<(string, object, OdbcType)> parametros)>();

            foreach (var parametro in parametros)
                statements.Add(InsertConCamposFaltantesConValoresNeutros(parametro.tabla, parametro.parametros, url, dsns));

            return statements;
        }

        public static List<(string statement, List<(string, object, OdbcType)> parametros)> ArmarStatementUpdate(List<(string tabla, List<(string, object)> parametros, List<(string, object)> filtros)> parametros, string url, List<string> dsns)
        {
            var statements = new List<(string statement, List<(string, object, OdbcType)> parametros)>();

            foreach (var parametro in parametros)
                statements.Add(Update(parametro.tabla, parametro.parametros, parametro.filtros));

            return statements;
        }

        public static (string statement, List<(string, object, OdbcType)> parametros) Update(string tabla, List<(string column, object value)> parametros, List<(string column, object value)> filtros)
        {

            var columnasUpdate = string.Join(", ", parametros.Select(parametro => $"{parametro.column} = {parametro.value}").ToList());
            var filtrosUpdate = string.Join("AND ", filtros.Select(filtro => $"{filtro.column} = {filtro.value}").ToList());
            var statement = $"UPDATE {tabla} SET {columnasUpdate} WHERE {filtrosUpdate}";
            
            return (statement, new List<(string, object, OdbcType)>());
        }

        public static List<(string statement, List<(string, object, OdbcType)> parametros)> ArmarStatementDelete(List<(string tabla, List<(string, object)> filtros)> parametros)
        {
            var statements = new List<(string statement, List<(string, object, OdbcType)> parametros)>();

            foreach (var parametro in parametros)
                statements.Add(Delete(parametro.tabla, parametro.filtros));

            return statements;
        }

        public static (string statement, List<(string, object, OdbcType)> parametros) Delete(string tabla, List<(string column, object value)> filtros)
        {
            var filtrosUpdate = string.Join("AND ", filtros.Select(filtro => $"{filtro.column} = {filtro.value}").ToList());
            var statement = $"DELETE FROM {tabla} WHERE {filtrosUpdate}";

            return (statement, new List<(string, object, OdbcType)>());
        }

    }
}
