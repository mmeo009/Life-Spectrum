using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    [CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
    public class StoryObject : ScriptableObject
    {
        public List<Option> options = new List<Option>();
    }
    [System.Serializable]
    public class Story
    {

    }
    [System.Serializable]
    public class Option
    {
        [Header("옵션 텍스트")][TextArea(10, 10)] public string OptionText;
        [Header("좋은(긍정적인) 질문인지")] public bool isPositive;
        [Header("바꿀 스텟")] public Enums.PlayerStats Stat;
        [Header("바꿀 양")] public int Amount;
        [Header("바꿀 방법")] public Enums.ActonMethod Method;
    }
}

