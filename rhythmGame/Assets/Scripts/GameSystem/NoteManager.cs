using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;

    public ScoreManager scoreManager;
    public Player player;
    public PoolManager poolManager;

    public AudioClip audioClip;                         //����� ����� Ŭ��
    public List<Note> notes = new List<Note>();         //��� ��Ʈ ������ ��� ����Ʈ
    public float bpm = 120f;                            //���� BPM
    public float speed = 1f;                            //��� �ӵ�
    public GameObject notePrefabs;                      //��Ʈ ������

    public float audioLatency = 0.1f;                   //����� ���� �ð�
    public float hitPosition = -8.0f;                   //��Ʈ ���� ��ġ
    public float noteSpeed = 10;                        //��Ʈ �̵��ӵ�

    private AudioSource audioSource;                    //����� �ҽ� ���۳�Ʈ
    private float startTime;                            //���� ���� �ð�
    [SerializeField]private List<Note> activeNotes = new List<Note>();  //������ �����ä����� �ʤ��Ѥ� ��Ʈ ����Ʈ 

    public List<NoteObject> nowNotes = new List<NoteObject>();     //�ľ��ϴ� ��Ʈ���� ������� �����ϴ� ����Ʈ

    private float spawnOffset;                          //��� ���� �ð� ������

    public bool debugMode = false;                      //����� ��� �÷���
    public GameObject hitPositionMarker;                //���� ��ġ ��Ŀ ������Ʈ

    public float initialDelay = 3f;                     //�ʱ� ���� �ð�

    public Queue<NoteObject> notePool = new Queue<NoteObject>();

    private bool GameOver = false;

    private void Awake()
    {
        instance = this;
    }

    //���� �ʱ�ȭ
    public void Initialized()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;                           //���� �ð��� ���� �ð���ŭ �̷�
        startTime = Time.time + initialDelay;                   //List ���� �ʱ�ȭ clear ���ִ� ���� ���� 
        activeNotes.Clear();
        activeNotes.AddRange(notes);
        spawnOffset = (10 - hitPosition) / noteSpeed;           //��Ʈ ���� �ð� ������ ���

        if (debugMode)
        {
            CreateHitPositionMarker();
        }

        //StartCoroutine(StartAudioWithDelay());                  //���� �� ����� ��� �ڷ�ƾ ����
        AudioPlay();
    }

    private void AudioPlay()
    {
        double StartTime = AudioSettings.dspTime + initialDelay;
        audioSource.PlayScheduled(StartTime);
    }

    //���� �� ����� ����� ���� �ڷ�ƾ
    //private IEnumerator StartAudioWithDelay()
    //{
    //    yield return new WaitForSeconds(initialDelay);
    //    audioSource.Play();
    //}

    void Update()
    {
        float currentTime = Time.time -  startTime;     //���� ���� �ð��� ���

        if (currentTime >= audioSource.clip.length + 2f || scoreManager.HP <= 0)
        {
            if (!GameOver)
            {
                GameOver = true;
                scoreManager.SendScore();
                Debug.Log("��������, �� ��ȯ�� �ʿ��ϸ� ���⼭ ����");
            }
            return;
        }

        //Ȱ��ȭ�� ��Ʈ�� ó��
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = activeNotes[i];
            if (currentTime >= note.startTime - spawnOffset && currentTime < note.startTime + note.duration)
            {
                GameObject temp = poolManager.SpawnFromPool("Note", new Vector3(10, note.trackIndex * 2, 0), Quaternion.identity);
                temp.GetComponent<NoteObject>().Initialized(note, noteSpeed, hitPosition, startTime);
                nowNotes.Add(temp.GetComponent<NoteObject>());
                //notePoolDequeue(note);
                activeNotes.RemoveAt(i);
            }
            else if (currentTime >= note.startTime + note.duration)
            {
                activeNotes.RemoveAt(i);
            }
        }
    }
    
    //���ο� ��Ʈ �߰�
    public void AddNote(Note note)
    {
        notes.Add(note);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SpawnNoteObject(Note note)
    {
        GameObject noteObject = Instantiate(notePrefabs, new Vector3(10,note.trackIndex * 2, 0),Quaternion.identity);
        noteObject.GetComponent<NoteObject>().Initialized(note, noteSpeed, hitPosition, startTime);
        nowNotes.Add(noteObject.GetComponent<NoteObject>());
    }

    //����� ���� �ð� ����
    public void AdjustAudioLatency(float latency)
    {
        audioLatency = latency;
    }

    public void notePoolEnqueue(NoteObject targetNote)
    {
        notePool.Enqueue(targetNote);
        targetNote.gameObject.SetActive(false);
        nowNotes.Remove(targetNote);
    }
    public void notePoolDequeue(Note note)
    {
        if (notePool.Count > 0)
        {
            NoteObject temp = notePool.Dequeue();
            temp.Initialized(note, noteSpeed, hitPosition, startTime);
            nowNotes.Add(temp);
        }
        else
        {
            SpawnNoteObject(note);
        }
    }

    //����׿� ���� ��ġ ��Ŀ ����
    private void CreateHitPositionMarker()
    {
        hitPositionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);                 //
        hitPositionMarker.transform.position = new Vector3(hitPosition, 0, 0);              //hit ��ġ�� �̵� ��Ų��.
        hitPositionMarker.transform.localScale = new Vector3(0.1f, 10.0f, 1.0f);            //������ ���� �����Ͽ� ���� �����.
    }
}
