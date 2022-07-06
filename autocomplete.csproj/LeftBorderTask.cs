// Вставьте сюда финальное содержимое файла LeftBorderTask.cs
using System.Collections.Generic;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int BinarSearch(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (right - 1 == left) return left;
            var mid = (left + right) / 2;
            if (string.Compare(phrases[mid], prefix) >= 0)
                return BinarSearch(phrases, prefix, left, mid);
            else
                return BinarSearch(phrases, prefix, mid, right);
        }

        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            return BinarSearch(phrases, prefix, left, right);
        }
    }
}

