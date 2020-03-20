using System;
using System.IO;
using System.Runtime.Serialization.Json;

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

        public static string SerializeObj<T>(T obj)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, obj);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

            return json;
        }
    }
}
