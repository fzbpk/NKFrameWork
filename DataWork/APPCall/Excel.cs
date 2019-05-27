using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
namespace NK.DataWork
{
    /// <summary>
    /// Excel处理类
    /// </summary>
    public class Excel
    {
        /// <summary>
        /// 文件位置
        /// </summary>
         public string FilePath { get; set; }

         public string LastError { get; private set; }

        /// <summary>
        /// 转DataTable
        /// </summary>
        /// <param name="isColumnName">第一行是否表头</param>
        /// <returns>DataTable</returns>
        public DataTable ToDataTable( bool isColumnName=true)
        {
            LastError = "";
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(FilePath))
                {
                    if (FilePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    else if (FilePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);
                        dataTable = new DataTable();
                        dataTable.TableName = sheet.SheetName;
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);
                                int cellCount = firstRow.LastCellNum;
                                if (isColumnName)
                                {
                                    startRow = 1;
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                if (fs != null)
                    fs.Close();
                return null;
            }
        }

        /// <summary>
        /// 转Excel
        /// </summary>
        /// <param name="dt">数据</param> 
        /// <returns>Excel</returns>
        public  bool DataTableToExcel(DataTable dt)
        {
            LastError = "";
            bool result = false;
            IWorkbook workbook = null;
            FileStream fs = null;
            IRow row = null;
            ISheet sheet = null;
            ICell cell = null;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet(dt.TableName);
                    int rowCount = dt.Rows.Count;
                    int columnCount = dt.Columns.Count;
                    row = sheet.CreateRow(0);
                    for (int c = 0; c < columnCount; c++)
                    {
                        cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].ColumnName);
                    }
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    using (fs = File.OpenWrite(FilePath))
                    {
                        workbook.Write(fs);
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                if (fs != null)
                    fs.Close();
                return false;
            }
        }


    }
}
