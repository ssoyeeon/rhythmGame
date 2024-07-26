using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    public int trackIndex;          //��Ʈ�� ���� �ε���
    public float startTime;         //��Ʈ ���� �ð�
    public float duration;          //��Ʈ ���� �ð�

    public Note(int trackIndex, float startTime, float duration)
    {
        this.trackIndex = trackIndex;
        this.startTime = startTime;
        this.duration = duration;
    }
}
