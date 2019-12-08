using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PTS.App.Utils
{
    public static class IListExtension
    {
        public static void Shuffle<T>(this IList<T> list, int offSet = 1)
        {
            int n = list.Count;
            while (n > offSet)
            {
                n--;
                int k = Utils.Random.Next(offSet, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
