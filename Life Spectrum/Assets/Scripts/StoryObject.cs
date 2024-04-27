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
        [Header("�ɼ� �ؽ�Ʈ")][TextArea(10, 10)] public string OptionText;
        [Header("����(��������) ��������")] public bool isPositive;
        [Header("�ٲ� ����")] public Enums.PlayerStats Stat;
        [Header("�ٲ� ��")] public int Amount;
        [Header("�ٲ� ���")] public Enums.ActonMethod Method;
    }
}

