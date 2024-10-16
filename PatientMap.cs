using CsvHelper.Configuration;

public class PatientMap : ClassMap<Patient>
{
    public PatientMap()
    {
        // Mapping the basic fields
        Map(m => m.PatientId).Name("Id");
        Map(m => m.BirthDate).Name("BIRTHDATE").TypeConverterOption.Format("yyyy-MM-dd");
        Map(m => m.HealthcareExpenses).Name("HEALTHCARE_EXPENSES");
        Map(m => m.HealthcareCoverage).Name("HEALTHCARE_COVERAGE");

        // New fields based on the risk calculator requirements
        Map(m => m.BMI).Name("BMI");  // Assuming BMI is a column in the CSV/Excel file
        Map(m => m.DialysisSessions).Name("DIALYSIS_SESSIONS");  // Mapping Dialysis Sessions if present

        // Add additional mappings as necessary for other clinical parameters
        // Map(m => m.SystolicBP).Name("SystolicBP");
        // Map(m => m.DiastolicBP).Name("DiastolicBP");
        // Map(m => m.SerumCreatinine).Name("SerumCreatinine");
        // Map(m => m.GFR).Name("GFR");
        // Map(m => m.Potassium).Name("Potassium");
        // Map(m => m.Sodium).Name("Sodium");
    }
}
