using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

/// <summary>
/// Descripción breve de FormPadre
/// </summary>
public class FormPadre : Page{

	public FormPadre(){	}

    protected override void OnInit(EventArgs e){
        base.OnInit(e);
        fnInicializaPagina();
    }

    private void fnInicializaPagina() {
        //if (usuario == null)
        //    Response.Redirect("Login.aspx");
    
    }

    protected void fnIniciaSesion(Usuario usuario){
        Session["usuario"] = usuario;
        //this.usuario = usuario;    
        //Session.
        //Sessionh
        //HttpContext.Current.s
    }

    public void cierraSesion(){
        Session.Abandon();
        FormsAuthentication.SignOut();
        Response.Redirect("Login.aspx");
    }

    public Usuario fnGetUsuario() {
        return (Usuario) Session["usuario"];
    }

}