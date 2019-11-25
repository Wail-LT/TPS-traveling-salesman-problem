using System;
using System.Collections.Generic;
using PTS.App.Objects;

namespace PTS.App.Utils
{
    public static class SelectionMethodes
    {
        private static Random random = new Random();

        public static Random Random => random;

        public static Journey Tournament(List<Journey> journeys, int size)
        {
            List<int> Participant = new List<int>();

            int index = -1;
            int rIndex;

            for ( int i = 0; i < size; i++)
            {
                do
                {
                    rIndex = random.Next(0, journeys.Count);
                } while (Participant.Contains(rIndex));

                Participant.Add(rIndex);

                if ( index != -1 && journeys[index].GetFitness() > journeys[rIndex].GetFitness())
                {
                    index = rIndex;
                }
            }

            return journeys[index];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
