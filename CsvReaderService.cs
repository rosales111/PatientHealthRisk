using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;


public class CsvReaderService
{
    public List<Patient> ReadPatientsFromCsv(string folderPath)
    {
        var patients = new List<Patient>();

        // Loop through CSV files in the folder
        foreach (string file in Directory.GetFiles(folderPath, "patients.csv"))
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Register the PatientMap to ensure columns are mapped correctly
                csv.Context.RegisterClassMap<PatientMap>();
                patients.AddRange(csv.GetRecords<Patient>().ToList());
            }
        }

        return patients;
    }
}
