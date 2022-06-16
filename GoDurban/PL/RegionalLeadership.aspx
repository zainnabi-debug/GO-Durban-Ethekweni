<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegionalLeadership.aspx.cs" Inherits="GoDurban.PL.RegionalLeadership" %>

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

    <%--  /*script for the dropdownchosen search*/--%>
    <script src="../../Scripts1/chosen.jquery.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../../Content1/chosen.css">
    <link rel="stylesheet" href="../../Content1/chosen.min.css">

    <%-- DDL Search --%>
    <link href="../../Content1/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="../../Content1/chosen.css" />

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
  
        .gvdatatable {
            width: 100%;
            word-wrap: break-word;
            table-layout: fixed;            
        }

         .ChkBox input {
            width: 20px;
            height: 20px;
        }
    </style>

    <script type="text/javascript">

        function ValidateTermsConditions(chkTermsConditions) {
            $("#chkTermsConditions").attr("checked") ? chkTermsConditions.style.border = '' : chkTermsConditions.style.border = '';
        }

        function ValidateReviewContract(chkReviewContract) {
            $("#chkReviewContract").attr("checked") ? chkReviewContract.style.border = '' : chkReviewContract.style.border = '';
        }


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
        function ValidateRegion(ddlRegion) {

            var region = ddlRegion.value;
            if (region.selectedvalue == "0") {
                ddlRegion.style.border = '1px solid red';
                ddlRegion.value = null;
                return false;
            }
            else {
                ddlRegion.style.border = '';
            }
        }
        //function ValidateStatus(ddlStatus) {

        //    var status = ddlStatus.value;
        //    if (status.selectedvalue == "0") {
        //        ddlStatus.style.border = '1px solid red';
        //        ddlStatus.value = null;
        //        return false;
        //    }
        //    else {
        //        ddlStatus.style.border = '';
        //    }
        //}
        //function ValidateReason(ddlReason) {

        //    var reason = ddlReason.value;
        //    if (reason.selectedvalue == "0") {
        //        ddlReason.style.border = '1px solid red';
        //        ddlReason.value = null;
        //        return false;
        //    }
        //    else {
        //        ddlReason.style.border = '';
        //    }
        //}

        function ValidateStreetNoName(txtStreetNo) {

            var strnoname = txtStreetNo.value;
            if (strnoname.length == "") {
                txtStreetNo.style.border = '1px solid red';
                txtStreetNo.value = null;
                return false;
            }
            else {
                txtStreetNo.style.border = '';
            }
        }
        function ValidateSuburb(txtSuburb) {

            var suburb = txtSuburb.value;
            if (suburb.length == "") {
                txtSuburb.style.border = '1px solid red';
                txtSuburb.value = null;
                return false;
            }
            else {
                txtSuburb.style.border = '';
            }
        }
        function ValidateCity(txtCity) {

            var city = txtCity.value;
            if (city.length == "") {
                txtCity.style.border = '1px solid red';
                txtCity.value = null;
                return false;
            }
            else {
                txtCity.style.border = '';
            }
        }
        function ValidateCode(txtCode) {

            var code = txtCode.value;
            if (code.length == "") {
                txtCode.style.border = '1px solid red';
                txtCode.value = null;
                return false;
            }
            else {
                txtCode.style.border = '';
            }
        }


        function openModal() {
            $('#myModal').modal('show');
        }

        function openModal() {
            $('#myModal1').modal('show');
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
                        title: 'Users',
                        exportOptions: {
                            columns: [1, 2],
                        }
                    },
                    {
                        extend: 'excel',
                        text: 'excel',
                        title: 'Users',
                        exportOptions: {
                            columns: [1, 2],
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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function allowNumeric(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code > 47 && code < 58)) // numeric (0-9)
            {
                e.preventDefault();
            }
        }

        function allowAlphaNumericSpace(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code == 32) && // space

                !(code > 64 && code < 91) && // upper alpha (A-Z)

                !(code > 96 && code < 123)) { // lower alpha (a-z)
                e.preventDefault();
            }
        }

        //function name(evt) {
        //    var charCode = (evt.which) ? evt.which : event.keyCode
        //    if (!(code == 32) &&
        //        !(code > 64 && code < 91) &&
        //        !(code > 96 && code < 123)) {
        //        return false;
        //    }
        //    return true;
        //}


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

        function CheckName1(txt) {
            var regName = new RegExp('^[0-9a-zA-Z\. ]+$');
            if (txt.value.match(regName) === null) {
                txt.value = txt.value.substr(0, (txt.value.length - 1));
            }
        }

        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

        function ValidateBusinessEmail(txtEmail) {

            if (!filter.test(txtEmail.value)) {
                //txtEmail.style.borderColor = "red";
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

                txtEmail.style.border = "";
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


        $(function () {
            $("[id$=txtDriContractExpiry],[id$=txtDriLicenseExpiry],[id$=txtDriPRDPExpiryDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                todayHighlight: true,
                //daysOfWeekDisabled: [0, 6],
                //startDate: new Date(),
                maxDate: new Date(),
                endDate: 'today',
                format: 'dd/mm/yyyy',
                buttonImage: '../Images/calendar.png',
                autoclose: true,
                orientation: 'auto top',
            });
        });

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
            <div>Regional Leadership Exists!</div>
        </div>
    </div>

    <div id="divwarning1" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
             <div>Chair Person Exists!</div>
        </div>
    </div>

     <div id="divwarning11" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
             <div>Edited and notification not sent. Please contact support team!</div>
        </div>
    </div>

    <style>
        .ChkBoxClass input {
            width: 20px;
            height: 20px;
        }
    </style>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>
   

    <h4>Create Regional Leadership</h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Chair Person Info"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Name: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" placeholder="Name" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateName(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Surname: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSurname" placeholder="Surname" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateSurname(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">ID No.: <span id="spAsterik1" runat="server" style="color: red; display: inline">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIDNo" placeholder="ID No." runat="server" CssClass="form-control limited" Height="34px" Width="250px" onkeypress="return isNumberKey(event)" onfocusout="ValidateIDNo(this);" MaxLength="13"></asp:TextBox>
                    </div>
                     <div class="col-md-2">Passport No.:  <span id="spAsterik" runat="server" style="color: red; display: none">*</span> <asp:CheckBox ID="chkPassport" runat="server" AutoPostBack="true" CssClass="ChkBox" OnCheckedChanged="chkPassport_CheckedChanged" /> </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPassport" placeholder="Passport No. If Not SA Citizen" runat="server" Enabled="false" CssClass="form-control limited" Height="34px" Width="250px" MaxLength="100" onfocusout="ValidatePassport(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Cell No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCellNo" placeholder="Cell No." runat="server" class="form-control limited" Height="34px" Width="250px" onkeypress="return isNumberKey(event)" onfocusout="ValidateCellNO(this);" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Gender: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGender" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateGender(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Gender--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Office No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtOfficeNo" placeholder="Office No." runat="server" class="form-control limited" Height="34px" Width="250px" onkeypress="return isNumberKey(event)" onfocusout="ValidateOfficeNO(this);" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Race: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRace" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateRace(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Race--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Email:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" placeholder="Email" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onfocusout="ValidateBusinessEmail(this);" onkeyup="ValidateEmailID(this);"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-2">Region: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlRegion" runat="server" AppendDataBoundItems="True" class="chosen-select" Width="250px" Height="34px" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">--Select Region--</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="lblRegion" runat="server" ForeColor="Red"> </asp:Label>
                            <script type="text/javascript">
                                var config = {
                                    '.chosen-select': {},
                                    '.chosen-select-deselect': { allow_single_deselect: true },
                                    '.chosen-select-no-single': { disable_search_threshold: 10 },
                                    '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
                                    '.chosen-select-width': { width: "95%" }
                                }
                                for (var selector in config) {
                                    $(selector).chosen(config[selector]);
                                }
                            </script>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">Street No. & Name: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStreetNo" placeholder="Street No. & Name" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeyup="CheckName1(this);" onkeydown="CheckName1(this);" onfocusout="ValidateStreetNoName(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Suburb: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSuburb" placeholder="Suburb" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateSuburb(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">City: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCity" placeholder="City" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="CheckName(this);" onkeydown="CheckName(this);" onfocusout="ValidateCity(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Code: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCode" placeholder="Code" runat="server" class="form-control limited" Height="34px" Width="250px" onkeypress="return isNumberKey(event)" MaxLength="4" onfocusout="ValidateCode(this);"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">Status: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateStatus(this);" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0">--Select Status--</asp:ListItem>
                        </asp:DropDownList>
                    </div> 
                    <div class="col-md-2" id="divReason" runat="server" style="display:none">Reason: <span style="color: red">*</span></div>
                    <div class="col-md-4" id="divReason1" runat="server" style="display:none">
                        <asp:DropDownList ID="ddlReason" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateReason(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Reason--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkTermsConditions" runat="server" Width="40px" Height="40px" CssClass="ChkBoxClass form-control" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateTermsConditions(this);" />
                        <a href="#" data-toggle="modal" data-target="#myModal1" id="aTermsConditions" runat="server">Accept Terms & Conditions</a> <span style="color: red">*</span>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkReviewContract" runat="server" Width="40px" Height="40px" CssClass="ChkBoxClass form-control" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateReviewContract(this);" />
                        <a href="#" data-toggle="modal" data-target="#myModal" id="aReviewContract" runat="server">Review Contract</a> <span style="color: red">*</span>
                    </div>
                </div>

                <p id="Idstatus">
                </p>


                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnAdd_Click" />
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>

               
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
                        <asp:GridView ID="gvRegionalLeader" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" AutoGenerateColumns="false" DataKeyNames="RegionalLeaderID" Font-Size="9"
                            OnPreRender="gvRegionalLeader_PreRender" OnRowCommand="gvRegionalLeader_RowCommand" OnRowDataBound="gvRegionalLeader_RowDataBound">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-info" Text="Edit" CommandName="EditItem"> Edit
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrint" runat="server" visible="false" CssClass="btn btn-info" Text="Print" CommandName="Print"> Print
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="RegionalLeaderID" HeaderText="RegionalLeaderID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Name" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." SortExpression="Name" />
                                <asp:BoundField DataField="Passport" HeaderText="Passport No." SortExpression="Name" />
                                <asp:BoundField DataField="CellNo" HeaderText="Cell No." SortExpression="Name" />
                                <asp:BoundField DataField="OfficeNo" HeaderText="Office No." SortExpression="Name" />
                                <%--<asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" />--%>
                                <asp:BoundField DataField="RegionName" HeaderText="Region" SortExpression="Name" />
                                <asp:BoundField DataField="RaceDescription" HeaderText="Race" SortExpression="Name" />
                                <asp:BoundField DataField="GenderDescription" HeaderText="Gender" SortExpression="Name" />
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" SortExpression="Name" />
                                <%--<asp:BoundField DataField="ReasonDescription" HeaderText="Reason" SortExpression="Name" />--%>
                                <%-- <asp:BoundField DataField="AddressStreet" HeaderText="AddressStreet" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressSuburb" HeaderText="AddressSuburb" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressCity" HeaderText="AddressCity" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressCode" HeaderText="AddressCode" SortExpression="Name" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal about-modal fade" id="myModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Contract</h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCloseModal" runat="server" Text="Close" CssClass="form-control" Width="60px" data-dismiss="modal" aria-label="Close" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal about-modal fade" id="myModal1" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Terms And Conditions</h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCloseModal1" runat="server" Text="Close" CssClass="form-control" Width="60px" data-dismiss="modal" aria-label="Close" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
