using EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de MapeosReporte
/// </summary>

namespace EntityModel
{
    public partial class MapeosReporte : EntityGeneral
    {
        public MapeosReporte fn_BuscaMapeoByNodoId(Guid nodoId)
        {
            TransparenciaEntities entity = new TransparenciaEntities();
            MapeosReporte mapeo = null;
            var query =
                from mapeoReporte in entity.MapeosReportes
                where mapeoReporte.MR_CT_NodoId == nodoId
                select new
                {
                    MR_MapeoId = mapeoReporte.MR_MapeoId,
                    MR_RutaFisicaReporte = mapeoReporte.MR_RutaFisicaReporte,
                    MR_CT_NodoId = mapeoReporte.MR_CT_NodoId,
                    MR_Prefijo = mapeoReporte.MR_Prefijo
                    //CT_NodoId = catalogoTransparencia.CT_NodoId
                };
            if (query.Count() > 0) {
                //Obtenemos el primer registro que nos haya traido la consulta
                var mapeoReporte = query.First();
                //Inicializamos el Objeto a Regresar
                mapeo = new MapeosReporte();
                //Asignamos todas sus propiedades
                mapeo.MR_MapeoId = mapeoReporte.MR_MapeoId;
                mapeo.MR_RutaFisicaReporte = mapeoReporte.MR_RutaFisicaReporte;
                mapeo.MR_CT_NodoId = mapeoReporte.MR_CT_NodoId;
                mapeo.MR_Prefijo = mapeoReporte.MR_Prefijo;
            }
            return mapeo;
        }

        public MapeosReporte fn_BuscaMapeoByPrefijo(string prefijo)
        {
            TransparenciaEntities entity = new TransparenciaEntities();
            MapeosReporte mapeo = null;
            var query =
                from mapeoReporte in entity.MapeosReportes
                where mapeoReporte.MR_Prefijo == prefijo
                select new
                {
                    MR_MapeoId = mapeoReporte.MR_MapeoId,
                    MR_RutaFisicaReporte = mapeoReporte.MR_RutaFisicaReporte,
                    MR_CT_NodoId = mapeoReporte.MR_CT_NodoId,
                    MR_Prefijo = mapeoReporte.MR_Prefijo
                };
            if (query.Count() > 0)
            {
                //Obtenemos el primer registro que nos haya traido la consulta
                var mapeoReporte = query.First();
                //Inicializamos el Objeto a Regresar
                mapeo = new MapeosReporte();
                //Asignamos todas sus propiedades
                mapeo.MR_MapeoId = mapeoReporte.MR_MapeoId;
                mapeo.MR_RutaFisicaReporte = mapeoReporte.MR_RutaFisicaReporte;
                mapeo.MR_CT_NodoId = mapeoReporte.MR_CT_NodoId;
                mapeo.MR_Prefijo = mapeoReporte.MR_Prefijo;
            }
            return mapeo;
        }

        public string fn_GetRutaFisicaMapeoNuevos(Guid nodoId, string prefijo)
        {
            //Consulta
            string sqlQuery = "SELECT [dbo].[fn_GetRutaFisicaMapeoNuevo] ({0},{1})";
            //Parametros
            var nodoIdParameter = new ObjectParameter("nodoId", nodoId);
            var prefijoParameter = new ObjectParameter("prefijo", prefijo);
            Object[] parameters = { nodoIdParameter.Value, prefijoParameter.Value };
            //Ejecutar Consulta
            DbRawSqlQuery<string> resultado = Entity.Database.SqlQuery<string>(sqlQuery, parameters);
            //Resultado
            return resultado.FirstOrDefault();
        }
    }
}