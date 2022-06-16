<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="GoDurban.Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <style>
        .error-template {
            padding: 40px 15px;
            text-align: center;
        }

        .error-actions {
            margin-top: 15px;
            margin-bottom: 15px;
        }

            .error-actions .btn {
                margin-right: 10px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="error-template">
                        <h1>Oops!</h1>
                        <h3>Sorry, an error occurred while processing your request.</h3>
                        <div class="error-actions">                           
                         
                        <a href="~/PL/Home" runat="server" id="Home" class="btn btn-default btn-lg"><span class="glyphicon glyphicon"></span><strong><< Go To Home Page</strong></a>
             
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <footer>
            <div class="copy-right-grids">
                <div class="container">
                    <div class="copy-left">
                        <p class="footer-gd">© 2017 Go Durban. All Rights Reserved. </p>
                    </div>
                </div>
            </div>
        </footer>

    </form>
</body>
</html>
<script src="Scripts/jquery-1.10.2.min.js"></script>
<script src="Scripts/bootstrap.js"></script>

