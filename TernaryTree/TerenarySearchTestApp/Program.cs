using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TernarySearchTree;
using System.IO;

namespace TerenarySearchTestApp
{
    class Program
    {
        private static Regex s_wordRegex = new Regex(@"[\w']+");
        private static long GC_MemoryStart, GC_MemoryEnd;



        static void Main(string[] args)
        {
            GC_MemoryStart = System.GC.GetTotalMemory(true);
            TernaryTreeExtended<int> tree = new TernaryTreeExtended<int>();

            var lines = File.ReadAllLines("../../resources/sa.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
             
                var words = GetWords(line);
                foreach (var word in words)
                {
                    tree.Add(word.Trim(), i);
                }

            }
         //   Console.WriteLine(tree.Key);
            GC_MemoryEnd = System.GC.GetTotalMemory(true);

            Console.WriteLine("========================");
            Console.WriteLine(ConvertBytesToMegabytes(GC_MemoryEnd - GC_MemoryStart) + " mb in use");
            Console.WriteLine("========================");


            string input;
            while ((input = Console.ReadLine()) != "xx")
            {
                Console.WriteLine("========================");
                IEnumerable<int> results = tree.Search(input.ToLower());
               
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


        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

    }
}
