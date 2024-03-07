using Electric.Class.DAO;
using Electric.Class.Screen;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static OfficeOpenXml.ExcelErrorValue;

namespace Electric.Class.Factory
{
    public class TableControl
    {

        public static void Init()
        {
            RoadTFormTable(DAOFactor.commodities, new EleCommodity(), ElectSet.Default.默认商品表格);
            RoadTFormTable(DAOFactor.books, new EleBooks(), ElectSet.Default.默认财务表格);
            RoadTFormTable(DAOFactor.orders, new EleOrder(), ElectSet.Default.默认财务表格);
        }

        public static void RoadTFormTable<T>(HashSet<T> ts, T obj, string Path)
        {
            try
            {
                var file = new FileInfo(Path);
                using (var pack = new ExcelPackage(file))
                {
                    var workshop = pack.Workbook.Worksheets[0];
                    var data = workshop.Cells["A2:H" + workshop.Dimension.End.Row];
                    var EleDAO = obj.GetType();
                    for (int i = 0; i < workshop.Dimension.End.Row; i++)
                    {
                        var DAOobj = Activator.CreateInstance(EleDAO);
                        var objTy = DAOobj.GetType();
                        var objPe = objTy.GetProperties();
                        for (int num = 0; num < objPe.Length; num++)
                        {
                            var value = data[i, num + 1].Value;
                            if (objPe[num].GetValue(obj).GetType() == value.GetType())
                            {
                                objPe[num].SetValue(DAOobj, value);
                            }
                            else
                            {
                                MessageBox.Show($"读取数据时出现类型错误，错误出现在{workshop.Name}中第{i}行第{num}列", "提示");
                            }
                        }
                        ts.Add((T)DAOobj);
                    }
                }
            }
            catch
            {
                MessageBox.Show($"\"表格错误，请查找+{Path}+是否存在该表格或表格是否正确\"", "错误",MessageBoxButton.OK);
            }
        }

        private static void CreateTable<T>(HashSet<T> ts, string Path)
        {
            try
            {
                var file = new FileInfo(Path);
                using (var pack = new ExcelPackage(file))
                {
                    var workshop = pack.Workbook.Worksheets.Add("Sheet1"); // 创建一个新的工作表
                    int rowNum = 1;

                    // 写入标题行
                    var objTy = typeof(T);
                    var objPe = objTy.GetProperties();
                    for (int num = 0; num < objPe.Length; num++)
                    {
                        workshop.Cells[1, num + 1].Value = objPe[num].Name;
                    }

                    // 遍历集合中的对象，写入数据行
                    foreach (var obj in ts)
                    {
                        rowNum++;
                        for (int num = 0; num < objPe.Length; num++)
                        {
                            workshop.Cells[rowNum, num + 1].Value = objPe[num].GetValue(obj);
                        }
                    }

                    pack.Save(); // 保存工作簿
                }
            }
            catch
            {
                MessageBox.Show($"表格错误，请检查路径 {Path} 是否存在该表格或表格是否正确", "错误", MessageBoxButton.OK);
            }
        }

        public static void CreateExcel(string path)
        {

        }

        public static void ExcelActControl(EleDAO obj,string Act) 
        {
            var tableName = "";
            if (obj is EleCommodity)
            {
                tableName = ElectSet.Default.默认商品表格;
            }
            else if (obj is EleOrder)
            {
                tableName = ElectSet.Default.默认财务表格;
            }
            else if (obj is EleBooks)
            {
                tableName = ElectSet.Default.默认财务表格;
            }

            ExcelAct(obj, tableName,Act);
        }

        private static void ExcelAct(EleDAO eleDAO, string path, string Act)
        {
            var values = eleDAO.GetType().GetProperties();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                if (Act == "添加")
                {
                    ExcelAdd(values, package,eleDAO);
                }
                if (Act == "删除")
                {
                    ExcelDelete(values, package, eleDAO.id);
                }
                if (Act == "修改")
                {
                    var r = ExcelDelete(values, package, eleDAO.id);
                    ExcelUpdate(values,package,r,eleDAO);
                }
                package.Save();
            }
        }

        private static void ExcelAdd(PropertyInfo[] values, ExcelPackage package,EleDAO eleDAO)
        {
            var worksheet = package.Workbook.Worksheets[1];
            int lastRow = worksheet.Dimension.End.Row + 1;
            for (int i = 0; i < values.Length; i++)
            {
                worksheet.Cells[lastRow, i + 1].Value = values[i].GetValue(eleDAO);
            }

        }

        private static int ExcelDelete(PropertyInfo[] values, ExcelPackage package, int eid)
        {
            int r = 0;
            var worksheet = package.Workbook.Worksheets[1];
            int rowCount = worksheet.Dimension.End.Row;
            for (int row = rowCount; row >= 1; row--)
            {
                var cellValue = int.Parse(worksheet.Cells[row, 1].Value.ToString());
                if (cellValue == eid)
                {
                    worksheet.DeleteRow(row);
                    r = row;
                    break;
                }
            }
            return r;
        }

        private static void ExcelUpdate(PropertyInfo[] values, ExcelPackage package,int insertRow,EleDAO eleDAO)
        {
            var worksheet = package.Workbook.Worksheets[1];
            int rowCount = worksheet.Dimension.End.Row;

            // 在原位置插入新数据
            for(int row = 0; row < values.Length; row++)
            {
                worksheet.Cells[insertRow, row].Value = values[row].GetValue(eleDAO);
                insertRow++;
            }

            // 调整表格样式和格式
            worksheet.Cells.AutoFitColumns();
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

            // 保存更改
            package.Save();
        }

    }
}
