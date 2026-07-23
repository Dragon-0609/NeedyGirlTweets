using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using ngov3;
using UnityEngine;

namespace Tweets.Patches;

// [HarmonyPatch(typeof(LoadPictures), nameof(LoadPictures.LoadPictureAsync))]
public static class LoadPicturePatch
{
	[HarmonyPrefix]
	public static bool Prefix(
		string address,
		LoadPictures.PictureType type,
		ref UniTask<Sprite> __result)
	{
		// Log("Prefix worked");
		if (ResourceChanger.Instance == null)
		{
			Debug.LogWarning($"ResourceChanger instance is null");
			return true;
		}

		// Log($"Should Intercept: {address} {ResourceChanger.Instance.CustomImages.Contains(address)}"); //(at {Environment.StackTrace})");
		
		if (!ResourceChanger.Instance.CustomImages.Contains(address))
			return true; // Let the original method execute

		if (ResourceChanger.Instance.IsVariantsKey(address))
		{
			address = ResourceChanger.Instance.ChooseVariant(address);
			if (address == "null")
			{
				return true;
			}
		}

		if (ResourceChanger.Instance.AlreadyLoadedSprites.ContainsKey(address))
		{
			Log($"Already loaded: {address}");
			__result = UniTask.FromResult(ResourceChanger.Instance.AlreadyLoadedSprites[address]);
			return false;
		}

		Sprite sprite = ResourceChanger.Instance.AssetBundle.LoadAsset<Sprite>(address);
		// __result = LoadSpriteAsync(sprite);

		if (sprite == null)
		{
			LogError($"Couldn't find or load {address}", true);
		}
		
		ResourceChanger.Instance.AlreadyLoadedSprites.Add(address, sprite);
		__result = UniTask.FromResult(sprite);
		
		Log($"Loaded: {address}");
		
		return false;
	}

	static MethodBase TargetMethod() => AccessTools.Method(
		typeof(LoadPictures),
		"LoadPictureAsync",
		new Type[]
		{
			typeof(string),
			typeof(LoadPictures.PictureType)
		});
}