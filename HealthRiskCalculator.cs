public static class HealthRiskCalculator
{
    public static int CalculateRiskPoints(Patient patient)
    {
        int points = 0;

        // Add logic for BMI
        if (patient.BMI < 18.5) points += 10;
        else if (patient.BMI >= 25 && patient.BMI <= 29.9) points += 4;
        else if (patient.BMI >= 30 && patient.BMI <= 34.9) points += 7;
        else if (patient.BMI >= 35) points += 10;

        // Blood Pressure
        if (patient.SystolicBP >= 180 || patient.DiastolicBP >= 120) points += 15;
        else if (patient.SystolicBP >= 140 || patient.DiastolicBP >= 90) points += 11;
        else if (patient.SystolicBP >= 130 || patient.DiastolicBP >= 80) points += 7;
        else if (patient.SystolicBP >= 120) points += 5;

        // Serum Creatinine
        if (patient.SerumCreatinine >= 6.0) points += 15;
        else if (patient.SerumCreatinine >= 4.0) points += 11;
        else if (patient.SerumCreatinine >= 2.0) points += 7;
        else if (patient.SerumCreatinine >= 1.4) points += 4;

        // GFR
        if (patient.GFR < 15) points += 25;
        else if (patient.GFR < 30) points += 21;
        else if (patient.GFR < 45) points += 17;
        else if (patient.GFR < 60) points += 12;
        else if (patient.GFR < 90) points += 6;

        // Potassium
        if (patient.Potassium > 6.0) points += 10;
        else if (patient.Potassium >= 5.6) points += 6;
        else if (patient.Potassium >= 5.1) points += 3;
        else if (patient.Potassium < 3.5) points += 8;

        // Sodium
        if (patient.Sodium > 145) points += 8;
        else if (patient.Sodium < 125) points += 10;
        else if (patient.Sodium < 130) points += 6;

        // Age
        if (patient.Age >= 76) points += 10;
        else if (patient.Age >= 61) points += 7;
        else if (patient.Age >= 41) points += 4;
        else if (patient.Age >= 18) points += 1;

        // Dialysis Adherence
        points += patient.DialysisAdherence;

        // Medications (points based on medications)
        foreach (var med in patient.Medications)
        {
            switch (med.ToLower())
            {
                case "lisinopril": case "enalapril": case "losartan": case "valsartan": points += 3; break;
                case "epoetin alfa": case "sevelamer": case "insulin": points += 6; break;
                case "warfarin": case "metoprolol": case "carvedilol": case "atorvastatin": case "simvastatin": points += 4; break;
            }
        }

        return points;
    }
}
