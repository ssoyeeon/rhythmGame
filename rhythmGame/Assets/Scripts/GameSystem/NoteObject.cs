using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Note note;               //��Ʈ ������
    public float speed;             //��Ʈ �̵��� �Ӥ��� 
    public float hitPosition;       //������ ��ġ 
    public float startTime;         //���̤� ���ڤ� �ð�

    public float attackCoolTime;    //���� ��Ÿ��

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
        attackCoolTime = 0;
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
        
        //��Ʈ �ı�!
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            if (transform.position.y > 5 && attackCoolTime == 0)
            {
                if (transform.position.x >= -5.5 && transform.position.x <= -4.5)
                {
                    Debug.Log("Perfect");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
                else if (transform.position.x > -4.5 && transform.position.x < -4)
                {
                    Debug.Log("Great");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
                else if (transform.position.x > -4 && transform.position.x < -3.5)
                {
                    Debug.Log("Bad");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            if (transform.position.y < 3)
            {
                if (transform.position.x >= -5.5 && transform.position.x <= -4.5)
                {
                    Debug.Log("Perfect");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
                else if (transform.position.x > -4.5 && transform.position.x < -4)
                {
                    Debug.Log("Great");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
                else if (transform.position.x > -4 && transform.position.x < -3.5)
                {
                    Debug.Log("Bad");
                    Destroy(gameObject);
                    attackCoolTime = 0.3f;
                    attackCoolTime -= Time.deltaTime;
                }
            }
        }
    }
}
