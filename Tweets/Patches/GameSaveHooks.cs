using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using ngov3;
using Tweets.Data;

namespace Tweets.Patches;

public static class GameSaveHooks
{

	public static void PatchSaves()
	{
		MethodInfo loadMethod = AccessTools.Method(typeof(EventManager), nameof(EventManager.Load));
		HarmonyMethod loadPatch = new HarmonyMethod(AccessTools.Method(typeof(GameSaveHooks), nameof(OnLoaded)));
		Patcher.HarmonyRef.Patch(loadMethod, postfix: loadPatch);

		HarmonyMethod savePatch = new HarmonyMethod(AccessTools.Method(typeof(GameSaveHooks), nameof(OnSaved)));
		Patcher.HarmonyRef.Patch(AccessTools.Method(typeof(SaveRelayer), nameof(SaveRelayer.SaveSlotData)),
			postfix: savePatch);
		
		HarmonyMethod startOverPatch = new HarmonyMethod(AccessTools.Method(typeof(GameSaveHooks), nameof(OnStartedOver)));
		Patcher.HarmonyRef.Patch(AccessTools.Method(typeof(EventManager), nameof(EventManager.StartOver)),
			postfix: startOverPatch);


	}

	private static void OnSaved(string fileName, SlotData slotData)
	{

		ES3.Save<List<DataVariant<string>>>("TweetVariants", ResourceChanger.Instance.GetChosenVariants(), fileName);

	}

	private static void OnLoaded()
	{
		
		string fileName = SingletonMonoBehaviour<Settings>.Instance.nowSaveFile;

		List<DataVariant<string>> tweetVariants =
			ES3.Load<List<DataVariant<string>>>("TweetVariants", fileName, defaultValue: null);
		ResourceChanger.Instance.SetChosenVariants(tweetVariants);
	}

	private static void OnStartedOver()
	{
		ResourceChanger.Instance.ClearChosenVariants();
	}
	
}