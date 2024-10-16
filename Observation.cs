namespace PatientHealthRisk
{
public class Observation
{
    public string PatientId { get; set; }  // The ID of the patient this observation belongs to
    public string Code { get; set; }  // The observation code (e.g., for BMI, Blood Pressure, etc.)
    public double Value { get; set; }  // The numerical value of the observation
    public DateTime Date { get; set; }  // The date the observation was recorded
}

}