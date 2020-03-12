using System;
namespace PTS.App.Utils
{
    public class Utils
    {
        public static readonly Random Random = new Random();

        public static ulong Factor(int nb)
        {
            ulong factor = (ulong)nb;
            for (ulong i = factor - 1 ; i > 0; i--)
                factor *= i;

            return factor;
        }
    }
}
