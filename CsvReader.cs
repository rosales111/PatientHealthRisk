using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class CsvReader
{
    public static List<Patient> ReadPatientsFromCsv(string folderPath)
    {
        var patients = new List<Patient>();

        // Directly use the folderPath as it is passed in
        foreach (string file in Directory.GetFiles(folderPath, "*.csv"))
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvHelper.CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Assuming the CSV files are in the correct format matching Patient class
                patients.AddRange(csv.GetRecords<Patient>().ToList());
            }
        }

        return patients;
    }
}
