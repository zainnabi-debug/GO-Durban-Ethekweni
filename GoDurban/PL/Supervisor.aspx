<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Supervisor.aspx.cs" Inherits="GoDurban.PL.Supervisor" %>

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
                aaSorting: [[1, "desc"]]

            });
        });

    </script>


    <style>
        /*.gvdatatable {
            width: 100%;
            word-wrap: break-word;
            table-layout: fixed;
        }*/

        .ChkBoxClass input {
            width: 20px;
            height: 20px;
        }
    </style>


    <h4>Supervisor Queue</h4>
    
    <div id="divRegion1" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo1">
                <h4 class="panel-title">
                    <a id="Second1" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo1" aria-expanded="false" aria-controls="collapseTwo1"><strong>
                        <asp:Label ID="Label3" runat="server" Text="Region"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo1" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="gvRegion" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvRegion_PreRender" AutoGenerateColumns="false"
                            DataKeyNames="RegionID" OnRowCommand="gvRegion_RowCommand">

                            <Columns>

                                <asp:BoundField DataField="RegionID" HeaderText="RegionID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="RegionName" HeaderText="Region " />
                                <asp:BoundField DataField="RegionNo" HeaderText="Region No." />
                                <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                <asp:BoundField DataField="BranchCode" HeaderText="Branch" />
                                <asp:BoundField DataField="AccountNo" HeaderText="Account No." />
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>

   <div id="divRegionalLeader" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo6">
                <h4 class="panel-title">
                    <a id="Second6" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo6" aria-expanded="false" aria-controls="collapseTwo6"><strong>
                        <asp:Label ID="Label5" runat="server" Text="Regional Leadership"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo6" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo6" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                             <asp:GridView ID="gvRegionalLeader" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvRegionalLeader_PreRender" AutoGenerateColumns="false" 
                            DataKeyNames="RegionalLeaderID" OnRowCommand="gvRegionalLeader_RowCommand">


                            <Columns>
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
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    

    <div id="divAssociation1" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo2">
                <h4 class="panel-title">
                    <a id="Second2" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo2" aria-expanded="false" aria-controls="collapseTwo2"><strong>
                        <asp:Label ID="Label4" runat="server" Text="Association"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo2" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo2" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="gvAssociation" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvAssociation_PreRender" AutoGenerateColumns="false" AllowSorting="true"
                            DataKeyNames="AssociationID" OnRowCommand="gvAssociation_RowCommand">

                            <Columns>

                                <asp:BoundField DataField="AssociationID" HeaderText="AssociationID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="AssociationName" HeaderText="Association" />
                                <asp:BoundField DataField="AssociationNo" HeaderText="Association No." />
                                <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                <asp:BoundField DataField="BranchCode" HeaderText="Branch" />
                                <asp:BoundField DataField="AccountNo" HeaderText="Account No." />
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>

 <div id="divAssociationLeadership" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo7">
                <h4 class="panel-title">
                    <a id="Second7" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo7" aria-expanded="false" aria-controls="collapseTwo7"><strong>
                        <asp:Label ID="Label6" runat="server" Text="Association Leadership"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo7" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo7" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                             <asp:GridView ID="gvAssociationLeadership" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvAssociationLeadership_PreRender" AutoGenerateColumns="false" 
                            DataKeyNames="AssociationLeaderID" OnRowCommand="gvAssociationLeadership_RowCommand">

                            <Columns>

                                <asp:BoundField DataField="AssociationLeaderID" HeaderText="AssociationLeaderID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Name" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." SortExpression="Name" />
                                <asp:BoundField DataField="Passport" HeaderText="Passport No." SortExpression="Name" />
                                <asp:BoundField DataField="CellNo" HeaderText="Cell No." SortExpression="Name" />
                                <asp:BoundField DataField="OfficeNo" HeaderText="Office No." SortExpression="Name" />
                                <%--<asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" ItemStyle-Width="50px" HeaderStyle-Width="30px" />--%>
                                <asp:BoundField DataField="AssociationName" HeaderText="Association" SortExpression="Name" />
                                <asp:BoundField DataField="RaceDescription" HeaderText="Race" SortExpression="Name" />
                                <asp:BoundField DataField="GenderDescription" HeaderText="Gender" SortExpression="Name" />
                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" SortExpression="Name" />
                                <%--<asp:BoundField DataField="ReasonDescription" HeaderText="Reason" SortExpression="Name" />--%>
                                <%-- <asp:BoundField DataField="AddressStreet" HeaderText="AddressStreet" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressSuburb" HeaderText="AddressSuburb" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressCity" HeaderText="AddressCity" SortExpression="Name" />
                                 <asp:BoundField DataField="AddressCode" HeaderText="AddressCode" SortExpression="Name" />--%>
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--</asp:Panel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>    

    <div id="divDriver" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo4">
                <h4 class="panel-title">
                    <a id="Second4" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo4" aria-expanded="false" aria-controls="collapseTwo4"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Driver"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo4" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo4" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="gvDriver" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            runat="server" OnPreRender="gvDriver_PreRender" AutoGenerateColumns="false" Font-Size="9"
                            DataKeyNames="DriverID" OnRowCommand="gvDriver_RowCommand">

                            <Columns>

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
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="divVehicle" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo5">
                <h4 class="panel-title">
                    <a id="Second5" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo5" aria-expanded="false" aria-controls="collapseTwo5"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Vehicle"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo5" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo5" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvVehicle" runat="server" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                            AutoGenerateColumns="false" DataKeyNames="VehicleID" Font-Size="9"
                            OnPreRender="gvVehicle_PreRender" OnRowCommand="gvVehicle_RowCommand">
                            <Columns>

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
                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    

    <div id="divOwner1" role="tablist" aria-multiselectable="true" runat="server" style="display: inline">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo3">
                <h4 class="panel-title">
                    <a id="Second3" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo3" aria-expanded="false" aria-controls="collapseTwo3"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Owner"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo3" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo3" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvOwner" Width="100%" runat="server" AutoGenerateColumns="false" CssClass="gvdatatable table table-striped table-bordered" 
                            DataKeyNames="OwnerID" OnRowDataBound="gvOwner_OnRowDataBound" OnRowCommand="gvOwner_RowCommand" OnPreRender="gvOwner_PreRender">

                            <Columns>
                                
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                         <a href="JavaScript:shrinkandgrow('div<%# Eval("OwnerID") %>');">
                                            <img alt="More" id="imgdiv<%# Eval("OwnerID") %>" width="25" border="1" src="../images/plus.gif" />
                                        </a>
                                        <div id="div<%# Eval("OwnerID") %>" style="display: none;">

                                            <asp:GridView ID="gvBank" runat="server" CssClass="gvdatatable table table-striped table-bordered" AutoGenerateColumns="false" DataKeyNames="OwnerID">
                                                <Columns>
                                                    <asp:BoundField DataField="BankName" HeaderText="Bank" SortExpression="Name" />
                                                    <asp:BoundField DataField="BranchCode" HeaderText="Branch" SortExpression="Name" />
                                                    <asp:BoundField DataField="AccountNo" HeaderText="Account No." SortExpression="Name" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="OwnerID" HeaderText="OwnerID" SortExpression="Id" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Name" />
                                <asp:BoundField DataField="IDNo" HeaderText="ID No." SortExpression="Name" />
                                <asp:BoundField DataField="CellNo" HeaderText="Cell No." SortExpression="Name" />
                                <%--  <asp:BoundField DataField="OfficeNo" HeaderText="Office No" SortExpression="Name" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" />--%>
                                <asp:BoundField DataField="RaceDescription" HeaderText="Race" SortExpression="Name" />
                                <asp:BoundField DataField="GenderDescription" HeaderText="Gender" SortExpression="Name" />
                                <%--<asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="Name" />--%>
                                <%--<asp:BoundField DataField="ReasonDescription" HeaderText="Reason" SortExpression="Name" />--%>

                                <%-- <asp:BoundField DataField="AddressStreet" HeaderText="AddressStreet" SortExpression="Name" />
                                <asp:BoundField DataField="AddressSuburb" HeaderText="AddressSuburb" SortExpression="Name" />
                                <asp:BoundField DataField="AddressCity" HeaderText="AddressCity" SortExpression="Name" />
                                <asp:BoundField DataField="AddressCode" HeaderText="AddressCode" SortExpression="Name" />--%>

                                <asp:BoundField DataField="StatusDescription" HeaderText="Status" SortExpression="Name" />
                                

                                <asp:TemplateField HeaderStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Height="30px" class="btn btn-outline btn-primary" Text="Select" CommandName="Select" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>

                        </asp:GridView>


                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
