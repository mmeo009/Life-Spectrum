using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    [CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
    public class StoryObject : ScriptableObject
    {
        [Header("이야기 텍스트")] [TextArea(10, 10)] public string StoryText;
        [Header("스토리 등장 조건 스텟")] public List<StatMin> statMins = new List<StatMin>();
        [Header("이전 스토리가 존재하는지")] public bool hasPreviousStory = false;
        [Header("이후 스토리")] public StoryObject nextStory;
        [Header("선택지들(ALL선택지 최소 2개)")] public List<Option> options = new List<Option>();
    }
    [System.Serializable]
    public class Option
    {
        [Header("옵션 텍스트")][TextArea(10, 10)] public string OptionText;
        [Header("좋은(긍정적인) 질문인지")] public bool isPositive;
        [Header("선택 이후 스토리")] public StoryObject nextStory;
        [Header("등장하는 나이")] public Enums.Age age;
        [Header("옵션에서 바꿀 스텟")] public List<Stat> stats = new List<Stat>();
    }
    [System.Serializable]
    public class Stat
    {
        [Header("바꿀 스텟")] public Enums.PlayerStats StatType;
        [Header("바꿀 양")] public int Amount;
        [Header("바꿀 방법")] public Enums.ActonMethod Method;
    }
    [System.Serializable]
    public class StatMin
    {
        [Header("조건 스텟")] public Enums.PlayerStats StatType;
        [Header("최소 양")] public int Amount;
    }
}

