using EntityModel;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Security;
using System.Web.UI;

public partial class Login : FormPadre
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAcceder_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Usuario usuario = null;
            try
            {
                //Buscamos si existe un usuario con el nombre y contraseña proporcionada
                IQueryable<Usuario> usuarios =  new TransparenciaEntities().fn_VerificaUsuario(txtUsuario.Text, Encriptacion.CifrarCadena(txtPassword.Text));
                //Si encontramos algun resultado
                if (usuarios != null && usuarios.Count() > 0)
                {
                    //Obtenemos el primer registro
                    usuario = usuarios.FirstOrDefault();
                    if (usuario != null)
                    {
                        //Creamos la Sesion para el usuario
                        fnIniciaSesion(usuario);
                        //Redireccionamos a la pagina de inicio
                        FormsAuthentication.RedirectFromLoginPage(usuario.USU_NombreUsuario, chkRecordarme.Checked);
                    }
                    // De lo contrario, alertamos al usuario
                    else
                    {
                        lblErrorUsuario.Text = "Nombre de Usuario y/o Contraseña incorrectos.";
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}