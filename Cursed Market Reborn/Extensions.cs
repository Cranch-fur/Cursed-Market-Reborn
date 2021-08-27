using System;

namespace Cursed_Market_Reborn
{
    public static class Extensions
    {
        public static T[] Subset<T>(this T[] array, int start, int count)
        {
            T[] array2 = new T[count];
            Array.Copy(array, start, array2, 0, count);
            return array2;
        }
    }
}
