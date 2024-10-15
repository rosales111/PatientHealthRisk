public class Patient
{
    public string Id { get; set; } = string.Empty;  // Initialize to avoid warnings
    public DateTime BirthDate { get; set; }
    public double HealthcareExpenses { get; set; }
    public double HealthcareCoverage { get; set; }

    // Properties required for HealthRiskCalculator
    public double BMI { get; set; }
    public double SystolicBP { get; set; }
    public double DiastolicBP { get; set; }
    public double SerumCreatinine { get; set; }
    public double GFR { get; set; }
    public double Potassium { get; set; }
    public double Sodium { get; set; }
    public int Age { get; set; } // Could be calculated from BirthDate if needed
    public bool DialysisAdherence { get; set; }
    public List<string> Medications { get; set; } = new List<string>();

    // Constructor and other methods (if any) can be added here
}
