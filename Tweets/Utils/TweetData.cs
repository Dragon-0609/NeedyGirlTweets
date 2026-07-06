#nullable enable
using System;
using System.Collections.Generic;

namespace Tweets.Utils;

[Serializable]
public class TweetData
{
	public string TweetId;
	public string CommandId;
	public string? publicImage;
	public string? privateImage;
	public string? publicText;
	public string? privateText;
}

[Serializable]
public class TweetCollection
{
	public List<TweetData> Tweets = new List<TweetData>();
}