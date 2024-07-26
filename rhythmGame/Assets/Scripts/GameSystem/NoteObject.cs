using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Note note;               //��Ʈ ������
    public float speed;             //��Ʈ �̵��� �Ӥ��� 
    public float hitPosition;       //������ ��ġ 
    public float startTime;         //���̤� ���ڤ� �ð�

    //��Ʈ ���������� �ʱ�ȭ

    public void Initialized(Note note, float spped, float hitPosition, float startTime)
    {
        this.note = note;
        this.speed = spped;
        this.hitPosition = hitPosition;
        this.startTime = startTime;

        //��Ʈ �ʱ� ��ġ �����ä� 
        float initalDistance = spped * (note.startTime - (Time.time - startTime));
        transform.position = new Vector3(hitPosition + initalDistance, note.trackIndex * 2, 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��Ʈ �̵��� 
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //���� ��ġ�� ������ �ı�
        if(transform.position.x < hitPosition - 1)
        {
            Destroy(gameObject);
        }
    }
}
