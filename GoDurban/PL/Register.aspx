<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GoDurban.PL.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <script src="../../Scripts1/jquery.dataTables.min.js"></script>
    <script src="../../Scripts1/dataTables.bootstrap.min.js"></script>
    <script src="../../Scripts1/dataTables.buttons.min.js"></script>
    <script src="../../Scripts1/jszip.min.js"></script>
    <script src="../../Scripts1/pdfmake.min.js"></script>
    <script src="../../Scripts1/vfs_fonts.js"></script>
   <%-- <script src="../../Scripts1/buttons.html5.min.js"></script>--%>
    <script src="../../Scripts1/dataTables.responsive.js"></script>
    <script src="../../Scripts1/buttons.bootstrap.min.js"></script>

    <link href="../../Content1/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content1/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="../../Content1/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="../../Content1/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content1/responsive.bootstrap.min.css" rel="stylesheet" />

    <%--<link rel="stylesheet" href="//netdna.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css">--%>


    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtIDNo,#txtCellNo').keypress(function (key) {
            //    if (key.charCode < 48 || key.charCode > 57) return false;
            //});

            $('.name').keypress(function (key) {
                if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && (key.charCode != 45)) return false;
            });

            $('.surrname').keypress(function (key) {
                if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && (key.charCode != 45)) return false;
            });
    </script>

    <script type="text/javascript">

            function ValidateName(txtName) {

                var name = txtName.value;
                if (name.length == "") {
                    txtName.style.border = '1px solid red';
                    txtName.value = null;
                    return false;
                }
                else {
                    txtName.style.border = '';
                }
            }

            function ValidateSurname(txtSurname) {

                var surname = txtSurname.value;
                if (surname.length == "") {
                    txtSurname.style.border = '1px solid red';
                    txtSurname.value = null;
                    return false;
                }
                else {
                    txtSurname.style.border = '';
                }
            }

            function ValidateCellNO(txtCellNo) {

                var cellNo = txtCellNo.value;
                if (cellNo.length != 10) {

                    //alert("Cell Number Must Be 10 Digits");
                    $('#feedbackcellnStatus').remove();
                    $("#Idstatus").append("<div id='feedbackcellnStatus'><center><font color='red'>Cell Number Must Be 10 Digits</font></center></div>");
                    txtCellNo.style.border = '1px solid red';
                    txtCellNo.value = null;
                    //document.getElementById("txtCellNo").focus();
                    return false;
                }
                else {

                    txtCellNo.style.border = '';
                    $('#feedbackcellnStatus').remove();
                }
                //break;
            }

            function ValidateGender(ddlGender) {

                var gender = ddlGender.value;
                if (gender.selectedvalue == "0") {
                    ddlGender.style.border = '1px solid red';
                    ddlGender.value = null;
                    return false;
                }
                else {
                    ddlGender.style.border = '';
                }
            }

            function ValidateRole(ddlRole) {

                var role = ddlRole.value;
                if (role.selectedvalue == "0") {
                    ddlRole.style.border = '1px solid red';
                    ddlRole.value = null;
                    return false;
                }
                else {
                    ddlRole.style.border = '';
                }
            }
            function ValidateRace(ddlRace) {

                var race = ddlRace.value;
                if (race.selectedvalue == "0") {
                    ddlRace.style.border = '1px solid red';
                    ddlRace.value = null;
                    return false;
                }
                else {
                    ddlRace.style.border = '';
                }
            }




            function openModal() {
                $('#myModal').modal('show');
            }


            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }


            function CheckName(txt) {
                var regName = new RegExp('^[a-zA-Z\. ]+$');
                if (txt.value.match(regName) === null) {
                    txt.value = txt.value.substr(0, (txt.value.length - 1));
                }
            }

            function ValidatePassword(txtPassword) {

                var password = txtPassword.value;
                if (password.length != 8) {

                    //alert("ID Number Must Be 13 Digits");
                    $('#feedbackIDStatus').remove();
                    $("#Idstatus").append("<div id='feedbackIDStatus'><center><font color='red'>Password Must Be 8 Characters</font></center></div>");
                    txtPassword.style.border = '1px solid red';
                    txtPassword.value = null;
                    //document.getElementById("txtCellNo").focus();
                    return false;
                }
                else {

                    txtPassword.style.border = '';
                    $('#feedbackIDStatus').remove();
                }
                //break;
            }


            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

            function ValidateBusinessEmail(txtEmail) {

                if (!filter.test(txtEmail.value)) {
                    txtEmail.style.borderColor = "red";
                    // alert("Invalid Email Address");
                    $('#feedbackEmailStatus').remove();
                    $("#Idstatus").append("<div id='feedbackEmailStatus'><center><font color='red'></font></center></div>");
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

            function ValidateIDNo(txtIDNo) {

                var idno = txtIDNo.value;
                if (idno.length != 13) {

                    //alert("ID Number Must Be 13 Digits");
                    $('#feedbackIDStatus').remove();
                    $("#Idstatus").append("<div id='feedbackIDStatus'><center><font color='red'>ID Number Must Be 13 Digits</font></center></div>");
                    txtIDNo.style.border = '1px solid red';
                    txtIDNo.value = null;
                    //document.getElementById("txtCellNo").focus();
                    return false;
                }
                else {

                    txtIDNo.style.border = '';
                    $('#feedbackIDStatus').remove();
                }
                //break;
            }

            function ValidateOfficeNO(txtOfficeNo) {

                var officeno = txtOfficeNo.value;
                if (officeno.length != 10) {

                    //alert("Office Number Must Be 10 Digits");
                    $('#feedbackofnStatus').remove();
                    $("#Idstatus").append("<div id='feedbackofnStatus'><center><font color='red'>Office Number Must Be 10 Digits</font></center></div>");
                    txtOfficeNo.style.border = '1px solid red';
                    txtOfficeNo.value = null;
                    //document.getElementById("txtCellNo").focus();
                    return false;
                }
                else {

                    txtOfficeNo.style.border = '';
                    $('#feedbackofnStatus').remove();
                }
                //break;
            }

            function ValidateCellNO(txtCellNo) {

                var cellNo = txtCellNo.value;
                if (cellNo.length != 10) {

                    //alert("Cell Number Must Be 10 Digits");
                    $('#feedbackcellnStatus').remove();
                    $("#Idstatus").append("<div id='feedbackcellnStatus'><center><font color='red'>Cell Number Must Be 10 Digits</font></center></div>");
                    txtCellNo.style.border = '1px solid red';
                    txtCellNo.value = null;
                    //document.getElementById("txtCellNo").focus();
                    return false;
                }
                else {

                    txtCellNo.style.border = '';
                    $('#feedbackcellnStatus').remove();
                }
                //break;
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
                border-bottom-leftt-radius: 5px;
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

    <div id="divsuccess" runat="server" style="display: none">
        <div class="alert alert-success">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Thank you for registering on the Moja Cruise System!</div>
            <div>Please use your ID No as a username and your password to login.</div>
        </div>
    </div>

    <div id="divdanger" runat="server" style="display: none">
        <div class="alert alert-danger">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Deleted Successfully!</div>
        </div>
    </div>

    <div id="divinfo" runat="server" style="display: none">
        <div class="alert alert-info">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Edited Successfully!</div>
        </div>
    </div>

    <div id="divwarning" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>ID Number Exists!</div>
        </div>
    </div>

     <div id="divwarning1" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Email Exists!</div>
        </div>
    </div>
  
        <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>
  
    <br />
    <div class="container">
        <div class="row">
            <div class="col-md-6 col-md-offset-3">
                <div class="panel panel-login">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div id="register_form" runat="server" action="#" method="post" role="form" style="display: block;">
                                    <h2>REGISTER</h2>

                                    <div class="form-group">
                                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="Name" CssClass="name form-control" Height="34px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateName(this);"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtSurname" runat="server" PlaceHolder="Surname" CssClass="surname form-control" Height="34px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateSurname(this);"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtEmail" runat="server" PlaceHolder="Email" CssClass="form-control" Height="34px" onkeyup="ValidateEmailID(this);" onfocusout="ValidateBusinessEmail(this);"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCellNo" runat="server" PlaceHolder="Cell No" CssClass="form-control" Height="34px" onkeypress="return isNumberKey(event)" onfocusout="ValidateCellNO(this);" MaxLength="10"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateRole(this);">
                                            <asp:ListItem Selected="True" Value="0">--Select Role--</asp:ListItem>
                                        </asp:DropDownList>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlRace" runat="server" CssClass="form-control" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateRace(this);">
                                            <asp:ListItem Selected="True" Value="0">--Select Race--</asp:ListItem>
                                        </asp:DropDownList>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateGender(this);">
                                            <asp:ListItem Selected="True" Value="0">--Select Gender--</asp:ListItem>
                                        </asp:DropDownList>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtIDNo" runat="server" PlaceHolder="Id No" CssClass="form-control" Height="34px" MaxLength="13" onfocusout="ValidateIDNo(this);" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtPassword" runat="server" PlaceHolder="Password(8 characters)" CssClass="form-control" Height="34px" TextMode="Password" onfocusout="ValidatePassword(this);" MaxLength="8"></asp:TextBox>
                                        <label><span style="color: red">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" PlaceHolder="Confirm Password" CssClass="form-control" TextMode="Password" Height="34px" onfocusout="ValidatePassword(this);" MaxLength="8"></asp:TextBox>

                                        <label><span style="color: red">*</span></label>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                            ControlToValidate="txtConfirmPassword"
                                            CssClass="ValidationError"
                                            ControlToCompare="txtPassword"
                                            ErrorMessage="Password Must Match"
                                            ToolTip="Password must be the same" ForeColor="Red" />

                                    </div>
                                    <p id="Idstatus">
                                    </p>
                                    <br />
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-xs-12 form-group pull-left">
                                                <asp:Button ID="btnAdd" runat="server" Text="Submit" Width="150px" class="form-control btn btn-register" OnClick="btnCreate_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <br />
        <br />
        <br />

        <!-- Modal -->
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">

                        <h4 class="modal-title" id="mdlModalTitle" runat="server" style="text-align: center">User Registered Successfully</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Label ID="lblCreateUser" runat="server" Text=""></asp:Label>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCloseModal" runat="server" Text="Close" CssClass="form-control" OnClick="DisplayUsers_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
