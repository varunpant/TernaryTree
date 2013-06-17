using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TernarySearchTree
{
    class Program
    {

        private static Regex s_wordRegex = new Regex(@"[\w']+");
        private static long GC_MemoryStart, GC_MemoryEnd;
        static void Main(string[] args)
        {
            GC_MemoryStart = System.GC.GetTotalMemory(true);
            TernaryTree2<int> tree = new TernaryTree2<int>();

            var lines = File.ReadAllLines("../../sa.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                // var words = StemWords(GetWords(line));
                var words = GetWords(line);
                foreach (var word in words)
                {
                    tree.put(word.Trim(), i);
                }

            }
            Console.WriteLine(tree.KeySize);
            GC_MemoryEnd = System.GC.GetTotalMemory(true);

            Console.WriteLine("========================");
            Console.WriteLine(ConvertBytesToMegabytes(GC_MemoryEnd - GC_MemoryStart) + " mb in use");
            Console.WriteLine("========================");


            string input;
            while ((input = Console.ReadLine()) != "xx")
            {
                Console.WriteLine("========================");
                IEnumerable<int> results = tree.search(input.ToLower());
                // IEnumerable<string> results = tree.prefixMatch(line);
                // IEnumerable<string> results = tree.wildcardMatch(line);
                foreach (var r in results)
                {
                    Console.WriteLine(lines[r]);
                }
                               
                Console.WriteLine("========================");
            }

        }

        public static string[] GetWords(params string[] list)
        {
            List<string> words = new List<string>();
            if (list != null)
            {
                foreach (string item in list)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string input = item.ToLower();
                        int idx = 0;

                        while (true)
                        {
                            Match match = s_wordRegex.Match(input, idx);
                            if (!match.Success)
                            {
                                break;
                            }

                            string word = input.Substring(match.Index, match.Length);
                            if (!words.Contains(word))
                            {
                                words.Add(word);
                            }

                            idx = match.Index + match.Length;
                        }
                    }
                }
            }

            return words.ToArray();
        }

        public static string[] StemWords(string[] words)
        {
            if (words == null)
            {
                return null;
            }

            List<string> stemmedWords = new List<string>(words.Length);
            Stemmer stemmer = new Stemmer();

            foreach (string word in words)
            {
                string stemmedWord = stemmer.Stem(word);
                if (!stemmedWords.Contains(stemmedWord))
                {
                    stemmedWords.Add(stemmedWord);
                }
            }

            return stemmedWords.ToArray();
        }


        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }




    }
}
