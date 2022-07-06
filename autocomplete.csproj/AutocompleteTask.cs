using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];

            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var rightCount = GetCountByPrefix(phrases, prefix);
            if (rightCount < count)
                count = rightCount;
            var wordList = new List<string>();
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            for (int i = 0; i < count; i++)
            {
                if (phrases[leftBorder + 1].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    wordList.Add(phrases[leftBorder + 1]);
                leftBorder++;
            }
            return wordList.ToArray();
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            return (rightBorder - leftBorder) - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            StartTests2(new List<string>(), "bit", 1, new string[0]);
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            StartTests2(new List<string> { "e", "f", "g" }, "", 10, new string[] { "e", "f", "g" });
        }

        [Test]
        public void MyFirstTest()
        {
            StartTests2(new List<string> { "k", "l", "m", "n", "n", "o" }, "n", 2,
                        new string[] { "n", "n" });
        }

        public void MySecondTest()
        {
            StartTests2(new List<string> { "aac", "abc", "abca", "abcd", "abcdef", "abcdeg", "abcgg" }, "abcd",
                        10, new string[] { "abcd", "abcdef", "abcdeg" });
        }

        public void MyThirdTest()
        {
            StartTests2(new List<string> { "gh", "ghh", "ghhe", "ghhf" }, "gh", 10,
                        new string[] { "gh", "ghh", "ghhe", "ghhf" });
        }

        public void StartTests2(List<string> dictionary, string prefix, int count, string[] expectArray)
        {
            var ourArray = AutocompleteTask.GetTopByPrefix(dictionary, prefix, count);
            Assert.AreEqual(expectArray.Length, ourArray.Length);
            for (int i = 0; i < ourArray.Length; i++)
                Assert.AreEqual(expectArray[i], ourArray[i]);
        }
    }
}
