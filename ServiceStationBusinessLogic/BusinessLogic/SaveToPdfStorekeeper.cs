using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using ServiceStationBusinessLogic.HelperModels;
using System.Collections.Generic;

namespace ServiceStationBusinessLogic.BusinessLogic
{
    static class SaveToPdfStorekeeper
    {
        public static void CreateDoc(PdfInfoStorekeeper info)
        {
            Document document = new Document();
            DefineStyles(document);
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph(info.Title);
            paragraph.Format.SpaceAfter = "1cm";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Style = "NormalTitle";
            paragraph = section.AddParagraph($"с {info.DateFrom.ToShortDateString()} по { info.DateTo.ToShortDateString()}");
            paragraph.Format.SpaceAfter = "1cm";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Style = "Normal";
            var tableSparePartWork = document.LastSection.AddTable();
            List<string> tableSparepartWorkHeader = new List<string> { "3cm", "3cm", "3cm", "4cm", "4cm" };
            foreach (var header in tableSparepartWorkHeader)
            {
                tableSparePartWork.AddColumn(header);
            }
            CreateRow(new PdfRowParameters
            {
                Table = tableSparePartWork,
                Texts = new List<string> { "Запчасть", "Количество", "Дата применения", "Работа", "Машина" },
                Style = "NormalTitle",
                ParagraphAlignment = ParagraphAlignment.Center
            });
            foreach (var sparePartWorkCar in info.ReportInfoes.SparePartWorkCar)
            {
                CreateRow(new PdfRowParameters
                {
                    Table = tableSparePartWork,
                    Texts = new List<string> {
                                sparePartWorkCar.SparePart,
                                sparePartWorkCar.Count.ToString(),
                                sparePartWorkCar.DatePassed.ToShortDateString(),
                                sparePartWorkCar.WorkName,
                                sparePartWorkCar.CarName,
                            },
                    Style = "Normal",
                    ParagraphAlignment = ParagraphAlignment.Left
                });
            }
            Paragraph totalInfoParagraph = document.LastSection.AddParagraph("Итог");
            totalInfoParagraph.Format.SpaceAfter = "1cm";
            totalInfoParagraph.Format.SpaceBefore = "1cm";
            totalInfoParagraph.Format.Alignment = ParagraphAlignment.Center;
            totalInfoParagraph.Style = "NormalTitle";

            var tableTotalInfo = document.LastSection.AddTable();
            List<string> tableTotalInfoHeader = new List<string> { "3cm", "3cm" };
            foreach (var header in tableTotalInfoHeader)
            {
                tableTotalInfo.AddColumn(header);
            }
            CreateRow(new PdfRowParameters
            {
                Table = tableTotalInfo,
                Texts = new List<string> { "Запчасть", "Общее количество" },
                Style = "NormalTitle",
                ParagraphAlignment = ParagraphAlignment.Center
            });
            foreach (var sparePart in info.ReportInfoes.TotalCount)
            {
                CreateRow(new PdfRowParameters
                {
                    Table = tableTotalInfo,
                    Texts = new List<string> {
                                sparePart.Item1,
                                sparePart.Item2.ToString()
                            },
                    Style = "Normal",
                    ParagraphAlignment = ParagraphAlignment.Left
                });
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true,
                PdfSharp.Pdf.PdfFontEmbedding.Always)
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(info.FileName);
        }

        /// Создание стилей для документа
        private static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 10;
            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }
        /// Создание и заполнение строки
        private static void CreateRow(PdfRowParameters rowParameters)
        {
            Row row = rowParameters.Table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                FillCell(new PdfCellParameters
                {
                    Cell = row.Cells[i],
                    Text = rowParameters.Texts[i],
                    Style = rowParameters.Style,
                    BorderWidth = 0.5,
                    ParagraphAlignment = rowParameters.ParagraphAlignment
                });
            }
        }

        /// Заполнение ячейки
        private static void FillCell(PdfCellParameters cellParameters)
        {
            cellParameters.Cell.AddParagraph(cellParameters.Text);
            if (!string.IsNullOrEmpty(cellParameters.Style))
            {
                cellParameters.Cell.Style = cellParameters.Style;
            }
            cellParameters.Cell.Borders.Left.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Right.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Top.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Bottom.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Format.Alignment = cellParameters.ParagraphAlignment;
            cellParameters.Cell.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
