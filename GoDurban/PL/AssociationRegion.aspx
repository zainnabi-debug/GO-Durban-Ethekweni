<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssociationRegion.aspx.cs" Inherits="GoDurban.PL.AssociationRegion" %>

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

        function validate() {
            var role = '<%= Session["UserRole"] %>';
            var check = document.getElementById('lnkDelete');
            if (role == 'User C') {
                check.disabled = true;
            }
            else {
                return confirm('Are you sure you want to unlink this record?');
                check.disabled = false;
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

    </script>

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


    <h4>Link Association With Region</h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Association & Region"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Select Region:</div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlRegion" runat="server" class="chosen-select" Width="250px" Height="34px" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="0">--Select Region--</asp:ListItem>
                            </asp:DropDownList><asp:Label ID="lblRegion" runat="server" ForeColor="Red"> </asp:Label>
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
                    <div class="col-md-2">Select Association:</div>
                    <div class="col-md-4">
                        <div class="side-by-side clearfix">
                            <asp:DropDownList ID="ddlAssociation" runat="server" AppendDataBoundItems="True" class="chosen-select" Width="250px" Height="34px">
                                <asp:ListItem Selected="True" Value="0">--Select Association--</asp:ListItem>
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
                            <asp:Label ID="lblAssociation" runat="server" ForeColor="Red"> </asp:Label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Link" Height="34px" Width="70px" class="btn btn-primary form-control" OnClick="btnAdd_Click" />
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
                        <asp:Label ID="lblAddb" runat="server" Text="Linked Association with Region"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvAssociationRegion" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            AutoGenerateColumns="false" DataKeyNames="AssociationRegionID"
                            OnPreRender="gvAssociationRegion_PreRender" OnRowCommand="gvAssociationRegion_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-primary form-control" Enabled="true" Text="Delete" Width="70px" CommandName="DeleteItem" OnClientClick="return validate()"> Unlink
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="AssociationRegionID" HeaderText="AssociationRegionID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="RegionName" HeaderText="Region" SortExpression="Name" />
                                <asp:BoundField DataField="AssociationName" HeaderText="Association" SortExpression="Name" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
