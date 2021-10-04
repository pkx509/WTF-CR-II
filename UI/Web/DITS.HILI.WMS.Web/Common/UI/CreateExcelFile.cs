using DITS.HILI.WMS.DailyPlanModel;
using DITS.HILI.WMS.DispatchModel.CustomModel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DITS.WMS.Web.Common
{
    public class ExcelFile
    {
        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }
        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        public void DailyPlanExcelFile(List<ProductionPlanCustomModel> model, List<ValidationImportFileResult> error, string fileName)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                        new Column // Id column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 4,
                            CustomWidth = true
                        },
                        new Column // Name and Birthday columns
                        {
                            Min = 2,
                            Max = 3,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 8,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "DailyPlan" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("line", CellValues.String, 2),
                    ConstructCell("Order seq", CellValues.String, 2),
                    ConstructCell("Product_code", CellValues.String, 2),
                    ConstructCell("Product_name", CellValues.String, 2),
                    ConstructCell("Qty", CellValues.String, 2),
                    ConstructCell("unit", CellValues.String, 2),
                    ConstructCell("weight", CellValues.String, 2),
                    ConstructCell("order no", CellValues.String, 2),
                    ConstructCell("order type", CellValues.String, 2),
                    ConstructCell("Film", CellValues.String, 2),
                    ConstructCell("Box", CellValues.String, 2),
                    ConstructCell("Powder", CellValues.String, 2),
                    ConstructCell("Oil", CellValues.String, 2),
                    ConstructCell("FD ", CellValues.String, 2),
                    ConstructCell("Stamp ", CellValues.String, 2),
                    ConstructCell("Sticker", CellValues.String, 2),
                    ConstructCell("Mark", CellValues.String, 2),
                    ConstructCell("delivery date", CellValues.String, 2),
                    ConstructCell("customer code", CellValues.String, 2),
                    ConstructCell("customer name", CellValues.String, 2),
                    ConstructCell("Working Time", CellValues.String, 2),
                    ConstructCell("frying oil ", CellValues.String, 2),
                    ConstructCell("ingredients", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                int index = 0;
                int i = typeof(ProductionPlanCustomModel).GetProperties().Count() + 1;
                uint[] styleIndex = new uint[i];

                foreach (ProductionPlanCustomModel item in model)
                {
                    row = new Row();

                    List<ValidationImportFileResult> x = error.Where(s => s.RowIndex == index).ToList();

                    row.Append(
                             ConstructCell(item.LineCode, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "LineCode") ? 3 : 1)).ToString())),
                             ConstructCell(item.Seq.Value.ToString(), CellValues.Number,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Seq") ? 3 : 1)).ToString())),
                             ConstructCell(item.ProductCode, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "ProductCode") ? 3 : 1)).ToString())),
                             ConstructCell(item.ProductName, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "ProductName") ? 3 : 1)).ToString())),
                             ConstructCell(item.ProductionQty.ToString(), CellValues.Number,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "ProductionQty") ? 3 : 1)).ToString())),
                             ConstructCell(item.ProductUnitName, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "ProductUnitName") ? 3 : 1)).ToString())),
                             ConstructCell(item.Weight_G.Value.ToString(), CellValues.Number,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Weight_G") ? 3 : 1)).ToString())),
                             ConstructCell(item.OrderNo, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "OrderNo") ? 3 : 1)).ToString())),
                             ConstructCell(item.OrderType, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "OrderType") ? 3 : 1)).ToString())),
                             ConstructCell(item.Film, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Film") ? 3 : 1)).ToString())),
                             ConstructCell(item.Box, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Box") ? 3 : 1)).ToString())),
                             ConstructCell(item.Powder, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Powder") ? 3 : 1)).ToString())),
                             ConstructCell(item.Oil, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Oil") ? 3 : 1)).ToString())),
                             ConstructCell(item.FD, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "FD") ? 3 : 1)).ToString())),
                             ConstructCell(item.Stamp, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Stamp") ? 3 : 1)).ToString())),
                             ConstructCell(item.Sticker, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Sticker") ? 3 : 1)).ToString())),

                             ConstructCell(item.Mark, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Mark") ? 3 : 1)).ToString())),


                             ConstructCell((item.DeliveryDate == null ? "" : item.DeliveryDate.Value.ToString("dd/MM/yyyy")),
                                               item.DeliveryDate == null ? CellValues.String : CellValues.Date,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "DeliveryDate") ? 3 : 1)).ToString())),
                             //ConstructCell(item.CustomerCode, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Field == "CustomerCode" ? 3 : 1)).ToString())),
                             //ConstructCell(item.CustomerName, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Field == "CustomerName" ? 3 : 1)).ToString())),
                             ConstructCell(item.WorkingTime, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "WorkingTime") ? 3 : 1)).ToString())),
                             ConstructCell(item.OilType, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "OilType") ? 3 : 1)).ToString())),
                             ConstructCell(item.Formula, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Any(z => z.Field == "Formula") ? 3 : 1)).ToString())));

                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
            }
        }

        public void PredispatchExcelFile(List<PreDispatchesImportModel> model, List<ObjectPropertyValidatorException> error, string fileName)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                        new Column // Id column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 4,
                            CustomWidth = true
                        },
                        new Column // Name and Birthday columns
                        {
                            Min = 2,
                            Max = 3,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column // Salary column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 8,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "DailyPlan" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();


                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Customer", CellValues.String, 2),
                    ConstructCell("Dispatch Type", CellValues.String, 2),
                    ConstructCell("Es Dispatch Date", CellValues.String, 2),
                    ConstructCell("Shipping Dock", CellValues.String, 2),
                    ConstructCell("Shipping Route", CellValues.String, 2),
                    ConstructCell("PO Number", CellValues.String, 2),
                    ConstructCell("Reference", CellValues.String, 2),
                    ConstructCell("Back Order(Y/N)", CellValues.String, 2),
                    ConstructCell("Remark", CellValues.String, 2),
                    ConstructCell("Urgent(Y/N)", CellValues.String, 2),
                    ConstructCell("Product Code", CellValues.String, 2),
                    ConstructCell("Lot", CellValues.String, 2),
                    ConstructCell("Quantity", CellValues.String, 2),
                    ConstructCell("Unit ", CellValues.String, 2),
                    ConstructCell("Rematk ", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                int index = 0;
                int i = typeof(PreDispatchesImportModel).GetProperties().Count() + 1;
                uint[] styleIndex = new uint[i];

                foreach (PreDispatchesImportModel item in model)
                {
                    row = new Row();
                    ObjectPropertyValidatorException x = error.FirstOrDefault(s => s.RowIndex == index);

                    row.Append(
                             ConstructCell(item.CustomerCode, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "CustomerCode" ? 2 : 1)).ToString())),

                             ConstructCell(item.DispatchType, CellValues.Number,
                                            uint.Parse((x == null ? 1 : (x.Property == "DispatchType" ? 2 : 1)).ToString())),

                             ConstructCell((item.EstDispatchDate.Value.ToString("yyyyMMdd") == "00010101" ? "" : item.EstDispatchDate.Value.ToString("dd/MM/yyyy")),
                                               item.EstDispatchDate.Value.ToString("yyyyMMdd") == "00010101" ? CellValues.String : CellValues.Date,
                                            uint.Parse((x == null ? 1 : (x.Property == "EstDispatchDate" ? 2 : 1)).ToString())),

                             //ConstructCell(item.Dock, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Property == "Dock" ? 2 : 1)).ToString())),

                             //ConstructCell(item.Route, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Property == "Route" ? 2 : 1)).ToString())),

                             ConstructCell(item.PONumber, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "PONumber" ? 2 : 1)).ToString())),

                             //ConstructCell(item.DispatchRefered, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Property == "DispatchRefered" ? 2 : 1)).ToString())),

                             ConstructCell(item.IsBackOrder, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "IsBackOrder" ? 2 : 1)).ToString())),

                             ConstructCell(item.Remark, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "Remark" ? 2 : 1)).ToString())),

                             ConstructCell(item.IsUrgent, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "IsUrgent" ? 2 : 1)).ToString())),

                             ConstructCell(item.ProductCode, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "ProductCode" ? 2 : 1)).ToString())),

                             //ConstructCell(item.Lot, CellValues.String,
                             //               uint.Parse((x == null ? 1 : (x.Property == "Lot" ? 2 : 1)).ToString())),

                             ConstructCell(item.Quantity.Value.ToString(), CellValues.Number,
                                            uint.Parse((x == null ? 1 : (x.Property == "Quantity" ? 2 : 1)).ToString())),

                             ConstructCell(item.UOM, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "UOM" ? 2 : 1)).ToString())),

                             ConstructCell(item.Remark2, CellValues.String,
                                            uint.Parse((x == null ? 1 : (x.Property == "Remark2" ? 2 : 1)).ToString())));


                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();
            }
        }

        #region [ Local ]
        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ),
                 new Font( // Index 2 - error
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FF0000" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default  
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header 
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true },// header
                    new CellFormat { FontId = 2, FillId = 0, BorderId = 1, ApplyBorder = true } // body
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
        #endregion
    }
}
