using System;

namespace Tweets.Data;

[Serializable]
public class DataVariant<T>
{
	public string Key;
	public T Value;
}

[Serializable]
public class DataVariantCollection<T>
{
	public string Key;
	public T[] Values;
}

