using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using OfficeOpenXml;
namespace exportTable.Controllers
{

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal ProductPrice { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

    }


    public class ProductController : Controller
    {
        public IActionResult ProductList()
        {

            string conStr = "Data Source=AYUSH\\AYUSH;Initial Catalog=CoffeeShopManagementSystem; Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Product_SelectAll";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            return View(dataTable);
        }
        public IActionResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context

            List<Product> productList = new List<Product>();
            string conStr = "Data Source=AYUSH\\AYUSH;Initial Catalog=CoffeeShopManagementSystem; Integrated Security=true;TrustServerCertificate=true;";
            SqlConnection sqlConnection = new SqlConnection(conStr);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Product_SelectAll";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Product product = new Product();
                product.ProductId = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.ProductCode = sqlDataReader["ProductCode"].ToString();
                product.ProductPrice = Convert.ToDecimal(sqlDataReader["ProductPrice"]);
                product.Description = sqlDataReader["Description"].ToString();
                product.UserId = Convert.ToInt32(sqlDataReader["UserID"]);
                productList.Add(product);
            }
            sqlConnection.Close();

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells[1, 1].Value = "ProductID";
                worksheet.Cells[1, 2].Value = "ProductName";
                worksheet.Cells[1, 3].Value = "ProductCode";
                worksheet.Cells[1, 4].Value = "ProductPrice";
                worksheet.Cells[1, 5].Value = "Description";
                worksheet.Cells[1, 6].Value = "UserID";

                int row = 2;
                foreach (var product in productList)
                {
                    worksheet.Cells[row, 1].Value = product.ProductId;
                    worksheet.Cells[row, 2].Value = product.ProductName;
                    worksheet.Cells[row, 3].Value = product.ProductCode;
                    worksheet.Cells[row, 4].Value = product.ProductPrice;
                    worksheet.Cells[row, 5].Value = product.Description;
                    worksheet.Cells[row, 6].Value = product.UserId;
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"ProductList-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}