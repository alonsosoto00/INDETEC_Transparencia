<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="Styles/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="Styles/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="Styles/AdminLTE.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
</head>
<body class="bg-white">
    <div class="form-box text-center" id="login-box">
        <form id="form1" runat="server">
            <%--        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" ValidationGroup="LoginUserValidationGroup"/>
            <fieldset>
                <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" ></asp:Login>
            </fieldset>   --%> 
            <a href="" class="logo">                
                <img src="Images/logo.png" />
            </a>
            <div class="text-left">
                <div class="header">Iniciar Sesión</div>

                <div class="body bg-gray">
                    <div class="form-group">
                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                            Usuario:
                        </div>
                        <div class="col-xs-8 col-sm-8 col-md-8 col-lg-8">
                            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtUsuarioValidator" runat="server" ErrorMessage="Usuario no puede ir vacio." ControlToValidate="txtUsuario" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br/>
                    <div class="form-group">
                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                            Contraseña: 
                        </div>
                        <div class="col-xs-8 col-sm-8 col-md-8 col-lg-8">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="txtContraseñaValidator" runat="server" ErrorMessage="Contraseña no puede ir vacia." ControlToValidate="txtPassword" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                        </div>
                    </div>          
                    <br/>
                    <div class="form-group">
                        <!--<input type="checkbox" name="remember_me"/> Recuérdame-->
                        <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6" style="font-weight:500px!important;">
                            <asp:CheckBox ID="chkRecordarme" runat="server" Text=" " /> Recordarme
                        </div>
                    </div>
                    <br/>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" />
                            <asp:Label ID="lblErrorUsuario" runat="server" Text=""></asp:Label>
                </div>
                <div class="footer bg-gray">                                                               
                    <asp:Button ID="btnAcceder" class="btn bg-primary btn-block" runat="server" Text="Acceder" OnClick="btnAcceder_Click" />
                    
                </div>
            </div>
        </form>
    </div>
</body>
</html>

