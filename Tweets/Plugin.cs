using BepInEx;
using BepInEx.Logging;
using NGO;
using ngov3;
using Tweets.Data;
using Tweets.Patches;

namespace Tweets
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public static Plugin Instance;
		private ResourceChanger _resourceChanger;

		internal bool LogEnabled
		{
			get => DataSaver.Load("Log", true);
			set => DataSaver.Save("Log", value);
		}

		internal bool Enabled
		{
			get => DataSaver.Load("Enabled", true);
			set => DataSaver.Save("Enabled", value);
		}

		internal bool AllowedToOverride
		{
			get => DataSaver.Load("Override Original", false);
			set => DataSaver.Save("Override Original", value);
		}

		internal ManualLogSource DebugLog => Logger;


		private void Awake()
		{
			Instance = this;
			_resourceChanger = new ResourceChanger();
			InitData();

			// Action_PlayMakeLove target point

			Patcher.Patch();

			if (!Enabled) return;
			
			AddImages();
		}

		private void InitData()
		{
			DataSaver.ValidateConfig("Enabled", true);
			DataSaver.ValidateConfig("Override Original", false, "General",
				"If enabled, will replace some game's original tweet images with custom (Restart to apply changes)");
			DataSaver.ValidateConfig("Log", true, "General", "Restart to apply changes");
		}

		private void AddImages()
		{
			_resourceChanger.AddPrivateImage(TweetType.Tweet158, CommandType.Yaru_sex, "ame_bed_1");
			_resourceChanger.AddPrivateImage(TweetType.Tweet159, CommandType.Yaru_sex, "ame_bed_2");
			_resourceChanger.AddPrivateImage(TweetType.Tweet160, CommandType.Yaru_sex, "ame_bed_3");
			_resourceChanger.AddPrivateImage(TweetType.Tweet161, CommandType.Yaru_sex, "ame_bed_4");
			_resourceChanger.AddPrivateImage(TweetType.Tweet162, CommandType.Yaru_sex, "ame_bed_5");
			_resourceChanger.AddPrivateImage(TweetType.Tweet163, CommandType.Yaru_sex, "ame_bed_6");
			_resourceChanger.AddPrivateImage(TweetType.AfterTweet_Taiken1, CommandType.None, "ame_dress_1");
			_resourceChanger.AddPrivateImage(TweetType.Tweet043, CommandType.Deai, "ame_dinder_1");
			_resourceChanger.AddPrivateImage(TweetType.Tweet205, CommandType.Deai, "ame_dinder_2");
			_resourceChanger.AddPrivateImage(TweetType.Tweet213, CommandType.Deai, "ame_dinder_3");
			_resourceChanger.AddPrivateImage(TweetType.Tweet044, CommandType.Deai, "ame_dinder_4");
			_resourceChanger.AddPrivateImage(TweetType.Tweet082, CommandType.Odekake_akiba, "ame_akiba_1");
			// _resourceChanger.TryAddPrivateImage(TweetType.Tweet081, CommandType.Odekake_akiba, "ame_akiba_2");
			_resourceChanger.AddPrivateImage(TweetType.Tweet081, CommandType.Odekake_akiba, "ame_akiba_3");
			_resourceChanger.AddPrivateImages(TweetType.Tweet115, CommandType.Odekake_ueno, "ame_park_1",
				"ame_park_2", "ame_park_3");

			_resourceChanger.AddPrivateImages(TweetType.Tweet279, CommandType.Odekake_hikarigaoka, "ame_hikarigaoka_1", "ame_hikarigaoka_3");
			_resourceChanger.AddPrivateImages(TweetType.Tweet019, CommandType.InternetYoutube, "ame_horror_1", "ame_horror_2", "ame_horror_3");
			
			if (AllowedToOverride)
			{
				_resourceChanger.AddPrivateImage(TweetType.Tweet099, CommandType.Odekake_hikarigaoka,
					"ame_hikarigaoka_2");

				_resourceChanger.AddPrivateImage(TweetType.Tweet100, CommandType.Odekake_ichigaya, "ame_fishing_1");
				
				_resourceChanger.AddPrivateImages(TweetType.Tweet104, CommandType.Odekake_nakano, "ame_arcade_1", "ame_arcade_1_1");
			}
			
			
			// _resourceChanger.LogToFile();
			// _resourceChanger.LogList(); 
		}
	}
}