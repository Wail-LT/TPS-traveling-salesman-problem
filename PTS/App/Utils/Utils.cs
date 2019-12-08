using System;
namespace PTS.App.Utils
{
    public class Utils
    {
        public static readonly Random Random = new Random();

        public static int Factor(int nb)
        {
            for (int i = nb-1 ; i > 0; i--)
                nb *= i;

            return nb;
        }
    }
}
