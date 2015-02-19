using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de NodoCatalogoReporte
/// </summary>
public class NodoCatalogoReporte
{
    public System.Guid Id { get; set; }
    public string Descripcion { get; set; }
    public string RutaFisica { get; set; }
    public string Extension { get; set; }
    public bool Seleccionado { get; set; }

    public NodoCatalogoReporte() { }
    
}