<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="GoDurban.PL.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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

    <style>
        .gvdatatable {
            width: 100%;
            word-wrap: break-word;
            table-layout: fixed;
        }

        .ChkBoxClass input {
            width: 20px;
            height: 20px;
        }
    </style>

    <script type="text/javascript">


        $(document).ready(function () {
            $('.gvdatatable').dataTable({
                dom: 'Bfrtip',
                responsive: false,
                buttons: [
                    'excelHtml5',
                    'pdfHtml5'
                ],
                "bSearchable": true,
                "bfilter": true,

                //columnDefs: [
                //    {
                //        "targets": [0],
                //        "orderable": false,
                //        "searchable": false

                //    },
                //    {
                //        "targets": [1],
                //        //"visible": false,
                //        "orderable": false,
                //        "searchable": true

                //    },
                //    {
                //        "targets": [2],
                //        "orderable": false,
                //        "searchable": true
                //    }],
                aaSorting: [[1, "asc"]]

            });
        });

        function openModal() {
            $('#myModal').modal('show');
        }

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

                txtEmail.style.border = "red";
                return false;
            }
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

        function ValidateCellNO(txtCellNO) {

            var cellNO = txtCellNO.value;
            if (cellNO.length != 10) {


                txtCellNO.style.border = '1px solid red';
            }
            else {

                txtCellNO.style.border = '';
            }
        }

        function ValidateWorkNO(txtWorkNO) {

            var workNO = txtWorkNO.value;
            if (workNO.length != 10) {


                txtWorkNO.style.border = '1px solid red';
            }
            else {

                txtWorkNO.style.border = '';
            }
        }

    </script>

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
            <div>Thank you for registering on the  Moja Cruise System!</div>
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

    <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>

    <h4>Edit User</h4>
    <div id="divUser" role="tablist" aria-multiselectable="true" runat="server" style="display: none">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="User"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Name:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="First name" CssClass="form-control" Width="250px" Height="34px" onkeyup="CheckName(this);" onkeydown="CheckName(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Surname:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSurname" runat="server" PlaceHolder="Surname" CssClass="form-control" Width="250px" Height="34px" onkeyup="CheckName(this);" onkeydown="CheckName(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Role:<span style="color: red">*</span></div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlRole" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0">--Select User Role--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">Email:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" PlaceHolder="Email" CssClass="form-control" Width="250px" Height="34px" onkeyup="ValidateEmailID(this);" onfocusout="ValidateBusinessEmail(this);"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2" id="divactive" runat="server" style="display: none">Active:</div>
                    <div class="col-sm-4" id="divactive1" runat="server" style="display: none">
                        <asp:CheckBox ID="chkStatus" runat="server" CssClass="ChkBoxClass" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Gender:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGender" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px">
                            <asp:ListItem Selected="True" Value="0">--Select gender--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">Race:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRace" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px">
                            <asp:ListItem Selected="True" Value="0">--Select Race--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">ID No:<span style="color: red">*</span></div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtIDNo" runat="server" PlaceHolder="Id No" CssClass="form-control" Enabled="false" Width="250px" Height="34px" MaxLength="13" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Cell No:<span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCellNo" runat="server" PlaceHolder="Cell No" CssClass="form-control" Width="250px" Height="34px" onkeypress="return isNumberKey(event)" onfocusout="ValidateCellNO(this);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                </div>
                <div class="row" id="Pass" runat="server" style="display: none">
                    <div class="col-md-2" id="divPass" runat="server" style="display: inline">Password:</div>
                    <div class="col-md-4" id="divPass1" runat="server" style="display: inline">
                        <asp:TextBox ID="txtPassword" runat="server" PlaceHolder="Password(8 characters)" CssClass="form-control" Width="250px" Height="34px" TextMode="Password" MaxLength="8"></asp:TextBox>
                    </div>
                    <div class="col-md-2" id="divConPass" runat="server" style="display: inline">Confirm Password:</div>
                    <div class="col-md-4" id="divConPass1" runat="server" style="display: inline">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" PlaceHolder="Confirm Password" CssClass="form-control" TextMode="Password" Width="250px" Height="34px" MaxLength="8"></asp:TextBox>

                    </div>
                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        CssClass="ValidationError"
                        ControlToCompare="txtPassword"
                        ErrorMessage="Password Must Match"
                        ToolTip="Password must be the same" ForeColor="Red" />

                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="70px" class="btn btn-primary form-control" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>
                    <span class="counter pull-right"></span>
                </div>
            </div>
        </div>
    </div>

    <div id="divUser1" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="User"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvUsers" Width="100%" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="gvUsers_PreRender" AutoGenerateColumns="false" DataKeyNames="UserID" OnRowCommand="gvUsers_RowCommand">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-info" Text="Edit" Width="50px" CommandName="EditItem"> Edit
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <%--  <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');"> Delete
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>

                                <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No" SortExpression="IdentityNumber" />
                                <asp:BoundField DataField="CellNo" HeaderText="Cell No" SortExpression="ForceNumber" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                <asp:BoundField DataField="RaceDescription" HeaderText="Race" SortExpression="RaceID" />
                                <asp:BoundField DataField="GenderDescription" HeaderText="Gender" SortExpression="GenderID" />
                                <asp:BoundField DataField="RoleDescription" HeaderText="User" SortExpression="UserTypeID" />
                                <asp:BoundField DataField="IsActive" HeaderText="Active" SortExpression="UserTypeID" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>
    </div>


</asp:Content>
