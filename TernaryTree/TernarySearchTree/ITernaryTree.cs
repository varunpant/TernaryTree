using System;
namespace TernarySearchTree
{
    interface ITernaryTree<T>
    {
        void Add(string key, T value);
        bool Contains(string key);
        System.Collections.Generic.IEnumerable<string> Keys { get; }
        int Length { get; }
        System.Collections.Generic.IEnumerable<T> NearSearch(string query, int distance);
        System.Collections.Generic.IEnumerable<string> PrefixMatch(string prefix);
        System.Collections.Generic.IEnumerable<T> Search(string prefix);
        T this[string key] { get; }
        System.Collections.Generic.IEnumerable<string> WildcardMatch(string pat);
    }
}
