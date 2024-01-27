using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
	public static class RandomExtensions {
		public static void Shuffle<T>(this List<T> array) {
			int n = array.Count;
			while (n > 1) {
				int k = Random.Range(0, n--);
				(array[n], array[k]) = (array[k], array[n]);
			}
		}
	}
}