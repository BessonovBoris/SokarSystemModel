namespace Game1;

public static class FloatCompare
{
    public static bool Equal(this float a, float b)
    {
        float epsilon = 1e-3f;

        if (a > b - epsilon && a < b + epsilon)
        {
            return true;
        }

        return false;
    }
}