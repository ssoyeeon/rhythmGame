using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //���� �޺� - �ʿ����ʿ���!!
    public NoteManager noteManager;

    public int playerHp;                         //HP�׿�
    public int playerAttack;                     //�����̿���
    public float attackCoolTime;                 //������ �ٷιٷ� �Ǹ� �ȵſ�...
    public int jumpForce;                        //���� �� ��!!!!!!!!!!!
    public float jumpCoolTime;                  //���� ��Ÿ��

    public float maxHigh;                       //���� �� �ִ� ����

    public float startTime;                      //���� �ð� ��Կ�

    public Rigidbody playerRigidbody;            //������ ���� ������ٵ� �����ͺ��Կ�

    public bool isDead;         //�׾�����?
    public bool isStart;        //�����߳���?
    public bool isEnd;          //������ ��������?
    public bool isJumping;      //�ٰ� �ֳ���?
    
    void Start()
    {
        startTime = 3;                  //���� �ð� �� 3�ʱ���
        playerHp = 150;                 //�÷��̾�� Hp�� 150���� ���������
        playerAttack = 20;             //�÷��̾��� ���ݷ��� �ִ� ������ ���߿� ��Ÿ �ϴ� ���̰� ���� �� �ֱ� �����̿���
        attackCoolTime = 0.2f;            //�÷��̾ ��Ÿ�ϸ� �̱����~? �׷� �ȵſ�! �׷��� ������ �ð��� �ٰſ���
        jumpCoolTime = 0.2f;            //�÷��̾ ��� �����ϸ� ��̰� ������~? ���! �׷��� ������ ��Ÿ���� �ݴϴٿ�~~
        jumpForce = 10;                 //�ϴ� �� �𸣰����ϱ� 10 ������ �ຼ�Կ�� 
        
        //�� ģ������ �ϴ� �� ��Ȱ��ȭ ���ݴϴٿ�
        isDead = false;
        isEnd = false;
        isJumping = false;
        isStart = false;
    }

    void Update()
    {
        startTime -= Time.deltaTime;    //���࿩
        if (startTime <= 0)             //0�̰ų� 0���� �Ʒ���
        {
            startTime = 0;              //0���� ������࿩
            isStart = true;             //���� ��ŸƮ �� ���� Ʈ��� �ٲ��ֱ���
        }   

        if(isStart == true)
        {
            GameStart();
            Attack();
            Jump();

            if (playerHp <= 0)
            {
                //Ʈ���� �սô� Ȥ�� �ִϸ��̼� �����Կ�~
                Dead();                     //�׾�� ��~
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                Jump();
                if(attackCoolTime <= 0)
                {
                    Attack();
                    attackCoolTime -= Time.deltaTime;
                } 
            }

            if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
                if (attackCoolTime <= 0)
                {
                    Attack();
                    attackCoolTime -= Time.deltaTime;
                }
            }
        }
    }

    void GameStart()                        //���� �Լ����� �� �����ſ���
    {
        //�Լ� ������!!
    }
    
    void Attack()                           //�÷��̾ ���� �� ��Ÿ���� �˴ϴ�.
    {
        //���� �� ��� �浹ó�� �� �� �����Բ� ����� 
    }    
    void Jump()
    {
        playerRigidbody.AddForce(new Vector3(0, jumpForce, 0));    //���� �� ���� ������ 10�Դϴٿ�
    }

    void Dead()
    {
        isEnd = true;                                        //������ ��������
        isDead = true;                                      //�׾����
        SceneManager.LoadScene("DeadScene");                //�׾��� �� �� ��ȯ
        playerRigidbody.velocity = Vector3.zero;            //�������ϱ��.
    }

    public void OnTriggerEnter(Collider other)
    {
        //������ �����Ѱ� �׳� Ʈ���ŷ� �浹 �����ϱ⿡�� !>!
        //������ ������°� ��Ʈ�ۿ� �����ϱ� ����~!
    }
}
