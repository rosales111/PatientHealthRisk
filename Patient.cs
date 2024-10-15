using System.Collections.Generic;

public class Patient
{
    public string ID { get; set; }
    public double BMI { get; set; }
    public int SystolicBP { get; set; }
    public int DiastolicBP { get; set; }
    public double SerumCreatinine { get; set; }
    public double GFR { get; set; }
    public double Potassium { get; set; }
    public double Sodium { get; set; }
    public int Age { get; set; }
    public List<string> Medications { get; set; }
    public int DialysisAdherence { get; set; }

    // Add more properties based on your parameter file
}
