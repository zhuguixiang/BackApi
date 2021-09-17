using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Kernal
{
    public class ExcelHelper
    {
        public static void Export(ExcelExportArgs args)
        {
            if (args.ColumnDefine == null || args.ColumnDefine.Count == 0)
                throw new ArgumentException("没有指定 ColumnDefine");

            if (String.IsNullOrEmpty(args.FilePath))
                throw new ArgumentException("没有指定 FilePath");


            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            int nextRowIndex = 0;
            int nextCellIndex = 0;

            //创建表头
            IRow row = sheet.CreateRow(nextRowIndex);
            nextRowIndex++;

            if (args.RowNumber)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue("序号");
                nextCellIndex++;
            }

            foreach (var item in args.ColumnDefine)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue(item.Title);

                nextCellIndex++;
            }

            //写入内容
            if (args.DataList != null)
            {
                int rowNumber = 0;
                foreach (var data in args.DataList)
                {
                    if (data == null)
                        continue;

                    rowNumber++;

                    nextCellIndex = 0;

                    row = sheet.CreateRow(nextRowIndex);

                    if (args.RowNumber)
                    {
                        ICell cell = row.CreateCell(nextCellIndex);
                        cell.SetCellValue(rowNumber.ToString());
                        nextCellIndex++;
                    }

                    foreach (var column in args.ColumnDefine)
                    {
                        object value = ReflectionHelper.GetPropertyValue(data, column.DataProperty);
                        if (value != null)
                        {
                            Type valueType = value.GetType();
                            if (valueType.IsEnum)
                            {
                                value = EnumHelper.GetDescription((Enum)value);
                                //DescriptionAttribute descriptionAttribute = valueType.GetCustomAttribute<DescriptionAttribute>();
                                //if (descriptionAttribute != null)
                                //{
                                //    value = descriptionAttribute.Description;
                                //}
                            }
                            else if (valueType == typeof(bool))
                            {
                                bool boolValue = bool.Parse(value.ToString());
                                if (boolValue)
                                    value = "是";
                                else
                                    value = "否";
                            }

                            ICell cell = row.CreateCell(nextCellIndex);
                            if (column.SetValueToCellFormatFunc == null)
                            {
                                cell.SetCellValue(value.ToString());
                            }
                            else
                            {
                                cell.SetCellValue(column.SetValueToCellFormatFunc(value));
                            }

                        }
                        nextCellIndex++;
                    }

                    nextRowIndex++;
                }
            }

            FileStream fsExportFile = new FileStream(args.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            workbook.Write(fsExportFile);

        }

        public static void ExportByDictionary(ExcelExportByDictionaryArgs args)
        {
            if (args.ColumnDefine == null || args.ColumnDefine.Count == 0)
                throw new ArgumentException("没有指定 ColumnDefine");

            if (String.IsNullOrEmpty(args.FilePath))
                throw new ArgumentException("没有指定 FilePath");


            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            int nextRowIndex = 0;
            int nextCellIndex = 0;

            //创建表头
            IRow row = sheet.CreateRow(nextRowIndex);
            nextRowIndex++;

            if (args.RowNumber)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue("序号");
                nextCellIndex++;
            }

            foreach (var item in args.ColumnDefine)
            {
                ICell cell = row.CreateCell(nextCellIndex);
                cell.SetCellValue(item.Title);

                nextCellIndex++;
            }

            //写入内容
            if (args.DataList != null)
            {
                int rowNumber = 0;
                foreach (var data in args.DataList)
                {
                    if (data == null)
                        continue;

                    rowNumber++;

                    nextCellIndex = 0;

                    row = sheet.CreateRow(nextRowIndex);

                    if (args.RowNumber)
                    {
                        ICell cell = row.CreateCell(nextCellIndex);
                        cell.SetCellValue(rowNumber.ToString());
                        nextCellIndex++;
                    }

                    foreach (var column in args.ColumnDefine)
                    {
                        if (data.ContainsKey(column.DataProperty))
                        {
                            object value = data[column.DataProperty];
                            if (value != null)
                            {
                                ICell cell = row.CreateCell(nextCellIndex);
                                if (column.SetValueToCellFormatFunc == null)
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                                else
                                {
                                    cell.SetCellValue(column.SetValueToCellFormatFunc(value));
                                }
                            }
                        }

                        nextCellIndex++;
                    }

                    nextRowIndex++;
                }
            }

            FileStream fsExportFile = new FileStream(args.FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            workbook.Write(fsExportFile);

        }

        public static List<ExcelExportColumnDefine> GetColumnDefineListByType(Type type)
        {
            List<ExcelExportColumnDefine> list = new List<ExcelExportColumnDefine>();

            PropertyInfo[] propertyInfoList = type.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.CanRead == false)
                    continue;

                List<ExcelExportFieldAttribute> attributeList = propertyInfo.GetCustomAttributes<ExcelExportFieldAttribute>().ToList();
                if (attributeList.Count == 0)
                    continue;

                foreach (ExcelExportFieldAttribute attribute in attributeList)
                {
                    ExcelExportColumnDefine define = new ExcelExportColumnDefine();
                    define.Title = attribute.Title;

                    if (String.IsNullOrEmpty(attribute.SubProperty))
                    {
                        define.DataProperty = propertyInfo.Name;
                    }
                    else
                    {
                        define.DataProperty = propertyInfo.Name + "." + attribute.SubProperty;
                    }

                    list.Add(define);
                }

            }

            return list;

        }

        public static List<T> Import<T>(ExcelImportArgs args) where T : new()
        {
            IWorkbook workbook = new XSSFWorkbook(args.FilePath);
            ISheet sheet = workbook.GetSheetAt(0);

            List<T> list = new List<T>();

            if (sheet.PhysicalNumberOfRows <= 1)
                return list;

            //获取列定义
            List<ExcelImportFieldAttribute> excelImportFieldAttributeList = GetExcelImportFieldAttributeList(typeof(T));

            //获取表头
            List<string> sheetTitleList = new List<string>();
            IRow titleRow = sheet.GetRow(0);

            foreach (ICell titleCell in titleRow.Cells)
            {
                if (titleCell.CellType == CellType.String)
                {
                    sheetTitleList.Add(titleCell.StringCellValue);
                }
                else if (titleCell.CellType == CellType.Numeric)
                {
                    sheetTitleList.Add(titleCell.NumericCellValue.ToString());
                }
            }

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                T obj = new T();

                for (int j = 0; j < sheetTitleList.Count; j++)
                {
                    //如果有连续的空白单元格，直接 row.Cells 获取的数据会丢失这些空白的单元格
                    ICell cell = row.FirstOrDefault(n => n.ColumnIndex == j);
                    if (cell == null)
                    {
                        cell = row.CreateCell(j);
                    }

                    string title = sheetTitleList[j];

                    ExcelImportFieldAttribute excelImportFieldAttribute = excelImportFieldAttributeList.SingleOrDefault(c => c.Title == title);
                    if (excelImportFieldAttribute == null)
                        continue;

                    // cell.SetCellType(CellType.String);
                    if (excelImportFieldAttribute.PropertyInfo.PropertyType == typeof(DateTime) || excelImportFieldAttribute.PropertyInfo.PropertyType == typeof(DateTime?))
                    {
                        if (cell.ToString() != String.Empty)
                            ReflectionHelper.SetPropertyValue(obj, excelImportFieldAttribute.PropertyInfo.Name, cell.DateCellValue);
                    }
                    else
                    {
                        ReflectionHelper.SetPropertyValue(obj, excelImportFieldAttribute.PropertyInfo.Name, cell.ToString());
                    }
                }

                list.Add(obj);

            }

            return list;
        }

        private static List<ExcelImportFieldAttribute> GetExcelImportFieldAttributeList(Type type)
        {
            List<ExcelImportFieldAttribute> list = new List<ExcelImportFieldAttribute>();

            PropertyInfo[] propertyInfoList = type.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.CanRead == false)
                    continue;

                ExcelImportFieldAttribute attribute = propertyInfo.GetCustomAttribute<ExcelImportFieldAttribute>();
                if (attribute == null)
                    continue;

                attribute.PropertyInfo = propertyInfo;

                list.Add(attribute);
            }

            return list;
        }

        public static void AddColumnMergedRegion(ISheet sheet, int startRowIndex, int endRowIndex, int columnIndex)
        {
            if (endRowIndex <= startRowIndex)
                return;

            string lastCellValue = null;
            int thisTimeStartRowIndex = startRowIndex;

            IRow row = sheet.GetRow(startRowIndex);
            if (row == null)
                return;

            ICell cell = row.FirstOrDefault(n => n.ColumnIndex == columnIndex);
            lastCellValue = cell.StringCellValue;

            for (int i = startRowIndex + 1; i <= endRowIndex; i++)
            {
                row = sheet.GetRow(i);
                cell = row.FirstOrDefault(n => n.ColumnIndex == columnIndex);
                if (cell == null)
                {
                    return;
                }

                string cellValue = cell.StringCellValue;

                //合并上面的单元格
                if (lastCellValue != cellValue)
                {
                    if(thisTimeStartRowIndex == i - 1)
                    {
                        thisTimeStartRowIndex = i;
                        lastCellValue = cellValue;

                        continue;
                    }

                    CellRangeAddress cellRangeAddress = new CellRangeAddress(thisTimeStartRowIndex, i - 1, columnIndex, columnIndex);
                    sheet.AddMergedRegion(cellRangeAddress);

                    thisTimeStartRowIndex = i;
                    lastCellValue = cellValue;
                }
                else
                {
                    //如果是最后一行，则要把上面可能一样的若干行合并
                    if (i == endRowIndex && thisTimeStartRowIndex != i)
                    {
                        CellRangeAddress cellRangeAddress = new CellRangeAddress(thisTimeStartRowIndex, i, columnIndex, columnIndex);
                        sheet.AddMergedRegion(cellRangeAddress);
                    }
                }
            }
        }
    }
}
