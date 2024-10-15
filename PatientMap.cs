using CsvHelper.Configuration;

public class PatientMap : ClassMap<Patient>
{
    public PatientMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.BirthDate).Name("BIRTHDATE").TypeConverterOption.Format("yyyy-MM-dd");
        Map(m => m.HealthcareExpenses).Name("HEALTHCARE_EXPENSES");
        Map(m => m.HealthcareCoverage).Name("HEALTHCARE_COVERAGE");
    }
}
