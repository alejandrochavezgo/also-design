namespace app.helpers;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using entities.models;
using Newtonsoft.Json;
using common.configurations;

public static class pdfHelper
{
    public static byte[] generatePurchaseOrderPdf(purchaseOrderModel purchaseOrder, List<enterpriseModel> enterprises, userModel user)
    {
        try
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
                var subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                var regularFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images");
                var logoPath = Path.Combine(folderPath, $"{configurationManager.appSettings["configurations:defaults:alsoLogo"]}");
                var logo = Image.GetInstance($"{logoPath}");
                logo.ScaleToFit(100f, 100f);
                logo.Alignment = Image.ALIGN_LEFT;

                var headerTable = new PdfPTable(4);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 1, 1, 4, 1 });

                var logoCell = new PdfPCell(logo);
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.PaddingTop = -40f;
                logoCell.PaddingLeft = -20f;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable.AddCell(logoCell);

                var imageText = new PdfPCell();
                imageText.Border = Rectangle.NO_BORDER;
                imageText.PaddingLeft = -15f;
                imageText.HorizontalAlignment = Element.ALIGN_LEFT;
                imageText.Phrase = new Phrase("ALSO DESIGN", regularFontBold);
                headerTable.AddCell(imageText);

                var headerText = new PdfPCell();
                headerText.Border = Rectangle.NO_BORDER;
                headerText.PaddingTop = -10f;
                headerText.PaddingLeft = -15f;
                headerText.HorizontalAlignment = Element.ALIGN_CENTER;
                headerText.Phrase = new Phrase("ALSO DESIGN\nDISEÑO ELECTROMECÁNICO INDUSTRIAL\n*RESPONSABILIDAD, CALIDAD E INNOVACIÓN TECNOLÓGICA*", regularFontBold);
                headerTable.AddCell(headerText);

                var codeCell = new PdfPCell(new Phrase($"{purchaseOrder.code}", regularFontBold));
                codeCell.PaddingTop = 0f;
                codeCell.Border = Rectangle.NO_BORDER;
                codeCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headerTable.AddCell(codeCell);
                document.Add(headerTable);
                document.Add(new Paragraph("\n"));

                var infoTable = new PdfPTable(4);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 1, 2, 2, 3 });
                var titleBackgroundColor = new BaseColor(52, 152, 219);

                infoTable.AddCell(new PdfPCell(new Phrase("ID", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("SUPPLIER", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("MAIN CONTACT", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("ADDRESS", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });

                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.id}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.supplier!.businessName}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.supplier.mainContactName}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.supplier.address}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });

                infoTable.AddCell(new PdfPCell(new Phrase("PHONE", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("RFC", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("PAYMENT TYPE", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase("CURRENCY", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER });

                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.supplier.mainContactPhone}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.supplier.rfc}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.payment!.description}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                infoTable.AddCell(new PdfPCell(new Phrase($"{purchaseOrder.currency!.description}", regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER });
                document.Add(infoTable);
                document.Add(new Paragraph("\n"));

                var itemsTable = new PdfPTable(6);
                itemsTable.WidthPercentage = 100;
                itemsTable.SetWidths(new float[] { 0.5f, 3, 1, 1, 1, 1 });
                itemsTable.AddCell(new PdfPCell(new Phrase("#", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                itemsTable.AddCell(new PdfPCell(new Phrase("DESCRIPTION", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                itemsTable.AddCell(new PdfPCell(new Phrase("QUANTITY", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                itemsTable.AddCell(new PdfPCell(new Phrase("UNIT", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                itemsTable.AddCell(new PdfPCell(new Phrase("UNIT VALUE", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_MIDDLE });
                itemsTable.AddCell(new PdfPCell(new Phrase("TOTAL", regularFontBold)) { Border = Rectangle.BOX, BackgroundColor = titleBackgroundColor, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                var itemNumber = 1;
                foreach (var item in purchaseOrder!.items!)
                {
                    itemsTable.AddCell(new PdfPCell(new Phrase(itemNumber.ToString(), regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    var descriptionCell = new PdfPCell();
                    descriptionCell.Border = Rectangle.BOX;
                    descriptionCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    descriptionCell.AddElement(new Phrase(item.description, regularFont));
                    descriptionCell.AddElement(new Phrase(item.material, regularFont));
                    descriptionCell.AddElement(new Phrase(item.details, regularFont));
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "IMAGEN.jpeg"); // Reemplaza con la ruta correcta
                    if (File.Exists(imagePath))
                    {
                        var image = Image.GetInstance(imagePath);
                        image.ScaleToFit(80f, 80f);
                        image.Alignment = Element.ALIGN_LEFT;
                        descriptionCell.AddElement(image);
                    }

                    descriptionCell.AddElement(new Phrase(item.notes, regularFont));
                    itemsTable.AddCell(descriptionCell);
                    itemsTable.AddCell(new PdfPCell(new Phrase(item.quantity.ToString(), regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    itemsTable.AddCell(new PdfPCell(new Phrase(item.unitDescription, regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    itemsTable.AddCell(new PdfPCell(new Phrase(item.unitValue.ToString("C2"), regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    itemsTable.AddCell(new PdfPCell(new Phrase(item.totalValue.ToString("C2"), regularFont)) { Border = Rectangle.BOX, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    itemNumber++;
                }
                document.Add(itemsTable);

                var amountsTable = new PdfPTable(3);
                amountsTable.SetWidths(new float[] { 60f, 30f, 10f });
                amountsTable.WidthPercentage = 100;

                var cell1 = new PdfPCell(new Phrase($"{purchaseOrder.generalNotes}", regularFontBold));
                cell1.Border = Rectangle.BOX;
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                amountsTable.AddCell(cell1);

                var cell2 = new PdfPCell(new Phrase($"SUBTOTAL\nIVA {purchaseOrder.taxRate}%\nTOTAL AMOUNT", regularFont));
                cell2.Border = Rectangle.BOX;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                amountsTable.AddCell(cell2);

                var cell3 = new PdfPCell(new Phrase($"{purchaseOrder.subtotal}\n{purchaseOrder.taxAmount}\n{purchaseOrder.totalAmount}", regularFontBold));
                cell3.Border = Rectangle.BOX;
                cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                amountsTable.AddCell(cell3);
                document.Add(amountsTable);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("ATENTAMENTE", regularFont));
                document.Add(new Paragraph($"{user.employee!.profession}. {user.firstname} {user.lastname}", regularFont));
                document.Add(new Paragraph($"{purchaseOrder!.user!.employee!.mainContactPhone}", regularFont));
                document.Add(new Paragraph($"{user.employee!.jobPosition}", regularFont));
                document.Add(new Paragraph("\n"));

                var footerTable = new PdfPTable(1);
                footerTable.SetWidths(new float[] { 100f });
                footerTable.WidthPercentage = 100;
                var backgroundColor = new BaseColor(52, 152, 219);
                var footerCell1 = new PdfPCell(new Phrase("TIJUANA, BAJA CALIFORNIA, MÉXICO", regularFontBold));
                footerCell1.BackgroundColor = backgroundColor;
                footerCell1.Border = Rectangle.NO_BORDER;
                footerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                footerCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                footerTable.AddCell(footerCell1);
                document.Add(footerTable);
                document.Close();

                return ms.ToArray();
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }
}