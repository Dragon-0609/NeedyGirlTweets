using System.Linq;
using System.Reflection;
using HarmonyLib;
using Messages.Utils;
using ngov3;

namespace Messages.Patches;

public class Patcher
{
	public static Patcher Instance;
	
	public Patcher()
	{
		Instance = this;
		Patch();
	}
	
	private static void Patch()
	{
		Harmony harmony = new Harmony(PluginGuid);
		PatchPrefix(harmony, AccessTools.Method(typeof(LoadPictures), nameof(LoadPictures.LoadPictureAsync)),
			AccessTools.Method(typeof(LoadPicturePatch), nameof(LoadPicturePatch.Prefix)));
		// PatchPrefix(harmony, AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetData)), AccessTools.Method(typeof(PoketterCellPatch), nameof(PoketterCellPatch.Prefix)));
		PatchPostfix(harmony, AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetDataStatic)),
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

	private void DelayedCheck()
	{
		// 1. Get the original method you targeted
		CheckPatch(AccessTools.Method(typeof(LoadPictures), nameof(LoadPictures.LoadPictureAsync)));
		CheckPatch(AccessTools.Method(typeof(PoketterCell2D), nameof(PoketterCell2D.SetData)));
	}

	private static void CheckPatch(MethodInfo target)
	{
		var original = target;

		string originalName = original != null ? original.Name : null;

		Log($"Does original ({originalName}) exist: {original != null}");

// 2. Get all patches applied to that method
		var patchInfo = Harmony.GetPatchInfo(original);

		Log($"Does patchInfo exist: {patchInfo != null}");

// 3. Check if your specific owner ID (your mod's ID) is in the Prefixes list
		if (patchInfo != null)
		{
			bool isMyPrefixApplied = patchInfo.Prefixes.Any(p => p.owner == PluginGuid);

			Log($"IsApplied {isMyPrefixApplied}");
		}
	}
}