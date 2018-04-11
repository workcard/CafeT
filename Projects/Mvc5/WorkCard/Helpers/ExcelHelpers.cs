using Excel;
//using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SmartTracking.Helpers
{
    public class ExcelHelpers
    {
        string _path;

        public ExcelHelpers(string path)
        {
            _path = path;
        }

        public IExcelDataReader GetExcelReader()
        {
            if (File.Exists(_path))
            {
                FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);

                IExcelDataReader reader = null;
                try
                {
                    if (_path.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    if (_path.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    return reader;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetWorksheetNames()
        {
            try
            {
                var reader = this.GetExcelReader();
                var workbook = reader.AsDataSet();
                var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
                return sheets;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<DataRow> GetData(string sheet, bool firstRowIsColumnNames = true)
        {
            try
            {
                var reader = this.GetExcelReader();
                reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
                var workSheet = reader.AsDataSet().Tables[sheet];
                var rows = from DataRow a in workSheet.Rows select a;
                return rows;
            }
            catch
            {
                return null;
            }
        }
    }
}