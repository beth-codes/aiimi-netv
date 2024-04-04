
using System.Collections.Generic;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;

public class ExcelService
{
    private const int HeaderRow = 1;
    private const int DataStartRow = 2;

   public List<Employee> ReadExcelFile(string filePath)
    {
    List<Employee> employees = new List<Employee>();
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    using (var package = new ExcelPackage(new FileInfo(filePath)))
    {
        ExcelWorksheet worksheet = package.Workbook.Worksheets["in"]; 

        for (int row = DataStartRow; row <= worksheet.Dimension.End.Row; row++)
        {
            Employee employee = new Employee();
            employee.Id = (row - DataStartRow + 1).ToString();
            employee.FirstName = worksheet.Cells[row, 1].Value?.ToString();
            employee.LastName = worksheet.Cells[row, 2].Value?.ToString();
            employee.JobTitle = worksheet.Cells[row, 3].Value?.ToString();
            employee.Phone = worksheet.Cells[row, 4].Value?.ToString();
            employee.Email = worksheet.Cells[row, 5].Value?.ToString();
            employees.Add(employee);
        }
    }

    return employees;
}

      public void AddEmployee(string filePath, Employee employee)
    {
        FileInfo existingFile = new FileInfo(filePath);

        // Check if the file exists
        if (existingFile.Exists)
        // {
        //     // If the file doesn't exist, create a new Excel package
        //     using (ExcelPackage package = new ExcelPackage(existingFile))
        //     {
        //         ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employees");

        //        // Add headers
        //         worksheet.Cells[HeaderRow, 1].Value = "Id";
        //         worksheet.Cells[HeaderRow, 2].Value = "FirstName";
        //         worksheet.Cells[HeaderRow, 3].Value = "LastName";
        //         worksheet.Cells[HeaderRow, 4].Value = "JobTitle";
        //         worksheet.Cells[HeaderRow, 5].Value = "Phone";
        //         worksheet.Cells[HeaderRow, 6].Value = "Email";

        //         // Add the new employee data
        //         int newRow = worksheet.Dimension?.End.Row + 1 ?? DataStartRow;
        //         worksheet.Cells[newRow, 1].Value = employee.Id;
        //         worksheet.Cells[newRow, 1].Value = employee.FirstName;
        //         worksheet.Cells[newRow, 2].Value = employee.LastName;
        //         worksheet.Cells[newRow, 3].Value = employee.JobTitle;
        //         worksheet.Cells[newRow, 4].Value = employee.Phone;
        //         worksheet.Cells[newRow, 5].Value = employee.Email;
        //         package.Save();
        //     }
        // }
        // elsE
        {
            // If the file exists, open the existing Excel package
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // Add the new employee data
                int newRow = worksheet.Dimension?.End.Row + 1 ?? DataStartRow;
                worksheet.Cells[newRow, 1].Value = employee.FirstName;
                worksheet.Cells[newRow, 2].Value = employee.LastName;
                worksheet.Cells[newRow, 3].Value = employee.JobTitle;
                worksheet.Cells[newRow, 4].Value = employee.Phone;
                worksheet.Cells[newRow, 5].Value = employee.Email;
                package.Save();
            }
        }
    }

    public class Employee
    {
        public string? Id { get; set; }
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? JobTitle { get; set; }

        [Required]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }
    }
}
