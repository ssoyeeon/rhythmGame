using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Sprite[] NoteSprites = new Sprite[3];
    public Note note;               //노트 정보
    public float speed;             //노트 이동 속도 
    public float hitPosition;       //판정 위치 
    public float startTime;         //게임 시작 시간

    public GameObject[] HitCheckEffect = new GameObject[4];

    private NoteManager noteManager;
    private SpriteRenderer noteImage;

    //노트 오브젝ㅌㅡ 초기화

    public void Initialized(Note note, float speed, float hitPosition, float startTime)
    {
        this.note = note;
        this.speed = speed;
        this.hitPosition = hitPosition;
        this.startTime = startTime;

        //노트 초기 위치 설정
        float initalDistance = speed * (note.startTime - (Time.time - startTime));
        transform.position = new Vector3(hitPosition + initalDistance, yPos(note.noteValue), 0);

        //노트 이미지 설정
        noteImage.sprite = NoteSprites[note.noteValue - 1];
    }
    private void Awake()
    {
        noteImage = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        noteManager = NoteManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //노트 이동 
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //판정 위치를 지나면 파괴
        if(transform.position.x <= hitPosition - 1)
        {
            //noteManager.notePoolEnqueue(this);
            noteManager.poolManager.ReturnToPool(this.gameObject);
            NoteManager.instance.scoreManager.AddScore(Timing.Miss);
            //Debug.Log("Miss");
        }
    }

    public void HitCheck(int noteindex)
    {
        float distance = Mathf.Abs(transform.position.x - hitPosition);

        if (distance > 1) return;

        if (note.noteValue == noteindex)
        {
            if (distance < 0.3f)
            {
                Debug.Log("Perfect");

                Instantiate(HitCheckEffect[0], transform.position, HitCheckEffect[0].transform.rotation);
                NoteManager.instance.scoreManager.AddScore(Timing.Perfect);
            }
            else if (distance < 0.5f)
            {
                Debug.Log("Great");
                Instantiate(HitCheckEffect[1], transform.position, HitCheckEffect[1].transform.rotation);
                NoteManager.instance.scoreManager.AddScore(Timing.Great);
            }
            else if (distance < 0.7f)
            {
                Debug.Log("Good");
                Instantiate(HitCheckEffect[2], transform.position, HitCheckEffect[2].transform.rotation);
                NoteManager.instance.scoreManager.AddScore(Timing.Good);
            }
            else
            {
                Debug.Log("Bad");
                Instantiate(HitCheckEffect[3], transform.position, HitCheckEffect[3].transform.rotation);
                NoteManager.instance.scoreManager.AddScore(Timing.Bad);
            }
        }
        else
        {
            //Debug.Log("Miss");
            NoteManager.instance.scoreManager.AddScore(Timing.Miss);
        }
        NoteManager.instance.notePoolEnqueue(this);
    }

    private float yPos(int value)
    {
        if(value == 1) return -1.5f;
        else if(value == 2) return 0.2f;
        else if (value == 3) return -0.65f;

        return 0;
    }
}
