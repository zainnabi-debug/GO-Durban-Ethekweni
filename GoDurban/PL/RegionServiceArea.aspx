<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegionServiceArea.aspx.cs" Inherits="GoDurban.PL.RegionServiceArea" %>

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

    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnAdd').click(function () {
                var chkboxrowcount = $("#<%=gvServiceArea.ClientID%> input[id*='chkRow']:checkbox:checked").size();
                if (chkboxrowcount == 0) {
                    alert("Please Select At Least One Record");
                    return false;
                }
                return true;
            });
        });

    </script>

    <script type="text/javascript">
        function grdHeaderCheckBox(objRef) {
            var grd = objRef.parentNode.parentNode.parentNode;
            var inputList = grd.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>


    <script type="text/javascript">
        function ValidateCheckbox() {
            $('#chkRow').click(function () {
                if ($(this).is(':checked'))
                    alert('checked');
                else
                    alert("One or more Checkboxes need to be checked");
            });
        }
    </script>

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

        function ValidateRegion(ddlRegionName) {

            var region = ddlRegionName.value;
            if (region.selectedvalue == "0") {

                //alert("Office Number Must Be 10 Digits");
                $('#feedbackofnStatus').remove();
                $("#Idstatus").append("<div id='feedbackofnStatus'><center><font color='red'>Select Region</font></center></div>");
                ddlRegionName.style.border = '1px solid red';
                ddlRegionName.value = null;
                //document.getElementById("txtCellNo").focus();
                return false;
            }
            else {

                ddlRegionName.style.border = '';
                $('#feedbackofnStatus').remove();
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

        <%-- link--%>


    <%--unlink--%>
        function grdHeaderCheckBoxUnlink(objRef) {
            var grd = objRef.parentNode.parentNode.parentNode;
            var inputList = grd.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
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

    <style>
        .ChkBoxClass input {
            width: 20px;
            height: 20px;
        }
    </style>


    <div id="divsuccess" runat="server" style="display: none">
        <div class="alert alert-success">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Linked Successfully!</div>
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
            <div>Unlinked Successfully!</div>
        </div>
    </div>

    <div id="divwarning" runat="server" style="display: none">
        <div class="alert alert-warning">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Already Linked!</div>
        </div>
    </div>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger" ForeColor="Red" Visible="false"> </asp:Label>


    <h4>Link Region With Service Area</h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Region"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Select Region:</div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlRegionName" runat="server" AutoPostBack="true" class="chosen-select" Width="250px" Height="34px" AppendDataBoundItems="True" onfocusout="ValidateRegion(this);" OnSelectedIndexChanged="ddlRegionName_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">--Select Region--</asp:ListItem>
                            </asp:DropDownList>
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
                            <br />
                            <asp:Label ID="lblRegion" runat="server" Text="Select Region!" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>

                <%--                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="150px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
                    </div>
                    <span class="counter pull-right"></span>
                </div>--%>


                <p id="Idstatus">
                </p>


            </div>
        </div>
    </div>


    <div id="accordion2" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="All Service Areas"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">



                <%--<div class="form-group pull-right">
                    <input type="text" class="search form-control" placeholder="Search">
                </div>
                <span class="counter pull-right"></span>--%>

                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="gvServiceArea" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            EmptyDataText="No records found" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                            OnPageIndexChanging="gvServiceArea_PageIndexChanging" PagerStyle-Font-Size="Large"
                            DataKeyNames="ServiceAreaID">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" Text="Select/Deselect All" onclick="grdHeaderCheckBox(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" CssClass="ChkBoxClass" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ServiceAreaID" HeaderText="Id" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ServiceAreaDescription" HeaderText="Service Area"></asp:BoundField>
                            </Columns>
                            <EmptyDataRowStyle Width="550px" ForeColor="Red" Font-Bold="true" />
                        </asp:GridView>

                    </div>
                </div>

                <div class="form-group pull-left">
                    <%--<asp:Button ID="btnSelect" runat="server" Text="Select All" Height="34px" Width="150px" CssClass="btn btn-primary form-control" OnClick="btnSelect_Click" />--%>

                    <asp:Button ID="btnAdd" runat="server" Text="Link" Height="34px" Width="70px" CssClass="btn btn-primary form-control" OnClick="btnAdd_Click" />
                    <asp:Label ID="lblCheck" runat="server" Text="Please Select Atleast One Service Area!" ForeColor="Red" Visible="false"></asp:Label>
                </div>
                <span class="counter pull-left"></span>
                <br />
                <br />

            </div>
        </div>
    </div>

    <div id="accordion3" role="tablist" aria-multiselectable="true" runat="server" style="display: none">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingThird">
                <h4 class="panel-title">
                    <a id="Third" data-toggle="collapse" data-parent="#accordion3" href="#collapseThird" aria-expanded="false" aria-controls="collapseThird"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Linked Service Areas"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseThird" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingThird" style="padding: 1%">


                <%--                <div class="form-group pull-right">
                    <input type="text" class="searched form-control" placeholder="Search">
                </div>
                <span class="counter pull-right"></span>--%>

                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="gvServiceAreaUnlink" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            EmptyDataText="No records found" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                            OnPageIndexChanging="gvServiceAreaUnlink_PageIndexChanging" PagerStyle-Font-Size="Large"
                            DataKeyNames="RegionSAreaID">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="170px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllUnlink" runat="server" Text="Select/Deselect All" onclick="grdHeaderCheckBoxUnlink(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRowUnlink" runat="server" CssClass="ChkBoxClass" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RegionSAreaID" HeaderText="Id" Visible="false"></asp:BoundField>
                                <asp:BoundField DataField="ServiceAreaDescription" HeaderText="Service Area" />
                            </Columns>
                            <EmptyDataRowStyle Width="550px" ForeColor="Red" Font-Bold="true" />
                        </asp:GridView>
                    </div>
                </div>

                <div class="form-group pull-left">
                    <%--<asp:Button ID="btnSelectAllUnlink" runat="server" Text="Select All" Height="34px" Width="150px" CssClass="btn btn-primary form-control" OnClick="btnSelectAllUnlink_Click" />--%>

                    <asp:Button ID="btnUnlink" runat="server" type="button" Text="Unlink" Height="34px" Width="70px" class="btn btn-primary form-control" OnClientClick="return confirm('Are you sure you want to unlink selected records?')" OnClick="btnUnlink_Click" />
                    <asp:Label ID="lblCheckUnlink" runat="server" Text="Please Select Atleast One Service Area!" ForeColor="Red" Visible="false"></asp:Label>
                </div>
                <span class="counter pull-left"></span>
                <br />
                <br />

            </div>
        </div>
    </div>

</asp:Content>
