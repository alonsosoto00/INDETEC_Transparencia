using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Reportes
/// </summary>
public class Reporte
{
    public string NombreReporte { get; set; }

    public List<NodoCatalogoReporte> Reportes { get; set; }

    public Reporte()
	{
        Reportes = new List<NodoCatalogoReporte>();
    }
}