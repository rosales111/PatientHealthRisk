using System;
using System.Collections.Generic;
using System.IO;  // For Path.Combine

public class Program
{
    public static void Main(string[] args)
    {
        // Get the base directory where the program is running from
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Combine it with the relative path to the CSV folder
        string folderPath = Path.Combine(baseDirectory, "CsvFiles", "synthea_sample_data_csv_apr2020", "csv");

        // Read patients from CSV files using CsvReader
        List<Patient> patients = CsvReader.ReadPatientsFromCsv(folderPath);

        // Evaluate risk and output results
        List<RiskResult> results = new List<RiskResult>();
        foreach (var patient in patients)
        {
            int score = HealthRiskCalculator.CalculateRiskPoints(patient);
            string risk = score >= 85 ? "At Risk" : "Not At Risk";

            results.Add(new RiskResult { ID = patient.ID, RiskStatus = risk });
        }

        // Write results to a new CSV file
        using (var writer = new StreamWriter("PatientRiskResults.csv"))
        using (var csv = new CsvHelper.CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(results);
        }

        Console.WriteLine("Risk evaluation completed. Check 'PatientRiskResults.csv'.");
    }
}
