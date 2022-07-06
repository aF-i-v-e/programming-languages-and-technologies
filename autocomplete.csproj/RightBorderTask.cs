using System;
using System.Collections.Generic;

namespace Autocomplete
{
	public class RightBorderTask
	{
		public static int FoundRightBorder(IReadOnlyList<string> phrases, string prefix, int left, int right)
		{
			while (right - 1 != left)
			{
				var mid = (left + right) / 2;
				if (string.Compare(phrases[mid], prefix, StringComparison.OrdinalIgnoreCase) >= 0
					&& !phrases[mid].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
					right = mid;
				else
					left = mid;
			}
			return right;
		}

		public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
		{
			if (phrases.Count > 0
				&& phrases[phrases.Count - 1].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				return phrases.Count;
			return FoundRightBorder(phrases, prefix, left, right);
		}
	}
}
