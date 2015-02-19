<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>

    <asp:Content ID="Content2" ContentPlaceHolderID="Contenedor" Runat="Server">
        <div>
            <table>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
        <!-- <p><asp:Label ID="lblBienvenida" runat="server"></asp:Label></p> -->
        <!-- <asp:Button Text="Cerrar Sesion" runat="server" OnClick="Unnamed1_Click"/> -->
        <br />
        <br />
        <br />

        <div class="container">
            <div ui-view></div>
        </div>

        <link rel="stylesheet" type="text/css" href="Styles/bootstrap.min.css" />
        <link rel="stylesheet" type="text/css" href="Styles/font-awesome.min.css" />
        <link rel="stylesheet" type="text/css" href="Styles/angular-ui-tree.min.css" />
        <link rel="stylesheet" type="text/css" href="Styles/demo.css" />

        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>

        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

        <script type="text/javascript" src="Scripts/Angular/angular.min.js"></script>

        <script type="text/javascript" src="Scripts/Angular/angular-file/angular-file-upload-shim.js"></script>
        <script type="text/javascript" src="Scripts/Angular/angular-file/angular-file-upload-all.js"></script>


        <script type="text/javascript" src="Scripts/Angular/angular-ui-router.js"></script>
        <script type="text/javascript" src="Scripts/Angular/ui-bootstrap-tpls-0.12.0.min.js"></script>
        <script type="text/javascript" src="Scripts/AngularTree/angular-ui-tree.js"></script>

        <script type="text/javascript" src="Scripts/AngularConfig/app.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/controllers.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/routes.js"></script>
        <!--    <script type="text/javascript" src="Scripts/AngularConfig/shortcuts.js"></script>-->

        <!-- DIRECTIVAS -->
        <script type="text/javascript" src="Scripts/AngularConfig/directives/arbol.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/directives/detalles.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/directives/archivos.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/directives/descripcion.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/directives/file-manager.js"></script>
        <script type="text/javascript" src="Scripts/AngularConfig/directives/upload.js"></script>
        
        <!-- -->
        <%--<script type="text/javascript" src="Scripts/AngularFileUpload/angular-file-upload-all.js"></script>--%>
        <script type="text/javascript" src="Scripts/AngularFileUpload/angular-file-upload-shim.min.js"></script> 
        <script type="text/javascript" src="Scripts/AngularFileUpload/angular-file-upload.min.js"></script> 
        <%--<script type="text/javascript" src="Scripts/AngularConfig/directives/uploader.js"></script>--%>
    </asp:Content>
