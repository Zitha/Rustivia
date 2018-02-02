// Decompiled with JetBrains decompiler
// Type: WeighBridgeApplication.Util.PrintTicket
// Assembly: WeighBridgeApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0008ECA4-ECDC-4057-A6F3-F7FD4CE2F23F
// Assembly location: C:\Users\Ndavhe\Desktop\Rustivia Weight Bridge\WeighBridgeApplication.exe
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using WeighBridgeApplication.Model;

namespace WeighBridgeApplication.Util
{
    public class PrintTicket
    {
        public void PrintTicketSlip(Purchase purchase)
        {
            Document document = new Document(new Rectangle(226f, 567f));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(document, (Stream)memoryStream);
                document.Open();
                PdfPTable pdfPtable1 = new PdfPTable(2)
                {
                    HorizontalAlignment = 0
                };
                pdfPtable1.SetWidths(new float[2] { 0.3f, 1f });
                pdfPtable1.SpacingBefore = 20f;
                Phrase phrase1 = new Phrase()
        {
          (IElement) new Chunk("Rustivia Metals CC\n\n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)),
          (IElement) new Chunk("54, Northreef,\n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)),
          (IElement) new Chunk("Activia park,\n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)),
          (IElement) new Chunk("Germiston, South Africa \n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)),
          (IElement) new Chunk(string.Format("{0} {1}", (object) "(T)", (object) "011 828 9961 \n"), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)),
          (IElement) new Chunk(string.Format("{0} {1}", (object) "Reg no.", (object) "1997/0025504/23 \n"), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK))
        };
                pdfPtable1.AddCell(PrintTicket.PhraseCell(phrase1, 0));
                PdfPCell cell1 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell1.VerticalAlignment = 4;
                cell1.Colspan = 5;
                pdfPtable1.AddCell(cell1);
                document.Add((IElement)pdfPtable1);
                PdfPTable pdfPtable2 = new PdfPTable(2)
                {
                    HorizontalAlignment = 0
                };
                pdfPtable2.SetWidths(new float[2] { 0.3f, 1f });
                pdfPtable2.SpacingBefore = 10f;
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Transaction No:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Format("{0}", (object)purchase.Id), FontFactory.GetFont("Microsoft Sans Serif", 2f, 2, BaseColor.BLACK)), 0));
                PdfPCell cell2 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell2.Colspan = 2;
                cell2.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell2);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Registation No:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                Phrase phrase2 = new Phrase(new Chunk(purchase.Truck.TruckRegNumber + "\n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(phrase2, 0));
                PdfPCell cell3 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell3.Colspan = 2;
                cell3.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell3);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Date:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                Phrase phrase3 = new Phrase(new Chunk(purchase.WeighBridgeInfo.DateIn.ToString() + "\n", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(phrase3, 0));
                PdfPCell cell4 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell4.Colspan = 2;
                cell4.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell4);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Supplier:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Kepmton Park", FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell5 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell5.Colspan = 2;
                cell5.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell5);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Driver:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Format("{0} {1}", (object)purchase.Driver.Firstname, (object)purchase.Driver.Surname), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell6 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell6.Colspan = 2;
                cell6.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell6);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Product:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(purchase.WeighBridgeInfo.Product, FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell7 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell7.Colspan = 2;
                cell7.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell7);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Comments:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(purchase.WeighBridgeInfo.Comments, FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell8 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell8.Colspan = 2;
                cell8.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell8);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("First Mass:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Format("{0} {1}", (object)purchase.WeighBridgeInfo.FirstMass, (object)"KG"), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell9 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell9.Colspan = 2;
                cell9.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell9);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Second Mass:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Format("{0} {1}", (object)purchase.WeighBridgeInfo.SecondMass, (object)"KG"), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell10 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell10.Colspan = 2;
                cell10.PaddingBottom = 2f;
                pdfPtable2.AddCell(cell10);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Nett Mass:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Format("{0} {1}", (object)purchase.WeighBridgeInfo.NettMass, (object)"KG"), FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell11 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell11.Colspan = 2;
                cell11.PaddingBottom = 5f;
                pdfPtable2.AddCell(cell11);
                FontFactory.GetFont("Microsoft Sans Serif", 20f, 5, new BaseColor((int)byte.MaxValue, 0, 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Weighbridge Clerk:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase(string.Empty, FontFactory.GetFont("Microsoft Sans Serif", 2f, 0, BaseColor.BLACK)), 0));
                PdfPCell cell12 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell12.Colspan = 2;
                cell12.PaddingBottom = 8f;
                pdfPtable2.AddCell(cell12);
                pdfPtable2.AddCell(PrintTicket.PhraseCell(new Phrase("Driver Signature:", FontFactory.GetFont("Microsoft Sans Serif", 2f, 1, BaseColor.BLACK)), 0));
                PdfPCell cell13 = PrintTicket.PhraseCell(new Phrase(), 1);
                cell13.Colspan = 2;
                cell13.PaddingBottom = 3f;
                pdfPtable2.AddCell(cell13);
                document.Add((IElement)pdfPtable2);
                document.Close();
                byte[] array = memoryStream.ToArray();
                using (FileStream fileStream = File.Create("C:\\Test.pdf"))
                {
                    fileStream.Write(array, 0, array.Length);
                    fileStream.Close();
                }
                memoryStream.Close();
            }
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte directContent = writer.DirectContent;
            directContent.SetColorStroke(color);
            directContent.MoveTo(x1, y1);
            directContent.LineTo(x2, y2);
            directContent.Stroke();
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell pdfPcell = new PdfPCell(phrase);
            BaseColor white = BaseColor.WHITE;
            pdfPcell.BorderColor = white;
            int num1 = 4;
            pdfPcell.VerticalAlignment = num1;
            int num2 = align;
            pdfPcell.HorizontalAlignment = num2;
            double num3 = 1.0;
            pdfPcell.PaddingBottom = (float)num3;
            double num4 = 0.0;
            pdfPcell.PaddingTop = (float)num4;
            return pdfPcell;
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            Image instance = Image.GetInstance("");
            instance.ScalePercent(scale);
            PdfPCell pdfPcell = new PdfPCell(instance);
            BaseColor white = BaseColor.WHITE;
            pdfPcell.BorderColor = white;
            int num1 = 4;
            pdfPcell.VerticalAlignment = num1;
            int num2 = align;
            pdfPcell.HorizontalAlignment = num2;
            double num3 = 0.0;
            pdfPcell.PaddingBottom = (float)num3;
            double num4 = 0.0;
            pdfPcell.PaddingTop = (float)num4;
            return pdfPcell;
        }
    }
}
