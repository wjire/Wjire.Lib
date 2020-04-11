using System;
using System.Collections.Generic;
using System.Linq;

namespace Wjire.Common
{
    /// <summary>
    /// 取随机数
    /// </summary>
    public static class RandomExtension
    {

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            Random random = new Random();
            return source.GetRandomElement(random);
        }


        public static T GetRandomElement<T>(this IEnumerable<T> source, Random random)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (source is ICollection<T> collection)
            {
                int count = collection.Count;
                if (count == 0)
                {
                    throw new InvalidOperationException("source is empty");
                }
                int index = random.Next(count);
                return collection.ElementAt(index);
            }
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw new InvalidOperationException("source is empty");
                }
                int count = 1;
                T current = iterator.Current;
                while (iterator.MoveNext())
                {
                    count++;
                    if (random.Next(count) == 0)
                    {
                        current = iterator.Current;
                    }
                }
                return current;
            }
        }
    }
}