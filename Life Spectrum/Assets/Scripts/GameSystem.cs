using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace LIFESPECTRUM
{

    public class GameSystem : MonoBehaviour
    {


        [SerializeField] private GameManager gameManager;

        [SerializeField] private List<StoryObject> storyObjects;
        [SerializeField] private List<StoryObject> endStorys;
        [SerializeField] private List<StoryObject> pickedStorys;
        [SerializeField] private int nowStoryNum;

        [SerializeField] private List<Debuff> myDebuffsPerSec = new List<Debuff>();
        [SerializeField] private float timer;
        [SerializeField] private string filePath;


        [HideInInspector] public static GameSystem Instance { get; private set; }

        public List<Debuff> myDebuffs;
        public Option[] nowOptions;
        public StoryObject nowStory;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
        }
        private void Update()
        {
            if (myDebuffsPerSec.Count > 0)
            {
                DebuffPerSec();
            }
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

                            if (gameManager.stats.maxIntelligence < 2)
                            {
                                gameManager.stats.maxIntelligence = 2;
                                gameManager.stats.statIntelligence = 1;
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

                            if (gameManager.stats.maxStrength < 2)
                            {
                                gameManager.stats.maxStrength = 2;
                                gameManager.stats.statStrength = 1;
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

                            if (gameManager.stats.maxPersonality < 2)
                            {
                                gameManager.stats.maxPersonality = 2;
                                gameManager.stats.statPersonality = 1;
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.maxPersonality = stat.amount;

                            if (gameManager.stats.maxPersonality < 2)
                            {
                                gameManager.stats.maxPersonality = 2;
                                gameManager.stats.statPersonality = 1;
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
                            gameManager.stats.maxMoney += stat.amount;
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.maxMoney -= stat.amount;

                            if (gameManager.stats.maxMoney < 2)
                            {
                                gameManager.stats.maxMoney = 2;
                                gameManager.stats.statMoney = 1;
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.maxMoney = stat.amount;

                            if (gameManager.stats.maxMoney < 2)
                            {
                                gameManager.stats.maxMoney = 2;
                                gameManager.stats.statMoney = 1;
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
            else
            {
                switch (stat.StatType)
                {
                    case Enums.PlayerStats.Intelligence:
                        if (stat.Method == Enums.ActonMethod.Add)
                        {
                            gameManager.stats.statIntelligence += stat.amount;

                            if (gameManager.stats.statIntelligence >= gameManager.stats.maxIntelligence)
                            {
                                PlayerDie(Enums.PlayerStats.Intelligence, true);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statIntelligence -= stat.amount;

                            if (gameManager.stats.statIntelligence <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Intelligence);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statIntelligence = stat.amount;

                            if (gameManager.stats.statIntelligence <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Intelligence);
                            }
                            else if (gameManager.stats.statIntelligence >= gameManager.stats.maxIntelligence)
                            {
                                PlayerDie(Enums.PlayerStats.Intelligence, true);
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

                            if (gameManager.stats.statStrength >= gameManager.stats.maxStrength)
                            {
                                PlayerDie(Enums.PlayerStats.Strength, true);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statStrength -= stat.amount;

                            if (gameManager.stats.statStrength <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Strength);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statStrength = stat.amount;

                            if (gameManager.stats.statStrength <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Strength);
                            }
                            else if (gameManager.stats.statStrength >= gameManager.stats.maxStrength)
                            {
                                PlayerDie(Enums.PlayerStats.Strength, true);
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

                            if (gameManager.stats.statPersonality >= gameManager.stats.maxPersonality)
                            {
                                PlayerDie(Enums.PlayerStats.Personality, true);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statPersonality -= stat.amount;

                            if (gameManager.stats.statPersonality <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Personality);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statPersonality = stat.amount;

                            if (gameManager.stats.statPersonality <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Personality);
                            }
                            else if (gameManager.stats.statPersonality >= gameManager.stats.maxPersonality)
                            {
                                PlayerDie(Enums.PlayerStats.Personality, true);
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

                            if(gameManager.stats.statMoney >= gameManager.stats.maxMoney)
                            {
                                PlayerDie(Enums.PlayerStats.Money, true);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Subtract)
                        {
                            gameManager.stats.statMoney -= stat.amount;

                            if (gameManager.stats.statMoney <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Money);
                            }
                        }
                        else if (stat.Method == Enums.ActonMethod.Set)
                        {
                            gameManager.stats.statMoney = stat.amount;

                            if (gameManager.stats.statMoney <= 0)
                            {
                                PlayerDie(Enums.PlayerStats.Money);
                            }
                            else if (gameManager.stats.statMoney >= gameManager.stats.maxMoney)
                            {
                                PlayerDie(Enums.PlayerStats.Money, true);
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

                    if (df.debuffType == Enums.DebuffType.PerSec)
                    {
                        myDebuffsPerSec.Find(myDebuff => myDebuff.debuffName == df.debuffName).amountOfTime += debuff.amountOfTime;
                    }
                }
                else
                {
                    myDebuffs.Add(debuff);

                    if(df.debuffType == Enums.DebuffType.PerSec)
                    {
                        myDebuffsPerSec.Add(debuff);
                    }
                }
            }
            else
            {
                myDebuffs.Add(debuff);

                if (debuff.debuffType == Enums.DebuffType.PerSec)
                {
                    myDebuffsPerSec.Add(debuff);
                }
            }
            GameManager.Instance.ReFrashBuffUI();
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
                        GameManager.Instance.RemoveDeBuffUI(debuff);
                        myDebuffs.Remove(debuff);
                    }
                }
            }

            GameManager.Instance.ReFrashBuffUI();
        }
        private void DebuffPerSec()
        {
            if(timer <= 0)
            {
                if (myDebuffsPerSec.Count > 0)
                {
                    List<Debuff> debuffsPerSec = new List<Debuff>();

                    foreach(Debuff persec in myDebuffsPerSec)
                    {
                        debuffsPerSec.Add(persec);
                    }

                    foreach (Debuff debuff in debuffsPerSec)
                    {
                        for (int i = 0; i < debuff.stat.Count; i++)
                        {
                            ChangePlayerStat(debuff.stat[i]);
                        }

                        var _debuff = myDebuffsPerSec.Find(_debuff => _debuff.debuffName == debuff.debuffName);

                        _debuff.amountOfTime -= 1;
                        debuff.amountOfTime -= 1;

                        if (_debuff.amountOfTime <= 0)
                        {
                            GameManager.Instance.RemoveDeBuffUI(debuff);
                            myDebuffsPerSec.Remove(_debuff);
                            myDebuffs.Remove(_debuff);
                        }
                    }
                }

                timer = 1;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            GameManager.Instance.ReFrashBuffUI();
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
                                if (StatMinChack(so.statMins[i]) == false)
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
            nowStoryNum = 0;
            return ShuffleStory(storyTemp);
        }
        private List<StoryObject> LoadStoryObjectsByNames(string[] storyNames)
        {
            List<StoryObject> storyTemp = new List<StoryObject>();

            foreach(string name in storyNames)
            {
                foreach(StoryObject so in storyObjects)
                {
                    if(Equals(so.name, name))
                    {
                        storyTemp.Add(so);
                        break;
                    }
                }
            }
            return storyTemp;
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
            int n = shuffled.Count;

            while (n > 1)
            {
                n--;
                int o = rng.Next(n + 1);
                var story = shuffled[o];
                shuffled[o] = shuffled[n];
                shuffled[n] = story;
            }

            return shuffled;
        }

        private StoryObject LoadNextStory(bool isAgeChanged = false)
        {
            if(isAgeChanged == true)
            {
                pickedStorys = PickStoryObjects(30);
                DebuffPerYear();
                return pickedStorys[nowStoryNum];
            }
            else
            {
                nowStoryNum++;
                if (nowStoryNum < pickedStorys.Count)
                {
                    DebuffPerYear();
                    return pickedStorys[nowStoryNum];
                }
                else
                {
                    pickedStorys = PickStoryObjects(30);
                    DebuffPerYear();
                    return pickedStorys[nowStoryNum];
                }
            }

        }
        private bool StatMinChack(StatMin stat)
        {
            switch (stat.StatType)
            {
                case Enums.PlayerStats.Intelligence :
                    if(gameManager.stats.statIntelligence >= stat.Amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Strength:
                    if (gameManager.stats.statStrength >= stat.Amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Personality:
                    if (gameManager.stats.statPersonality >= stat.Amount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Age:
                    if (stat.age != Enums.Age.None)
                    {
                        switch(stat.age)
                        {
                            case Enums.Age.Infancy:
                                if (gameManager.stats.age < 6)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case Enums.Age.Adolescence :
                                if (gameManager.stats.age >= 6 && gameManager.stats.age <= 19)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case Enums.Age.Youth :
                                if (gameManager.stats.age >= 20 && gameManager.stats.age <= 39)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case Enums.Age.MiddleAge :
                                if (gameManager.stats.age >= 40 && gameManager.stats.age <= 59)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            case Enums.Age.Elderly:
                                if (gameManager.stats.age >= 60)
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
                    else if(stat.age == Enums.Age.None)
                    {

                        if (gameManager.stats.age >= stat.Amount)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                case Enums.PlayerStats.Money:
                    if (gameManager.stats.statMoney >= stat.Amount)
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

        private bool AgeChack(Enums.Age ageRange, float age = 0)
        {
            if(age <= 0)
            {
                switch (ageRange)
                {
                    case Enums.Age.Infancy:
                        if (gameManager.stats.age < 6)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case Enums.Age.Adolescence:
                        if (gameManager.stats.age >= 6 && gameManager.stats.age <= 19)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case Enums.Age.Youth:
                        if (gameManager.stats.age >= 20 && gameManager.stats.age <= 39)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case Enums.Age.MiddleAge:
                        if (gameManager.stats.age >= 40 && gameManager.stats.age <= 59)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case Enums.Age.Elderly:
                        if (gameManager.stats.age >= 60)
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
            else
            {
                return (gameManager.stats.age >= age) ? true : false;
            }
        }

        private void PlayerDie(Enums.PlayerStats stat, bool isMaxStat = false)
        {
            
        }

        public void ApplyOption(Option option)
        {
            for(int i = 0; i< option.stats.Count; i++)
            {
                ChangePlayerStat(option.stats[i]);
            }

            for(int i = 0; i < option.debuffs.Count; i++)
            {
                AddMyDebuff(option.debuffs[i]);
            }

            if (gameManager.stats.age == 6 || gameManager.stats.age == 20 || gameManager.stats.age == 40 || gameManager.stats.age == 60)
            {
                nowStory = LoadNextStory(true);
                nowOptions = PickOptions();
            }
            else
            {
                nowStory = LoadNextStory(false);
                nowOptions = PickOptions();
            }

            gameManager.ChangeStoryUI(nowStory);
            gameManager.ChangeStatUI();
            gameManager.ChangeAgeAndUI();
        }
        public Option[] PickOptions()
        {
            Option[] tempOptions = new Option[2];

            if (nowStory.options.Count == 2)
            {
                tempOptions = nowStory.options.ToArray();
            }
            else
            {
                var positive = new List<Option>();
                var negative = new List<Option>();

                foreach(Option o in nowStory.options)
                {
                    if(o.isPositive == true)
                    {
                        positive.Add(o);
                    }
                    else
                    {
                        negative.Add(o);
                    }
                }

                for(int i =0; i < positive.Count; i++)
                {
                    if(AgeChack(positive[i].age) == true)
                    {
                        tempOptions[0] = positive[i];
                        break;
                    }
                }

                for (int i = 0; i < negative.Count; i++)
                {
                    if (AgeChack(negative[i].age) == true)
                    {
                        tempOptions[1] = negative[i];
                        break;
                    }
                }
            }

            return tempOptions;
        }
        public void StartFirstStory()
        {
            nowStory = storyObjects.Find(so => so.name == "SC_1001");
            nowOptions = PickOptions();
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
        [ContextMenu("Save All Storys")]
        public void SaveAllStorys()
        {
            foreach(StoryObject data in storyObjects)
            {
                string filePath = Application.persistentDataPath + $"/{data.name}.Json";
                // JSON 직렬화

                var settings = new JsonSerializerSettings
                {
                    // Image 변수를 무시하도록 설정
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                string jsonData = JsonConvert.SerializeObject(data, settings);
                // 파일 저장
                File.WriteAllText(filePath, jsonData);
            }

        }
        [ContextMenu("Load All Storys")]
        public void LoadAllData()
        {

            foreach (StoryObject data in storyObjects)
            {
                string filePath = Application.persistentDataPath + $"/{data.name}.Json";
                string jsonData = File.ReadAllText(filePath);

                StoryObject _data = ScriptableObject.CreateInstance<StoryObject>();
                JsonUtility.FromJsonOverwrite(jsonData, _data);

                data.CopyFrom(_data);
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssets();
            }

        }
#endif
    }
}