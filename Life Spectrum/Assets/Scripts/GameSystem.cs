using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace LIFESPECTRUM
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameSystem))]
    public class GameSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameSystem gameSystem = (GameSystem)target;

            if (GUILayout.Button("Load Story"))
            {
                gameSystem.LoadStoryObject();
            }
            else if(GUILayout.Button("Pick Cards"))
            {
                gameSystem.PickStory();
            }
            else if (GUILayout.Button("Reset Storys"))
            {
                gameSystem.ResetStory();
            }
        }

    }
#endif
    public class GameSystem : MonoBehaviour
    {
        public static GameSystem Instance;
        private GameManager gameManager;
        public List<StoryObject> storyObjects;
        public List<StoryObject> pickedStorys;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }
        List<StoryObject> PickStoryObjects(int amount)
        {
            List<StoryObject> storyTemp = new List<StoryObject>();

            if(amount > storyObjects.Count)
            {
                amount = storyObjects.Count;
            }

            foreach(StoryObject so in storyObjects)
            {
                if(storyTemp.Count < amount)
                {
                    if (so.hasPreviousStory == false)
                    {
                        bool addStory = true;
                        if (so.statMins.Count > 0)
                        {
                            for(int i = 0; i < so.statMins.Count; i++)
                            {
                                if (StatMinChack(so.statMins[i].StatType, so.statMins[i].Amount) == false)
                                {
                                    addStory = false;
                                    break;
                                }
                            }
                        }

                        if (addStory == true)
                        {
                            storyTemp.Add(so);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return ShuffleStory(storyTemp);
        }

        public List<StoryObject> ShuffleStory(List<StoryObject> list, StoryObject addStory = null)
        {
            List<StoryObject> shuffled = new List<StoryObject>();

            foreach(StoryObject so in list)
            {
                shuffled.Add(so);
            }

            if (addStory != null)
                shuffled.Add(addStory);

            System.Random rng = new System.Random();
            int n = list.Count;

            while(n > 1)
            {
                int o = rng.Next(n + 1);
                var story = shuffled[o];
                shuffled[o] = shuffled[n];
                shuffled[n] = story;
            }

            return shuffled;
        }

        bool StatMinChack(Enums.PlayerStats stat, int amount)
        {
            switch(stat)
            {
                case Enums.PlayerStats.Intelligence :
                    if(gameManager.stats.statIntelligence >= amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Strength:
                    if (gameManager.stats.statStrength >= amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Personality:
                    if (gameManager.stats.statPersonality >= amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Age:
                    if (gameManager.stats.age >= amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Money:
                    if (gameManager.stats.statMoney >= amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }


#if UNITY_EDITOR
        [ContextMenu("Load Story")]
        public void LoadStoryObject()
        {
            storyObjects = new List<StoryObject>();

            var _story = Resources.LoadAll<StoryObject>("");

            foreach(StoryObject so in _story)
            {
                storyObjects.Add(so);
            }
        }
        [ContextMenu("PickCards")]
        public void PickStory()
        {
            if(storyObjects.Count <= 0)
            {
                Debug.LogWarning("�̺� ���丮�� ���� �ε��϶��");
            }
            else
            {
                pickedStorys = PickStoryObjects(2);
            }

        }
        [ContextMenu("ResetStorys")]
        public void ResetStory()
        {
            storyObjects.Clear();
            pickedStorys.Clear();
        }
#endif
    }
}
