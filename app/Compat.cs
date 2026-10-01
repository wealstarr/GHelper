namespace GHelper;

internal static class Compat
{
    public static int Clamp(int value, int min, int max)
    {
        if (min > max) throw new ArgumentException("min must be <= max");
        return value < min ? min : value > max ? max : value;
    }

    public static float Clamp(float value, float min, float max)
    {
        if (min > max) throw new ArgumentException("min must be <= max");
        return value < min ? min : value > max ? max : value;
    }

    public static double Clamp(double value, double min, double max)
    {
        if (min > max) throw new ArgumentException("min must be <= max");
        return value < min ? min : value > max ? max : value;
    }

    public static decimal Clamp(decimal value, decimal min, decimal max)
    {
        if (min > max) throw new ArgumentException("min must be <= max");
        return value < min ? min : value > max ? max : value;
    }
}
