using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public PlayerController playercontroller;

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
    private List<Note> activeNotes = new List<Note>();  //������ �����ä����� �ʤ��Ѥ� ��Ʈ ����Ʈ 
    private float spawnOffset;                          //��� ���� �ð� ������

    public bool debugMode = false;                      //����� ��� �÷���
    public GameObject hitPositionMarker;                //���� ��ġ ��Ŀ ������Ʈ

    public float initialDelay = 3f;                     //�ʱ� ���� �ð�

    //���� �ʱ�ȭ
    public void Initialized()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;                           //���� �ð��� ���� �ð���ŭ �̷�
        startTime = Time.time + initialDelay;                   //List ���� �ʱ�ȭ clear ���ִ� ���� ���� 
        activeNotes.Clear();
        activeNotes.AddRange(notes);
        spawnOffset = (10 - hitPosition) / noteSpeed;           //��Ʈ ���� �ð� ������ ���

        playercontroller.notes = this.notes;                    //��Ʈ �÷��̾����� ������

        if (debugMode)
        {
            CreateHitPositionMarker();
        }    

        StartCoroutine(StartAudioWithDelay());                  //���� �� ����� ��� �ڷ�ƾ ����
    }



    //���� �� ����� ����� ���� �ڷ�ƾ
    private IEnumerator StartAudioWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        audioSource.Play();
    }

    void Update()
    {
        float currentTime = Time.time -  startTime;     //���� ���� �ð��� ���
        
        //Ȱ��ȭ�� ��Ʈ�� ó��
        for(int i = activeNotes.Count -1; i >= 0; i--)
        {
            Note note = activeNotes[i];
            if(currentTime >= note.startTime - spawnOffset && currentTime < note.startTime + note.duration)
            {
                SpawnNoteObject(note);
                activeNotes.RemoveAt(i);
            }
            else if(currentTime >= note.startTime + note.duration)
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

    }

    //����� ���� �ð� ����
    public void AdjustAudioLatency(float latency)
    {
        audioLatency = latency;
    }

    //����׿� ���� ��ġ ��Ŀ ����
    private void CreateHitPositionMarker()
    {
        hitPositionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);                 //
        hitPositionMarker.transform.position = new Vector3(hitPosition, 0, 0);              //hit ��ġ�� �̵� ��Ų��.
        hitPositionMarker.transform.localScale = new Vector3(0.1f, 10.0f, 1.0f);            //������ ���� �����Ͽ� ���� �����.
    }
}
