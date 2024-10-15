using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Get the base directory where the program is running from
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine it with the relative path to the CSV folder
            string folderPath = Path.Combine(baseDirectory, "CsvFiles", "synthea_sample_data_csv_apr2020", "csv");

            // Check if patients.csv exists in the specified folder
            string patientsFilePath = Path.Combine(folderPath, "patients.csv");
            if (!File.Exists(patientsFilePath))
            {
                Console.WriteLine("Error: patients.csv file not found in the specified folder.");
                return;
            }

            // Instantiate CsvReaderService and read patients from CSV files
            var csvReaderService = new CsvReaderService();
            List<Patient> patients = csvReaderService.ReadPatientsFromCsv(folderPath);

            // Evaluate risk and output results
            List<RiskResult> results = new List<RiskResult>();
            foreach (var patient in patients)
            {
                int score = HealthRiskCalculator.CalculateRiskPoints(patient);
                string risk = score >= 85 ? "At Risk" : "Not At Risk";

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
