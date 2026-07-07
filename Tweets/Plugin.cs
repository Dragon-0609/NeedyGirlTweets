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
			DataSaver.ValidateConfig("Enabled", true, "General", "Restart to apply changes");
			DataSaver.ValidateConfig("Log", true);
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

			// _resourceChanger.LogToFile();
			// _resourceChanger.LogList(); 
		}
	}
}