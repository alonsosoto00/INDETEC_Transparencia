using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de NodoCatalogoTransparencia
/// </summary>
public class NodoCatalogoTransparencia
{
    public string Id { get; set; }
    public string Title { get; set; }
    public bool Aplica { get; set; }
    public List<NodoCatalogoTransparencia> Nodes { get; set; }

    public NodoCatalogoTransparencia()
    {
        this.Nodes = new List<NodoCatalogoTransparencia>();
    }
}