using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimpleSequenceEditor : EditorWindow
{
    private SequenceData sequenceData;      //������ ������ ���� ����
    private Vector2 scrollPos;              //��ũ�� �� ��ġ
    private float beatHedight = 20;         //�� ��Ʈ�� ����
    private float trackWidth = 50;          //�� Ʈ���� �ʺ�
    private int totalBeats;                 //�� ��Ʈ ��
    private bool isPlaying = false;         //������, ���� �ߺ�
    private int currentBeatTime = 0;        //���� ��� ���� ��Ʈ �ð�
    private int playFromBeat = 0;           //����� ������ ��Ʈ
    private float startTime = 0;            //��� ���� �ð�
    private AudioSource audioSource;        //����� ����� ���� ����� �ҽ�

    [MenuItem("Tool/Simple Sequence Editor")]

    private static void ShowWindow()
    {
        var window = GetWindow<SimpleSequenceEditor>();
        window.titleContent = new GUIContent("Simple Sequencer");
        window.Show();
    }

    private void OnEnable()                     // ������ ������ â�� ������ ��
    {
        EditorApplication.update += Update;     //������Ʈ �Լ��� �̺�Ʈ�� ���
        CreateAudioSource();                    //������� ����� ���̱� ������ ����� ����� ���ش�.
    }

    private void OnDisable()                    //������ ������ â�� �ݾ��� ��
    {
        EditorApplication.update -= Update;     //������Ʈ �Լ��� �̺�Ʈ ����
        if (audioSource != null)                         //������ ������ â�� �ݾ��� �� ����� �ҽ��� ��� ���� ���
        {
            DestroyImmediate(audioSource.gameObject);   //��ϵ� ����� �ҽ� ������Ʈ�� �ı� �Ѵ�.
            audioSource = null;
        }
    }

    private void CreateAudioSource()            //����Ƽ Scene���� ó�� ����� �ҽ��� ��� �ϱ� ���ؼ� ������ �ʰ� ���
    {
        var audioSourceGameObject = new GameObject("EditorAudioSource");
        audioSourceGameObject.hideFlags = HideFlags.HideAndDontSave;
        audioSource = audioSourceGameObject.AddComponent<AudioSource>();
    }

    private void InitializeTracks()             //������ �����Ͱ� ������ �� �����͸� �������ִ� �Լ�
    {
        if (sequenceData == null) return;       //�����Ͱ� ���� ��� �׳� ����

        if (sequenceData.trackNotes == null)     //�����Ͱ� �ִµ� Ʈ�� ��Ʈ�� ���� ��� �ڷ����� ���� �����ش�.
        {
            sequenceData.trackNotes = new List<List<int>>();
        }

        while (sequenceData.trackNotes.Count < sequenceData.numberOfTracks)      //Ʈ�� �� ��ŭ List<int>()�� �־��ش�.
        {
            sequenceData.trackNotes.Add(new List<int>());
        }

        foreach (var track in sequenceData.trackNotes)           //�� Ʈ�� ��Ʈ�� ��Ʈ ����ŭ 0 ������ �����͸� �־��ش�.
        {
            while (track.Count < totalBeats)
            {
                track.Add(0);
            }
        }

        if (audioSource != null)                                //������ ������ �������� ����� Ŭ���� ���� ��� ����� �����͸� �Ҵ��Ѵ�.
        {
            audioSource.clip = sequenceData.audioClip;
        }
    }

    private void Update()
    {
        if (this == null)            //�� ��ũ��Ʈ�� �ı� �Ǿ����� Ȯ��
        {
            EditorApplication.update -= Update;     //�̺�Ʈ �Ҵ��� �������ش�.
            return;
        }

        if (isPlaying && audioSource != null && audioSource.isPlaying)
        {
            float elapseTime = audioSource.time;                                        //����� �ð��� �����´�.
            currentBeatTime = Mathf.FloorToInt(elapseTime * sequenceData.bpm / 60f);    //BPM���� 60�� �帥 �ð��� ���ϸ� ���� ��Ʈ �̴�.

            if (currentBeatTime >= totalBeats)
            {
                StopPlayBack();
            }
            Repaint();                          //������Ʈ�� ���� �Ŀ� ������Ʈ ( ȭ�� ������ ������ �ʴ� ������Ʈ�̱� ������)
        }
    }

    private void StartPlayBack(int fromBeat)                //������� Ư�� ��Ʈ���� �÷��� ���ִ� �Լ� 
    {
        if (sequenceData == null || sequenceData.audioClip == null || audioSource == null) return;   //�� ������ �ϳ��� ������ ����

        isPlaying = true;
        currentBeatTime = fromBeat;
        playFromBeat = fromBeat;

        if (audioSource.clip != sequenceData.audioClip)          //���� ������Ʈ Ŭ���� �޾ƿ� ������ ������ ����� Ŭ���� �ٸ��ٸ�
        {
            audioSource.clip = sequenceData.audioClip;          //���� ������ ������ Ŭ������ ���� �����ش�.
        }

        float startTime = fromBeat * 60f / sequenceData.bpm;    //���� �ð��� ��Ʈ�� BPM���� ���� ����Ͽ� �����Ѵ�.
        audioSource.time = startTime;                           //����� �ð��� ������ �ð����� ����
        audioSource.Play();

        this.startTime = (float)EditorApplication.timeSinceStartup - startTime;     //�����Ϳ� �ð��� �ݿ��Ѵ�.
        EditorApplication.update += Update;                                         //������Ʈ �̺�Ʈ�� ���
    }

    private void PausePlayBack()
    {
        isPlaying = false;
        if (audioSource != null) audioSource.Pause();
    }
    private void StopPlayBack()
    {
        isPlaying = false;
        if (audioSource != null) audioSource.Stop();
        EditorApplication.update -= Update;                     //������Ʈ �̺�Ʈ ����
    }

    private void DrawBeat(int trackIndex, int beatIndex)
    {
        if (sequenceData == null || sequenceData.trackNotes == null || trackIndex >= sequenceData.trackNotes.Count) return;

        Rect rect = GUILayoutUtility.GetRect(trackWidth, beatHedight);
        bool isCurrentBeat = currentBeatTime == beatIndex;  //currentBeatTime �� beatIndex�� �����ϸ� true �ƴϸ� false
        //���� Ʈ���� ���� ��Ʈ �ε����� �ش��ϴ� ��Ʈ ���� �������鼭 �˻��ؼ� ��ȿ�� �ε����� �ƴ� ��� 0���� ���� �ȴ�.
        int noteValue = (sequenceData.trackNotes[trackIndex].Count > beatIndex) ? sequenceData.trackNotes[trackIndex][beatIndex] : 0;

        Color color = Color.gray;
        if (isCurrentBeat) color = Color.cyan;
        else
        {
            switch (noteValue)
            {
                case 1: color = Color.green; break;
                case 2: color = Color.yellow; break;
                case 3: color = Color.red; break;
                case 4: color = Color.blue; break;
            }
        }

        EditorGUI.DrawRect(rect, color);

        //������ �����쿡���� ���콺 Ŭ�� �̺�Ʈ�� �Ʒ��� ���� ���·� �޾� �´�.
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0)           //���콺 ���� Ŭ��
            {
                noteValue = (noteValue + 1) % 5;
                while (sequenceData.trackNotes[trackIndex].Count <= beatIndex)
                {
                    sequenceData.trackNotes[trackIndex].Add(0);
                }
                sequenceData.trackNotes[trackIndex][beatIndex] = noteValue;
            }
            else if (Event.current.button == 1)     //���콺 ������ Ŭ��
            {
                if (sequenceData.trackNotes[trackIndex].Count > beatIndex)
                {
                    sequenceData.trackNotes[trackIndex][beatIndex] = 0;
                }
            }
            Event.current.Use();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(200));                        //GUI ���̾ƿ� ���� ����
        EditorGUILayout.LabelField("������ ������ ����", EditorStyles.boldLabel);   //��Ʈ

        sequenceData = (SequenceData)EditorGUILayout.ObjectField("������ ������", sequenceData, typeof(SequenceData), false);

        if (sequenceData == null)
        {
            EditorGUILayout.EndVertical();                                          //GUI ��
            return;
        }

        InitializeTracks();

        EditorGUILayout.LabelField("BPM", sequenceData.bpm.ToString());
        EditorGUILayout.LabelField("����� Ŭ��", sequenceData.audioClip != null ? sequenceData.name : "Note");

        EditorGUILayout.LabelField("Ʈ�� �� ����", EditorStyles.boldLabel);
        int newNumberOfTracks = EditorGUILayout.IntField("Ʈ�� ��", sequenceData.numberOfTracks);
        if (newNumberOfTracks != sequenceData.numberOfTracks)        //Ʈ�� ���� �ٸ� ���
        {
            sequenceData.numberOfTracks = newNumberOfTracks;        //Ʈ�� ���� �����ͼ� ����
            InitializeTracks();
        }

        if (sequenceData.numberOfTracks < 1) sequenceData.numberOfTracks = 1;       //Ʈ���� 1 ���Ϸ� �������� �ּ� Ʈ���� 1

        if (sequenceData.audioClip != null)
        {
            totalBeats = Mathf.FloorToInt((sequenceData.audioClip.length / 60f) * sequenceData.bpm);    //��Ʈ ���� ����Ѵ�.
        }
        else
        {
            totalBeats = 0;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(isPlaying ? "�Ͻ� ����" : "���"))
        {
            if (isPlaying) PausePlayBack();
            else StartPlayBack(currentBeatTime);
        }
        if (GUILayout.Button("ó�� ���� ���"))
        {
            StartPlayBack(0);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Ư�� ��Ʈ���� ���", EditorStyles.boldLabel);
        playFromBeat = EditorGUILayout.IntSlider("��Ʈ �ε���", playFromBeat, 0, totalBeats - 1);
        if (GUILayout.Button("�ش� ��Ʈ���� ���"))
        {
            StartPlayBack(playFromBeat);
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);    //UI ���� ���� 10
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height - 210));
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(100));
        GUILayout.Space(beatHedight);

        for (int j = 0; j < totalBeats; j++)
        {
            float beatTime = j * 60 / sequenceData.bpm;
            int minutes = Mathf.FloorToInt(beatTime / 60f);
            int seconds = Mathf.FloorToInt(beatTime % 60f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{minutes:00}:{seconds:00}", GUILayout.Width(50));
            EditorGUILayout.LabelField($"{j}", GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(beatHedight - EditorGUIUtility.singleLineHeight);
        }

        EditorGUILayout.EndVertical();

        for (int i = 0; i < sequenceData.numberOfTracks; i++)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label($"Ʈ�� {i + 1}", GUILayout.Width(trackWidth));
            for (int j = 0; j < totalBeats; j++)
            {
                DrawBeat(i, j);
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("������ ����"))
        {
            sequenceData.SaveToJson();
        }

        if (GUILayout.Button("������ �ҷ�����"))
        {
            sequenceData.LoadFromJson();
            InitializeTracks();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }
}
