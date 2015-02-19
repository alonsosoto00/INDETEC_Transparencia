
<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;
using Newtonsoft.Json;
using EntityModel;

public class UploadHandler : FormPadre, IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.Files.Count > 0)
        {
            //Obtenemos los Archivos a Procesar
            HttpFileCollection files = context.Request.Files;
            
            //Obtenemos los datos para crear el Registro en BD
            var datos = context.Request.Form["data"];         
            
            //Deserealizamos los datos
            //Nota: Al deserealizar, La propiedad CatalogoTransparenciaReporte.CTR_MR_MapeoId no corresposponde al 
            //      ID del mapeo, sino al ID del Nodo con el que se esta trabajando. Nos sirve ese dato para despues
            //      buscar el ID del mapeo correcto.
            CatalogoTransparenciaReporte reporte = JsonConvert.DeserializeObject<CatalogoTransparenciaReporte>(datos);

            //Buscamos el ID del Mapeo al que corresponde el Nodo
            //al que le subiremos los Archivos
            MapeosReporte mapeo = new MapeosReporte().fn_BuscaMapeoByNodoId(reporte.CTR_MR_MapeoId);
            
            //Por cada Archivo que se va a subir, crear un Registro en BD
            for (int i = 0; i < files.Count; i++)
            {
                //Obtenemos el Archivo
                HttpPostedFile file = files[i];
                //Formamos el nombre del Archivo
                string fileName = mapeo.MR_Prefijo + "_" + (reporte.CTR_FechaInicio != null ? reporte.CTR_FechaInicio.Value.Year.ToString() + (reporte.CTR_FechaInicio.Value.Month < 10 ? "0" : "") + reporte.CTR_FechaInicio.Value.Month.ToString() + (reporte.CTR_FechaInicio.Value.Day < 10 ? "0" : "") + reporte.CTR_FechaInicio.Value.Day.ToString() + "_" : "") + (reporte.CTR_FechaFin.Year.ToString() + (reporte.CTR_FechaFin.Month < 10 ? "0" : "") + reporte.CTR_FechaFin.Month.ToString() + (reporte.CTR_FechaFin.Day < 10 ? "0" : "") + reporte.CTR_FechaFin.Day.ToString()) + System.IO.Path.GetExtension(file.FileName);
                //Formamos la ruta y nombre del Archivo
                string ruta = context.Server.MapPath("~/App_Data" + mapeo.MR_RutaFisicaReporte);
                //Verificamos si existe la ruta en donde queremos guardar el archivo
                if (!System.IO.Directory.Exists(ruta))
                {
                    //Si no existe, creamos los directorios
                    System.IO.Directory.CreateDirectory(ruta);
                }                
                //Guardamos el Archivo
                file.SaveAs(ruta + fileName);
                ////Guardamos el registro en la Base de Datos
                try
                {
                    EntityGeneral.BeginTransaction();
                    EntityGeneral.Entity.sp_CreaCatalogoTransparenciaReportes(mapeo.MR_MapeoId, fileName, reporte.CTR_DescripcionArchivo, reporte.CTR_FechaInicio, reporte.CTR_FechaFin, false, new Guid("4DEBBBE4-731F-4458-B2E1-7CF7DB623699"),true);
                    EntityGeneral.CommitTransaction();
                }
                catch (Exception ex) 
                {
                    //Damos Rollback a la transaccion
                    EntityGeneral.RollBackTransaction();
                    //Borramos el Archivo que se habia subido
                    if(System.IO.File.Exists(ruta + fileName)){
                        try
                        {
                            System.IO.File.Delete(ruta + fileName);
                        }
                        catch (Exception ioex) { }
                    }
                    throw new Exception("No se pudo guardar el archivo");
                }
            }
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write("File/s uploaded successfully!");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}