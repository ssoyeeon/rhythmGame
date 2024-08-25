using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RhythmGame
{
    /// <summary>
    /// ������ �����͸� ������
    /// </summary>
    [System.Serializable]
    public class LevelData
    {
        [SerializeField]
        private int highScore;                  // �ְ� ����
        [SerializeField]
        private int clearCount;                 // Ŭ���� Ƚ��
        [SerializeField]
        private int playCount;                  // �÷��� Ƚ��
        private LevelObject levelObject;        // �� ������ ������
        
        public LevelData(LevelObject _levelObject)
        {
            levelObject = _levelObject;
        }
        /// <summary>
        /// ���̽��ھ�, Ŭ����ī��Ʈ, �÷��� ī��Ʈ ����
        /// </summary>
        /// <param name="dataName">highScore, clearCount, playCount</param>
        /// <returns>�ش��ϴ� ���� ��ȯ</returns>
        public int GetValue(string dataName)
        {
            switch(dataName)
            {
                case nameof(highScore):
                    return highScore;
                case nameof(clearCount):
                    return clearCount;
                case nameof(playCount):
                    return playCount;
                default:
                    return -1;
            }
        }
        /// <summary>
        /// ��Ʈ �����͸� ��ȯ��
        /// </summary>
        /// <returns>LevelObject�� ��ȯ��</returns>
        public LevelObject GetLevelObject()
        {
            return levelObject;
        }
    }

    public class LevelManager : MonoBehaviour
    {
        public List<LevelObject> levelObjects;              // ������ �������� ���� ����Ʈ
        private Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();      // ������
        private List<string> levelKeys = new List<string>();
        public Button[] Buttons;                            // �߰��Ϸ��� ���⿡
        private List<Button> levelButtons = new List<Button>();          // ���� ��ư��
        private int levelIndex;
        private int levelCount;

        void Awake()
        {
            if (levelObjects.Count != 0 || levelObjects != null)
            {
                AddLevelDataToDictionary();
            }

            SetLevelButtons(Buttons);
        }
        /// <summary>
        /// ��ư�� �����ϴ� �Լ�
        /// ��ư�� ���� ������ ������ ���� ��� ���� ��ư���� ��Ȱ��ȭ ��
        /// </summary>
        /// <param name="buttons">������ �ҷ��� ��ư��</param>
        public void SetLevelButtons(Button[] buttons)
        {
            if(levelButtons.Count == 0 || levelButtons == null)
            {
                foreach (var button in buttons)
                {
                    levelButtons.Add(button);
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    if(!levelButtons.Contains(button))
                    {
                        levelButtons.Add(button);
                    }
                }
            }

            for(int i = 0; i < levelButtons.Count; i ++)
            {
                if(levels.Count <= i)
                {
                    levelButtons[i].gameObject.SetActive(false);
                }
                else
                {
                    levelButtons[i].transform.GetChild(0).GetComponent<Text>().text = levels[levelKeys[i]].GetLevelObject().levelName;
                    levelButtons[i].onClick.AddListener(() =>StartLevel(levels[levelKeys[i]]));

                    if(levels[levelKeys[i]].GetLevelObject().LevelImage)
                    {
                        levelButtons[i].image.sprite = levels[levelKeys[i]].GetLevelObject().LevelImage;
                    }
                }

            }
        }
        public void StartLevel(LevelData levelData)
        {
            Debug.Log("���� ����!");
            // �߰�����
        }
        /// <summary>
        /// ���������͸� ��ųʸ����� ã�ƿ�
        /// </summary>
        /// <param name="levelKey">�ַ� ����� Ű�� Ű ����Ʈ���� ����</param>
        /// <param name="levelName">������ �̸��� ���̵��� ã���� ��</param>
        /// <param name="levelDifficulty">������ �̸��� ���̵��� ã���� ��</param>
        /// <returns>��ųʸ����� ���� �����͸� ��ȯ��</returns>
        public LevelData GetLevelDataFromDictionary(string levelKey, string levelName = null, int levelDifficulty = -1)
        {
            if(levelKey == null)
            {
                if(levelName == null || levelDifficulty == -1)
                {
                    return null;
                }

                levelKey = levelName + "_" + levelDifficulty.ToString();
            }

            if(levelKeys.Contains(levelKey))
            {
                return levels[levelKey];
            }

            return null;
        }

        /// <summary>
        /// ���� �����͵��� ��ųʸ��� ����
        /// ó�� �ѹ��� �����ϴ� �Լ���
        /// </summary>

        private void AddLevelDataToDictionary()
        {
            if (levelObjects.Count == 0)
            {
                return;
            }

            for (int i = 0; i < levelObjects.Count; i++)
            {
                string levelKey = levelObjects[i].levelName.ToString() + "_" + levelObjects[i].difficulty.ToString();

                if (!levels.ContainsKey(levelKey))
                {
                    LevelData levelData = new LevelData(levelObjects[i]);
                    levels.Add(levelKey, levelData);
                    levelKeys.Add(levelKey);
                    Debug.Log(levelData.GetLevelObject().levelName.ToString() + "is Added in Dictionary");
                }
            }

            levelCount = levels.Count;
        }
        /// <summary>
        /// �ε����� ���� ȣ���Ҷ� �����
        /// �ε����� ���� ������ ������� 1234 ���ư� �ε����� �� Ŭ���� ���� �ִ� �ε�����
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns>��ųʸ��� �ִ� ���� �����͸� ��ȯ�ؿ�</returns>
        private LevelData GetLevelDataByIndex(Enums.Modifier modifier)
        {
            if(modifier != Enums.Modifier.NONE)
            {
                if (modifier == Enums.Modifier.POSITIVE)
                {
                    levelIndex++;

                    if (levelIndex >= levelCount)
                    {
                        levelIndex = 0;
                    }
                }
                else
                {
                    levelIndex--;

                    if(levelIndex < 0)
                    {
                        levelIndex = levelCount - 1;
                    }
                }
            }

            return levels[levelKeys[levelIndex]];
        }
    }
}

