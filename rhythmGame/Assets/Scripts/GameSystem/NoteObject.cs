using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Note note;               //��Ʈ ����
    public float speed;             //��Ʈ �̵� �ӵ� 
    public float hitPosition;       //���� ��ġ 
    public float startTime;         //���� ���� �ð�

    private NoteManager noteManager;

    //��Ʈ ���������� �ʱ�ȭ

    public void Initialized(Note note, float speed, float hitPosition, float startTime)
    {
        this.note = note;
        this.speed = speed;
        this.hitPosition = hitPosition;
        this.startTime = startTime;

        //��Ʈ �ʱ� ��ġ ����
        float initalDistance = speed * (note.startTime - (Time.time - startTime));
        transform.position = new Vector3(hitPosition + initalDistance, yPos(note.noteValue), 0);
    }
    void Start()
    {
        noteManager = NoteManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //��Ʈ �̵� 
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //���� ��ġ�� ������ �ı�
        if(transform.position.x < hitPosition - 1)
        {
            noteManager.notePoolEnqueue(this);
            noteManager.scoreManager.ResetCombo();
            Debug.Log("Miss");
        }
    }

    public void HitCheck(int noteindex)
    {
        float distance = Mathf.Abs(transform.position.x - hitPosition);

        if (distance > 1) return;

        if (note.noteValue == noteindex)
        {
            if (distance < 0.5f)
            {
                Debug.Log("Perfect");
                NoteManager.instance.scoreManager.AddScore(10);
            }
            else if (distance < 0.7f)
            {
                Debug.Log("Great");
                NoteManager.instance.scoreManager.AddScore(5);
            }
            else
            {
                Debug.Log("Bad");
                NoteManager.instance.scoreManager.AddScore(1);
            }
        }
        else
        {
            Debug.Log("Miss");
            NoteManager.instance.scoreManager.ResetCombo();
        }
        NoteManager.instance.notePoolEnqueue(this);
    }

    private float yPos(int value)
    {
        if(value == 1) return -2;
        else if(value == 2) return 2;
        else if (value == 3) return 0;

        return 0;
    }
}
