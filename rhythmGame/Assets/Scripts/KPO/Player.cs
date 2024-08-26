using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float AttackCoolTime = 0.3f;

    private NoteManager noteManager;
    private float InputCurrentTime = 0.1f;      //동시 입력으로 인식하는 시간

    private bool LeftInput = false;
    private bool RightInput = false;

    // Start is called before the first frame update
    void Start()
    {
        noteManager = NoteManager.instance;   
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackCoolTime > 0)
        {
            AttackCoolTime -= Time.deltaTime;
            return;
        }

        //노트 파괴!
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            LeftInput = true;
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            RightInput = true;
        }

        if(LeftInput || RightInput)
        {
            InputCurrentTime -= Time.deltaTime;
        }

        if(InputCurrentTime < 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        int trackIndex = 0;
        if(LeftInput && RightInput)
        {
            trackIndex = 3;
            Debug.Log("동시입력");
        }
        else if(LeftInput)
        {
            trackIndex = 1;
            Debug.Log("왼쪽입력");
        }
        else if (RightInput)
        {
            trackIndex = 2;
            Debug.Log("오른쪽입력");
        }

        LeftInput = false;
        RightInput = false;
        AttackCoolTime = 0.3f;
        InputCurrentTime = 0.1f;

        if (noteManager.nowNotes.Count == 0) return;
        noteManager.nowNotes[0].HitCheck(trackIndex);
    }
}
