using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //���� �޺� - �ʿ����ʿ���!!

    public int Hp;              //HP�׿�
    public int Attack;          //�����̿���
    public float AttackSpeed;   //������ �ٷιٷ� �Ǹ� �ȵſ�...

    public float startTime;     //���� �ð� ��Կ�

    public Rigidbody playerRigidbody;        //������ ���� ������ٵ� �����ͺ��Կ�

    public bool isDead;         //�׾�����?
    public bool isStart;        //�����߳���?
    public bool isEnd;          //������ ��������?
    public bool isJumping;      //�ٰ� �ֳ���?
    
    void Start()
    {
        startTime = 3;                  //���� �ð� �� 3�ʱ���
        Hp = 150;                       //�÷��̾�� Hp�� 150���� ���������
        Attack = 20;                    //�÷��̾��� ���ݷ��� �ִ� ������ ���߿� ��Ÿ �ϴ� ���̰� ���� �� �ֱ� �����̿���
        AttackSpeed = 0.2f;             //�÷��̾ ��Ÿ�ϸ� �̱����~? �׷� �ȵſ�! �׷��� ������ �ð��� �ٰſ���
    }

    void Update()
    {
        startTime -= Time.deltaTime;    //���࿩
        if (startTime <= 0)             //0�̰ų� 0���� �Ʒ���
        {
            startTime = 0;              //0���� ������࿩
            isStart = true;             //���� ��ŸƮ �� ���� Ʈ��� �ٲ��ֱ���
        }   

        if(Hp <= 0)
        {
            //Ʈ���� �սô� Ȥ�� �ִϸ��̼� �����Կ�~
            Dead();                     //�׾�� ��~
        }
    }

    void GameStart()
    {
        //�Լ� ������!!
    }

    void Jump()
    {
        playerRigidbody.AddForce(new Vector3(0, 10, 0));
    }

    void Dead()
    {
        isEnd = true;                                        //������ ��������
        isDead = true;                                      //�׾����
        SceneManager.LoadScene("DeadScene");                //�׾��� �� �� ��ȯ
        playerRigidbody.velocity = Vector3.zero;            //�������ϱ��.
    }
}
