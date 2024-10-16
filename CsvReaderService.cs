using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

public class CsvReaderService
{
    public List<Patient> ReadPatientsFromExcel(string excelFilePath)
    {
        var patients = new List<Patient>();

        // Ensure the file exists
        if (!File.Exists(excelFilePath))
        {
            throw new FileNotFoundException($"Excel file not found at {excelFilePath}");
        }

        // Load the Excel file using EPPlus
        using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];  // Assuming the first worksheet contains patient data
            int rowCount = worksheet.Dimension.Rows;

            // Debugging: Output column headers
            Console.WriteLine("Reading Excel File - Headers:");
            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                Console.WriteLine($"Column {col}: {worksheet.Cells[1, col].Text}");
            }

            // Loop through each row (assuming row 1 is the header)
            for (int row = 2; row <= rowCount; row++)  // Start at row 2 to skip the header
            {
                var patient = new Patient
                {
                    Id = worksheet.Cells[row, 2].Text,  // Assuming Patient ID is in column 1
                    BMI = double.TryParse(worksheet.Cells[row, 2].Text, out var bmi) ? bmi : 0,
                    SystolicBP = double.TryParse(worksheet.Cells[row, 3].Text, out var systolicBP) ? systolicBP : 0,
                    DiastolicBP = double.TryParse(worksheet.Cells[row, 4].Text, out var diastolicBP) ? diastolicBP : 0,
                    SerumCreatinine = double.TryParse(worksheet.Cells[row, 5].Text, out var creatinine) ? creatinine : 0,
                    GFR = double.TryParse(worksheet.Cells[row, 6].Text, out var gfr) ? gfr : 0,
                    Potassium = double.TryParse(worksheet.Cells[row, 7].Text, out var potassium) ? potassium : 0,
                    Sodium = double.TryParse(worksheet.Cells[row, 8].Text, out var sodium) ? sodium : 0
                };

                // Debugging: Output patient details
                Console.WriteLine($"Read Patient ID: {patient.Id}, BMI: {patient.BMI}, SystolicBP: {patient.SystolicBP}, DiastolicBP: {patient.DiastolicBP}");

                patients.Add(patient);
            }
        }

        return patients;
    }
}
