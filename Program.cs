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
            string excelFilePath = Path.Combine(baseDirectory, "CsvFiles", "MockPatientDataRevised1-2.xlsx");

            // Instantiate the service to read patients from Excel file
            var excelReaderService = new CsvReaderService();
            List<Patient> patients = excelReaderService.ReadPatientsFromExcel(excelFilePath);

            // Read all observations from the Excel file (create a method to read observations if not already present)
            List<Observation> observationsList = excelReaderService.ReadObservationsFromExcel(excelFilePath); // Implement this method to read observation data

            // Evaluate risk and output results
            List<RiskResult> results = new List<RiskResult>();
            foreach (var patient in patients)
            {
                // Debugging: Output the patient ID and check if it exists
                if (string.IsNullOrEmpty(patient.Id))
                {
                    Console.WriteLine("Warning: Patient ID is missing.");
                }
                else
                {
                    Console.WriteLine($"Processing Patient ID: {patient.Id}");
                }

                // Fetch the latest observations for the patient
                var latestObservations = CsvReaderService.GetLatestObservations(patient.Id, observationsList);

                // Debugging: Output the latest observation values
                Console.WriteLine($"BMI: {latestObservations["BMI"]}, SystolicBP: {latestObservations["SystolicBP"]}, DiastolicBP: {latestObservations["DiastolicBP"]}");

                // Calculate risk score using the latest observations
                int score = HealthRiskCalculator.CalculateRiskPoints(patient, latestObservations);
                string risk = score >= 85 ? "At Risk" : "Not At Risk";

                // Debugging: Output risk score and risk status
                Console.WriteLine($"Patient ID: {patient.Id}, Risk Score: {score}, Risk Status: {risk}");

                // Add the result to the list
                results.Add(new RiskResult { ID = patient.Id, RiskStatus = risk });
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
