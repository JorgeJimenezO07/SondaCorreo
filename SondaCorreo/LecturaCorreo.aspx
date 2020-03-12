<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LecturaCorreo.aspx.cs" Inherits="SondaCorreo.LecturaCorreo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sonda Correo</title>
    <link href="css/reset.css" rel="stylesheet" />
    <link href="css/fonts.css" rel="stylesheet" />
    <link href="css/fontawesome-all.min.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
</head>
<body>
    <div class="limiter">
        <div class="container">
            <div class="wrap">
                <span class="title">
                    <img src="img/logoetb.png" />
                    <label>Sonda Correo</label>
                    <div class="wrap-input" id="cont-loading">
                    </div>
                    <div class="wrap-input">
                        <button type="button" class="btn" onclick="clearTimeout(time)">Detener</button>
                    </div>
                </span>
            </div>
            <footer>
                <p class="credits"><%= DateTime.Now.Date.Year %> © ETB S.A. ESP. Todos los derechos reservados.</p>
            </footer>
        </div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="js/siteJs.js"></script>
</body>
</html>