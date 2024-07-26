using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SequenceData sequenceData;
    public NoteManager noteManager;
    public float playbackSpeed = 1.0f;
    private bool notesGenerated = false;

    void Start()
    {
        if (sequenceData == null)
        {
            Debug.LogError("sequenceData null");
            return;
        }

        sequenceData.LoadFromJson();

        if(sequenceData.trackNotes == null || sequenceData.trackNotes.Count == 0 )
        {
            initializeTrackNotes();
        }
        //�Ŵ����� ������ �����͸� �����ͼ� ���� ��Ų��.
        noteManager.audioClip = sequenceData.audioClip;
        noteManager.bpm = sequenceData.bpm;
        noteManager.SetSpeed(playbackSpeed);

        GenerateNotes();
        noteManager.Initialized();
    }

    private void initializeTrackNotes()
    {
        sequenceData.trackNotes = new List<List<int>>();
        for (int i = 0; i < sequenceData.numberOfTracks; i++)
        {
            sequenceData.trackNotes.Add(new List<int>());
        }
    }

    //��Ʈ ����
    private void GenerateNotes()
    {
        if (notesGenerated) return;         //�̹� ��Ʈ�� �����Ǿ��ٸ� �ߺ� ���� ����

        noteManager.notes.Clear();          //��Ʈ �Ŵ����� �����Ͽ� ��Ʈ �ʱ�ȭ

        for(int trackIndex = 0;  trackIndex < sequenceData.trackNotes.Count; trackIndex++)      //��Ʈ Ʈ�� ��
        {
            for(int beatIndex = 0; beatIndex < sequenceData.trackNotes[trackIndex].Count; beatIndex++)
            {
                int noteVaule = sequenceData.trackNotes[trackIndex][beatIndex];

                if(noteVaule != 0)
                {
                    float startTime = beatIndex * 60f / sequenceData.bpm;
                    float durtaion = noteVaule * 60f  / sequenceData.bpm;
                    Note note = new Note(trackIndex, startTime, durtaion);
                    noteManager.AddNote(note);
                }
            }
        }
        notesGenerated = true;
    }

    //��� �ӵ� ����
    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = speed;
        noteManager.SetSpeed(speed);        //���ǵ带 �޾Ƽ� ��Ʈ �Ŵ����� ����
    }

    //JSON �����Ϳ��� ������ ������ �ε�
    public void LoadSequenceDataFromJson()
    {
        sequenceData.LoadFromJson();

        if(sequenceData.trackNotes == null || sequenceData.trackNotes.Count == 0)
        {
            initializeTrackNotes();
        }

        //�Ŵ����� ������ �����͸� �����ͼ� ���� ��Ų��.
        noteManager.audioClip = sequenceData.audioClip;
        noteManager.bpm = sequenceData.bpm;
        noteManager.SetSpeed(playbackSpeed);

        notesGenerated = false;                 //���ο� �����͸� �ε� �����Ƿ� ��Ʈ ����� ���
        GenerateNotes();
        noteManager.Initialized();
    }
}
