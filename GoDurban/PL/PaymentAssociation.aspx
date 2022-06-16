<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentAssociation.aspx.cs" Inherits="GoDurban.PL.PaymentAssociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function shrinkandgrow(input) {
            var displayIcon = "img" + input;
            if ($("#" + displayIcon).attr("src") == "../images/plus.gif") {
                $("#" + displayIcon).closest("tr")
                    .after("<tr><td></td><td colspan = '100%'>" + $("#" + input)
                        .html() + "</td></tr>");
                $("#" + displayIcon).attr("src", "../images/minus.gif");
            } else {
                $("#" + displayIcon).closest("tr").next().remove();
                $("#" + displayIcon).attr("src", "../images/plus.gif");
            }
        }
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
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnExportTo').click(function () {
                var chkboxrowcount = $("#<%=gvPayment.ClientID%> input[id*='chkRow']:checkbox:checked").size();
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
        function ValidateCheckbox() {
            $('#chkRow').click(function () {
                if ($(this).is(':checked'))
                    alert('checked');
                else
                    alert("One or more Checkboxes need to be checked");
            });
        }
    </script>

    <style>
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

    <style>
        .gvdatatable {
            width: 100%;
            word-wrap: break-word;
            table-layout: fixed;
        }

        .HeaderStyle {
            border: solid 1px White;
            background-color: #81BEF7;
            font-weight: bold;
            text-align: center;
        }
    </style>

    <%-- <style>
        .rTable {
            display: table;
            width: 100%;
        }

        .rTableRow {
            display: table-row;
        }

        .rTableHeading {
            display: table-header-group;
            background-color: #ddd;
        }

        .rTableCell, .rTableHead {
            display: table-cell;
            padding: 3px 10px;
            border: 1px solid #999999;
        }

        .rTableHeading {
            display: table-header-group;
            background-color: #ddd;
            font-weight: bold;
        }

        .rTableFoot {
            display: table-footer-group;
            font-weight: bold;
            background-color: #ddd;
        }

        .rTableBody {
            display: table-row-group;
        }
    </style>--%>


    <div id="divsuccess" runat="server" style="display: none">
        <div class="alert alert-success">
            <a href="#" class="close" data-dismiss="alert">&times;</a>
            <div>Sent Successfully!</div>
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
            <div>Association Exists!</div>
        </div>
    </div>

    <asp:Label ID="lblError" runat="server" ForeColor="Red"> </asp:Label>

    <h4>Payment-Association</h4>

   <p>Records for the current month and any voided voucher/ payment transactions</p>

    <div id="divPayment" role="tablist" aria-multiselectable="true" runat="server" visible="false">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Payment-Association"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-12">

                         <asp:GridView ID="gvPayment" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvPayment_PreRender" AutoGenerateColumns="false" AllowSorting="true" 
                            DataKeyNames="PaymentID" Font-Size="9" OnRowDataBound="gvPayment_RowDataBound">

                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" runat="server" Text="Select/Deselect All" onclick="grdHeaderCheckBox(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" CssClass="ChkBoxClass" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PaymentID" HeaderText="PaymentID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <%--<asp:BoundField DataField="TransNo" HeaderText="Trans No." />--%>
                                <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy/MM/dd}" />
                                <%--<asp:BoundField DataField="DocTy" HeaderText="DocTy" />--%>
                                <asp:BoundField DataField="SP" HeaderText="SP" />
                                <%--<asp:BoundField DataField="STSCD" HeaderText="STSCD" />--%>
                                <%--<asp:BoundField DataField="LongAddress" HeaderText="LongAddress" />--%>
                                <asp:BoundField DataField="AccountName" HeaderText="Account Name" />
                                <asp:BoundField DataField="AccountNo" HeaderText="Account No." />
                                <%--<asp:BoundField DataField="IDIssuer" HeaderText="IDIssuer" />--%>
                                <%--<asp:BoundField DataField="ID1Code" HeaderText="ID1Code" />--%>
                                <%--<asp:BoundField DataField="TaxID" HeaderText="TaxID" />--%>
                                <%--<asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />--%>
                                <%--<asp:BoundField DataField="address" HeaderText="Address" />--%>
                                <%--<asp:BoundField DataField="AddressLine1" HeaderText="AddressLine1" />
                                <asp:BoundField DataField="AddressLine2" HeaderText="AddressLine2" />
                                <asp:BoundField DataField="AddressLine3" HeaderText="AddressLine3" />--%>
                                <%--<asp:BoundField DataField="AddressLine4" HeaderText="AddressLine4" />--%>
                                <%-- <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="PostalCode" HeaderText="PostalCode" />--%>
                                <asp:BoundField DataField="Amount" HeaderText="Amount(R)" />
                                <%--<asp:BoundField DataField="AddressNo" HeaderText="AddressNo" />--%>
                                <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" />
                                <asp:BoundField DataField="IsPaymentSent" HeaderText="Payment Sent" />
                                <asp:BoundField DataField="VoidedPayment" HeaderText="Payment Voided" />
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <a href="JavaScript:shrinkandgrow('div<%# Eval("PaymentID") %>');">
                                            <img alt="More" id="imgdiv<%# Eval("PaymentID") %>" width="25" border="1" src="../images/plus.gif" />
                                        </a>
                                        <div id="div<%# Eval("PaymentID") %>" style="display: none;">

                                            <asp:GridView ID="gvBank" runat="server" CssClass="gvdatatable table table-striped table-bordered" AutoGenerateColumns="false" DataKeyNames="PaymentID">
                                                <Columns>
                                                    <asp:BoundField DataField="LineNo" HeaderText="Line No." />
                                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No." />
                                                    <asp:BoundField DataField="DocNo" HeaderText="Doc No." />
                                                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                                    <asp:BoundField DataField="BankTransit" HeaderText="Bank Transit" />
                                                    <asp:BoundField DataField="BankAccNo" HeaderText="Bank Account No." />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                        </asp:GridView>

                    </div>
                </div>
                <hr />
               <div class="row" id="divExport" runat="server">
                    <div class="form-group pull-left">
                    </div>
                    <span class="counter pull-right"></span>
                    <asp:Button ID="Button1" runat="server" Text="Export To Excel" Height="34px" Width="160px" class="btn btn-primary form-control" OnClick="Button1_Click" style="padding-left:10px!important"/>
                    <asp:Button ID="Button2" runat="server" Text="Export To PDF" Height="34px" Width="160px" class="btn btn-primary form-control" style="padding-left:10px!important"/>
                        <asp:Button ID="btnExportTo" runat="server" Text="Send to Staging Area" Height="34px" Width="190px" class="btn btn-primary form-control" OnClick="btnExportTo_Click" OnClientClick="initLoadHoldOn();"/>                       
                        <asp:Label ID="lblSent" runat="server" Text="Record(s) sent!" ForeColor="Green" Visible="false" style="padding-left:10px"></asp:Label>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
