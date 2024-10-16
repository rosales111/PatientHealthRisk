using OfficeOpenXml;
using PatientHealthRisk;
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
                    Id = worksheet.Cells[row, 2].Text,  // Assuming Patient ID is in column 2
                    BMI = double.TryParse(worksheet.Cells[row, 3].Text, out var bmi) ? bmi : 0,  // Assuming BMI is in column 3
                    SystolicBP = double.TryParse(worksheet.Cells[row, 4].Text, out var systolicBP) ? systolicBP : 0,  // Assuming Systolic BP is in column 4
                    DiastolicBP = double.TryParse(worksheet.Cells[row, 5].Text, out var diastolicBP) ? diastolicBP : 0,  // Assuming Diastolic BP is in column 5
                    SerumCreatinine = double.TryParse(worksheet.Cells[row, 6].Text, out var creatinine) ? creatinine : 0,  // Assuming Serum Creatinine is in column 6
                    GFR = double.TryParse(worksheet.Cells[row, 7].Text, out var gfr) ? gfr : 0,  // Assuming GFR is in column 7
                    Potassium = double.TryParse(worksheet.Cells[row, 8].Text, out var potassium) ? potassium : 0,  // Assuming Potassium is in column 8
                    Sodium = double.TryParse(worksheet.Cells[row, 9].Text, out var sodium) ? sodium : 0  // Assuming Sodium is in column 9
                };

                // Debugging: Output patient details
                Console.WriteLine($"Read Patient ID: {patient.Id}, BMI: {patient.BMI}, SystolicBP: {patient.SystolicBP}, DiastolicBP: {patient.DiastolicBP}");

                patients.Add(patient);
            }
        }

        return patients;
    }

    // New method to read observations
    public List<Observation> ReadObservationsFromExcel(string excelFilePath)
    {
        var observations = new List<Observation>();

        // Load the Excel file using EPPlus
        using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
        {
            var worksheet = package.Workbook.Worksheets["Observations"];  // Assuming Observations sheet is named "Observations"
            int rowCount = worksheet.Dimension.Rows;

            // Loop through each row to read observations
            for (int row = 2; row <= rowCount; row++)  // Start at row 2 to skip the header
            {
                var observation = new Observation
                {
                    PatientId = worksheet.Cells[row, 2].Text,  // Assuming Patient ID is in column 2
                    Code = worksheet.Cells[row, 5].Text,  // Assuming Observation Code is in column 5
                    Value = double.TryParse(worksheet.Cells[row, 6].Text, out var value) ? value : 0,  // Assuming Value is in column 6
                    Date = DateTime.TryParse(worksheet.Cells[row, 1].Text, out var date) ? date : DateTime.MinValue  // Assuming Date is in column 1
                };

                observations.Add(observation);
            }
        }

        return observations;
    }

    public static Dictionary<string, double> GetLatestObservations(string patientId, List<Observation> allObservations)
    {
        Dictionary<string, double> observations = new Dictionary<string, double>();

        // Filter observations by patient ID
        var patientObservations = allObservations.Where(o => o.PatientId == patientId).ToList();

        // Get the latest BMI, SystolicBP, DiastolicBP
        var latestBMI = patientObservations.Where(o => o.Code == "39156-5")
                                           .OrderByDescending(o => o.Date)
                                           .FirstOrDefault();
        var latestSystolicBP = patientObservations.Where(o => o.Code == "8480-6")
                                                  .OrderByDescending(o => o.Date)
                                                  .FirstOrDefault();
        var latestDiastolicBP = patientObservations.Where(o => o.Code == "8462-4")
                                                   .OrderByDescending(o => o.Date)
                                                   .FirstOrDefault();

        // Add to observations dictionary (default to 0 if not found)
        observations["BMI"] = latestBMI != null ? latestBMI.Value : 0;
        observations["SystolicBP"] = latestSystolicBP != null ? latestSystolicBP.Value : 0;
        observations["DiastolicBP"] = latestDiastolicBP != null ? latestDiastolicBP.Value : 0;

        return observations;
    }
}
