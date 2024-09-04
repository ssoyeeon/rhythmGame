using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float AttackCoolTime = 0.3f;

    public Animator animator;
    public Rigidbody2D m_rigidbody;

    private NoteManager noteManager;
    private float InputCurrentTime = 0.1f;      //���� �Է����� �ν��ϴ� �ð�

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
        if(animator.GetBool("Grounded"))
        {
            //transform.position = new Vector3(-3.8f, -2.17f, 0);
        }
        else
        {
            //transform.position = new Vector3(-3.8f, -0.64f, 0);
        }

        if (AttackCoolTime > 0)
        {
            AttackCoolTime -= Time.deltaTime;
            return;
        }

        //��Ʈ �ı�!
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
        animator.SetBool("Attack", true);
        int trackIndex = 0;
        if(LeftInput && RightInput)
        {
            trackIndex = 3;
            animator.SetBool("IsJumpping", true);
            animator.SetBool("Grounded", false);
            transform.position = new Vector3(-3.8f, -0.64f, 0);
        }
        else if(LeftInput)
        {
            trackIndex = 2;
            animator.SetBool("IsJumpping", true);
            animator.SetBool("Grounded", false);
            transform.position = new Vector3(-3.8f, -0.94f, 0);
        }
        else if (RightInput)
        {
            trackIndex = 1;
            animator.SetBool("IsJumpping", false);
            animator.SetBool("Grounded", true);
            transform.position = new Vector3(-3.8f, -2.17f, 0);
        }

        m_rigidbody.velocity = Vector2.zero;

        LeftInput = false;
        RightInput = false;
        AttackCoolTime = 0.3f;
        InputCurrentTime = 0.1f;

        if (noteManager.nowNotes.Count == 0) return;
        noteManager.nowNotes[0].HitCheck(trackIndex);
    }
}
