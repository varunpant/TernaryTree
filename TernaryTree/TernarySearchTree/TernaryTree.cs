using System;
using System.Collections.Generic;

namespace TernarySearchTree
{

    /// <summary>
    /// Creates an instance of terenary search tree.
    /// </summary>
    /// <typeparam name="T">Value</typeparam>
    public class TernaryTree<T>
    {
        /// <summary>
        /// The size of tree.
        /// </summary>
        int N;

        // <summary>
        /// The root
        /// </summary>
        Node root;

        /// <summary>
        /// Node instance.
        /// </summary>
        class Node
        {
            /// <summary>
            /// character
            /// </summary>
            internal char c;

            /// <summary>
            /// The  left, middle, and right subtries.
            /// </summary>
            internal Node left, mid, right;

            /// <summary>
            /// The value associated with string
            /// </summary>
            internal T value;
        }

        /// <summary>
        /// Gets the number of keys in tree.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Length
        {
            get
            {
                return N;
            }
        }

        /// <summary>
        /// Determines whether the tree [contains] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the tree [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            Node node = get(root, key, 0);
            if (IsNull(node)) return false;
            return true;
        }

        /// <summary>
        /// Gets the <see cref="`0"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="`0"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[string key]
        {
            get
            {
                Node node = get(root, key, 0);
                if (IsNull(node)) return default(T);
                return node.value;
            }

        }

        /// <summary>
        /// Gets the specified x.
        /// </summary>
        /// <param name="node">The x.</param>
        /// <param name="key">The key.</param>
        /// <param name="charIndex">The d.</param>
        /// <returns></returns>
        Node get(Node node, string key, int charIndex)
        {
            if (node == null) return null;
            char c = key[charIndex];
            if (c < node.c) return get(node.left, key, charIndex);
            else if (c > node.c) return get(node.right, key, charIndex);
            else if (charIndex < key.Length - 1)
                return get(node.mid, key, charIndex + 1);
            else return node;
        }
        
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, T value)
        {
            if (!Contains(key)) N++;
            root = Add(root, key, value, 0);
        }
        
        /// <summary>
        /// Adds the specified node in the tree.
        /// </summary>
        /// <param name="node">The Node.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The val.</param>
        /// <param name="charIndex">The d.</param>
        /// <returns></returns>
        Node Add(Node node, string key, T value, int charIndex)
        {
            char charAtIndex = key[charIndex];
            if (node == null) { node = new Node(); node.c = charAtIndex; }
            if (charAtIndex < node.c) node.left = Add(node.left, key, value, charIndex);
            else if (charAtIndex > node.c) node.right = Add(node.right, key, value, charIndex);
            else if (charIndex < key.Length - 1)
                node.mid = Add(node.mid, key, value, charIndex + 1);
            else node.value = value;
            return node;
        }

        /// <summary>
        /// Returns Longests prefix of query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public string longestPrefixOf(string query)
        {
            if (query == null || query.Length == 0) return null;
            int length = 0;
            Node x = root;
            int i = 0;
            while (x != null && i < query.Length)
            {
                char c = query[i];
                if (c < x.c) x = x.left;
                else if (c > x.c) x = x.right;
                else
                {
                    i++;
                    if (x.value != null) length = i;
                    x = x.mid;
                }
            }
            return query.Substring(0, length);
        }
        
        /// <summary>
        /// Returns all keys in symbol table.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Keys()
        {
            Queue<string> queue = new Queue<string>();
            collect(root, "", queue);
            return queue;
        }
        
        /// <summary>
        /// Returns all keys starting with a given prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public IEnumerable<string> PrefixMatch(string prefix)
        {
            Queue<string> queue = new Queue<string>();
            Node x = get(root, prefix, 0);
            if (x == null) return queue;
            if (x.value != null) queue.Enqueue(prefix);
            collect(x.mid, prefix, queue);
            return queue;
        }
        
        /// <summary>
        /// Collects all keys in subtrie rooted at x with given prefix.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="queue">The queue.</param>
        void collect(Node x, string prefix, Queue<string> queue)
        {
            if (x == null) return;
            collect(x.left, prefix, queue);
            if (x.value != null) queue.Enqueue(prefix + x.c);
            collect(x.mid, prefix + x.c, queue);
            collect(x.right, prefix, queue);
        }
        
        /// <summary>
        /// Returns all keys matching given wilcard pattern.
        /// </summary>
        /// <param name="pat">The pat.</param>
        /// <returns></returns>
        public IEnumerable<string> wildcardMatch(string pat)
        {
            Queue<string> queue = new Queue<string>();
            collect(root, "", 0, pat, queue);
            return queue;
        }

        /// <summary>
        /// Collects all nodes for the specified prefix pattern.
        /// </summary>
        /// <param name="node">The Node.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="charIndex">The index of char.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="query">The query.</param>
        void collect(Node node, string prefix, int charIndex, string pattern, Queue<string> query)
        {
            if (node == null) return;
            char charAtIndex = pattern[charIndex];
            if (charAtIndex == '.' || charAtIndex < node.c) collect(node.left, prefix, charIndex, pattern, query);
            if (charAtIndex == '.' || charAtIndex == node.c)
            {
                if (charIndex == pattern.Length - 1 && node.value != null) query.Enqueue(prefix + node.c);
                if (charIndex < pattern.Length - 1) collect(node.mid, prefix + node.c, charIndex + 1, pattern, query);
            }
            if (charAtIndex == '.' || charAtIndex > node.c) collect(node.right, prefix, charIndex, pattern, query);
        }
        
        /// <summary>
        /// Searches  all vals of keys starting with given prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public IEnumerable<T> Search(string prefix)
        {
            Queue<T> queue = new Queue<T>();
            Node node = get(root, prefix, 0);
            if (node == null) return queue;
            if (!IsNull(node.value)) queue.Enqueue(node.value);
            collect(node.mid, prefix, queue);
            return queue;
        }
        
        /// <summary>
        /// Collects all vals of keys in subtrie rooted at x with given prefix.
        /// </summary>
        /// <param name="node">The x.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="queue">The queue.</param>
        void collect(Node node, string prefix, Queue<T> queue)
        {
            if (node == null) return;
            collect(node.left, prefix, queue);
            if (!IsNull(node.value)) queue.Enqueue(node.value);
            collect(node.mid, prefix + node.c, queue);
            collect(node.right, prefix, queue);
        }

        /// <summary>
        /// Determines whether the specified value is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is null; otherwise, <c>false</c>.
        /// </returns>
        static bool IsNull<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

    }
}
