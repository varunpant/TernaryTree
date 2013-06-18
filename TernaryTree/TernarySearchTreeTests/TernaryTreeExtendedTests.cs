using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TernarySearchTree;

namespace TernarySearchTreeTests
{
    [TestClass]
    public class TernaryTreeExtendedTests
    {
        [TestMethod]
        public void TestCreateTerenaryTree()
        {
            TernaryTree<string> tree = new TernaryTree<string>();
            var InputKeys = "aback abacus abalone abandon abase abash abate abbas abbe abbey abbot Abbott".ToLower().Split(' ');
            foreach (var key in InputKeys)
            {
                tree.Add(key, "value of " + key);
            }

            Assert.IsNotNull(tree);
            Assert.AreEqual(InputKeys.Length, tree.Length);

            IEnumerable<string> Keys = tree.Keys;
            Assert.IsNotNull(Keys);

            int count = getCount(Keys);
            Assert.AreEqual(InputKeys.Length, count);


        }

        [TestMethod]
        public void TestAddingDuplicateKeysInTree()
        {
            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();

            tree.Add("Key1", "value1");
            Assert.AreEqual(1, tree.Length);
            Assert.AreEqual("value1", tree["Key1"][0]);

            tree.Add("Key1", "value2");
            Assert.AreEqual(1, tree.Length);
            Assert.AreEqual("value2", tree["Key1"][1]);

        }

        [TestMethod]
        public void TestAddingNullValuesInTree()
        {
            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();
            tree.Add("Key1", null);
            Assert.AreEqual(1, tree.Length);
            Assert.IsNull(tree["Key1"][0]);
            Assert.IsTrue(tree.Contains("Key1"));

        }

        [TestMethod]
        public void TestAddingNullKeysInTree()
        {
            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();
            try
            {
                tree.Add(null, null);
                Assert.Fail("null key allowed.");

            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("Keys cannot be null or empty.", e.Message);
            }

            try
            {
                tree.Add("", null);
                Assert.Fail("empty key allowed.");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("Keys cannot be null or empty.", e.Message);
            }

            Assert.AreEqual(0, tree.Length);

            tree.Add(" ", "white space");
            Assert.AreEqual(1, tree.Length);
            tree.Add(" a ", "another key with space");
            Assert.AreEqual("white space", tree[" "][0]);

        }

        [TestMethod]
        public void TestTreePrefixSearch()
        {
            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();
            var InputKeys = "aback abacus abalone abandon abase abash abate abbas abbe abbey abbot Abbott".ToLower().Split(' ');
            foreach (var key in InputKeys)
            {
                tree.Add(key, "value of " + key);
            }

            {
                var matches = tree.PrefixMatch("ab");
                int count = getCount(matches);
                Assert.AreEqual(InputKeys.Length, count);
            }

            {
                var matches = tree.PrefixMatch("aba");
                int count = getCount(matches);
                Assert.AreEqual(7, count);
            }

            {
                var matches = tree.PrefixMatch("abb");
                int count = getCount(matches);
                Assert.AreEqual(5, count);
            }

            {
                var matches = tree.PrefixMatch("XXX");
                int count = getCount(matches);
                Assert.AreEqual(0, count);
            }
        }

        [TestMethod]
        public void TestTreeWildCardSearch()
        {
            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();
            var InputKeys = "aback abacus abalone abandon abase abash abate abbas abbe abbey abbot Abbott".ToLower().Split(' ');
            foreach (var key in InputKeys)
            {
                tree.Add(key, "value of " + key);
            }

            {
                var matches = tree.WildcardMatch("...cus");
                int count = getCount(matches);
                Assert.AreEqual(1, count);
            }

            {
                var matches = tree.WildcardMatch("....y");
                int count = getCount(matches);
                Assert.AreEqual(1, count);
            }

            {
                var matches = tree.WildcardMatch("a.b.t.");
                int count = getCount(matches);
                Assert.AreEqual(1, count);
            }

            {
                var matches = tree.WildcardMatch("aba....");
                int count = getCount(matches);
                Assert.AreEqual(2, count);
            }

            {
                var matches = tree.WildcardMatch("..a..");
                int count = getCount(matches);
                Assert.AreEqual(4, count);
            }

        }

        [TestMethod]
        public void TestTreePrefixSearchForValues()
        {

            TernaryTreeExtended<string> tree = new TernaryTreeExtended<string>();
            var InputKeys = "aback abacus abalone abandon abase abash abate abbas abbe abbey abbot Abbott".ToLower().Split(' ');
            foreach (var key in InputKeys)
            {
                tree.Add(key, "value of " + key);
            }

            {
                var matches = tree.Search("ab");
                int count = getCount(matches);
                Assert.AreEqual(12, count);
                int i = 0;
                foreach (var val in matches)
                {
                    Assert.AreEqual("value of " + InputKeys[i++], val);
                }
            }
        }

        private static int getCount(IEnumerable<string> Keys)
        {
            IEnumerator iterator = Keys.GetEnumerator();
            int count = 0;
            try
            {

                while (iterator.MoveNext())
                {
                    count++;
                }
                return count;
            }
            finally
            {
                IDisposable disposable = iterator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

            }

        }
    }
}
