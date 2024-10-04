using System.Collections.Generic;

namespace Alkawa
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this Queue<T> _queue, IEnumerable<T> _elements)
        {
            foreach (var element in _elements)
            {
                _queue.Enqueue(element);
            }
        }
        
        public static void AddRange<T>(this HashSet<T> _hashSet, List<T> _elements)
        {
            foreach (var element in _elements)
            {
                _hashSet.Add(element);
            }
        }
        
        public static void AddRange<T>(this HashSet<T> _hashSet, HashSet<T> _elements)
        {
            foreach (var element in _elements)
            {
                _hashSet.Add(element);
            }
        }
        
        public static void AddRange<T>(this HashSet<T> _hashSet, IEnumerable<T> _elements)
        {
            foreach (var element in _elements)
            {
                _hashSet.Add(element);
            }
        }

        public static bool IsNullOrEmpty<T>(this HashSet<T> _this)
        {
            return _this == null || _this.Count == 0;
        }

    }
}