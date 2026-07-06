#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HarmonyLib;
using Newtonsoft.Json;
using NGO;
using ngov3;
using Tweets.Utils;
using Tweets.Utils.Data;
using UnityEngine;
using TweetData = Tweets.Utils.TweetData;

namespace Tweets;

public class ResourceChanger
{
	private ResourceLocalMaster _resourceMaster;
	private List<TweetMaster.Param> _tweetList;
	private AssetBundle _assetBundle;
	public static ResourceChanger Instance;
	private List<TweetMaster.Param> _tweetRawList;

	private Dictionary<string, LoadPictures.PictureHandleData> ImagesCache;
	private List<string> _customImages = new();
	private Dictionary<string, Sprite> _alreadyLoadedSprites = new();
	

	public AssetBundle AssetBundle => _assetBundle;

	public List<string> CustomImages => _customImages;

	public Dictionary<string, Sprite> AlreadyLoadedSprites => _alreadyLoadedSprites;

	public ResourceLocalMaster ResourceMaster => _resourceMaster;

	public ResourceChanger()
	{
		Instance = this;
		_tweetList =
			(List<TweetMaster.Param>)AccessTools.Method(typeof(TweetFetcher), "getTweetRawList").Invoke(null, null);
		_resourceMaster = GetResourceLocalMaster();
		ImagesCache = typeof(LoadPictures).GetField("handleDatas", BindingFlags.NonPublic | BindingFlags.Static)?
			.GetValue(null) as Dictionary<string, LoadPictures.PictureHandleData>;

		Log($"Is tweet list found: {_tweetList != null}");

		_assetBundle = LoadAssetBundle(PluginBundle);

		if (ImagesCache == null)
		{
			LogError($"Images Cache was not found in ResourceChanger", true);
		}
	}

	public void LogList()
	{
		foreach (TweetMaster.Param param in _tweetList)
		{
			LogParam(param);
		}
	}

	public void LogToFile()
	{
		TweetCollection collection = new TweetCollection();
		
		foreach (TweetMaster.Param param in _tweetList)
		{
			Log($"Adding {param.Id}, command: {param.CommandID}");
			collection.Tweets.Add(new TweetData()
			{
				TweetId = param.Id,
				CommandId = param.CommandID,
				publicImage = ProcessField(param.OmoteImageId),
				privateImage = ProcessField(param.UraImageId),
				publicText = ProcessField(param.OmoteBodyEn),
				privateText = ProcessField(param.UraBodyEn),
			});
		}

		string json = JsonConvert.SerializeObject(collection, Formatting.Indented);
		Log($"{collection.Tweets.Count} tweets count, content: {json}");
		File.WriteAllText("tweets.json", json);

		return;
		
		string? ProcessField(string field)
		{
			if (field == null) return null;
			string trim = field.Trim();
			if (trim.Length < 2) return null;

			if (trim.Equals("N/A", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			
			return field;
		}
	}

	public static void LogParam(TweetMaster.Param param)
	{
		string command = "None";
		CommandType type = CommandType.None;
		if (int.TryParse(param.CommandID, out var id) && EnumHelper.TryGetEnum(id, out CommandType type1))
		{
			command = type1.ToString();
			// type = type1;
		}else if (CommandType.TryParse(param.CommandID, out CommandType type2))
		{
			command = type2.ToString();
			// type = type2;
		}

		/*if (type == CommandType.Yaru_sex && param.Id == "Tweet159")
		{
			param.UraImageId = "ame_1";
		}*/
		
		Log(
			$"Tweet ID:{param.Id}, command ID: {param.CommandID} ({command}), public image: {param.OmoteImageId}, private image: {param.UraImageId}, \npublic text: {param.OmoteBodyEn}\n\nprivate text:{param.UraBodyEn}\n");
	}

	public bool TryAddPrivateImage(TweetType tweet, CommandType command, string image)
	{
		string tId = tweet.ToString();
		string cId = command.ToString();

		TweetMaster.Param finding = _tweetList.Find(item => item.Id == tId && item.CommandID == cId);

		if (finding == null)
		{
			return false;
		}

		finding.UraImageId = image;
		
		_customImages.Add(image);
		// Log($"Added {image} to customImages");
		
		_resourceMaster.ResourceList.Add(new ResourceLocal()
		{
			FileName = image,
			Id = image,
			Path = image
		});
		
		return true;
	}

	public bool CheckPrivateImage(TweetType tweet, CommandType command)
	{
		string tId = tweet.ToString();
		string cId = command.ToString();
		TweetMaster.Param finding = _tweetList.Find(item => item.Id == tId && item.CommandID == cId);
		
		
		if (finding == null)
		{
			return false;
		}

		Log($"Found: '{finding.UraImageId}'");
		return finding.UraImageId.StartsWith("ame");
	}

	private ResourceLocalMaster GetResourceLocalMaster()
	{
		return Resources.Load<ResourceLocalMaster>("LocalMaster/ResourceLocalMaster");
	}
	
	private AssetBundle LoadAssetBundle(string bundleName)
	{
		Stream bundleResource = GetAssetBundleResource(bundleName);
		Debug.Log($"Is Bundle null: {bundleResource == null}");
		if (bundleResource == null)
		{
			LogError($"{bundleName} was null", true);
			return null;
		}
		
		var bytes = bundleResource.ReadAllBytes();
		AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
		bundleResource.Dispose();
		
		return assetBundle;
	}
	
	private static Stream GetAssetBundleResource(string name)
	{
		Debug.Log($"Loading Bundle: {"Tweets.AssetBundles." + name}");
		return Assembly.GetExecutingAssembly().GetManifestResourceStream("Tweets.AssetBundles." + name);
	}

	
}