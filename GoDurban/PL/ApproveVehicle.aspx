<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApproveVehicle.aspx.cs" Inherits="GoDurban.PL.ApproveVehicle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        $(function () {
            $("[id$=txtVehLastServiced]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                endDate: new Date(),
                //format: 'dd-mm-yyyy',
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',
                forceParse: false,
                todayHighlight: true,
            });
        });

        $(function () {
            $("[id$=txtVehOperatingLicenseExpiry],[id$=txtVehCORExpiry]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                startDate: new Date(),
                //format: 'dd-mm-yyyy',
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',
                todayHighlight: true,
                forceParse: false
            });
        });

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

    </script>

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

        //window.pressed = function () {
        //    var a = document.getElementById('fuCORScan');
        //    if (!(a.value == "")) {
        //        var theSplit = a.value.split('\\');
        //        fileLabel.innerHTML = theSplit[theSplit.length - 1];
        //    }
        //};

        function openModal() {
            $('#myModal').modal('show');
        }


        function ValidateCapacity(txtCapacity) {

            var capacity = txtCapacity.value;
            if (capacity.length == "") {
                txtCapacity.style.border = '1px solid red';
                txtCapacity.value = null;
                return false;
            }
            else {
                txtCapacity.style.border = '';
            }
        }
        function ValidateVehNoPlate(txtVehNoPlate) {

            var noplate = txtVehNoPlate.value;
            if (noplate.length == "") {
                txtVehNoPlate.style.border = '1px solid red';
                txtVehNoPlate.value = null;
                return false;
            }
            else {
                txtVehNoPlate.style.border = '';
            }
        }
        function ValidateVehEngineNO(txtVehEngineNO) {

            var engno = txtVehEngineNO.value;
            if (engno.length == "") {
                txtVehEngineNO.style.border = '1px solid red';
                txtVehEngineNO.value = null;
                return false;
            }
            else {
                txtVehEngineNO.style.border = '';
            }
        }
        function ValidateOperatingLicense(txtOperatingLicense) {

            var ol = txtOperatingLicense.value;
            if (ol.length == "") {
                txtOperatingLicense.style.border = '1px solid red';
                txtOperatingLicense.value = null;
                return false;
            }
            else {
                txtOperatingLicense.style.border = '';
            }
        }
        function ValidateVehVinNo(txtVehVinNo) {

            var vin = txtVehVinNo.value;
            if (vin.length == "") {
                txtVehVinNo.style.border = '1px solid red';
                txtVehVinNo.value = null;
                return false;
            }
            else {
                txtVehVinNo.style.border = '';
            }
        }
        function ValidateCORExpiry(txtVehCORExpiry) {

            var corexpiry = txtVehCORExpiry.value;
            if (corexpiry.value == "0") {
                txtVehCORExpiry.style.border = '1px solid red';
                txtVehCORExpiry.value = null;
                return false;
            }
            else {
                txtVehCORExpiry.style.border = '';
            }
        }

        function ValidateOperatingLicenseExpiry(txtVehOperatingLicenseExpiry) {

            var olexpiry = txtVehOperatingLicenseExpiry.value;
            if (olexpiry.value == "0") {
                txtVehOperatingLicenseExpiry.style.border = '1px solid red';
                txtVehOperatingLicenseExpiry.value = null;
                return false;
            }
            else {
                txtVehOperatingLicenseExpiry.style.border = '';
            }
        }

        function ValidateLastServiced(txtVehLastServiced) {

            var lastserv = txtVehLastServiced.value;
            if (lastserv.value == "0") {
                txtVehLastServiced.style.border = '1px solid red';
                txtVehLastServiced.value = null;
                return false;
            }
            else {
                txtVehLastServiced.style.border = '';
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
        function ValidateReason(ddlReason) {

            var reason = ddlReason.value;
            if (reason.selectedvalue == "0") {
                ddlReason.style.border = '1px solid red';
                ddlReason.value = null;
                return false;
            }
            else {
                ddlReason.style.border = '';
            }
        }

        function ValidateCORScan(fuCORScan) {

            var corscan = fuCORScan.value;
            if (corscan.value == false) {
                fuCORScan.style.border = '1px solid red';
                fuCORScan.value = null;
                return false;
            }
            else {
                fuCORScan.style.border = '';
                $(this).removeClass("bar");
            }
        }

        function Validate() {
            var ddlOwner = document.getElementById("ddlOwner");
            if (ddlOwner.value == "0") {
                //If the "Please Select" option is selected display error.
                ddlOwner.style.borderColor = "red";
                return false;
            }
            return true;
        }

        function CheckName1(txt) {
            var regName1 = new RegExp('^[-0-9a-zA-Z\. ]+$');
            if (txt.value.match(regName1) === null) {
                txt.value = txt.value.substr(0, (txt.value.length - 1));
            }
        }

        function showButton1() {
           <%-- document.getElementById("<%=divCOR.ClientID%>").style.display = 'inline';--%>
        }

        function allowNumeric(e) {
            var code = ('charCode' in e) ? e.charCode : e.keyCode;
            if (!(code > 47 && code < 58)) // numeric (0-9)
            {
                e.preventDefault();
            }
        }


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }


        $('#fuCORScan').change(function () {
            $(this).removeClass("bar");
        })

    </script>

    <style>
        .bar:after {
            content: "Please select a file";
            background-color: white;
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

    <script type="text/javascript">
        var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt"];
        function ValidateSingleInput(oInput) {
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            document.getElementById("<%=divCOR.ClientID%>").style.display = 'inline';
                            break;
                        }
                    }

                    if (!blnValid) {
                        alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                        oInput.value = "";
                        document.getElementById("<%=divCOR.ClientID%>").style.display = 'none';
                        return false;
                    }
                }
            }
            return true;
        }

        function ValidateSize(file) {
            var FileSize = file.files[0].size / 1024 / 1024; // in MB
            if (FileSize > 2) {
                alert('File size exceeds 2 MB');
                $(file).val('');
                document.getElementById("<%=divCOR.ClientID%>").style.display = 'none';
                return false;
            } else {
                document.getElementById("<%=divCOR.ClientID%>").style.display = 'inline';
            }
        }
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
            <div>Approved Successfully!</div>
        </div>
    </div>

    <div id="divinfo1" runat="server" style="display: none">
        <div class="alert alert-info">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Edited Successfully!</div>
        </div>
    </div>

    <div id="divwarning" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Vehicle Exists!</div>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>


    <h4>Approve Vehicle</h4>

    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>--%>


    <div id="divVehiclee" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label1" runat="server" Text=" Owner"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Select Owner: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlOwner" runat="server" Enabled="false" CssClass="chosen-select" Width="250px" Height="34px" AppendDataBoundItems="True" AutoPostBack="true" OnSelectedIndexChanged="ddlOwner_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">--Select Owner--</asp:ListItem>
                            </asp:DropDownList><br />
                            <asp:Label ID="lblOwner" runat="server" ForeColor="Red"> </asp:Label>

                            <asp:Label ID="lblOwner1" runat="server" ForeColor="Red"> </asp:Label>
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

            </div>
        </div>
    </div>

    <div id="divVehicleee" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Vehicle Info"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">


                <div class="row">
                    <div class="col-md-2">Vehicle Type: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlVehicleInfo" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="chosen-select" Width="250px" Height="34px" AutoPostBack="true" OnSelectedIndexChanged="ddlVehicleInfo_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">--Select Vehicle Type--</asp:ListItem>
                            </asp:DropDownList><br />
                            <asp:Label ID="lblVehicle" runat="server" ForeColor="Red"> </asp:Label>
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


                    <div class="col-md-2" id="divCapacity" runat="server" style="display: none">Capacity: <span style="color: red">*</span></div>
                    <div class="col-md-4" id="divCapacity1" runat="server" style="display: none">
                        <asp:TextBox ID="txtCapacity" runat="server" Enabled="false" CssClass="form-control" Width="70px" Height="34px" MaxLength="5" onkeypress="return isNumberKey(event)" onfocusout="ValidateCapacity(this);"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">Number Plate: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehNoPlate" runat="server" Enabled="false" PlaceHolder="Number Plate" CssClass="form-control" Width="250px" Height="34px" MaxLength="20" onkeyup="CheckName1(this);" onkeydown="CheckName1(this);" onfocusout="ValidateVehNoPlate(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Engine No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehEngineNO" runat="server" Enabled="false" PlaceHolder="Engine No." CssClass="form-control" Width="250px" MaxLength="50" Height="34px" onkeyup="CheckName1(this);" onkeydown="CheckName1(this);" onfocusout="ValidateVehEngineNO(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Year Registered: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYearRegistered" runat="server" Enabled="false" CssClass="form-control" Width="100px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Last Service: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehLastServiced" runat="server" Enabled="false" PlaceHolder="Last Date Serviced" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateLastServiced(this);"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Operating License: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtOperatingLicense" runat="server" Enabled="false" PlaceHolder="Operating License" CssClass="form-control" Width="250px" Height="34px" onkeyup="CheckName1(this);" onkeydown="CheckName1(this);" onfocusout="ValidateOperatingLicense(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">OL Expiry: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehOperatingLicenseExpiry" runat="server" Enabled="false" PlaceHolder="Operating License Expiry" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateOperatingLicenseExpiry(this);"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2" id="divYear" runat="server" style="display: none">Year: <span style="color: red">*</span></div>
                    <div class="col-md-4" id="divYear1" runat="server" style="display: none">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" Width="100px" Height="34px"></asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">Status: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateStatus(this);" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0">--Select Status--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" id="divReason" runat="server" style="display: none">Reason: <span style="color: red">*</span></div>
                    <div class="col-md-4" id="divReason1" runat="server" style="display: none">
                        <asp:DropDownList ID="ddlReason" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateReason(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Reason--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">COR Expiry: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehCORExpiry" runat="server" Enabled="false" PlaceHolder="COR Expiry" CssClass="form-control" Width="250px" Height="34px" onfocusout="ValidateCORExpiry(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-2">VIN No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehVinNo" runat="server" Enabled="false" PlaceHolder="VIN No." CssClass="form-control" MaxLength="50" Width="250px" Height="34px" onkeyup="CheckName1(this);" onkeydown="CheckName1(this);" onfocusout="ValidateVehVinNo(this);"></asp:TextBox>
                    </div>
                </div>

                <asp:TextBox ID="txtCOR" runat="server" Visible="false"></asp:TextBox>
                <asp:TextBox ID="txtCOR1" runat="server" Visible="false"></asp:TextBox>
                <div class="row">
                    <div class="col-md-2">COR Scan: <span style="color: red"></span></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuCORScan" onchange="showButton1();ValidateSingleInput(this);ValidateSize(this);" runat="server" Enabled="false" onfocusout="ValidateCORScan(this);" />
                        <asp:Label ID="lblCORScan" runat="server" Text="File Uploaded Successfully!" ForeColor="Green" Visible="false" />
                        <asp:Label ID="lblCORScan1" runat="server" Text="Upload File First!" ForeColor="Red" Visible="false" />
                        <div id="divCOR" runat="server" style="display: none">
                            <button runat="server" id="btnCOR" title="btnUploadDoc" style="width: 100px; height: 30px" type="button" class="btn btn-success btn-block" causesvalidation="false" onserverclick="btnCOR_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>
                    </div>
                    <br />

                </div>

                <br />

                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Height="34px" Width="70px" class="btn btn-primary form-control" OnClientClick="initLoadHoldOn();return Validate()" OnClick="btnAdd_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" Visible="false" runat="server" Text="Cancel" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-md-2" id="divDoc" runat="server" style="display: none">
                        <asp:Button ID="btnDoc" runat="server" Text="View Files" Height="34px" Width="90px" class="btn btn-primary form-control" OnClick="btnDoc_Click" OnClientClick="initLoadHoldOn();"/>
                    </div>

                </div>
            </div>
        </div>
    </div>


    <div id="divVehicle" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Vehicle"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvVehicle" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            AutoGenerateColumns="false" DataKeyNames="VehicleID" Font-Size="9"
                            OnPreRender="gvVehicle_PreRender" OnRowCommand="gvVehicle_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApprove" runat="server" CssClass="btn btn-info" Text="Approve" CommandName="ApproveItem" OnClientClick="initLoadHoldOn();"> Select
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="VehicleID" HeaderText="VehicleID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="NameSurname" HeaderText="Owner" SortExpression="Name" />
                                <asp:BoundField DataField="NumberPlate" HeaderText="Number Plate" />
                                <asp:BoundField DataField="LastServiced" HeaderText="Last Serviced" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="EngineNumber" HeaderText="Engine No." />
                                <asp:BoundField DataField="VINNumber" HeaderText="VIN No." />
                                <%--<asp:BoundField DataField="CORScanID" HeaderText="COR Scan" SortExpression="Name" />--%>
                                <asp:BoundField DataField="CORExpiry" HeaderText="COR Expiry" SortExpression="Name" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="OperatingLicense" HeaderText="OL Code" />
                                <asp:BoundField DataField="OperatingLicenseExpiry" HeaderText="OL Expiry" DataFormatString="{0:yyyy-MM-dd}" />

                                <%--<asp:BoundField DataField="ReasonDescription" HeaderText="Reason" />--%>
                                <asp:BoundField DataField="Make" HeaderText="Make" />
                                <asp:BoundField DataField="Model" HeaderText="Model" />
                                <asp:BoundField DataField="Capacity" HeaderText="Capacity" />
                                <%--<asp:BoundField DataField="YearRegistered" HeaderText="Year Registered" />--%>
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="divUploadDocuments" role="tablist" aria-multiselectable="true" runat="server" style="display: none">
        <div class=" panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label12" runat="server" Text="Upload Files"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseEleven" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingEleven" style="padding: 1%">

                <div class="row">
                    <div class="col-md-12">

                        <hr />
                        <asp:GridView ID="gvUploadVehicle" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" AutoGenerateColumns="false" PageSize="10"
                            EmptyDataText="No Documents Found" DataKeyNames="NAME" OnRowCommand="gvUploadVehicle_Commands">
                            <Columns>
                                <asp:BoundField DataField="NAME" HeaderText="File Name" />

                                <asp:BoundField DataField="REFERENCE_NUMBER" HeaderText="Category" />

                                <asp:BoundField DataField="CREATION_DATE" HeaderText="Creation Date" DataFormatString="{0:yyyy/MM/dd}" />
                                <%--<asp:BoundField DataField="Link_URL" HeaderText="Link URL" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />--%>
                                <asp:HyperLinkField DataNavigateUrlFields="Link_URL"
                                    DataTextField="NAME"
                                    HeaderText="View Document"
                                    Target="_blank" />

                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkView" runat="server" CssClass="btn btn-info" Text="View" CommandName="View">
                                                
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Enabled="false" CssClass="btn btn-danger" Text="Delete" CommandName="DeleteFile"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle Width="550px" ForeColor="Red" Font-Bold="true" />
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
                    <h5 class="modal-title">Are You Sure You Want To Delete The Document?</h5>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">

                    <div class="form-group pull-left">
                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" Text="Yes" CommandName="DeleteItem"></asp:LinkButton>
                    </div>
                    <span class="counter pull-left"></span>

                    <div class="form-group pull-right">
                        <asp:LinkButton ID="lnkClose" runat="server" Text="No" CssClass="btn btn-info" Width="60px" data-dismiss="modal" aria-label="Close"></asp:LinkButton>
                    </div>
                    <span class="counter pull-right"></span>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <asp:Button ID="btnBack" runat="server" Text="Previous Page" Height="34px" Width="120px" class="btn btn-primary form-control" OnClick="btnBack_Click" />
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
