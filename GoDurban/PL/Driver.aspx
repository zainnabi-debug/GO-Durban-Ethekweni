<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Driver.aspx.cs" Inherits="GoDurban.PL.Driver" %>

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

        $(function () {
            $("[id$=txtLicenseExpiry]").datepicker({
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

        $(function () {
            $("[id$=txtPRDPExpiry]").datepicker({
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

        $(function () {
            $("[id$=txtEmploymentContractExpiry]").datepicker({
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

        function onChangeApplication(rbl) {
            //Get the selected value from radiobuttonlist
            var selectedvalue = $("#" + rbl.id + " input:radio:checked").val();
            if (selectedvalue == "Yes") {
                //if so then show the controls
                document.getElementById('<%=divEmpContScan.ClientID %>').style.display = 'inline';
                document.getElementById('<%=divEmpContScan1.ClientID %>').style.display = 'inline';
                document.getElementById('<%=spAsterik.ClientID %>').style.display = 'inline';
                //document.getElementById('<%=txtEmploymentContractExpiry.ClientID %>').style.border = '1px solid red';
                //document.getElementById('<%=fuEmploymentContractScan.ClientID %>').style.border = '1px solid red';

            }
            else {
                //if not then hide the controls
                document.getElementById('<%=divEmpContScan.ClientID %>').style.display = 'none';
                document.getElementById('<%=divEmpContScan1.ClientID %>').style.display = 'none';
                document.getElementById('<%=spAsterik.ClientID %>').style.display = 'none';
                document.getElementById('<%=txtEmploymentContractExpiry.ClientID %>').style.border = '';
                document.getElementById('<%=fuEmploymentContractScan.ClientID %>').style.border = '';
            }
        }


        function openModal() {
            $('#myModal').modal('show');
        }

        function openModal() {
            $('#myModal1').modal('show');
        }

        function openModal() {
            $('#myModal2').modal('show');
        }


        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        var space = false;
        $(function () {
            $(document).keyup(function (evt) {
                if (evt.keyCode == 32) {
                    space = false;
                }
            }).keydown(function (evt) {
                if (evt.keyCode == 32) {
                    space = true;
                    console.log('space')
                }
            });
        });

        function CheckName(txt) {
            var regName = new RegExp('^[a-zA-Z\. ]+$');
            if (txt.value.match(regName) === null) {
                txt.value = txt.value.substr(0, (txt.value.length - 1));
            }
            else if (txt.KeyChar == ' ') {
                txt.value = txt.value.substr(0, (txt.value.length - 1));
            }
        }

        var frm = document.getElementById('txtName');

        if (frm === 'text') {
            frm.onsubmit = function () {
                if (/\s/.test(frm.value))
                    return false;
            }
        }

        function CheckName1(txt) {
            var regName1 = new RegExp('^[0-9a-zA-Z\. ]+$');
            if (txt.value.match(regName1) === null) {
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

        function ValidateEmpContract(txtName) {

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

        function ValidateLicenseCode(ddlLicenseCode) {

            var licensecode = ddlLicenseCode.value;
            if (licensecode.selectedvalue == "0") {
                ddlLicenseCode.style.border = '1px solid red';
                ddlLicenseCode.value = null;
                return false;
            }
            else {
                ddlLicenseCode.style.border = '';
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
        function ValidateLicenseExpiry(txtLicenseExpiry) {

            var licenseexpiry = txtLicenseExpiry.value;
            if (licenseexpiry.value == "0") {
                txtLicenseExpiry.style.border = '1px solid red';
                txtLicenseExpiry.value = null;
                return false;
            }
            else {
                txtLicenseExpiry.style.border = '';
            }
        }
        function ValidatePRDPCode(txtPRDPCode) {

            var prdpcode = txtPRDPCode.value;
            if (prdpcode.length == "") {
                txtPRDPCode.style.border = '1px solid red';
                txtPRDPCode.value = null;
                return false;
            }
            else {
                txtPRDPCode.style.border = '';
            }
        }
        function ValidatePRDPExpiry(txtPRDPExpiry) {

            var prdpexpiry = txtPRDPExpiry.value;
            if (prdpexpiry.value == "0") {
                txtPRDPExpiry.style.border = '1px solid red';
                txtPRDPExpiry.value = null;
                return false;
            }
            else {
                txtPRDPExpiry.style.border = '';
            }
        }
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

        function ValidateEmploymentContractExpiry(txtEmploymentContractExpiry) {

            var empcontractexpiry = txtEmploymentContractExpiry.value;
            if (empcontractexpiry.value == "0") {
                txtEmploymentContractExpiry.style.border = '1px solid red';
                txtEmploymentContractExpiry.value = null;
                return false;
            }
            else {
                txtEmploymentContractExpiry.style.border = '';
            }
        }

        function ValidateTermsConditions(chkTermsConditions) {
            $("#chkTermsConditions").attr("checked") ? chkTermsConditions.style.border = '' : chkTermsConditions.style.border = '';
        }

        function ValidateReviewContract(chkReviewContract) {
            $("#chkReviewContract").attr("checked") ? chkReviewContract.style.border = '' : chkReviewContract.style.border = '';
        }

        function ValidatePRDPScan(fuPRDPScan) {

            var prdpscan = fuPRDPScan.value;
            if (prdpscan.value == false) {
                fuPRDPScan.style.border = '1px solid red';
                fuPRDPScan.value = null;
                return false;
            }
            else {
                fuPRDPScan.style.border = '';
            }
        }

        function ValidateRecommendationLetterScan(fuRecommendationLetterScan) {

            var recletter = fuRecommendationLetterScan.value;
            if (recletter.value == false) {
                fuRecommendationLetterScan.style.border = '1px solid red';
                fuRecommendationLetterScan.value = null;
                return false;
            }
            else {
                fuRecommendationLetterScan.style.border = '';
            }
        }

        function ValidateDriverLicenseScan(fuDriverLicenseScan) {

            var drilicensescan = fuDriverLicenseScan.value;
            if (drilicensescan.value == false) {
                fuDriverLicenseScan.style.border = '1px solid red';
                fuDriverLicenseScan.value = null;
                return false;
            }
            else {
                fuDriverLicenseScan.style.border = '';
            }
        }

        function ValidateEmploymentContractScan(fuEmploymentContractScan) {

            var empcontractscan = fuEmploymentContractScan.value;
            if (empcontractscan.value == false) {
                fuEmploymentContractScan.style.border = '1px solid red';
                fuEmploymentContractScan.value = null;
                return false;
            }
            else {
                fuEmploymentContractScan.style.border = '';
            }
        }




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


        function showButton1() {
            <%--document.getElementById("<%=divEmpContract.ClientID%>").style.display = 'inline';--%>
        }
        function showButton2() {
           <%-- document.getElementById("<%=divRecLetter.ClientID%>").style.display = 'inline';--%>
        }
        function showButton3() {
            <%--document.getElementById("<%=divDriverLicense.ClientID%>").style.display = 'inline';--%>
        }
        function showButton4() {
           <%-- document.getElementById("<%=divPRDPScan.ClientID%>").style.display = 'inline';--%>
        }

    </script>


    <script type="text/javascript">
        var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt"];
        function ValidateSingleInputEmp(oInput) {
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            document.getElementById("<%=divEmpContract.ClientID%>").style.display = 'inline';
                            break;
                        }
                    }

                    if (!blnValid) {
                        alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                        oInput.value = "";
                        document.getElementById("<%=divEmpContract.ClientID%>").style.display = 'none';
                        return false;
                    }
                }
            }
            return true;
        }

        function ValidateSize1(file) {
            var FileSize = file.files[0].size / 1024 / 1024; // in MB
            if (FileSize > 2) {
                alert('File size exceeds 2 MB');
                $(file).val('');
                document.getElementById("<%=divEmpContract.ClientID%>").style.display = 'none';
                return false;
            } else {
                document.getElementById("<%=divEmpContract.ClientID%>").style.display = 'inline';
            }
        }

        var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt"];
        function ValidateSingleInputRec(oInput) {
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            document.getElementById("<%=divRecLetter.ClientID%>").style.display = 'inline';
                            break;
                        }
                    }

                    if (!blnValid) {
                        alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                        oInput.value = "";
                        document.getElementById("<%=divRecLetter.ClientID%>").style.display = 'none';
                        return false;
                    }
                }
            }
            return true;
        }

        function ValidateSize2(file) {
            var FileSize = file.files[0].size / 1024 / 1024; // in MB
            if (FileSize > 2) {
                alert('File size exceeds 2 MB');
                $(file).val('');
                document.getElementById("<%=divRecLetter.ClientID%>").style.display = 'none';
                return false;
            } else {
                document.getElementById("<%=divRecLetter.ClientID%>").style.display = 'inline';
            }
        }

        var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt"];
        function ValidateSingleInputDri(oInput) {
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            document.getElementById("<%=divDriverLicense.ClientID%>").style.display = 'inline';
                            break;
                        }
                    }

                    if (!blnValid) {
                        alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                        oInput.value = "";
                        document.getElementById("<%=divDriverLicense.ClientID%>").style.display = 'none';
                        return false;
                    }
                }
            }
            return true;
        }

        function ValidateSize3(file) {
            var FileSize = file.files[0].size / 1024 / 1024; // in MB
            if (FileSize > 2) {
                alert('File size exceeds 2 MB');
                $(file).val('');
                document.getElementById("<%=divDriverLicense.ClientID%>").style.display = 'none';
                return false;
            } else {
                document.getElementById("<%=divDriverLicense.ClientID%>").style.display = 'inline';
            }
        }

        var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt"];
        function ValidateSingleInputPRD(oInput) {
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            document.getElementById("<%=divPRDPScan.ClientID%>").style.display = 'inline';
                            break;
                        }
                    }

                    if (!blnValid) {
                        alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                        oInput.value = "";
                        document.getElementById("<%=divPRDPScan.ClientID%>").style.display = 'none';
                        return false;
                    }
                }
            }
            return true;
        }

        function ValidateSize4(file) {
            var FileSize = file.files[0].size / 1024 / 1024; // in MB
            if (FileSize > 2) {
                alert('File size exceeds 2 MB');
                $(file).val('');
                document.getElementById("<%=divPRDPScan.ClientID%>").style.display = 'none';
                return false;
            } else {
                document.getElementById("<%=divPRDPScan.ClientID%>").style.display = 'inline';
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
            <div>Edited Successfully!</div>
        </div>
    </div>

    <div id="divwarning" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Driver Exists!</div>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>


    <h4>Create Driver</h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
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
                    <asp:Label ID="lblApproved" runat="server" Text="No Approved Owner!" ForeColor="Red" Visible="false" />
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlOwner" runat="server" class="chosen-select" Width="250px" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateOwner(this);" OnSelectedIndexChanged="ddlOwner_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">--Select Owner--</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="lblOwner" runat="server" ForeColor="Red"> </asp:Label>
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
    <div id="accordion1" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text=" Driver Info"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Name: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" placeholder="Name" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeypress="return isSpaceKey(event)" onkeyup="CheckName(this); SetButtonStatus(this,'divShow'); " onkeydown="CheckName(this);" onfocusout="ValidateName(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">Surname: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSurname" placeholder="Surname" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeyup="CheckName(this); SetButtonStatus(this,'divShow'); " onkeydown="CheckName(this);" onfocusout="ValidateSurname(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSurname"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">ID No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIDNo" placeholder="ID No." runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onkeypress="return isNumberKey(event)" onfocusout="ValidateIDNo(this);" MaxLength="13"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtIDNo"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">Office No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtOfficeNo" placeholder="Office No." runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onkeypress="return isNumberKey(event)" onfocusout="ValidateOfficeNO(this);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Cell No.: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCellNo" placeholder="Cell No." runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onkeypress="return isNumberKey(event)" onfocusout="ValidateCellNO(this);" MaxLength="10"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCellNo"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">Gender: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGender" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateGender(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Gender--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Email:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" placeholder="Email" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="ValidateEmailID(this);" onfocusout="ValidateBusinessEmail(this);"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-2">Race: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRace" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateRace(this);">
                            <asp:ListItem Selected="True" Value="0">--Select Race--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">License Expiry: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLicenseExpiry" placeholder="License Expiry" runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateLicenseExpiry(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtLicenseExpiry"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">License Code: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLicenseCode" runat="server" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateLicenseCode(this);">
                            <asp:ListItem Selected="True" Value="0">--Select License Code--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">PRDP Code: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPRDPCode" placeholder="PRDP Code" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidatePRDPCode(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPRDPCode"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">PRDP Expiry: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPRDPExpiry" placeholder="PRDP Expiry" runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidatePRDPExpiry(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPRDPExpiry"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                </div>



                <%--   <div class="row">
                <div class="col-md-2">: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="FileUpload1" onchange="showButton2()" runat="server" />
                        <div id="div1" runat="server" style="display: none">
                            <button runat="server" id="Button1" style="width: 100px; height: 30px" title="btnUploadDoc" type="button" class="btn btn-success btn-block" onserverclick="btnRecLetter_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>
                    </div></div>--%>

                <br />
                <div class="row">
                    <div class="col-md-2">Street No. & Name: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStreetNo" placeholder="Street No. & Name" runat="server" class="form-control limited" MaxLength="100" Height="34px" Width="250px" onkeyup="CheckName1(this); SetButtonStatus(this,'divShow')" onkeydown="CheckName1(this);" onfocusout="ValidateStreetNoName(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtStreetNo"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">Suburb: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSuburb" placeholder="Suburb" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="CheckName(this); SetButtonStatus(this,'divShow')" onkeydown="CheckName(this);" onfocusout="ValidateSuburb(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSuburb"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">City: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCity" placeholder="City" runat="server" class="form-control limited" MaxLength="50" Height="34px" Width="250px" onkeyup="CheckName(this); SetButtonStatus(this,'divShow')" onkeydown="CheckName(this);" onfocusout="ValidateCity(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                    <div class="col-md-2">Code: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCode" placeholder="Code" runat="server" class="form-control limited" Height="34px" Width="250px" onkeyup="SetButtonStatus(this,'divShow')" onkeypress="return isNumberKey(event)" MaxLength="4" onfocusout="ValidateCode(this);"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCode"
                            CssClass="text-danger" ErrorMessage="" />--%>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">Status: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" AppendDataBoundItems="True" CssClass="form-control" Width="250px" Height="34px" onkeyup="SetButtonStatus(this,'divShow')" onfocusout="ValidateStatus(this);" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
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
                <br />
                <div class="row">
                    <div class="col-md-2">Employment Contract: <span style="color: red">*</span></div>
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="rblEmploymentContract" runat="server" onclick="onChangeApplication(this)" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblEmploymentContract_SelectedIndexChanged">
                            <asp:ListItem Value="Yes" Text="Yes" />
                            <asp:ListItem Value="No" Text="No" Selected="True" />
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-md-2">Employment Contract Expiry: <span id="spAsterik" runat="server" style="color: red; display: none">*</span></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmploymentContractExpiry" placeholder="Employment Contract Expiry" runat="server" onkeyup="SetButtonStatus(this,'divShow')" class="form-control limited" Height="34px" Width="250px" onfocusout="ValidateEmploymentContractExpiry(this);"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <asp:TextBox ID="txtRecommendationLetterScan" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtRecommendationLetterScan1" runat="server" Visible="false"></asp:TextBox>
                    <div class="col-md-2">Recommendation Letter Scan: <span style="color: red"></span></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuRecommendationLetterScan" onchange="showButton2();ValidateSingleInputRec(this);ValidateSize2(this);" runat="server" onfocusout="ValidateRecommendationLetterScan(this);" />
                        <asp:Label ID="lblRecLetter" runat="server" Text="File Uploaded Successfully!" ForeColor="Green" Visible="false" />
                        <asp:Label ID="lblRecLetter1" runat="server" Text="Upload File First!" ForeColor="Red" Visible="false" />
                        <asp:Label ID="lblRecLetter2" runat="server" Text="File Exists. Try Upload A New One!" ForeColor="Red" Visible="false" />
                        <div id="divRecLetter" runat="server" style="display: none">
                            <button runat="server" id="btnRecLetter" style="width: 100px; height: 30px" title="btnUploadDoc" type="button" class="btn btn-success btn-block" onserverclick="btnRecLetter_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>
                        <br />
                    </div>

                    <asp:TextBox ID="txtEmpContScan" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtEmpContScan1" runat="server" Visible="false"></asp:TextBox>
                    <div class="col-md-2" id="divEmpContScan" runat="server" style="display: none">Employment Contract Scan: <span style="color: red"></span></div>
                    <div class="col-md-4" id="divEmpContScan1" runat="server" style="display: none">
                        <asp:FileUpload ID="fuEmploymentContractScan" onchange="showButton1();ValidateSingleInputEmp(this);ValidateSize1(this);" runat="server" onfocusout="ValidateEmploymentContractScan(this);" />
                        <asp:Label ID="lblEmpScan" runat="server" Text="File Uploaded Successfully!" ForeColor="Green" Visible="false" />
                        <asp:Label ID="lblEmpScan1" runat="server" Text="Upload File First!" ForeColor="Red" Visible="false" />
                        <asp:Label ID="lblEmpScan2" runat="server" Text="File Exists. Try Upload A New One!" ForeColor="Red" Visible="false" />
                        <div id="divEmpContract" runat="server" style="display: none">
                            <button runat="server" id="btnEmpContract" title="btnUploadDoc" style="width: 100px; height: 30px" type="button" class="btn btn-success btn-block" onserverclick="btnEmpContract_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>
                        <br />
                    </div>
                </div>

                <div class="row">
                    <asp:TextBox ID="txtDriverLicense" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtDriverLicense1" runat="server" Visible="false"></asp:TextBox>
                    <div class="col-md-2">Driver License Scan: <span style="color: red"></span></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuDriverLicenseScan" onchange="showButton3();ValidateSingleInputDri(this);ValidateSize3(this);" runat="server" onfocusout="ValidateDriverLicenseScan(this);" />
                        <asp:Label ID="lblDriLicense" runat="server" Text="File Uploaded Successfully!" ForeColor="Green" Visible="false" />
                        <asp:Label ID="lblDriLicense1" runat="server" Text="Upload File First!" ForeColor="Red" Visible="false" />
                        <asp:Label ID="lblDriLicense2" runat="server" Text="File Exists. Try Upload A New One!" ForeColor="Red" Visible="false" />
                        <div id="divDriverLicense" runat="server" style="display: none">
                            <button runat="server" id="btnDriverLicense" style="width: 100px; height: 30px" title="button" type="button" class="btn btn-success btn-block" onserverclick="btnDriverLicense_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>

                        <br />
                    </div>

                    <asp:TextBox ID="txtPRDPScan" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtPRDPScan1" runat="server" Visible="false"></asp:TextBox>
                    <div class="col-md-2">PRDP Scan: <span style="color: red"></span></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuPRDPScan" onchange="showButton4();ValidateSingleInputPRD(this);ValidateSize4(this);" runat="server" onfocusout="ValidatePRDPScan(this);" />
                        <asp:Label ID="lblPRDPScan" runat="server" Text="File Uploaded Successfully!" ForeColor="Green" Visible="false" />
                        <asp:Label ID="lblPRDPScan1" runat="server" Text="Upload File First!" ForeColor="Red" Visible="false" />
                        <asp:Label ID="lblPRDPScan2" runat="server" Text="File Exists. Try Upload A New One!" ForeColor="Red" Visible="false" />
                        <div id="divPRDPScan" runat="server" style="display: none">
                            <button runat="server" id="btnPRDPScan" style="width: 100px; height: 30px" title="btnUploadDoc" type="button" class="btn btn-success btn-block" onserverclick="btnPRDPScan_ServerClick">
                                <i class="glyphicon glyphicon-file"></i>
                                Upload
                            </button>
                        </div>
                        <br />
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
                <br />

                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnAdd_Click" OnClientClick="initLoadHoldOn();" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-md-2" id="divDoc" runat="server" style="display: none">
                        <asp:Button ID="btnDoc" runat="server" Text="View Files" Height="34px" Width="90px" class="btn btn-primary form-control" OnClick="btnDoc_Click" OnClientClick="initLoadHoldOn();"/>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="divDriver" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion2" href="#collapseFour" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Edit"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseFour" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvDriver" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            AutoGenerateColumns="false" DataKeyNames="DriverID" Font-Size="9"
                            OnPreRender="gvDriver_PreRender" OnRowCommand="gvDriver_RowCommand" OnRowDataBound="gvDriver_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-info" Text="Edit" CommandName="EditItem" OnClientClick="initLoadHoldOn();"> Edit
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrint" runat="server" Visible="false" CssClass="btn btn-info" Text="Print" CommandName="Print"> Print
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="DriverID" HeaderText="DriverID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="OwnerName" HeaderText="Owner" SortExpression="Name" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Name" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." SortExpression="Name" />
                                <asp:BoundField DataField="CellNo" HeaderText="Cell No." SortExpression="Name" />
                                <%--<asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" />--%>
                                <asp:BoundField DataField="RaceDescription" HeaderText="Race" SortExpression="Name" />
                                <asp:BoundField DataField="GenderDescription" HeaderText="Gender" SortExpression="Name" />
                                <asp:BoundField DataField="CalcDriverNO" HeaderText="Driver No." SortExpression="Name" />
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

    <div id="divUploadDocuments" role="tablist" aria-multiselectable="true" runat="server" style="display: none">
        <div class=" panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label12" runat="server" Text="Upload Files"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseThree" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingEleven" style="padding: 1%">

                <div class="row">
                    <div class="col-md-12">

                        <hr />
                        <asp:GridView ID="gvUpload" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" AutoGenerateColumns="false" PageSize="10"
                            EmptyDataText="No Documents Found" DataKeyNames="NAME" OnRowCommand="gvAttachments_Commands">
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
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" Text="Delete" CommandName="DeleteFile"></asp:LinkButton>
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

    <div class="modal about-modal fade" id="myModal2" tabindex="-1" role="dialog">
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
                        <%--<button id="lnkDelete" type="button" class="btn btn-danger" Text="Yes"></button>--%>

                        <%--<asp:Button ID="btnDeleteDoc" runat="server" CssClass="btn btn-danger" Text="Yes" OnClick="btnDeleteDoc_Click" />--%>
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

</asp:Content>
