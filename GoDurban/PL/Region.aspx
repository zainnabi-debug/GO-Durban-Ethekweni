<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Region.aspx.cs" Inherits="GoDurban.PL.Region" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
     <%--<link href="../Content1/bootstrap-datepicker.css" rel="stylesheet" />--%>
    <link href="../Content1/bootstrap-datepicker.min.css" rel="stylesheet" />
    <%--<script src="../Scripts1/bootstrap-datepicker.js"></script>--%>
    <script src="../Scripts1/bootstrap-datepicker.min.js"></script>

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

    <%--  /*script for the dropdownchosen search*/--%>
    <script src="../../Scripts1/chosen.jquery.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../../Content1/chosen.css">
    <link rel="stylesheet" href="../../Content1/chosen.min.css">

    <%-- DDL Search --%>
    <link href="../../Content1/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../Content1/chosen.css" />

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

    <style>
        .modal-dialog {
            position: center;
            left: 5%;
            top: 5%;
            /*margin-top: -250px;*/
            width: 50%;
            margin: 0 auto;
            /*align-content: center;*/
        }
    </style>

    <script type="text/javascript">

        function CheckName(txt) {
            var regName = new RegExp('^[a-zA-Z\. ]+$');
            if (txt.value.match(regName) === null) {
                txt.value = txt.value.substr(0, (txt.value.length - 1));
            }
        }

        function ValidateStatus(ddlStatus) {

            var status = ddlStatus.value;
            if (status.selectedvalue == "0") {
                ddlStatus.style.border = '1px solid red';
                ddlStatus.value = null;
                return false;
            }
            else {
                ddlStatus.style.border = '';
            }
        }

        function ValidateAccount(txtAccNo) {

            var ass = txtAccNo.value;
            if (ass.length == "") {
                txtAccNo.style.border = '1px solid red';
                txtAccNo.value = null;
                return false;
            }
            else {
                txtAccNo.style.border = '';
            }
        }

        function ValidateBank(ddlBank) {

            var bank = ddlBank.value;
            if (bank.selectedvalue == "0") {

                ddlBank.style.border = '1px solid red';
                ddlBank.value = null;
                return false;
            }
            else {

                ddlBank.style.border = '';
            }
        }

        function ValidateBranch(ddlBranch) {

            var branch = ddlBranch.value;
            if (branch.selectedvalue == "0") {

                ddlBranch.style.border = '1px solid red';
                ddlBranch.value = null;
                return false;
            }
            else {

                ddlBranch.style.border = '';
            }
        }

        function ValidateAccNo(txtAccNo) {

            var accno = txtAccNo.value;
            if (accno.length == "") {
                txtAccNo.style.border = '1px solid red';
                txtAccNo.value = null;
                return false;
            }
            else {
                txtAccNo.style.border = '';
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        
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

                buttons: [
                    {
                        extend: 'pdf',
                        text: 'PDF',
                        title: 'Owner',
                        exportOptions: {
                            columns: [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
                        }
                    },
                    {
                        extend: 'excel',
                        text: 'excel',
                        title: 'Owner',
                        exportOptions: {
                            columns: [4, 5, 6, 7, 8, 9, 10, 11, 12],
                        }
                    }

                ],

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

        //$("#divsuccess").fadeTo(2000, 500).slideUp(500, function () {
        //    $("#divsuccess").slideUp(500);
        //});

        //$("#divdanger").fadeTo(2000, 500).slideUp(500, function () {
        //    $("#divdanger").slideUp(500);
        //});

        //$("#divinfo").fadeTo(2000, 500).slideUp(500, function () {
        //    $("#divinfo").slideUp(500);
        //});

        //$("#divwarning").fadeTo(2000, 500).slideUp(500, function () {
        //    $("#divwarning").slideUp(500);
        //});

        

    </script>

    <div id="divsuccess" runat="server" style="display: none">
        <div class="alert alert-success">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Saved Successfully!</div>
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
            <div>Region Exists!</div>
        </div>
    </div>

    <div id="divwarning1" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Account No. Exists!</div>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" ForeColor="Red"> </asp:Label>


    <h4>Create Region-Bank</h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Region-Bank"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">                

               <div class="row">
                    <div class="col-md-2">Region: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRegion" placeholder="Region" runat="server" class="form-control limited" Height="34px" Width="250px" MaxLength="100" onfocusout="ValidateRegion(this);"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">Bank Name: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBank" runat="server" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateBank(this);" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged">
                             <asp:ListItem Selected="True" Value="0">--Select Bank--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">Bank Branch: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                         <asp:DropDownList ID="ddlBranch" runat="server" Enabled="false" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateBranch(this);" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                              <asp:ListItem Selected="True" Value="0">--Select Bank Name First--</asp:ListItem>
                         </asp:DropDownList> 
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">Account No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAccNo" placeholder="Account No." runat="server" class="form-control limited" Height="34px" Width="250px" MaxLength="100" onkeypress="return isNumberKey(event)" onfocusout="ValidateAccNo(this);"></asp:TextBox>
                    </div>

                    <div class="col-md-2">Status: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateStatus(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Status--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                               
                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnAdd_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>
                    <span class="counter pull-right"></span>
                </div>
            </div>
        </div>
    </div>

    <div id="accordion2" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Edit"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

                       <asp:GridView ID="gvRegion" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            OnPreRender="gvRegion_PreRender" AutoGenerateColumns="false"
                            DataKeyNames="RegionID" OnRowCommand="gvRegion_RowCommand">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-info" Text="Edit" CommandName="EditItem"> Edit
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>


                                <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');"> Delete
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="RegionID" HeaderText="RegionID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="RegionName" HeaderText="Region " />
                                <asp:BoundField DataField="RegionNo" HeaderText="Region No." />
                                <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                <asp:BoundField DataField="BranchCode" HeaderText="Branch" />
                                <asp:BoundField DataField="AccountNo" HeaderText="Account No." />
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>