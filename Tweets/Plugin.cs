using BepInEx;
using BepInEx.Logging;
using NGO;
using ngov3;
using Tweets.Patches;
using Tweets.Utils.Data;

namespace Tweets
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public static Plugin Instance;
		private ResourceChanger _resourceChanger;
		private Patcher _patcher;

		public bool LogEnabled
		{
			get => DataSaver.Load("Log", true);
			set => DataSaver.Save("Log", value);
		}

		public bool Enabled
		{
			get => DataSaver.Load("Enabled", true);
			set => DataSaver.Save("Enabled", value);
		}

		public ManualLogSource DebugLog => Logger;


		private void Awake()
		{
			Instance = this;
			_resourceChanger = new ResourceChanger();
			InitData();

			// Action_PlayMakeLove target point

			_patcher = new Patcher();

			if (!Enabled) return;
			
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet158, CommandType.Yaru_sex, "ame_bed_1");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet159, CommandType.Yaru_sex, "ame_bed_2");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet160, CommandType.Yaru_sex, "ame_bed_3");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet161, CommandType.Yaru_sex, "ame_bed_4");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet162, CommandType.Yaru_sex, "ame_bed_5");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet163, CommandType.Yaru_sex, "ame_bed_6");
			_resourceChanger.TryAddPrivateImage(TweetType.AfterTweet_Taiken1, CommandType.None, "ame_dress_1");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet043, CommandType.Deai, "ame_dinder_1");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet205, CommandType.Deai, "ame_dinder_2");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet213, CommandType.Deai, "ame_dinder_3");
			_resourceChanger.TryAddPrivateImage(TweetType.Tweet044, CommandType.Deai, "ame_dinder_4");
			
			// _resourceChanger.LogToFile();
			// _resourceChanger.LogList(); 
		}
		
		private void InitData()
		{
			DataSaver.ValidateConfig("Enabled", true, "General", "Restart to apply changes");
			DataSaver.ValidateConfig("Log", true);
		}
	}
}