using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //점수 콤보 - 필요행필요행!!

    public int Hp;              //HP네여
    public int Attack;          //공격이에여
    public float AttackSpeed;   //어택이 바로바로 되면 안돼여...

    public float startTime;     //시작 시간 잴게여

    public Rigidbody playerRigidbody;        //점프를 위한 리지드바디를 가져와볼게요

    public bool isDead;         //죽었나여?
    public bool isStart;        //시작했나여?
    public bool isEnd;          //게임이 끝났나여?
    public bool isJumping;      //뛰고 있나여?
    
    void Start()
    {
        startTime = 3;                  //시작 시간 전 3초구여
        Hp = 150;                       //플레이어에게 Hp를 150으로 지정해줘요
        Attack = 20;                    //플레이어의 공격력을 주는 이유는 나중에 연타 하는 아이가 생길 수 있기 때문이에여
        AttackSpeed = 0.2f;             //플레이어가 연타하면 이기겠죠~? 그럼 안돼요! 그렇기 때문에 시간을 줄거에여
    }

    void Update()
    {
        startTime -= Time.deltaTime;    //빼줘여
        if (startTime <= 0)             //0이거나 0보다 아래면
        {
            startTime = 0;              //0으로 만들어줘여
            isStart = true;             //게임 스타트 불 값을 트루로 바꿔주구여
        }   

        if(Hp <= 0)
        {
            //트위닝 합시당 혹은 애니메이션 넣을게요~
            Dead();                     //죽어라 얍~
        }
    }

    void GameStart()
    {
        //함수 총집합!!
    }

    void Jump()
    {
        playerRigidbody.AddForce(new Vector3(0, 10, 0));
    }

    void Dead()
    {
        isEnd = true;                                        //게임이 끝났구요
        isDead = true;                                      //죽었어요
        SceneManager.LoadScene("DeadScene");                //죽었을 때 씬 전환
        playerRigidbody.velocity = Vector3.zero;            //끝났으니까요.
    }
}
