using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace ToolBox.Extensions
{
	internal static class MonoBehaviourExtensions
	{
		private static readonly LinkedList<Action> _queuedActions = new LinkedList<Action>();
		private static Coroutine _queueingCoroutine;

		public static Coroutine ExecuteDelayed(this MonoBehaviour self, Action action, int frameCount = 1)
		{
			return self.StartCoroutine(ExecuteDelayed_Routine(action, frameCount));
		}

		public static Coroutine ExecuteDelayed2(this MonoBehaviour self, Action action, int frameCount = 1)
		{
			return self.StartCoroutine(ExecuteDelayed_Routine(action, frameCount));
		}

		private static IEnumerator ExecuteDelayed_Routine(Action action, int frameCount = 1)
		{
			for (int i = 0; i < frameCount; ++i)
				yield return null;
			action();
		}

		public static Coroutine ExecuteDelayed(
			this MonoBehaviour self,
			Action action,
			float delay,
			bool timeScaled = true)
		{
			return self.StartCoroutine(ExecuteDelayed_Routine(action, delay, timeScaled));
		}

		private static IEnumerator ExecuteDelayed_Routine(Action action, float delay, bool timeScaled)
		{
			if (timeScaled)
				yield return new WaitForSeconds(delay);
			else
				yield return new WaitForSecondsRealtime(delay);
			action();
		}

		public static Coroutine ExecuteDelayedFixed(
			this MonoBehaviour self,
			Action action,
			int waitCount = 1)
		{
			return self.StartCoroutine(ExecuteDelayedFixed_Routine(action, waitCount));
		}

		private static IEnumerator ExecuteDelayedFixed_Routine(Action action, int waitCount)
		{
			for (int i = 0; i < waitCount; ++i)
				yield return new WaitForFixedUpdate();
			action();
		}

		public static Coroutine ExecuteDelayed(
			this MonoBehaviour self,
			Func<bool> waitUntil,
			Action action)
		{
			return self.StartCoroutine(ExecuteDelayed_Routine(waitUntil, action));
		}

		private static IEnumerator ExecuteDelayed_Routine(Func<bool> waitUntil, Action action)
		{
			yield return new WaitUntil(waitUntil);
			action();
		}

		public static void QueueAction(this MonoBehaviour self, Action action)
		{
			_queuedActions.AddFirst(action);
			if (_queueingCoroutine != null)
				return;
			_queueingCoroutine = self.StartCoroutine(QueueAction_Routine());
		}

		private static IEnumerator QueueAction_Routine()
		{
			while (_queuedActions.Count != 0)
			{
				yield return null;
				try
				{
					Action action = _queuedActions.Last.Value;
					if (action != null)
						action();
					_queuedActions.RemoveLast();
				}
				catch (Exception ex)
				{
					Debug.LogError("Queued action:\n" + ex);
				}
			}

			_queueingCoroutine = null;
		}
	}
}