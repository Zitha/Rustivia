using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WeighBridgeApplication.RustiviaService.DbContext;

namespace WeighBridgeApplication.Util
{
    public class PrintTicket
    {
        public void PrintTicketSlip(WeighBridgeInfo weighBridgeInfo)
        {
            var pageRectangle = new Rectangle(226f, 567f);
            //            var document = new Document(PageSize.A6, 88f, 88f, 10f, 10f);
            var document = new Document(pageRectangle);
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                Phrase phrase = null;
                PdfPCell cell = null;
                PdfPTable table = null;
                BaseColor color = null;

                document.Open();

                //Header Table
                table = new PdfPTable(2) {HorizontalAlignment = Element.ALIGN_LEFT};
                table.SetWidths(new[] {0.3f, 1f});
                table.SpacingBefore = 20f;

                //Company Name and Address
                phrase = new Phrase
                {
                    new Chunk("Rustivia Metals CC\n\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                    new Chunk("54, Northreef,\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                    new Chunk("Activia park,\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                    new Chunk("Germiston, South Africa \n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                    new Chunk(string.Format("{0} {1}", "(T)", "011 828 9961 \n"),
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                    new Chunk(string.Format("{0} {1}", "Reg no.", "1997/0025504/23 \n"),
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK))
                };


                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Colspan = 5;
                table.AddCell(cell);

                //Separater Line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 85f, document.Top - 25f, document.PageSize.Width - 240f, document.Top - 25f, color);
                document.Add(table);

                table = new PdfPTable(2) {HorizontalAlignment = Element.ALIGN_LEFT};
                table.SetWidths(new[] {0.3f, 1f});
                table.SpacingBefore = 10f;

                //Transaction number
                table.AddCell(
                    PhraseCell(
                        new Phrase("Transaction No:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(string.Format("{0}", weighBridgeInfo.Id),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.ITALIC, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Registration number
                table.AddCell(
                    PhraseCell(
                        new Phrase("Registation No:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                phrase =
                    new Phrase(new Chunk(weighBridgeInfo.Truck.TruckRegNumber + "\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Date 
                table.AddCell(
                    PhraseCell(
                        new Phrase("Date:", FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                phrase =
                    new Phrase(new Chunk(weighBridgeInfo.Date + "\n",
                        FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)));
                table.AddCell(PhraseCell(phrase, Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Supplier
                table.AddCell(
                    PhraseCell(
                        new Phrase("Supplier:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase("Kepmton Park",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Driver
                table.AddCell(
                    PhraseCell(
                        new Phrase("Driver:", FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Format("{0} {1}", weighBridgeInfo.Driver.Firstname, weighBridgeInfo.Driver.Surname),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Product
                table.AddCell(
                    PhraseCell(
                        new Phrase("Product:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(weighBridgeInfo.Product,
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //Comments
                table.AddCell(
                    PhraseCell(
                        new Phrase("Comments:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(weighBridgeInfo.Comments,
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //FirstMass
                table.AddCell(
                    PhraseCell(
                        new Phrase("First Mass:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Format("{0} {1}", weighBridgeInfo.FirstMass, "KG"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //SecondMass
                table.AddCell(
                    PhraseCell(
                        new Phrase("Second Mass:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Format("{0} {1}", weighBridgeInfo.SecondMass, "KG"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 2f;
                table.AddCell(cell);

                //NettMass
                table.AddCell(
                    PhraseCell(
                        new Phrase("Nett Mass:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Format("{0} {1}", weighBridgeInfo.NettMass, "KG"),
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 5f;
                table.AddCell(cell);

                //WeighBridge Cleck
                Font verdana = FontFactory.GetFont("Microsoft Sans Serif", 20, Font.BOLD | Font.UNDERLINE,
                    new BaseColor(255, 0, 0));


                table.AddCell(
                    PhraseCell(
                        new Phrase("Weighbridge Clerk:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                table.AddCell(
                    PhraseCell(
                        new Phrase(
                            string.Empty,
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.NORMAL, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 8f;
                table.AddCell(cell);

                //Add line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 177f, document.Top - 105f, document.PageSize.Width - 300f, document.Top - 105f, color);
                //Driver Signature
                table.AddCell(
                    PhraseCell(
                        new Phrase("Driver Signature:",
                            FontFactory.GetFont("Microsoft Sans Serif", 2, Font.BOLD, BaseColor.BLACK)),
                        Element.ALIGN_LEFT));
                cell = PhraseCell(new Phrase(), Element.ALIGN_CENTER);
                cell.Colspan = 2;
                cell.PaddingBottom = 3f;
                table.AddCell(cell);

                //Add line
                //                color = BaseColor.BLACK;
                //                DrawLine(writer, 177f, document.Top - 116f, document.PageSize.Width - 300f, document.Top - 116f, color);

                document.Add(table);
                document.Close();
                byte[] bytes = memoryStream.ToArray();


                using (FileStream fs = File.Create("C:\\Test.pdf"))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
                //                var fs2 = new FileStream("C:\\Test.pdf", FileMode.Open);
                //                var reader = new StreamReader(fs2);
                //                string result = reader.ReadToEnd();
                //                var printDialog = new PrintDialog();
                //                if (printDialog.ShowDialog() == true)
                //                {
                //                    var doc = new FlowDocument(new Paragraph(new Run()))
                //                    {
                //                        Name = "FlowDoc"
                //                    };
                //                    IDocumentPaginatorSource idpSource = doc;
                //                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");
                //                }
                memoryStream.Close();
            }
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            var cell = new PdfPCell(phrase)
            {
                BorderColor = BaseColor.WHITE,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = align,
                PaddingBottom = 1f,
                PaddingTop = 0f
            };
            return cell;
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            Image image = Image.GetInstance("");
            image.ScalePercent(scale);
            var cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }
    }
}