<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperatingLicense.aspx.cs" Inherits="GoDurban.PL.OperatingLicense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <script src="../../Scripts1/jquery.dataTables.min.js"></script>
    <script src="../../Scripts1/dataTables.bootstrap.min.js"></script>
    <script src="../../Scripts1/dataTables.buttons.min.js"></script>
    <script src="../../Scripts1/jszip.min.js"></script>
    <script src="../../Scripts1/pdfmake.min.js"></script>
    <script src="../../Scripts1/vfs_fonts.js"></script>
    <script src="../../Scripts1/buttons.html5.min.js"></script>
    <script src="../../Scripts1/dataTables.responsive.js"></script>
    <script src="../../Scripts1/buttons.bootstrap.min.js"></script>

    <link href="../../Content1/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content1/dataTables.bootstrap.css" rel="stylesheet" />
    <link href="../../Content1/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="../../Content1/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content1/responsive.bootstrap.min.css" rel="stylesheet" />


    <script type="text/javascript">

        $(document).ready(function () {
            $('.gvdatatable').dataTable({
                dom: 'Bfrtip',
                responsive: false,
                buttons: [
                    'excelHtml5',
                    'pdfHtml5'
                ],

                "order": [[2, "desc"]],
                buttons: [
                    //{
                    //    extend: 'pdf',
                    //    text: 'PDF',
                    //    title: 'Operating License',
                    //    exportOptions: {
                    //        columns: [3],
                    //    }
                    //},
                    //{
                    //    extend: 'excel',
                    //    text: 'excel',
                    //    title: 'Operating License',
                    //    exportOptions: {
                    //        columns: [3],
                    //    }
                    //}

                ],
                columnDefs: [
                    {
                        "targets": [0],
                        "orderable": false,
                        "searchable": false

                    },
                    {
                        "targets": [2],
                        "visible": false,
                        "orderable": false,
                        "searchable": false

                    },
                    {
                        "targets": [1],
                        "orderable": false,
                        "searchable": false
                    }]

            });
        });
                

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
            <div>Operating License Exists!</div>
        </div>
    </div>


    <h4>Link Operating License With Routes </h4>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Operating License-Routes "></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">OL Code:</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="txtOLCode" placeholder="OL Code" runat="server" class="form-control limited" Height="34px" Width="250px"></asp:TextBox>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-md-2">Description:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDescription" placeholder="Description" runat="server" class="form-control limited" Height="34px" Width="250px"></asp:TextBox>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-md-2">Route Code:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRouteCode" placeholder="Route Code" runat="server" class="form-control limited" Height="34px" Width="250px"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Height="34px" Width="150px" class="btn btn-primary form-control" OnClick="btnAdd_Click"/>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="34px" Width="150px" class="btn btn-primary form-control" OnClick="btnCancel_Click" />
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
                        <asp:Label ID="lblAddb" runat="server" Text="Edit/Delete"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvOL" Width="100%" CssClass="gvdatatable table table-striped table-bordered" runat="server"
                            OnPreRender="gvOL_PreRender" AutoGenerateColumns="false" DataKeyNames="OperatingLicenseID" OnRowCommand="gvOL_RowCommand">

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
                                        <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');"> Delete
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="OperatingLicenseID" HeaderText="OperatingLicenseID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="OLCode" HeaderText="OL Code" SortExpression="OLCode" />
                                 <asp:BoundField DataField="RouteCode" HeaderText="Route Code" SortExpression="RouteCode" />
                                 <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
