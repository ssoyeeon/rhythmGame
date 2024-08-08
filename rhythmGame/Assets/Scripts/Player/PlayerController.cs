using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //���� �޺� - �ʿ����ʿ���!!
    public NoteManager noteManager;

    public Note note;

    public GameObject player;

    public int playerHp;                         //HP�׿�
    public int playerAttack;                     //�����̿���
    public float attackCoolTime;                 //������ �ٷιٷ� �Ǹ� �ȵſ�...
    public int jumpPosition;                    //���� �� ��!!!!!!!!!!!
    public float jumpCoolTime;                  //���� ��Ÿ��

    public float maxHigh;                       //���� �� �ִ� ����

    public float startTime;                      //���� �ð� ��Կ�

    public Rigidbody playerRigidbody;            //������ ���� ������ٵ� �����ͺ��Կ�

    public bool isDead;         //�׾�����?
    public bool isStart;        //�����߳���?
    public bool isEnd;          //������ ��������?
    public bool isJumping;      //�ٰ� �ֳ���?

    public Vector3 bad = new Vector3(-4.6f, -4.6f);     //�̷��� �ϸ� ����... ������... �ϴ�... ����... ��û�̿���.... x ��ǥ�� �ؾ���Ӥ��� ���Ť� ���ŤǤ���������Ǥ��Ĥ� �������ʤ����о� �������äǤ� 
    public Vector3 great = new Vector3(-4.7f, -4f);
    public Vector3 perfect = new Vector3(-5.6f, -4.8f);
    
    void Start()
    {
        startTime = 3;                  //���� �ð� �� 3�ʱ���
        playerHp = 150;                 //�÷��̾�� Hp�� 150���� ���������
        playerAttack = 20;             //�÷��̾��� ���ݷ��� �ִ� ������ ���߿� ��Ÿ �ϴ� ���̰� ���� �� �ֱ� �����̿���
        attackCoolTime = 0f;            //�÷��̾ ��Ÿ�ϸ� �̱����~? �׷� �ȵſ�! �׷��� ������ �ð��� �ٰſ���
        jumpCoolTime = 0f;            //�÷��̾ ��� �����ϸ� ��̰� ������~? ���! �׷��� ������ ��Ÿ���� �ݴϴٿ�~~
        jumpPosition = 6;                 //�ϴ� �� �𸣰����ϱ� 10 ������ �ຼ�Կ�� 
        
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
         
            if (playerHp <= 0)
            {
                //Ʈ���� �սô� Ȥ�� �ִϸ��̼� �����Կ�~
                Dead();                     //�׾�� ��~
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                if(jumpCoolTime <= 0)
                {
                    Jump();
                    Attack();
                    jumpCoolTime = 0.2f;                //���� ��Ÿ�� �ݴϴٿ��
                }
            }
            jumpCoolTime -= Time.deltaTime;             //���� ��Ÿ�� ���� ����~~

            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
                if (attackCoolTime <= 0)
                {
                    Attack();                           //���� ����
                    attackCoolTime = 0.2f;              //���� ��Ÿ���� �ݴϴٿ��
                }
            }
            attackCoolTime -= Time.deltaTime;   //���� ��Ÿ�� ���� ����
        }
    }

    void GameStart()                        //���� �Լ����� �� �����ſ���
    {
        //�Լ� ������!!
    }
    
    void Attack()                           //�÷��̾ ���� �� ��Ÿ���� �˴ϴ�.
    {

    }    

    void Jump()
    {
        player.transform.position = new Vector3(-7, jumpPosition, 0);       //���� ����
        playerRigidbody.velocity = Vector3.zero;                            //���� �߷� ��������
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
