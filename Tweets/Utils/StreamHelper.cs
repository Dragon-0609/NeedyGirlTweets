using System.IO;

namespace Tweets.Utils;

public static class StreamHelper
{
	public static byte[] ReadAllBytes(this Stream stream){
		if (stream is MemoryStream memoryStream)
		{
			return memoryStream.ToArray();
		}
		else
		{
			using MemoryStream memoryStream2 = new MemoryStream();
			stream.CopyTo(memoryStream2);
			return memoryStream2.ToArray();
		}
	}
}