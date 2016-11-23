﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Anaximander.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<T> LocalMinima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMinima(x => x);
        }

        public static IEnumerable<TSource> LocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderBy(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Item;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) < 0) && (back[1].Value.CompareTo(back[2].Value) < 0))
                        {
                            yield return back[1].Item;
                        }
                    }
                }

                var backLargest = back.OrderBy(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Item;
                }
            }
        }

        public static IEnumerable<T> LocalMaxima<T>(this IEnumerable<T> source) where T : IComparable
        {
            return source.LocalMaxima(x => x);
        }

        public static IEnumerable<TSource> LocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            if (!source.Any())
            {
                throw new InvalidOperationException("Source collection is empty");
            }

            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Item = x,
                    Value = comparison(x)
                })
                .Window(3)
                .Select(x => x.ToList())
                .GetEnumerator();

            if (windows.MoveNext())
            {
                var front = windows.Current;

                if (front.OrderByDescending(x => x.Value).First().Index == 0)
                {
                    yield return front.First().Item;
                }

                var back = front;
                while (windows.MoveNext())
                {
                    back = windows.Current;

                    if (back.Count == 3)
                    {
                        if ((back[1].Value.CompareTo(back[0].Value) > 0) && (back[1].Value.CompareTo(back[2].Value) > 0))
                        {
                            yield return back[1].Item;
                        }
                    }
                }

                var backLargest = back.OrderByDescending(x => x.Value).First();
                if ((backLargest.Index != 0) && (backLargest.Index == back.Max(x => x.Index)))
                {
                    yield return back.Last().Item;
                }
            }
        }

        public static IEnumerable<int> IndexOfLocalMinima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.Select(comparison).IndexOfLocalMinima();
        }

        public static IEnumerable<int> IndexOfLocalMinima<T>(this IEnumerable<T> source) where T : IComparable
        {
            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Value = x
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) < 0)
            {
                yield return front[0].Index;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) < 0) && (x[1].Value.CompareTo(x[2].Value) < 0))
                {
                    yield return x[1].Index;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) < 0)
            {
                yield return back[2].Index;
            }
        }

        public static IEnumerable<int> IndexOfLocalMaxima<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> comparison) where TCompare : IComparable
        {
            return source.Select(comparison).IndexOfLocalMaxima();
        }

        public static IEnumerable<int> IndexOfLocalMaxima<T>(this IEnumerable<T> source) where T : IComparable
        {
            var windows = source
                .Select((x, i) => new
                {
                    Index = i,
                    Value = x
                })
                .Window(3)
                .Select(x => x.ToList());

            var front = windows.First();
            if (front[0].Value.CompareTo(front[1].Value) > 0)
            {
                yield return front[0].Index;
            }

            var back = front;
            foreach (var x in windows)
            {
                back = x;
                if ((x[1].Value.CompareTo(x[0].Value) > 0) && (x[1].Value.CompareTo(x[2].Value) > 0))
                {
                    yield return x[1].Index;
                }
            }

            if (back[2].Value.CompareTo(back[1].Value) > 0)
            {
                yield return back[2].Index;
            }
        }
    }
}