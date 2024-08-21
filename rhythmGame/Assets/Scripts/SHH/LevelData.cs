using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RhythmGame
{
    [CreateAssetMenu(fileName = "LevelObject", menuName = "ScriptableObjects/LevelObject", order = 1)]
    public class LevelObject : ScriptableObject
    {
        public string levelName;            // ������ �̸�
        [Range (0, 100)] 
        public int unlockLevel;             // �ر� ����
        [Range(1, 20)] 
        public int difficulty;              // ������ ���̵�
        public NoteObject noteData;         // ��Ʈ ������
        public List<ClearReward> clearRewards;  // Ŭ��� ���� ����
    }
    [System.Serializable]
    public class ClearReward
    {
        public int clearCount;              // Ŭ���� Ƚ��
        // ������ ���� �߰� ����
    }
}

