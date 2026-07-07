using System.Linq;
using System.Reflection;
using HarmonyLib;
using ngov3;

namespace Tweets.Patches;

public static class Patcher
{
	private static Harmony _harmony;

	internal static Harmony HarmonyRef => _harmony;

	internal static void Patch()
	{
		_harmony = new Harmony(PluginGuid);
		PatchImageHandling();
		
		GameSaveHooks.PatchSaves();
	}

	private static void PatchImageHandling()
	{
		PatchPrefix(_harmony, AccessTools.Method(typeof(LoadPictures), nameof(LoadPictures.LoadPictureAsync)),
			AccessTools.Method(typeof(LoadPicturePatch), nameof(LoadPicturePatch.Prefix)));
		// PatchPrefix(harmony, AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetData)), AccessTools.Method(typeof(PoketterCellPatch), nameof(PoketterCellPatch.Prefix)));
		PatchPostfix(_harmony, AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetDataStatic)),
			AccessTools.Method(typeof(PoketterCellPatch), nameof(PoketterCellPatch.Postfix)));
		
		
		Plugin.Instance.Invoke(nameof(DelayedCheck), 3f);
	}

	private static void PatchPrefix(Harmony harmony, MethodInfo target, MethodInfo patchMethod)
	{
		HarmonyMethod prefix = new HarmonyMethod(patchMethod);

		Log($"Does target exist: {target != null}, does prefix exist: {prefix != null} ({patchMethod != null})");

		MethodInfo patch = harmony.Patch(target, prefix);

		if (patch != null)
		{
			Log($"Patch to {target.Name} applied successfully!");
		}
		else
		{
			LogError($"Patch to {target.Name} failed", true);
		}
	}

	private static void PatchPostfix(Harmony harmony, MethodInfo target, MethodInfo patchMethod)
	{
		HarmonyMethod postfix = new HarmonyMethod(patchMethod);

		Log($"Does target exist: {target != null}, does postfix exist: {postfix != null} ({patchMethod != null})");

		MethodInfo patch = harmony.Patch(target, postfix: postfix);

		if (patch != null)
		{
			Log($"Patch to {target.Name} applied successfully!");
		}
		else
		{
			LogError($"Patch to {target.Name} failed", true);
		}
	}

	private static void DelayedCheck()
	{
		CheckPatch(AccessTools.Method(typeof(LoadPictures), nameof(LoadPictures.LoadPictureAsync)));
		CheckPatch(AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetData)));
	}

	private static void CheckPatch(MethodInfo target)
	{
		var original = target;

		string originalName = original != null ? original.Name : null;

		Log($"Does original ({originalName}) exist: {original != null}");

		var patchInfo = Harmony.GetPatchInfo(original);

		Log($"Does patchInfo exist: {patchInfo != null}");

		if (patchInfo != null)
		{
			bool isMyPrefixApplied = patchInfo.Prefixes.Any(p => p.owner == PluginGuid);

			Log($"IsApplied {isMyPrefixApplied}");
		}
	}
}