using System;
using System.Collections.Generic;

namespace TernarySearchTree
{

    /// <summary>
    /// Creates an instance of terenary search tree.
    /// </summary>
    /// <typeparam name="T">Value</typeparam>
    public class TernaryTree<T> : TernarySearchTree.ITernaryTree<T>
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
            /// The value associated .
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
            Node node = Get(root, key, 0);
            if (node == null) return false;
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
                Node node = Get(root, key, 0);
                if (node == null) return default(T);
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
        Node Get(Node node, string key, int charIndex)
        {
            if (node == null) return null;
            char c = key[charIndex];
            if (c < node.c) return Get(node.left, key, charIndex);
            else if (c > node.c) return Get(node.right, key, charIndex);
            else if (charIndex < key.Length - 1)
                return Get(node.mid, key, charIndex + 1);
            else return node;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, T value)
        {
            if (string.IsNullOrEmpty(key)) { throw new InvalidOperationException("Keys cannot be null or empty."); }
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
        /// Returns all keys in tree.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public IEnumerable<string> Keys
        {
            get
            {
                Queue<string> queue = new Queue<string>();
                Collect(root, "", queue);
                return queue;
            }
        }

        /// <summary>
        /// Returns all keys starting with a given prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public IEnumerable<string> PrefixMatch(string prefix)
        {
            Queue<string> queue = new Queue<string>();
            Node node = Get(root, prefix, 0);
            if (node == null) return queue;
            if (node.value != null) queue.Enqueue(prefix);
            Collect(node.mid, prefix, queue);
            return queue;
        }

        /// <summary>
        /// Collects all keys in subtrie rooted at x with given prefix.
        /// </summary>
        /// <param name="node">The x.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="queue">The queue.</param>
        void Collect(Node node, string prefix, Queue<string> queue)
        {
            if (node == null) return;
            Collect(node.left, prefix, queue);
            if (node.value != null) queue.Enqueue(prefix + node.c);
            Collect(node.mid, prefix + node.c, queue);
            Collect(node.right, prefix, queue);
        }

        /// <summary>
        /// Returns all keys matching given wilcard pattern.
        /// </summary>
        /// <param name="pat">The pat.</param>
        /// <returns></returns>
        public IEnumerable<string> WildcardMatch(string pat)
        {
            Queue<string> queue = new Queue<string>();
            Collect(root, "", 0, pat, queue);
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
        void Collect(Node node, string prefix, int charIndex, string pattern, Queue<string> query)
        {
            if (node == null) return;
            char charAtIndex = pattern[charIndex];
            if (charAtIndex == '.' || charAtIndex < node.c) Collect(node.left, prefix, charIndex, pattern, query);
            if (charAtIndex == '.' || charAtIndex == node.c)
            {
                if (charIndex == pattern.Length - 1 && node.value != null) query.Enqueue(prefix + node.c);
                if (charIndex < pattern.Length - 1) Collect(node.mid, prefix + node.c, charIndex + 1, pattern, query);
            }
            if (charAtIndex == '.' || charAtIndex > node.c) Collect(node.right, prefix, charIndex, pattern, query);
        }

        /// <summary>
        /// Searches  all vals of keys starting with given prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public IEnumerable<T> Search(string prefix)
        {
            Queue<T> queue = new Queue<T>();
            Node node = Get(root, prefix, 0);
            if (node == null) return queue;
            if (!IsNull(node.value)) queue.Enqueue(node.value);
            Collect(node.mid, prefix, queue);
            return queue;
        }

        /// <summary>
        /// Collects all values of keys in subtrie rooted at x with given prefix.
        /// </summary>
        /// <param name="node">The x.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="queue">The queue.</param>
        void Collect(Node node, string prefix, Queue<T> queue)
        {
            if (node == null) return;
            Collect(node.left, prefix, queue);
            if (!IsNull(node.value)) queue.Enqueue(node.value);
            Collect(node.mid, prefix + node.c, queue);
            Collect(node.right, prefix, queue);
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
        
        /// <summary>
        /// Returns all values for keys in the dictionary that are within a given Hamming distance of a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="distance">Hamming distance.</param>
        /// <returns></returns>
        public IEnumerable<T> NearSearch(string query, int distance)
        {
            Queue<T> queue = new Queue<T>();
            if (!string.IsNullOrWhiteSpace(query) && distance > 0)
                Collect(query, root, queue, distance);
            return queue;
        }

        /// <summary>
        /// Collects all values of keys which are within a given Hamming Distance.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="node">The node.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="d">The d.</param>
        void Collect(string query, Node node, Queue<T> queue, int d)
        {
            if (node == null) return;
            char c = query[0];
            if (d > 0 || c < node.c) { Collect(query, node.left, queue, d); }
            if (!IsNull(node.value))
            {
                if (query.Length <= d)
                {
                    queue.Enqueue(node.value);
                }
            }
            else
            {
                Collect(query.Length > 1 ? query.Substring(1) : query, node.mid, queue, c == node.c ? d : d - 1);
            }
            if (d > 0 || c > node.c) { Collect(query, node.right, queue, d); }

        }
    }
}
