using UnityEngine;
using UnityEditor;
using System.IO;

public static class SpriteToNativeAsset
{
    // Set false if image dimensions are NOT multiples of 4 or if need a lossless result.
    const bool COMPRESS = true;
    const bool DELETE_ORIGINAL = true;

    [MenuItem("Assets/Convert Texture to .asset")]
    static void Convert()
    {
        string srcPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(srcPath) ||
            !(srcPath.EndsWith(".jpg") || srcPath.EndsWith(".jpeg") || srcPath.EndsWith(".png")))
        {
            return;
        }

        byte[] bytes = File.ReadAllBytes(srcPath);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(bytes);
        tex.name = Path.GetFileNameWithoutExtension(srcPath);

        if (COMPRESS)
            tex.Compress(true);               // -> DXT1/DXT5, smaller .asset

        // Create the .asset with the texture first, so it is a persistent object that the sprite can safely reference.
        string dir = Path.GetDirectoryName(srcPath);
        string outPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(dir, tex.name + ".asset"));
        AssetDatabase.CreateAsset(tex, outPath);

        // Build the Sprite from the now-persistent texture, embed it in the same file, then PROMOTE it to be the main object. This is what makes the .asset itself read as a Sprite.
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                      new Vector2(0.5f, 0.5f), 100f);   // center pivot, 100 PPU
        sprite.name = tex.name;
        AssetDatabase.AddObjectToAsset(sprite, tex);
        AssetDatabase.SetMainObject(sprite, outPath);   // the main line

        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(outPath);

        if (DELETE_ORIGINAL)
            AssetDatabase.DeleteAsset(srcPath);

        Debug.Log($"Created {outPath} as asset");
    }
}
