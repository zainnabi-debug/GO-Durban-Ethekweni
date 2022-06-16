using System;
using GoDurban.Models;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GoDurban.PL
{
    public partial class AuditTrail : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                Response.Redirect("~/Index.aspx?ShowDialog=yes");
            }
            else
            {
                string type = Session["UserRole"].ToString();

                if (type != "Admin" && type != "User" && type != "Supervisor")
                {
                    Response.Redirect("~/Error.aspx");
                }
            }

            if (!IsPostBack)
            {
                DeleteAuditTrail();
                LoadData();
            }
        }



        public void DeleteAuditTrail()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                cmd = new SqlCommand("DELETE FROM tbl_AuditTrail WHERE (Actions IS NULL OR NewData = '')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void LoadData()
        {
            var list = (from a in db.tbl_AuditTrail.ToList().OrderByDescending(x => x.Date)
                        where ((a.Actions == "Inserted" || a.Actions == "Updated") && (a.TableName == "Region" || a.TableName == "Association" || a.TableName == "Owner" || a.TableName == "ApproveVehicle" || a.TableName == "ApproveRegion" || a.TableName == "ApproveOwner" || a.TableName == "ApproveDriver" || a.TableName == "ApproveAssociation"))
                        select new
                        {
                            a.Id,
                            a.Actions,
                            a.ChangedColums,
                            a.NewData,
                            a.OldData,
                            a.TableIdValue,
                            a.Name,
                            a.Surname,
                            a.Date,
                            a.TableName,
                        });
            gvAuditTrail.DataSource = list;
            gvAuditTrail.DataBind();
        }

        protected void gvAuditTrail_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvAuditTrail.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAuditTrail.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAuditTrail.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAuditTrail.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAuditTrail_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvMore = (GridView)e.Row.FindControl("gvMore");
                int id = Convert.ToInt32(e.Row.Cells[1].Text);

                var list = (from a in db.tbl_AuditTrail
                            where a.Id == id
                            select new
                            {
                                a.Id,
                                a.ChangedColums,
                                a.OldData,
                                a.NewData
                            });
                gvMore.DataSource = list.ToList();
                gvMore.DataBind();
            }
        }

        protected void gvAuditTrail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string id = gvAuditTrail.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "Print")
            {
                //CreatePDF();               

                // Create a Document object
                var document = new Document(PageSize.A4, 50, 50, 25, 25);

                // Create a new PdfWrite object, writing the output to a MemoryStream
                var output = new MemoryStream();
                var writer = PdfWriter.GetInstance(document, output);

                // Open the Document for writing
                document.Open();

                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
                var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);


                document.Add(new Paragraph("", titleFont));
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Audit Trail", titleFont));

                document.Add(new Paragraph("", subTitleFont));
                var details = new PdfPTable(2);
                details.HorizontalAlignment = 0;
                details.SpacingBefore = 5;
                details.SpacingAfter = 5;
                details.DefaultCell.Border = 0;

                string date, name, surname, actions, page, changedfield, oldvalue, newvalue;
                var data = db.tbl_AuditTrail.ToList().FirstOrDefault(x => x.Id == Convert.ToInt16(id));
                date = data.Date.ToString();
                name = data.Name;
                surname = data.Surname;
                actions = data.Actions;
                page = data.TableName;
                changedfield = data.ChangedColums;
                oldvalue = data.OldData;
                newvalue = data.NewData;

                //details.SetWidths(new int[] { 1, 4 });
                details.AddCell(new Phrase("Date:", bodyFont));
                details.AddCell(date);
                details.AddCell(new Phrase("Name:", bodyFont));
                details.AddCell(name);
                details.AddCell(new Phrase("Surname:", bodyFont));
                details.AddCell(surname);
                details.AddCell(new Phrase("Action:", bodyFont));
                details.AddCell(actions);
                details.AddCell(new Phrase("Page:", bodyFont));
                details.AddCell(page);
                details.AddCell(new Phrase("Changed Field(s):", bodyFont));
                if (data.ChangedColums != null)
                { details.AddCell(changedfield); }
                else
                { details.AddCell("N/A"); }

                details.AddCell(new Phrase("Old Value:", bodyFont));
                if (data.OldData != null)
                { details.AddCell(oldvalue); }
                else
                { details.AddCell("N/A"); }

                details.AddCell(new Phrase("New Value:", bodyFont));
                details.AddCell(newvalue);
                document.Add(details);

                // Add the "Address" subtitle

                //// Add ending message
                //var endingMessage = new Paragraph("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.", endingMessageFont);
                //endingMessage.SetAlignment("Center");
                //document.Add(endingMessage);


                //document.Add(new Paragraph("\n"));

                var godbn = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/godurban.png"));
                godbn.SetAbsolutePosition(10, 800);
                godbn.ScaleAbsolute(200f, 40f);
                godbn.ScaleToFit(500f, 40f);
                godbn.ScaledHeight.ToString();
                godbn.ScaledWidth.ToString();

                //godbn.ScaleToFit(105f, 105f);
                //godbn.Alignment = Element.ALIGN_LEFT;

                document.Add(godbn);

                var muvo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/moja.png"));
                muvo.SetAbsolutePosition(415, 800);
                muvo.ScaleAbsolute(170f, 40f);
                godbn.ScaleToFit(300f, 40f);
                godbn.ScaledHeight.ToString();
                godbn.ScaledWidth.ToString();

                //muvo.ScaleToFit(105f, 105f);
                //muvo.Alignment = Element.ALIGN_RIGHT;

                document.Add(muvo);

                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=AuditTrail.pdf");
                Response.BinaryWrite(output.ToArray());
            }
        }
    }
}