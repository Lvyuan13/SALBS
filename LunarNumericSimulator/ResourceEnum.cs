public enum Resources {
    CO2 = 0,
    H2O = 1,
    H = 2,
    O = 3,
    Food = 4,
    CH4 = 5,
    Heat = 6,
    N = 7,
    ElecticalEnergy = 8,
    Humidity = 9
}

public static class ResourceExtensions
{
    public static string ToString(this Resources res)
    {
        switch (res)
        {
            case Resources.CO2:
                return "CO2";
            case Resources.CH4:
                return "CH4";
            case Resources.H2O:
                return "H2O";
            case Resources.H:
                return "H";
            case Resources.O:
                return "O";
            case Resources.Food:
                return "Food";
            case Resources.Heat:
                return "Heat";
            case Resources.N:
                return "N";
            case Resources.ElecticalEnergy:
                return "ElecticalEnergy";
            case Resources.Humidity:
                return "Humidity";
            default:
                throw new System.Exception("This resource does not exist!");
        }
    }

    public static bool IsAtmospheric(this Resources res)
    {
        switch (res)
        {
            case Resources.CO2:
                return true;
            case Resources.CH4:
                return true;
            case Resources.H2O:
                return false;
            case Resources.H:
                return false;
            case Resources.O:
                return true;
            case Resources.Food:
                return false;
            case Resources.Heat:
                return false;
            case Resources.N:
                return true;
            case Resources.ElecticalEnergy:
                return false;
            case Resources.Humidity:
                return true;
            default:
                throw new System.Exception("This resource does not exist!");
        }
    }
}