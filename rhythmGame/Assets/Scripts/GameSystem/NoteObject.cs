using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Note note;               //노트 정ㅂㅗ
    public float speed;             //노트 이도ㅇ 속ㄷㅗ 
    public float hitPosition;       //판저ㅇ 위치 
    public float startTime;         //게이ㅁ 시자ㄱ 시간

    public float perfect1 = 3f;
    public float perfect2 = 1f;

    //노트 오브젝ㅌㅡ 초기화

    public void Initialized(Note note, float spped, float hitPosition, float startTime)
    {
        this.note = note;
        this.speed = spped;
        this.hitPosition = hitPosition;
        this.startTime = startTime;

        //노트 초기 위치 설ㅈㅓㅇ 
        float initalDistance = spped * (note.startTime - (Time.time - startTime));
        transform.position = new Vector3(hitPosition + initalDistance, note.trackIndex * 2, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //노트 이도ㅇ 
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //판정 위치를 지나면 파괴
        if(transform.position.x < hitPosition - 1)
        {
            Destroy(gameObject);
        }
        
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            if (transform.position.y > 5)
            {
                if ( transform.position.x >= -5.5 && transform.position.x <= -5)
                {
                    Debug.Log("Perfect");
                }
                else if (transform.position.x > -5 && transform.position.x < -4.5)
                {
                    Debug.Log("Great");
                }
                else if (transform.position.x > -4.5 && transform.position.x < -4)
                {
                    Debug.Log("Bad");
                }
            }
        }
    }
}
