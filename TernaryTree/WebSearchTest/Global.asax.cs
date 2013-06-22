using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using TernarySearchTree;
using System.Text.RegularExpressions;
using System.IO;

namespace WebSearchTest
{
    public class Global : System.Web.HttpApplication
    {
        private static TernaryTreeExtended<int> tree = null;
        private static string[] lines = null;

        /// <summary>
        /// 
        /// </summary>
        public static TernaryTreeExtended<int> Index
        {
            get
            {
                if (tree == null)
                {
                    populateTree();
                }
                return tree;
            }
        }

        public static string[] Data
        {
            get
            {
                return lines;
            }
        }

        private static Regex s_wordRegex = new Regex(@"[\w']+");

        protected void Application_Start(object sender, EventArgs e)
        {
            populateTree();
        }


        static void populateTree()
        {
            tree = new TernaryTreeExtended<int>();
            string fileName = HttpContext.Current.Server.MapPath("~/App_Code/sa.txt");
            lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                var words = GetWords(line);
                foreach (var word in words)
                {
                    tree.Add(word.Trim(), i);
                }

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

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}