using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

[CreateAssetMenu(fileName = "NewSequence" , menuName = "Sequencer/Sequence")]
public class SequenceData : ScriptableObject
{
    public int bpm;                                                     //������ BPM
    public int numberOfTracks;                                         //��Ʈ Ʈ�� ��
    public AudioClip audioClip;                                       //����� Ŭ��
    public List<List<int>> trackNotes = new List<List<int>>();        //2���� ������ ������ �ִ´�.
    public TextAsset trackJsonFile;                                 //.json ���� �ؽ�Ʈ ����

    public void SaveToJson()
    {
        if(trackJsonFile == null)
        {
            Debug.LogError("Track JSON ������ �����ϴ�.");
            return;
        }

        var data = JsonConvert.SerializeObject(new
        {
            bpm,
            numberOfTracks,
            audioClipPath = AssetDatabase.GetAssetPath(audioClip),
            trackNotes
        }, Formatting.Indented);

        System.IO.File.WriteAllText(AssetDatabase.GetAssetPath(trackJsonFile), data);
        AssetDatabase.Refresh();
    }

    public void LoadFromJson()
    {
        if(trackJsonFile == null)
        {
            Debug.LogError("Track JSON ������ �����ϴ�.");
            return;
        }

        var data = JsonConvert.DeserializeAnonymousType(trackJsonFile.text, new
        {
            bpm = 0,
            numberOfTracks = 0,
            //AudioClipPath = "",
            trackNotes = new List<List<int>>()
        });

        bpm = data.bpm;
        numberOfTracks = data.numberOfTracks;
        //audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(data.AudioClipPath);
        trackNotes = data.trackNotes;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SequenceData))]
public class SequenceDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sequenceData = (SequenceData)target;

        DrawDefaultInspector();

        if(sequenceData != null)
        {
            EditorGUILayout.LabelField("Track Notes", EditorStyles.boldLabel);
            for(int i = 0; i < sequenceData.trackNotes.Count; i++)
            {
                EditorGUILayout.LabelField($"Track {i + 1} : [{string.Join(".", sequenceData.trackNotes[i])}");
            }
        }

        if (GUILayout.Button("Load form JSON")) sequenceData.LoadFromJson();
        if (GUILayout.Button("Save form JSON")) sequenceData.SaveToJson();

        if(GUI.changed)
        {
            EditorUtility.SetDirty(sequenceData);
        }
    }
}
#endif
