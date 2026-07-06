using HarmonyLib;

namespace Messages.Utils;

public static class HarmonyHelper
{
	public static HarmonyMethod GetHarmonyMethod<T>(this T classTarget, string methodName)
	{
		return new HarmonyMethod(classTarget.GetType().GetMethod(methodName, AccessTools.all));
	}
}