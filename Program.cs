using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using OfficeOpenXml;
using PatientHealthRisk;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Get the base directory where the program is running from
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Path to the Excel file (replace CsvFiles path with the actual Excel file path)
            string excelFilePath = Path.Combine(baseDirectory, "CsvFiles", "MockPatientData1-2.xlsx");

            // Instantiate the service to read patients from the Excel file
            var excelReaderService = new CsvReaderService();
            List<Patient> patients = excelReaderService.ReadPatientsFromExcel(excelFilePath);

            // Evaluate risk and output results
            List<RiskResult> results = new List<RiskResult>();
            foreach (var patient in patients)
            {
                // Debugging: Output the patient ID and check if it exists
                if (string.IsNullOrEmpty(patient.PatientId))
                {
                    Console.WriteLine("Warning: Patient ID is missing.");
                }
                else
                {
                    Console.WriteLine($"Processing Patient ID: {patient.PatientId}");
                }

                // Debugging: Output the latest observation values
                Console.WriteLine($"BMI: {patient.BMI}, SystolicBP: {patient.SystolicBP}, DiastolicBP: {patient.DiastolicBP}");

                // Calculate risk score using the latest observations
                int score = HealthRiskCalculator.CalculateRiskPoints(patient, new Dictionary<string, double>
                {
                    {"BMI", patient.BMI},
                    {"SystolicBP", patient.SystolicBP},
                    {"DiastolicBP", patient.DiastolicBP},
                    {"SerumCreatinine", patient.SerumCreatinine},
                    {"GFR", patient.GFR},
                    {"Potassium", patient.Potassium},
                    {"Sodium", patient.Sodium}
                });
                string risk = score >= 85 ? "At Risk" : "Not At Risk";

                // Debugging: Output risk score and risk status
                Console.WriteLine($"Patient ID: {patient.PatientId}, Risk Score: {score}, Risk Status: {risk}");

                // Add the result to the list
                results.Add(new RiskResult { ID = patient.PatientId, RiskStatus = risk });
            }

            // Write results to a new CSV file in the same directory
            string resultsFilePath = Path.Combine(baseDirectory, "PatientRiskResults.csv");
            using (var writer = new StreamWriter(resultsFilePath))
            using (var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(results);
            }

            Console.WriteLine($"Risk evaluation completed. Check '{resultsFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
