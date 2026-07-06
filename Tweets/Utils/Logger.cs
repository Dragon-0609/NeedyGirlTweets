namespace Tweets.Utils;

public static class Logger
{
	public static void Log(string msg)
	{
		if (Plugin.Instance.LogEnabled)
			Plugin.Instance.DebugLog.LogMessage(msg);
	}

	public static void LogWarning(string msg)
	{
		if (Plugin.Instance.LogEnabled)
			Plugin.Instance.DebugLog.LogWarning(msg);
	}

	public static void LogError(string msg, bool forceShow = false)
	{
		if (Plugin.Instance.LogEnabled || forceShow)
			Plugin.Instance.DebugLog.LogError(msg);
	}
}