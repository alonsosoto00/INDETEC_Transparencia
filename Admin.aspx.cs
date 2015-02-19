using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Services;
using EntityModel;
using System.Data.Entity.Core.Objects;
using System.Web.Http;
using System.Collections;

public partial class Admin : FormPadre
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        cierraSesion();
        
    }
    
    [WebMethod ]
    public static string fn_FormaArbol()
    {
        StringBuilder salida = new StringBuilder();
        //Buscamos el arbol de Transparencia
        List<NodoCatalogoTransparencia> arbol = fn_FormaArbol(new TransparenciaEntities().fn_GetArbolCatalogoTransparencia().ToList<ArbolCatalogoTransparencia>());
        //Obtenemos los hijos nodos del padre (ROOT)
        NodoCatalogoTransparencia nodoROOT = arbol.First<NodoCatalogoTransparencia>();
        //Formamos el JSON con el Arbol
        salida.Append(JsonConvert.SerializeObject(nodoROOT.Nodes));
        //Retornamos 
        return salida.ToString();
    }

    private static List<NodoCatalogoTransparencia> fn_FormaArbol(List<ArbolCatalogoTransparencia> nodos)
    {
        List<NodoCatalogoTransparencia> arbol = new List<NodoCatalogoTransparencia>();
        NodoCatalogoTransparencia nodo = new NodoCatalogoTransparencia();
        nodo.Id = "";
        nodo.Title = "ROOT";
        nodo.Aplica = true;
        nodo.Nodes = fn_FormaArbol(nodos, nodo.Id);
        //Agregamos el nodo al resultado
        arbol.Add(nodo);
        //Retornamos el Arbol
        return arbol;
    }

    private static List<NodoCatalogoTransparencia> fn_FormaArbol(List<ArbolCatalogoTransparencia> nodos, string nodoPadreId)
    {
        NodoCatalogoTransparencia nodo;
        ArbolCatalogoTransparencia nodoArbol;
        List<NodoCatalogoTransparencia> arbol = new List<NodoCatalogoTransparencia>();
        while (nodos.Count() > 0)
        {
            nodoArbol = nodos.First<ArbolCatalogoTransparencia>();
            //Si CT_NodoPadreId es igual a nodoPadreId, quiere decir que es hijo de nodoPadreId y lo agregamos como su hijo
            if (nodoArbol.CT_NodoPadreId.ToString().CompareTo(nodoPadreId) == 0)
            {
                //Eliminamos el primer elemento de la lista, para procesar el siguiente registro
                nodos.RemoveAt(0);
                //Creamos el nodo con su Id, Title y Nodes
                nodo = new NodoCatalogoTransparencia();
                nodo.Id = nodoArbol.CT_NodoId.ToString();
                nodo.Title = nodoArbol.CT_Descripcion;
                nodo.Aplica = nodoArbol.CT_Aplica;
                //Seguimos formando el Arbol
                nodo.Nodes = fn_FormaArbol(nodos, nodoArbol.CT_NodoId.ToString());
                //Agregamos el nodo al resultado
                arbol.Add(nodo);
            }
            //De lo contrario rompemos el ciclo para intentar con otro padre
            else
            {
                break;
            }
        }
        return arbol;
    }

    //(EnableSession = true)
    [WebMethod]
    public static void fn_EliminaNodo(string nodoId)
    {
        //if (IsAjaxRequest(Request))
        //{
            //string nodoId = "26CC0633-ACC7-4B5F-8057-16DFCEDECFF7";
            string modificadoPor = "4DEBBBE4-731F-4458-B2E1-7CF7DB623699";
            //Borramos el registro
            new TransparenciaEntities().sp_EliminaNodoCatalogoTransparencia(new Guid(nodoId), new Guid(modificadoPor));
            //Mostramos mensaje
            //lblNodoEliminado.InnerText = "Nodo Borrado";
        //}
    }

    [WebMethod]
    public static void fn_ActualizaDescripcionNodo(string nodoId, string descripcion)
    {
        //Actualizamos el registro en la base de datos
        new TransparenciaEntities().sp_ActualizaDescripcionNodoCatalogoTransparencia(new Guid(nodoId), descripcion, new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699"));
    }

    [WebMethod]
    public static void fn_ActualizaTextoNodo(string nodoId, string texto)
    {
        //Actualizamos el registro en la base de datos
        new TransparenciaEntities().sp_ActualizaTextoNodoCatalogoTransparencia(new Guid(nodoId), texto, new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699"));
    }

    [WebMethod]
    public static void fn_ActualizaRangoNodo(string nodoId, int rango, string tipoRangoId)
    {
        //Actualizamos el registro en la base de datos
        new TransparenciaEntities().sp_ActualizaRangoNodoCatalogoTransparencia(new Guid(nodoId), new Guid(tipoRangoId), rango, new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699"));
    }

    [WebMethod]
    public static void fn_ActualizaAplicaNodo(string nodoId, bool aplica)
    {
        //Actualizamos el registro en la base de datos
        new TransparenciaEntities().sp_ActualizaAplicaNodoCatalogoTransparencia(new Guid(nodoId), aplica, new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699"));
    }

    [WebMethod]
    public static void fn_ActualizaPrefijoNodo(string nodoId, string prefijo)
    {
        //Verificamos si ya existe un Mapeo para el Nodo que estamos editando
        MapeosReporte mapeo = new MapeosReporte().fn_BuscaMapeoByNodoId(new Guid(nodoId));
        //Si nos trajo NULL, quiere decir que aun no hay mapeo para el nodo y habria que crearlo
        if (mapeo == null)
        {
            //Verificamos si existe algun mapeo ya con ese prefijo
            mapeo = new MapeosReporte().fn_BuscaMapeoByPrefijo(prefijo);
            //Ya ya existiera, alertamos al usuario para que lo cambie
            if (mapeo != null) {
                throw new Exception("Ya existe un Mapeo con ese mismo Prefijo, favor de cambiarlo.");
            }
            //De lo contrario continuamos
            //Formamos la ruta Fisica donde se guardaran los reportes para el nuevo mapeo
            string ruta = new MapeosReporte().fn_GetRutaFisicaMapeoNuevos(new Guid(nodoId), prefijo);
            //Y Guardamos el nuevo mapeo en la base de datos
            ObjectParameter mapeoID = new ObjectParameter("mapeoID", typeof(System.Guid));
            new TransparenciaEntities().sp_CreaMapeoReportes(new Guid(nodoId), ruta, prefijo, mapeoID);
        }
        //Si ya existe un mapeo, verificamos que el nuevo prefijo que se le quiere dar no exista ya para otro mapeo
        else
        {
            //Verificamos si existe algun mapeo ya con ese prefijo
            mapeo = new MapeosReporte().fn_BuscaMapeoByPrefijo(prefijo);
            //Si nos trajo algo, verificamos que el mapeo no sea el mismo
            if (mapeo != null) {
                if (mapeo.MR_CT_NodoId.CompareTo(new Guid(nodoId)) != 0)
                {
                    throw new Exception("Mensaje: No se puede actualizar el prefijo. \n" + "Motivo:  Ya existe un Mapeo con ese mismo Prefijo, favor de cambiarlo.");
                }
            }
        }
        //Actualizamos el registro en la base de datos
        new TransparenciaEntities().sp_ActualizaPrefijoNodoCatalogoTransparencia(new Guid(nodoId), prefijo);
    }

    [WebMethod]
    public static void fn_ActualizaPosicionNodo(string nodoJSON) {
        //Creamos un Objeto CatalogoTransparencia en base a lo que recibimos del nodoJSON
        CatalogoTransparencia nodo = JsonConvert.DeserializeObject<CatalogoTransparencia>(nodoJSON);
        nodo.CT_USU_ModificadoPorId = new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699");
        new TransparenciaEntities().sp_ActualizaPosicionNodoCatalogoTransparencia(nodo.CT_NodoId, 
                                                                                  nodo.CT_NodoPadreId, 
                                                                                  nodo.CT_Orden, 
                                                                                  nodo.CT_USU_ModificadoPorId);
    }

    [WebMethod ]
    public static void fn_GuardaNuevoNodo(string nodoJSON)
    {
        //Creamos un Objeto CatalogoTransparencia en base a lo que recibimos del nodoJSON
        CatalogoTransparencia nodo = JsonConvert.DeserializeObject<CatalogoTransparencia>(nodoJSON);
        nodo.CT_USU_ModificadoPorId = new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699");
        new TransparenciaEntities().sp_CreaNodoCatalogoTransparencia(nodo.CT_NodoPadreId,
                                                                     nodo.CT_Descripcion,
                                                                     nodo.CT_Orden,
                                                                     nodo.CT_USU_ModificadoPorId,
                                                                     new ObjectParameter("nodoId", typeof(System.Guid)));
    }

    [WebMethod ]
    public static string fn_GetListadoArchivosNodo(string nodoId, bool mostrarFiltrados) {
        List<ListadoArchivos> archivos = new TransparenciaEntities().fn_GetListadoArchivos(new Guid(nodoId), mostrarFiltrados).ToList<ListadoArchivos>();
        if (archivos != null && archivos.Count() > 0) {
            List<Reporte> reportes = new List<Reporte>();
            foreach (ListadoArchivos archivo in archivos){
                Reporte reporte = fn_GetReporteByKey(reportes, archivo.CTR_DescripcionArchivo);
                //Si no se encontro un Nodo con el nombre de Reporte, lo agregamos
                if (reporte == null) {
                    reporte = new Reporte();
                    reporte.NombreReporte = archivo.CTR_DescripcionArchivo;
                    reportes.Add(reporte);
                }
                //Agregamos el Archivo a la lista
                NodoCatalogoReporte nodoReporte = new NodoCatalogoReporte();
                nodoReporte.Id = archivo.CTR_ReporteId;
                nodoReporte.Descripcion = archivo.CTR_DescripcionArchivo;
                nodoReporte.Extension = archivo.CTR_ExtensionArchivo;
                nodoReporte.RutaFisica = archivo.MR_RutaFisicaReporte;
                nodoReporte.Seleccionado = archivo.CTR_Selected;
                reporte.Reportes.Add(nodoReporte);
            }
            //Formamos el JSON
            StringBuilder salida = new StringBuilder();
            salida.Append(JsonConvert.SerializeObject(reportes));
            //Retornamos 
            return salida.ToString();
        }
        return null;
    }

    private static Reporte fn_GetReporteByKey(List<Reporte> reportes, string key) {
        foreach (Reporte reporte in reportes) {
            if (reporte.NombreReporte.CompareTo(key) == 0)
                return reporte;
        }
        return null;
    }

    [WebMethod]
    public static string fn_GetDatosNodo(string nodoId) {
        StringBuilder resultado = new StringBuilder();
        ArrayList datos = new CatalogoTransparencia().fn_GetDatosNodoCatalogoTransparencia(new Guid(nodoId));
        if (datos != null && datos.Count > 0)
        {
            resultado.Append(JsonConvert.SerializeObject(datos[0]));
        }
        return resultado.ToString();
    }

    [WebMethod]
    public static string fn_GetControlMaestroTiposRangos() {
        StringBuilder resultado = new StringBuilder();
        List<ControlesMaestrosTiposRango> controles = new TransparenciaEntities().fn_GetCMTiposRangos().ToList<ControlesMaestrosTiposRango>();
        resultado.Append(JsonConvert.SerializeObject(controles, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
        return resultado.ToString();    
    }

    /*
    public static bool IsAjaxRequest(HttpRequest request)
    {
        if (request["isAjax"] != null)
        {
            return true;
        }
        var page = HttpContext.Current.Handler as Page;
        if (request.HttpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase) && !page.IsPostBack)
        {
            return true;
        }
        if (request != null)
        {
            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
        return false;
    }
     */
}