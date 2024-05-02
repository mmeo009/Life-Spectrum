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
        public List<Debuff> myDebuffs;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }
        private void ChangePlayerStat(Stat stat)
        {
            if (stat.isMaxAmount == true)
            {
                switch (stat.StatType)
                {
                    case Enums.PlayerStats.Intelligence:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.maxIntelligence += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.maxIntelligence -= stat.amount;

                            if (gameManager.stats.maxIntelligence < 1)
                            {
                                gameManager.stats.maxIntelligence = 1;
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.maxIntelligence = stat.amount;
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Strength:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.maxStrength += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.maxStrength -= stat.amount;

                            if (gameManager.stats.maxStrength < 1)
                            {
                                gameManager.stats.maxStrength = 1;
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.maxStrength = stat.amount;
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Personality:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.maxPersonality += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.maxPersonality -= stat.amount;

                            if (gameManager.stats.maxPersonality < 1)
                            {
                                gameManager.stats.maxPersonality = 1;
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.maxIntelligence = stat.amount;
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Money:
                        Debug.LogError("존재 하지 않아!");
                        break;
                    case Enums.PlayerStats.Age:
                        Debug.LogError("존재 하지 않아!");
                        break;
                    default:
                        Debug.LogError("존재 하지 않아!");
                        break;
                }
            }
            else
            {
                switch (stat.StatType)
                {
                    case Enums.PlayerStats.Intelligence:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.statIntelligence += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statIntelligence -= stat.amount;

                            if (gameManager.stats.statIntelligence <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.Intelligence);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statIntelligence = stat.amount;

                            if (gameManager.stats.statIntelligence <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.Intelligence);
                            }
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Strength:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.statStrength += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statStrength -= stat.amount;

                            if (gameManager.stats.statStrength <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statStrength);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statStrength = stat.amount;

                            if (gameManager.stats.statStrength <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statStrength);
                            }
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Personality:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.statPersonality += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statPersonality -= stat.amount;

                            if (gameManager.stats.statPersonality <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statPersonality);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statPersonality = stat.amount;

                            if (gameManager.stats.statPersonality <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statPersonality);
                            }
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Money:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.statMoney += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statMoney -= stat.amount;

                            if (gameManager.stats.statMoney <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statMoney);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statMoney = stat.amount;

                            if (gameManager.stats.statMoney <= 0)
                            {
                                // TODO : PlayerDie(Enums.PlayerStats.statMoney);
                            }
                        }
                        else
                        {
                            Debug.LogError("존재 하지 않아!");
                        }
                        break;
                    case Enums.PlayerStats.Age:
                        Debug.LogError("존재 하지 않아!");
                        break;
                    default:
                        Debug.LogError("존재 하지 않아!");
                        break;
                }
            }
        }
        private void AddMyDebuff(Debuff debuff)
        {
            if (myDebuffs == null) myDebuffs = new List<Debuff>();

            var df = myDebuffs.Find(myDebuff => myDebuff.debuffName == debuff.debuffName);

            if(df != null)
            {
                if(df.debuffType == debuff.debuffType)
                {
                    df.amountOfTime += debuff.amountOfTime;
                }
                else
                {
                    myDebuffs.Add(debuff);
                }
            }
            else
            {
                myDebuffs.Add(debuff);
            }
        }
        private void DebuffPerYear()
        {
            List<Debuff> debuffsPerYear = new List<Debuff>();

            foreach(Debuff debuff in myDebuffs)
            {
                if (debuff.debuffType == Enums.DebuffType.PerYear) debuffsPerYear.Add(debuff);
            }
            
            if(debuffsPerYear.Count > 0)
            {
                foreach(Debuff debuff in debuffsPerYear)
                {
                    for (int i = 0; i < debuff.stat.Count; i++)
                    {
                        ChangePlayerStat(debuff.stat[i]);
                    }

                    debuff.amountOfTime -= 1;

                    if(debuff.amountOfTime <= 0)
                    {
                        myDebuffs.Remove(debuff);
                    }
                }
            }
        }
        private List<StoryObject> PickStoryObjects(int amount)
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

        private List<StoryObject> ShuffleStory(List<StoryObject> list, StoryObject addStory = null)
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
        private bool StatMinChack(Enums.PlayerStats stat, int amount)
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
                Debug.LogWarning("이봐 스토리를 먼저 로드하라뀨");
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
