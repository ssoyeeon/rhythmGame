using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{
    [Header("��ü �ӵ�")] public float Speed = 2;

    [Header("�ϴ� �ӵ� ����")][Range(0f, 1f)]public float SkySpeedScale = 0.3f;
    [Header("���� �ӵ� ����")][Range(0f, 1f)] public float CloudSpeedScale = 0.6f;
    [Header("�� �� ��Ÿ�� �ӵ� ����")][Range(0f, 1f)] public float GroundSpeedScale = 1f;

    [Header("�ϴ� �̹���")] public GameObject[] SkyObject = new GameObject[2];
    [Header("���� �̹���")] public GameObject[] CloudObject = new GameObject[2];
    [Header("�� �� ��Ÿ�� �̹���")] public GameObject[] GroundObject = new GameObject[2];


    // Update is called once per frame
    void Update()
    {
        Vector3 speed = Vector3.left * Speed * Time.deltaTime;
        BackGroundMove(SkyObject[0], speed, SkySpeedScale);
        BackGroundMove(SkyObject[1], speed, SkySpeedScale);
        BackGroundMove(CloudObject[0], speed, CloudSpeedScale);
        BackGroundMove(CloudObject[1], speed, CloudSpeedScale);
        BackGroundMove(GroundObject[0], speed, GroundSpeedScale);
        BackGroundMove(GroundObject[1], speed, GroundSpeedScale);

    }

    private void BackGroundMove(GameObject BG, Vector3 speed, float SpeedScale)
    {
        BG.transform.Translate(speed * SpeedScale);
        if(BG.transform.position.x <= -12.5f)
        {
            BG.transform.position = new Vector3(12.5f, 0, 0);
        }
    }
}
