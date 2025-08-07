namespace SistemaDeCalidad.API.Repositories.Queries
{
    public static class Soportes
    {
        public static string SoporteSegunId(string id)
        {

        var consulta = $"SELECT " +
                $"cabecera.id_tipo as tipoId, " +
                $"cabecera.numero, " +
                $"cabecera.nombre as Tarea, " +
                $"cabecera.id_cliente as clienteId, " +
                $"cabecera.nombreClie as nombreCliente, " +
                $"cabecera.usuario as usuarioCliente, " +
                $"cabecera.fecha, " +
                $"cabecera.fe_cierra as fechaResolucion, " +
                $"detalle.obs as Detalle, " +
                $"responsables.nombre as Responsable " +
                $"FROM ClavesSoportes " +
                $"INNER JOIN cabecera ON cabecera.id_tipo + STR(cabecera.numero, 10) = clavesSoportes.id_tipo + STR(clavesSoportes.numero, 10) " +
                $"INNER JOIN detalle ON detalle.id_tipo + STR(detalle.numero, 10) = cabecera.id_tipo + STR(cabecera.numero, 10) " +
                $"INNER JOIN responsables ON responsables.id_respon = cabecera.id_respon " +
                $"WHERE " +
                $"clavesSoportes.hash = '{id}' " +
                $"ORDER BY detalle.orden ASC " +
                $"TOP 1";

            return consulta;
        }
    }
}
