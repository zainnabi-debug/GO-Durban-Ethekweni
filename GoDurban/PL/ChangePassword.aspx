<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="GoDurban.PL.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" href="//netdna.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css">

    <script type="text/javascript">

        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

        function ValidateBusinessEmail(txtEmail) {

            if (!filter.test(txtEmail.value)) {
                txtEmail.style.borderColor = "red";
                // alert("Invalid Email Address");
                $('#feedbackEmailStatus').remove();
                $("#Idstatus").append("<div id='feedbackEmailStatus'><center><font color='red'>Invalid Email Address </font></center></div>");
                txtEmail.value = null;
                //document.getElementById("txtEmail").focus();
                return false;
            }
            else {

                $('#feedbackEmailStatus').remove();

            }
        }

        function ValidateEmailID(txtEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (txtEmail.value == "") {
                txtEmail.style.border = "";
                return true;
            }
            else if (filter.test(txtEmail.value)) {
                txtEmail.style.border = "";
                return true;
            }
            else {

                txtEmail.style.borderColor = "red";
                return false;
            }
        }





        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function capLock(e) {
            kc = e.keyCode ? e.keyCode : e.which;
            sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
            else
                document.getElementById('divMayus').style.visibility = 'hidden';
        }
    </script>

    <style>
        @import url(https://fonts.googleapis.com/css?family=Roboto:400,300,100,700,500);

        /*body {
            padding-top: 90px;
            background: #F7F7F7;
            color: #666666;
            font-family: 'Roboto', sans-serif;
            font-weight: 100;
        }*/

        body {
            width: 100%;
            /*background: -webkit-linear-gradient(left, #22d686, #24d3d3, #22d686, #24d3d3);
  background: linear-gradient(to right, #22d686, #24d3d3, #22d686, #24d3d3);*/
            background-size: 600% 100%;
            -webkit-animation: HeroBG 20s ease infinite;
            animation: HeroBG 20s ease infinite;
        }

        @-webkit-keyframes HeroBG {
            0% {
                background-position: 0 0;
            }

            50% {
                background-position: 100% 0;
            }

            100% {
                background-position: 0 0;
            }
        }

        @keyframes HeroBG {
            0% {
                background-position: 0 0;
            }

            50% {
                background-position: 100% 0;
            }

            100% {
                background-position: 0 0;
            }
        }


        .panel {
            border-radius: 5px;
        }

        label {
            font-weight: 300;
        }

        .panel-login {
            border: none;
            -webkit-box-shadow: 0px 0px 49px 14px rgba(188,190,194,0.39);
            -moz-box-shadow: 0px 0px 49px 14px rgba(188,190,194,0.39);
            box-shadow: 0px 0px 49px 14px rgba(188,190,194,0.39);
        }

            .panel-login .checkbox input[type=checkbox] {
                margin-left: 0px;
            }

            .panel-login .checkbox label {
                padding-left: 25px;
                font-weight: 300;
                display: inline-block;
                position: relative;
            }

            .panel-login .checkbox {
                padding-left: 20px;
            }

                .panel-login .checkbox label::before {
                    content: "";
                    display: inline-block;
                    position: absolute;
                    width: 17px;
                    height: 17px;
                    left: 0;
                    margin-left: 0px;
                    border: 1px solid #cccccc;
                    border-radius: 3px;
                    background-color: #fff;
                    -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                }

                .panel-login .checkbox label::after {
                    display: inline-block;
                    position: absolute;
                    width: 16px;
                    height: 16px;
                    left: 0;
                    top: 0;
                    margin-left: 0px;
                    padding-left: 3px;
                    padding-top: 1px;
                    font-size: 11px;
                    color: #555555;
                }

                .panel-login .checkbox input[type="checkbox"] {
                    opacity: 0;
                }

                    .panel-login .checkbox input[type="checkbox"]:focus + label::before {
                        outline: thin dotted;
                        outline: 5px auto -webkit-focus-ring-color;
                        outline-offset: -2px;
                    }

                    .panel-login .checkbox input[type="checkbox"]:checked + label::after {
                        font-family: 'FontAwesome';
                        content: "\f00c";
                    }

            .panel-login > .panel-heading .tabs {
                padding: 0;
            }

            .panel-login h2 {
                font-size: 20px;
                font-weight: 300;
                margin: 30px;
            }

            .panel-login > .panel-heading {
                color: #848c9d;
                background-color: #e8e9ec;
                border-color: #fff;
                text-align: center;
                border-bottom-left-radius: 5px;
                border-bottom-right-radius: 5px;
                border-top-left-radius: 0px;
                border-top-right-radius: 0px;
                border-bottom: 0px;
                padding: 0px 15px;
            }

            .panel-login .form-group {
                padding: 0 30px;
            }

            .panel-login > .panel-heading .login {
                padding: 20px 30px;
                /*border-bottom-leftt-radius: 5px;*/
            }

            .panel-login > .panel-heading .register {
                padding: 20px 30px;
                background: #2d3b55;
                border-bottom-right-radius: 5px;
            }

            .panel-login > .panel-heading a {
                text-decoration: none;
                color: #666;
                font-weight: 300;
                font-size: 16px;
                -webkit-transition: all 0.1s linear;
                -moz-transition: all 0.1s linear;
                transition: all 0.1s linear;
            }

                .panel-login > .panel-heading a#register-form-link {
                    color: #fff;
                    width: 100%;
                    text-align: right;
                }

                .panel-login > .panel-heading a#login-form-link {
                    width: 100%;
                    text-align: left;
                }

            .panel-login input[type="text"], .panel-login input[type="email"], .panel-login input[type="password"] {
                height: 45px;
                border: 0;
                font-size: 16px;
                -webkit-transition: all 0.1s linear;
                -moz-transition: all 0.1s linear;
                transition: all 0.1s linear;
                -webkit-box-shadow: none;
                box-shadow: none;
                border-bottom: 1px solid #e7e7e7;
                border-radius: 0px;
                padding: 6px 0px;
            }

            .panel-login input:hover,
            .panel-login input:focus {
                outline: none;
                -webkit-box-shadow: none;
                -moz-box-shadow: none;
                box-shadow: none;
                border-color: #ccc;
            }

        .btn-login {
            background-color: #E8E9EC;
            outline: none;
            color: #2D3B55;
            font-size: 14px;
            height: auto;
            font-weight: normal;
            padding: 14px 0;
            text-transform: uppercase;
            border: none;
            border-radius: 0px;
            box-shadow: none;
        }

            .btn-login:hover,
            .btn-login:focus {
                color: #fff;
                background-color: #2D3B55;
            }

        .forgot-password {
            text-decoration: underline;
            color: #888;
        }

            .forgot-password:hover,
            .forgot-password:focus {
                text-decoration: underline;
                color: #666;
            }

        .btn-register {
            background-color: #E8E9EC;
            outline: none;
            color: #2D3B55;
            font-size: 14px;
            height: auto;
            font-weight: normal;
            padding: 14px 0;
            text-transform: uppercase;
            border: none;
            border-radius: 0px;
            box-shadow: none;
        }

            .btn-register:hover,
            .btn-register:focus {
                color: #fff;
                background-color: #2D3B55;
            }
    </style>


    <script type="text/javascript">

        $("#divsuccess").fadeTo(2000, 500).slideUp(500, function () {
            $("#divsuccess").slideUp(500);
        });

        $("#divdanger").fadeTo(2000, 500).slideUp(500, function () {
            $("#divdanger").slideUp(500);
        });

        $("#divinfo").fadeTo(2000, 500).slideUp(500, function () {
            $("#divinfo").slideUp(500);
        });

        $("#divwarning").fadeTo(2000, 500).slideUp(500, function () {
            $("#divwarning").slideUp(500);
        });
    </script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#chkShowPassword").bind("click", function () {
                var txtCurrentPassword = $("[id*=txtCurrentPassword]");
                if ($(this).is(":checked")) {
                    txtCurrentPassword.after('<input onchange = "PasswordChanged(this);" id = "txt_' + txtCurrentPassword.attr("id") + '" type = "text" value = "' + txtCurrentPassword.val() + '" />');
                    txtCurrentPassword.hide();
                } else {
                    txtCurrentPassword.val(txtCurrentPassword.next().val());
                    txtCurrentPassword.next().remove();
                    txtCurrentPassword.show();
                }
            });
        });
        function PasswordChanged(txt) {
            $(txt).prev().val($(txt).val());
        }
    </script>

    <div id="divsuccess" runat="server" style="display: none">
        <div class="alert alert-success">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Your Password Changed Sucessfully!</div>
            <div>Please Use Your ID No. As A Username And Your New Password To Login.</div>
        </div>
    </div>

    <div id="divdanger" runat="server" style="display: none">
        <div class="alert alert-danger">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Try Again Please</div>
        </div>
    </div>

    <div id="divinfo" runat="server" style="display: none">
        <div class="alert alert-info">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Not Reset Successfully!</div>
        </div>
    </div>

    <div id="divwarning" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>!</div>
        </div>
    </div>


    <center><asp:Label ID="lblSuccess" runat="server" CssClass="text-success" ForeColor="Green" Visible="false"> </asp:Label></center>
    <center><asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label></center>


    <br />
    <div class="container">
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-login">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div id="login_form" runat="server" action="#" method="post" role="form" style="display: block;">
                                    <h2><b>CHANGE PASSWORD</b></h2>

                                    <div class="form-group">
                                        <asp:TextBox ID="txtEmail" runat="server" required="true" Enabled="false" PlaceHolder="Email" CssClass="form-control" Height="34px"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCurrentPassword" runat="server" required="true" PlaceHolder="Current Password" CssClass="form-control" Height="34px" TextMode="Password"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <label for="chkShowPassword">
                                            <input type="checkbox" id="chkShowPassword" />
                                            Show current password</label>
                                    </div>
                                    <br />
                                    <div class="form-group">
                                        <asp:TextBox ID="txtNewPassword" runat="server" required="true" PlaceHolder="New Password" CssClass="form-control" Height="34px" TextMode="Password"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" required="true" PlaceHolder="Confirm Password" CssClass="form-control" TextMode="Password" Height="34px"></asp:TextBox>

                                        <label><span style="color: red">*</span></label>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                            ControlToValidate="txtConfirmPassword"
                                            CssClass="ValidationError"
                                            ControlToCompare="txtNewPassword"
                                            ErrorMessage="Password Must Match"
                                            ToolTip="Password must be the same" ForeColor="Red" />

                                    </div>

                                    <div class="form-group pull-left">
                                        <asp:Button ID="btnAdd" Width="150px" class="form-control btn btn-register" runat="server" Text="Change" OnClick="btnSubmit_Click" />
                                    </div>
                                    <span class="counter pull-left"></span>

                                    <div class="form-group pull-right">
                                        <asp:HyperLink ID="hlLogin" runat="server" Width="150px" class="form-control btn btn-register" NavigateUrl="../Index.aspx">Login</asp:HyperLink>
                                    </div>
                                    <span class="counter pull-right"></span>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="container" id="divMore" runat="server" visible="false">
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <%-- <div class="panel panel-login">
                    <div class="panel-body">--%>
                <div class="row">
                    <div class="col-lg-12">
                        <div id="Div1" runat="server" action="#" method="post" role="form" style="display: block;">
                            <div class="form-group pull-right">
                                <asp:HyperLink ID="HyperLink3" runat="server" Width="150px" class="form-control btn btn-register" NavigateUrl="/PL/Home.aspx">Home Page</asp:HyperLink>
                            </div>
                            <span class="counter pull-right"></span>

                        </div>
                    </div>
                </div>
                <%--  </div>
                </div>--%>
            </div>
        </div>
    </div>

    <br />
    <br />
    <br />

</asp:Content>
