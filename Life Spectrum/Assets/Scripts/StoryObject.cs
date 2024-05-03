using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace LIFESPECTRUM
{
    [CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
    public class StoryObject : ScriptableObject
    {
        [Header("이름 텍스트")] [TextArea(3, 10)] public string titleText;
        [Header("이야기 텍스트")] [TextArea(10, 10)] public string storyText;
        [JsonIgnore] [Header("이미지 스프라이트")] public Material image = null;
        [Header("스토리 등장 조건 스텟")] public List<StatMin> statMins = new List<StatMin>();
        [Header("이전 스토리가 존재하는지")] public bool hasPreviousStory = false;
        [Header("아무 선택지도 없는 스토리인지")] public bool hasntAnyOptions = false;
        [Header("시간이 흐르지 않는 스토리인지")] public bool timeDoesntPass = false;
        [JsonIgnore] [Header("이후 스토리")] public StoryObject nextStory;
        [Header("선택지들(ALL선택지 최소 2개)")] public List<Option> options = new List<Option>();
        public void CopyFrom(StoryObject other)
        {
            this.titleText = other.titleText;
            this.storyText = other.storyText;
            this.statMins = other.statMins;
            this.hasPreviousStory = other.hasPreviousStory;
            this.hasntAnyOptions = other.hasntAnyOptions;
            this.nextStory = other.nextStory;
            this.options = other.options;
        }
    }
    [System.Serializable]
    public class Option
    {
        [Header("옵션 텍스트")][TextArea(10, 10)] public string optionText;
        [Header("옵션 세부 텍스트")] [TextArea(10, 10)] public string optionDetailText;
        [Header("좋은(긍정적인) 질문인지")] public bool isPositive;
        [Header("선택 이후 스토리")] public StoryObject nextStory;
        [Header("등장하는 나이")] public Enums.Age age;
        [Header("옵션에서 바꿀 스텟")] public List<Stat> stats = new List<Stat>();
        [Header("옵션에서 등장하는 디버프")] public List<Debuff> debuffs = new List<Debuff>();
    }
    [System.Serializable]
    public class Stat
    {
        [Header("바꿀 스텟")] public Enums.PlayerStats StatType;
        [Header("바꿀 스텟이 최대 인지")] public bool isMaxAmount;
        [Header("바꿀 양")] public int amount;
        [Header("바꿀 방법")] public Enums.ActonMethod Method;
    }
    [System.Serializable]
    public class StatMin
    {
        [Header("조건 스텟")] public Enums.PlayerStats StatType;
        [Header("등장하는 나이")] public Enums.Age age;
        [Header("최소 양")] public int Amount;
    }
    [System.Serializable]
    public class Debuff
    {
        [Header("디버프 이름")] public string debuffName;
        [Header("디버프 타입")] public Enums.DebuffType debuffType;
        [Header("디버프 지속 시간")] public int amountOfTime;
        [Header("시간 당 바꿀 스텟")] public List<Stat> stat = new List<Stat>();
    }
}

