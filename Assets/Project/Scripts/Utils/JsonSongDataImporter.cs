using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
public class JsonSongDataImporter
{
    [MenuItem("Tools/Import JSON To SongData")]
    public static void Import()
    {
        string path = EditorUtility.OpenFilePanel("Select JSON", "", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = System.IO.File.ReadAllText(path);

        List<NoteData> notes = JsonHelper.FromJson<NoteData>(json);

        SongData songData = ScriptableObject.CreateInstance<SongData>();
        songData.songName = System.IO.Path.GetFileNameWithoutExtension(path);
        songData.notes = notes;

        string assetPath = "Assets/ScriptableObjects/" + songData.songName + ".asset";
        AssetDatabase.CreateAsset(songData, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log("Import success: " + assetPath);
    }
}
#endif