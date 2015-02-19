using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatalogoTransparencia
/// </summary>

namespace EntityModel
{
    public partial class CatalogoTransparencia : EntityGeneral
    {
        public ArrayList fn_GetDatosNodoCatalogoTransparencia(Guid nodoId){
            ArrayList datos = null;
            var query =
                from catalogoTransparencia in Entity.CatalogoTransparencias
                join mp in Entity.MapeosReportes on catalogoTransparencia.CT_NodoId equals mp.MR_CT_NodoId 
                into LMapeosReporte from mapeosReporte in LMapeosReporte.DefaultIfEmpty()
                where catalogoTransparencia.CT_NodoId == @nodoId
                select new
                {
                    catalogoTransparencia.CT_NodoId,
                    catalogoTransparencia.CT_Texto,
                    catalogoTransparencia.CT_CMTR_TipoRangoId,
                    catalogoTransparencia.CT_Rango,
                    catalogoTransparencia.CT_Sistema,
                    catalogoTransparencia.CT_Aplica,
                    mapeosReporte.MR_Prefijo
                };
            if (query.Count() > 0) {
                datos = new ArrayList(query.ToList());
            }
            return datos;
        }
    }
}