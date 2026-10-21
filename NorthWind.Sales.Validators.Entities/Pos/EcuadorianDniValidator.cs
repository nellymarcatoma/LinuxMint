namespace NorthWind.Sales.Validators.Entities.Pos;

public static class EcuadorianDniValidator
{
    public static bool IsValid(string? dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
            return false;

        string cleanDni = dni.Trim();
        if (cleanDni.Length != 10 || !cleanDni.All(char.IsDigit))
            return false;

        int province = int.Parse(cleanDni.Substring(0, 2));
        if (province < 1 || (province > 24 && province != 30))
            return false;

        int thirdDigit = int.Parse(cleanDni[2].ToString());
        if (thirdDigit >= 6)
            return false;

        int[] coefficients = [2, 1, 2, 1, 2, 1, 2, 1, 2];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            int val = int.Parse(cleanDni[i].ToString()) * coefficients[i];
            if (val >= 10)
                val -= 9;
            sum += val;
        }

        int remainder = sum % 10;
        int verifier = remainder == 0 ? 0 : 10 - remainder;
        int tenthDigit = int.Parse(cleanDni[9].ToString());

        return verifier == tenthDigit;
    }
}
