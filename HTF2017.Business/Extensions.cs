using System;
using System.Collections.Generic;
using System.Linq;

namespace HTF2017.Business
{
    public static class Extensions
    {
        public static T RandomSingle<T>(this List<T> values)
        {
            if (values.Any())
            {
                int randomIndex = new Random().Next(0, values.Count);
                return values[randomIndex];
            }
            return default(T);
        }
    }
}