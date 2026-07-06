using System;
using System.Collections;

// ReSharper disable CheckNamespace

namespace ToolBox.Extensions
{
	/// <summary>
	/// Suspends a coroutine until the supplied predicate returns <c>true</c>.
	/// Re-implements <c>UnityEngine.WaitUntil</c>, which may be absent from the
	/// game's managed-stripped <c>UnityEngine.CoreModule.dll</c>. Because it lives
	/// in the same namespace it takes precedence over the engine type when yielded.
	/// </summary>
	internal sealed class WaitUntil : IEnumerator
	{
		private readonly Func<bool> _predicate;

		public WaitUntil(Func<bool> predicate)
		{
			_predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		// null means "wait a single frame" between polls.
		public object Current => null;

		// Keep advancing (waiting) while the predicate is not yet satisfied.
		public bool MoveNext() => !_predicate();

		public void Reset()
		{
		}
	}
}