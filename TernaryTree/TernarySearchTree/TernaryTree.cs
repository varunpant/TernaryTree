using System;
using System.Collections.Generic;


namespace TernarySearchTree
{
    // public class TernaryTree
    public class TernaryTree<T>
    {
        private int N;       // size
        private Node root; // root of trie
        private class Node
        {
            internal char c; // character
            internal Node left, mid, right; // left, middle, and right subtries
            internal T val; // value associated with string
        }
        // return number of key-value pairs

        public int Size
        {
            get
            {
                return N;
            }
        }

        public bool contains(string key)
        {
            return get(key) != null;
        }

        public T get(string key)
        {
            Node x = get(root, key, 0);
            if (IsNull(x)) return default(T);
            return x.val;
        }

        private Node get(Node x, string key, int d)
        {
            if (x == null) return null;
            char c = key[d];
            if (c < x.c) return get(x.left, key, d);
            else if (c > x.c) return get(x.right, key, d);
            else if (d < key.Length - 1)
                return get(x.mid, key, d + 1);
            else return x;
        }
        public void put(string key, T val)
        {
            if (!contains(key)) N++;
            root = put(root, key, val, 0);
        }
        private Node put(Node x, string key, T val, int d)
        {
            char c = key[d];
            if (x == null) { x = new Node(); x.c = c; }
            if (c < x.c) x.left = put(x.left, key, val, d);
            else if (c > x.c) x.right = put(x.right, key, val, d);
            else if (d < key.Length - 1)
                x.mid = put(x.mid, key, val, d + 1);
            else x.val = val;
            return x;
        }


        public string longestPrefixOf(string s)
        {
            if (s == null || s.Length == 0) return null;
            int length = 0;
            Node x = root;
            int i = 0;
            while (x != null && i < s.Length)
            {
                char c = s[i];
                if (c < x.c) x = x.left;
                else if (c > x.c) x = x.right;
                else
                {
                    i++;
                    if (x.val != null) length = i;
                    x = x.mid;
                }
            }
            return s.Substring(0, length);
        }

        // all keys in symbol table
        public IEnumerable<string> keys()
        {
            Queue<string> queue = new Queue<string>();
            collect(root, "", queue);
            return queue;
        }

        // all keys starting with given prefix
        public IEnumerable<string> prefixMatch(string prefix)
        {
            Queue<string> queue = new Queue<string>();
            Node x = get(root, prefix, 0);
            if (x == null) return queue;
            if (x.val != null) queue.Enqueue(prefix);
            collect(x.mid, prefix, queue);
            return queue;
        }

        // all keys in subtrie rooted at x with given prefix
        private void collect(Node x, string prefix, Queue<string> queue)
        {
            if (x == null) return;
            collect(x.left, prefix, queue);
            if (x.val != null) queue.Enqueue(prefix + x.c);
            collect(x.mid, prefix + x.c, queue);
            collect(x.right, prefix, queue);
        }

        // return all keys matching given wilcard pattern
        public IEnumerable<string> wildcardMatch(string pat)
        {
            Queue<string> queue = new Queue<string>();
            collect(root, "", 0, pat, queue);
            return queue;
        }

        private void collect(Node x, string prefix, int i, string pat, Queue<string> q)
        {
            if (x == null) return;
            char c = pat[i];
            if (c == '.' || c < x.c) collect(x.left, prefix, i, pat, q);
            if (c == '.' || c == x.c)
            {
                if (i == pat.Length - 1 && x.val != null) q.Enqueue(prefix + x.c);
                if (i < pat.Length - 1) collect(x.mid, prefix + x.c, i + 1, pat, q);
            }
            if (c == '.' || c > x.c) collect(x.right, prefix, i, pat, q);
        }


        // all vals of keys starting with given prefix
        public IEnumerable<T> search(string prefix)
        {
            Queue<T> queue = new Queue<T>();
            Node x = get(root, prefix, 0);
            if (x == null) return queue;
            if (!IsNull(x.val)) queue.Enqueue(x.val);
            collect(x.mid, prefix, queue);
            return queue;
        }

        // all vals of keys in subtrie rooted at x with given prefix
        private void collect(Node x, string prefix, Queue<T> queue)
        {
            if (x == null) return;
            collect(x.left, prefix, queue);
            if (!IsNull(x.val)) queue.Enqueue(x.val);
            collect(x.mid, prefix + x.c, queue);
            collect(x.right, prefix, queue);
        }

        public static bool IsNull<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

    }
}
