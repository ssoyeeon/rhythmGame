using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParamBehaviour : StateMachineBehaviour  //������Ʈ �ӽ� Bechaviouer 
{
    public string parameter = "IsAttacking";                    //�ִϸ��̼����� ������ �Ķ���� ���� ����
    public bool IsTrue = false;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�ִϸ��̼��� ����ǰ� ��ȯ�Ǵ� �������� ������ �ִϸ��̼� �Ķ���� ���� ture -> false ��Ų��. 
        animator.SetBool(parameter, IsTrue);
    }
}
