using UnityEngine;

namespace Utilities {
	public static class MonoBehaviourExtensions {
		public static void EnsureCoroutineStopped(this MonoBehaviour value, ref Coroutine routine) {
			if (routine == null) return;
			value.StopCoroutine(routine);
			routine = null;
		}
	}
}