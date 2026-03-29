using System.Collections.Generic;
using PoolMN;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class NoteSpawner : MonoBehaviour
{
    public CameraController cameraController;
    public SongData songData;
    public Transform spawnLine;
    public Transform hitLine;
    public float spawnOffset = 2f;
    public float scaleOffset = 0.7f;
    public float minX = 2f;
    public float maxX = -2f;
    public float gapWidth = 1.5f;
    private int currentIndex = 0;
    private string normalType = "Note_N";
    private string longType = "Note_L";
    private string strongType = "Note_S";
    private string lollipopType = "Note_Lolipop";
    private bool isReady = false;
    float frozenTime;
    bool isStopped = false;

    public void Ready()
    {
        Invoke(nameof(CanPlay), 1f);
    }

    private void CanPlay()
    {
        isReady = true;
        AudioMN.Instance.PlaySong(songData.audioClip);
    }

    public void StopPlay()
    {
        frozenTime = AudioMN.Instance.GetTime();
        AudioMN.Instance.StopSong();
        isReady = false;
        isStopped = true;
        currentIndex = 0;
    }

    public void ClearNotes()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            PoolManager.instance.ReturnToPool(child.gameObject);
        }
    }

    void Update()
    {
        if (!isReady) return;

        SpawnNotes();
    }

    void SpawnNotes()
    {
        float songTime = GameplayMN.Instance.GetSongTime();
        var notes = songData.notes;

        while (currentIndex < notes.Count)
        {
            var note = notes[currentIndex];

            if (songTime >= note.ta - spawnOffset)
            {
                Spawn(note);
                currentIndex++;
            }
            else break;
        }
    }

    void Spawn(NoteData data)
    {
        NoteType type = GetNoteType(data);

        Vector3 spawnPos = GetSpawnPosition(data);
        Vector3 hitPos = GetHitPosition(data);

        var pool = PoolManager.GetPoolByName(GetPrefabName(type));
        if (pool == null)
        {
            Debug.LogError("Pool not found for prefab: " + GetPrefabName(type));
            return;
        }

        var go = pool.GetPooledObject(false);
        go.transform.SetParent(gameObject.transform);
        go.transform.position = spawnPos;
        go.transform.localScale = Vector3.one * scaleOffset;
        Note note = go.GetComponent<Note>();
        var candyNumber = GetLane(data.n);
        var isLolipop = IsLollipop(data);
        var isLastNote = (currentIndex == songData.notes.Count - 1);

        note.Init(data.ta, spawnOffset, spawnPos, hitPos, candyNumber, isLolipop, isLastNote);
    }

    bool IsLollipop(NoteData data)
    {
        return songData.lollipopNoteIds.Contains(data.id);
    }

    string GetPrefabName(NoteType type)
    {
        switch (type)
        {
            case NoteType.Normal: return normalType;
            case NoteType.Long: return longType;
            case NoteType.Strong: return strongType;
            case NoteType.Lollipop: return lollipopType;
        }

        return normalType;
    }

    private NoteType GetNoteType(NoteData data)
    {
        if (IsLollipop(data))
            return NoteType.Lollipop;

        int typeIndex = data.pid % 3;

        switch (typeIndex)
        {
            case 0: return NoteType.Normal;
            case 1: return NoteType.Long;
            case 2: return NoteType.Strong;
        }

        return NoteType.Normal;
    }

    float GetXFromNote(int n)
    {
        int minNote = 96;

        int index = n - minNote;
        int lane = index < 3 ? 0 : 1;

        int localIndex = lane == 0 ? index : index - 3;

        float mid = (minX + maxX) / 2;

        float t = localIndex / 2f;
        t = 1f - t;

        if (lane == 1)
        {
            return Mathf.Lerp(minX, mid - gapWidth / 2, t);
        }
        else
        {
            return Mathf.Lerp(mid + gapWidth / 2, maxX, t);
        }
    }

    int GetLane(int n)
    {
        return (n <= 98) ? 0 : 1;
    }

    Vector3 GetSpawnPosition(NoteData data)
    {
        float spawnY = spawnLine.position.y;
        float x = GetXFromNote(data.n);
        return new Vector3(x, spawnY, 0);
    }

    Vector3 GetHitPosition(NoteData data)
    {
        float hitY = hitLine.position.y;
        float x = GetXFromNote(data.n);
        return new Vector3(x, hitY, 0);
    }

    void OnDrawGizmos()
    {
        if (spawnLine == null || hitLine == null) return;

        Gizmos.color = Color.red;

        int total = 6;
        int half = total / 2;
        float mid = (minX + maxX) / 2;

        // Lane trái
        for (int i = 0; i < half; i++)
        {
            float t = i / (float)(half - 1);
            float x = Mathf.Lerp(minX, mid - gapWidth / 2, t);

            Gizmos.DrawLine(
                new Vector3(x, spawnLine.position.y, 0),
                new Vector3(x, hitLine.position.y, 0)
            );
        }

        // Lane phải
        for (int i = 0; i < half; i++)
        {
            float t = i / (float)(half - 1);
            float x = Mathf.Lerp(mid + gapWidth / 2, maxX, t);

            Gizmos.DrawLine(
                new Vector3(x, spawnLine.position.y, 0),
                new Vector3(x, hitLine.position.y, 0)
            );
        }
    }
}

[System.Serializable]
public class NotePrefabSet
{
    public GameObject shortPrefab;
    public GameObject longPrefab;
    public GameObject strongPrefab;
    public GameObject lollipopPrefab;
}