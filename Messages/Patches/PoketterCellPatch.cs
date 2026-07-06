using System.Collections;
using Cysharp.Threading.Tasks;
using ngov3;

namespace Messages.Utils;

public static class PoketterCellPatch
{
	public static bool Prefix(PoketterCell2D __instance, TweetDrawable nakami, ref UniTask __result)
	{
		/*if (nakami.cmdType == CmdType.PlayMakeLove)
		{
			// nakami.
		}*/
		
		Log($"Setting data to {nakami.cmdType}, image: '{nakami.ImageId}', text: {nakami.BodyEn}");

		return true;
	}
	public static void Postfix(PoketterCell2D __instance, TweetDrawable nakami, ref UniTask __result)
	{
		bool exist = (bool) __instance.GetPrivate("_imageExist");

		if (exist && ResourceChanger.Instance.CustomImages.Contains(nakami.ImageId))
			Plugin.Instance.StartCoroutine(DelayedLayoutUpdate(__instance));
		
		// Log($"Does image exist {exist} in {nakami.ImageId} ({nakami.BodyEn})");
	}

	private static IEnumerator DelayedLayoutUpdate(PoketterCell2D poketterCell2D)
	{
		yield return null;
		poketterCell2D.OnLanguageUpdated();
	}
}