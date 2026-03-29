using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "RhythmGame/SongData")]
public class SongData : ScriptableObject
{
    public string songName;
    public AudioClip audioClip;

    [Header("Chart Data")]
    public List<NoteData> notes = new List<NoteData>();

    [Header("Special Notes")]
    public List<int> lollipopNoteIds;
}

[System.Serializable]
public class LollipopConfig
{
    public List<int> noteIds;
}