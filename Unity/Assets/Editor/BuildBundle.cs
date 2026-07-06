using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildBundle
{
	[MenuItem("Tools/Build Bundle &B")]
	public static void Build()
	{
		string path = PlayerPrefs.GetString("path", "");

		if (string.IsNullOrEmpty(path))
		{
			EditorUtility.DisplayDialog("Error", "Please set path to build bundle", "OK");
			return;
		}

		BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None,
			BuildTarget.StandaloneWindows64);

		Debug.Log("Bundles were created");
	}

	[MenuItem("Tools/Set Bundle Path &S")]
	public static void SetBundlePath()
	{
		string path = EditorUtility.SaveFolderPanel("Select Bundle Path", PlayerPrefs.GetString("path", ""), "");
		if (!Directory.Exists(path))
		{
			return;
		}
		
		PlayerPrefs.SetString("path", path);
	}
}