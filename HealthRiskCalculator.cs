public static class HealthRiskCalculator
{
    public static int CalculateRiskPoints(Patient patient, Dictionary<string, double> observations)
    {
        int points = 0;

        // Extract observations if they exist, otherwise default to 0 or null
        double bmi = observations.ContainsKey("BMI") ? observations["BMI"] : 0;
        double systolicBP = observations.ContainsKey("SystolicBP") ? observations["SystolicBP"] : 0;
        double diastolicBP = observations.ContainsKey("DiastolicBP") ? observations["DiastolicBP"] : 0;
        double serumCreatinine = observations.ContainsKey("SerumCreatinine") ? observations["SerumCreatinine"] : 0;
        double gfr = observations.ContainsKey("GFR") ? observations["GFR"] : 0;
        double potassium = observations.ContainsKey("Potassium") ? observations["Potassium"] : 0;
        double sodium = observations.ContainsKey("Sodium") ? observations["Sodium"] : 0;

        // BMI Logic
        if (bmi < 18.5) points += 10;  // Underweight
        else if (bmi >= 25 && bmi <= 29.9) points += 4;  // Overweight
        else if (bmi >= 30 && bmi <= 34.9) points += 7;  // Obese
        else if (bmi >= 35) points += 10;  // Severe Obesity

        // Blood Pressure Logic
        if (systolicBP >= 180 || diastolicBP >= 120) points += 15;  // Hypertensive Crisis
        else if (systolicBP >= 140 || diastolicBP >= 90) points += 11;  // Hypertension Stage 2
        else if (systolicBP >= 130 || diastolicBP >= 80) points += 7;  // Hypertension Stage 1
        else if (systolicBP >= 120 && diastolicBP < 80) points += 5;  // Elevated BP

        // Serum Creatinine Logic
        if (serumCreatinine >= 6.0) points += 15;  // Critical Elevation
        else if (serumCreatinine >= 4.0) points += 11;  // High Elevation
        else if (serumCreatinine >= 2.0) points += 7;  // Moderate Elevation
        else if (serumCreatinine >= 1.4) points += 4;  // Mild Elevation

        // GFR Logic
        if (gfr < 15) points += 25;  // Kidney Failure
        else if (gfr < 30) points += 21;  // Severe CKD
        else if (gfr < 45) points += 17;  // Moderate CKD Stage 3
        else if (gfr < 60) points += 12;  // Moderate CKD Stage 2
        else if (gfr < 90) points += 6;  // Mild CKD Stage 1

        // Potassium Logic
        if (potassium > 6.0) points += 10;  // Severe
        else if (potassium >= 5.6) points += 6;  // Moderate
        else if (potassium >= 5.1) points += 3;  // Mild
        else if (potassium < 3.5) points += 8;  // Low

        // Sodium Logic
        if (sodium > 145) points += 8;  // Hypernatremia
        else if (sodium < 125) points += 10;  // Severe Hyponatremia
        else if (sodium < 130 && sodium >= 125) points += 6;  // Moderate Hyponatremia

        // Age Logic
        if (patient.Age >= 76) points += 10;  // Age 76+
        else if (patient.Age >= 61) points += 7;  // Age 61-75
        else if (patient.Age >= 41) points += 4;  // Age 41-60
        else if (patient.Age >= 18) points += 1;  // Age 18-40

        // Dialysis Adherence Logic
        switch (patient.DialysisSessions)  // Assuming DialysisSessions indicates the number of sessions attended
        {
            case 0: points += 20; break;  // No adherence
            case 1: points += 15; break;
            case 2: points += 10; break;
            case 3: points += 0; break;  // Full adherence, no points
        }

        // Medications (points based on medications)
        foreach (var med in patient.Medications)
        {
            switch (med.ToLower())
            {
                case "lisinopril": case "enalapril": case "losartan": case "valsartan": points += 3; break;
                case "epoetin alfa": case "sevelamer": case "insulin": case "furosemide": points += 6; break;
                case "warfarin": case "metoprolol": case "carvedilol": case "atorvastatin": case "simvastatin": points += 4; break;
            }
        }

        return points;
    }
}
