using OfficeOpenXml;

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
            // Read the Patient Info sheet
            var patientInfoSheet = package.Workbook.Worksheets["Patient Info"];
            int rowCount = patientInfoSheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)  // Start at row 2 to skip the header
            {
                var patient = new Patient
                {
                    PatientId = patientInfoSheet.Cells[row, 1].Text,  // Column 1A for Patient ID
                    BirthDate = DateTime.TryParse(patientInfoSheet.Cells[row, 2].Text, out var birthDate) 
                        ? birthDate 
                        : DateTime.MinValue  // Column 1B for BIRTHDATE
                };

                if (string.IsNullOrEmpty(patient.PatientId))
                {
                    Console.WriteLine("Warning: Patient ID is missing.");
                    continue;
                }

                // Now we need to find this patient's observations
                var observations = GetPatientObservations(package, patient.PatientId);

                // Debugging: Output retrieved observation values
                Console.WriteLine($"Observations for Patient ID {patient.PatientId}: BMI={observations.GetValueOrDefault("BMI")}, SystolicBP={observations.GetValueOrDefault("SystolicBP")}, DiastolicBP={observations.GetValueOrDefault("DiastolicBP")}");

                // Assign observation values to the patient
                patient.BMI = observations.ContainsKey("BMI") ? observations["BMI"] : 0;
                patient.SystolicBP = observations.ContainsKey("SystolicBP") ? observations["SystolicBP"] : 0;
                patient.DiastolicBP = observations.ContainsKey("DiastolicBP") ? observations["DiastolicBP"] : 0;
                patient.SerumCreatinine = observations.ContainsKey("SerumCreatinine") ? observations["SerumCreatinine"] : 0;
                patient.GFR = observations.ContainsKey("GFR") ? observations["GFR"] : 0;
                patient.Potassium = observations.ContainsKey("Potassium") ? observations["Potassium"] : 0;
                patient.Sodium = observations.ContainsKey("Sodium") ? observations["Sodium"] : 0;

                patients.Add(patient);
            }
        }

        return patients;
    }

    // Helper function to get observations for a patient from the "Observations" sheet
    public Dictionary<string, double> GetPatientObservations(ExcelPackage package, string patientId)
    {
        var observationsSheet = package.Workbook.Worksheets["Observations"];
        int rowCount = observationsSheet.Dimension.Rows;

        var observations = new Dictionary<string, double>();

        // Map for observation codes (assuming these are correct)
        var codeMap = new Dictionary<string, string>
        {
            {"39156-5", "BMI"},  // BMI code
            {"8480-6", "SystolicBP"},  // Systolic BP code
            {"8462-4", "DiastolicBP"},  // Diastolic BP code
            {"33914-3", "GFR"},  // GFR code
            {"6298-4", "Potassium"},  // Potassium code
            {"2947-0", "Sodium"}  // Sodium code
        };

        for (int row = 2; row <= rowCount; row++)
        {
            var currentPatientId = observationsSheet.Cells[row, 2].Text;  // Column 1B for Patient ID

            if (currentPatientId == patientId)
            {
                // Read the observation code from column 5 (Code)
                var observationCode = observationsSheet.Cells[row, 5].Text;

                // Check if the code matches any of the known medical observations
                if (codeMap.ContainsKey(observationCode))
                {
                    var observationType = codeMap[observationCode];

                    // Read the value from column 7 (Value)
                    if (double.TryParse(observationsSheet.Cells[row, 7].Text, out var observationValue))
                    {
                        observations[observationType] = observationValue;
                    }
                }
            }
        }

        return observations;
    }

// Helper function to calculate Dialysis Sessions from the "Procedures" sheet
public int GetDialysisSessions(ExcelPackage package, string patientId)
{
    var proceduresSheet = package.Workbook.Worksheets["Procedures"];
    int rowCount = proceduresSheet.Dimension.Rows;

    int dialysisCount = 0;

    // Normalize the patientId for comparison (trim spaces and ensure case-insensitive comparison)
    patientId = patientId.Trim();

    Console.WriteLine($"Looking for dialysis sessions for Patient ID: '{patientId}' (Length: {patientId.Length})");

    for (int row = 2; row <= rowCount; row++)
    {
        var currentPatientId = proceduresSheet.Cells[row, 2].Text?.Trim();  // Column 1B for Patient ID (trim spaces)
        var procedureCode = proceduresSheet.Cells[row, 4].Text?.Trim();  // Column 4 for CODE (trim spaces)

        // Debugging output to ensure correct data is being read and compared
        Console.WriteLine($"Row {row}: Patient ID = '{currentPatientId}' (Length: {currentPatientId?.Length}), Procedure Code = '{procedureCode}'");

        // Check if the row matches the patient and is a dialysis procedure
        if (currentPatientId == patientId && procedureCode == "265764009")
        {
            dialysisCount++;
            Console.WriteLine($"Dialysis found for Patient ID: {patientId} on row {row}");
        }
    }

    // Debugging output to confirm the number of dialysis sessions found
    Console.WriteLine($"Total Dialysis Sessions for Patient ID {patientId}: {dialysisCount}");

    return dialysisCount;
}





}


