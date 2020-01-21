using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Garbage
{
	internal class ArrayListDemo
	{
		internal static void Run()
		{
			ArrayList firstList = new ArrayList { "100", 1, 3, 23.4, 5, '9' };
			ArrayList secondList = new ArrayList { 5, 3, '9', 6, 4, 4, 23.4, "100", 1000 };
			ArrayList intersection = new ArrayList(firstList
					.OfType<object>()
					.Intersect(secondList.OfType<object>())
				.ToArray());
		}
	}

	internal static class ArrayListExtensions
	{
		internal static IEnumerable<object> ToEnumerable(this ArrayList arrayList)
		{
			foreach (var item in arrayList)
				yield return item;
		}
	}
}