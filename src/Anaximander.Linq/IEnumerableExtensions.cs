﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    /// <summary>
    /// Extension methods for IEnumerable.
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Orders a collection using a provided collection to set the order. Items whose sort key does not appear in the provided order appear last, in their original order.
        /// </summary>
        /// <param name="source">A collection of items to be sorted.</param>
        /// <param name="sortKeySelector">A func to derive a key on which to sort from the items in the source collection.</param>
        /// <param name="ordering">A collection denoting the desired order</param>
        /// <returns>A collection sorted to match the ordering collection, with missing items sorted to the end.</returns>
        public static IOrderedEnumerable<T> OrderToMatch<T, TKey>(this IEnumerable<T> source, Func<T, TKey> sortKeySelector, IEnumerable<TKey> ordering)
        {
            var orderLookup = ordering
                .Select((x, i) => new { key = x, index = i })
                .ToDictionary(k => k.key, v => v.index);

            if (!orderLookup.Any())
            {
                throw new ArgumentException("Ordering collection cannot be empty.", nameof(ordering));
            }

            T[] sourceArray = source.ToArray();

            return sourceArray
                .OrderBy(x =>
                {
                    int index;
                    if (orderLookup.TryGetValue(sortKeySelector(x), out index))
                    {
                        return index;
                    }
                    return Int32.MaxValue;
                })
                .ThenBy(x => Array.IndexOf(sourceArray, x));
        }
    }
}