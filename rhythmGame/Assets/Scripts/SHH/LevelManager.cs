using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    [System.Serializable]
    public class LevelData
    {
        [SerializeField]
        private int highScore;                  // 최고 점수
        [SerializeField]
        private int clearCount;                 // 클리어 횟수
        [SerializeField]
        private int playCount;                  // 플레이 횟수
        private LevelObject levelObject;        // 이 레벨의 데이터
        
        public LevelData(LevelObject _levelObject)
        {
            levelObject = _levelObject;
        }

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
        public LevelObject GetLevelObject()
        {
            return levelObject;
        }
    }
    public class LevelManager : MonoBehaviour
    {
        public List<LevelObject> levelObjects;              // 게임의 레벨들을 담을 리스트
        private Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();      // 레벨들
        private List<string> levelKeys;

        void Start()
        {

        }
        private void AddLevelDataToDictionary()
        {
            if(levelObjects.Count < 0)
            {
                return;
            }

            for(int i = 0; i < levelObjects.Count; i ++)
            {
                string levelKey = levelObjects[i].levelName.ToString() + "_" + levelObjects[i].difficulty.ToString();

                if(!levels.ContainsKey(levelKey))
                {
                    LevelData levelData = new LevelData(levelObjects[i]);
                    levels.Add(levelKey, levelData);
                    levelKeys.Add(levelKey);
                }
            }
        }
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


    }
}

