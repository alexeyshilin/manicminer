
public static class SpectrumShortExtensions
{
    public static int GetX(this short s)
    {
        int val = s & 0x1f; // 1f - 11111
        return val;
    }

    public static int GetY(this short s)
    {
        //int val = s & 0x01e0; // 1f - 11111
        //val = (val >> 5);

        int val = s;

        val = val >> 5;
        val = val & 0xf;

        return val;
    }
}