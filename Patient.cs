public class Patient
{
    public string Id { get; set; } = string.Empty;  // Patient identifier
    public DateTime BirthDate { get; set; }         // To calculate age
    public double HealthcareExpenses { get; set; }  // Healthcare-related financials
    public double HealthcareCoverage { get; set; }  // Insurance coverage
    
    // Medical Measurements
    public double BMI { get; set; }                 // Body Mass Index
    public double SystolicBP { get; set; }          // Systolic Blood Pressure
    public double DiastolicBP { get; set; }         // Diastolic Blood Pressure
    public double SerumCreatinine { get; set; }     // Serum Creatinine
    public double GFR { get; set; }                 // Glomerular Filtration Rate
    public double Potassium { get; set; }           // Potassium levels
    public double Sodium { get; set; }              // Sodium levels
    
    // Track Dialysis Sessions
    public int DialysisSessions { get; set; }       // Number of dialysis sessions

    // List of medications
    public List<string> Medications { get; set; } = new List<string>();

    // Calculate age dynamically
    public int Age 
    {
        get 
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)  // Check if birthday has passed this year
            {
                age--;
            }
            return age;
        }
    }
}
