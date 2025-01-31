using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MonoTwitchSettingsCreator
{
    private const string AssetPath = "Assets/TwitchMono/Resources/TwitchMonoSettings.asset";

    static MonoTwitchSettingsCreator()
    {
        var existingObject = AssetDatabase.LoadAssetAtPath<MonoTwitchSettingsScriptableObject>(AssetPath);

        if (existingObject != null) return;
        var directoryPath = Path.GetDirectoryName(AssetPath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
            
        var newObject = ScriptableObject.CreateInstance<MonoTwitchSettingsScriptableObject>();
        AssetDatabase.CreateAsset(newObject, AssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
