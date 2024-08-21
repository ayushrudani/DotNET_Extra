# Excel Export

## Exporting to Excel

The `ExportToExcel` method is used to export the data to an Excel file. The method is called on the `DataTable` object. The method takes the following parameters:

- `fileName`: The name of the Excel file to be created.
- `sheetName`: The name of the sheet in the Excel file.
- `header`: The header to be displayed in the Excel file.

The following code snippet demonstrates how to export data to an Excel file:

## Step 1: Create a Controller and Action Method

```csharp
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

```

## Step 2: Create a View

```html

@using System.Data

<main id="main" class="main">
    <div class="pagetitle">
        <h1>Product</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <a asp-controller="Home" asp-action="Index">
                        <i class="bi bi-house-door"></i>
                    </a>
                </li>
                <li class="breadcrumb-item">
                    <a>Product List</a>
                </li>
            </ol>
        </nav>
        <div class="d-flex justify-content-end align-items-center">
            <div>
                <a class="btn btn-outline-primary" asp-controller="Product" asp-action="ExportToExcel"><i
                        class="bi bi-plus-lg"></i>&nbsp;Export Product</a>
            </div>
        </div>
    </div><!-- End Page Title -->

    <table class="table table-hover table-header-fixed">
        <thead>
            <tr>
                <th scope="col">Product ID</th>
                <th scope="col">Product Name</th>
                <th class="col">Product Code</th>
                <th class="col">Product Price</th>
                <th class="col">Description</th>
                <th class="col">User ID</th>
                <th class="text-center">Action</th>
            </tr>
        </thead>
        <tbody id="sample_2">
            @foreach (DataRow dataRow in Model.Rows)
            {
                <tr>
                    <td>@dataRow["ProductID"]</td>
                    <td>@dataRow["ProductName"]</td>
                    <td>@dataRow["ProductCode"]</td>
                    <td>@dataRow["ProductPrice"]</td>
                    <td>@dataRow["Description"]</td>
                    <td>@dataRow["UserName"]</td>
                    <td class="text-center">
                        <a class="btn btn-outline-success btn-xs" asp-controller="Product" asp-action="ProductEdit"
                            asp-route-ProductID="@dataRow["ProductID"]">
                            <i class="bi bi-pencil-fill"></i>
                        </a>
                        <a class="btn btn-outline-danger btn-xs" asp-controller="Product" asp-action="ProductDelete"
                            asp-route-ProductID="@dataRow["ProductID"]">
                            <i class="bi bi-x"></i>
                        </a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</main>

```

## Step 3: Create an Action Method to Export Data to Excel

```csharp

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

```

## Step 4: Run the Application

Run the application and navigate to the `ProductList` action method. Click on the `Export Product` button to export the data to an Excel file.
