using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    [CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
    public class StoryObject : ScriptableObject
    {
        [Header("�̾߱� �ؽ�Ʈ")] [TextArea(10, 10)] public string StoryText;
        [Header("���丮 ���� ���� ����")] public List<StatMin> statMins = new List<StatMin>();
        [Header("���� ���丮�� �����ϴ���")] public bool hasPreviousStory = false;
        [Header("���� ���丮")] public StoryObject nextStory;
        [Header("��������(ALL������ �ּ� 2��)")] public List<Option> options = new List<Option>();
    }
    [System.Serializable]
    public class Option
    {
        [Header("�ɼ� �ؽ�Ʈ")][TextArea(10, 10)] public string OptionText;
        [Header("����(��������) ��������")] public bool isPositive;
        [Header("���� ���� ���丮")] public StoryObject nextStory;
        [Header("�����ϴ� ����")] public Enums.Age age;
        [Header("�ɼǿ��� �ٲ� ����")] public List<Stat> stats = new List<Stat>();
    }
    [System.Serializable]
    public class Stat
    {
        [Header("�ٲ� ����")] public Enums.PlayerStats StatType;
        [Header("�ٲ� ��")] public int Amount;
        [Header("�ٲ� ���")] public Enums.ActonMethod Method;
    }
    [System.Serializable]
    public class StatMin
    {
        [Header("���� ����")] public Enums.PlayerStats StatType;
        [Header("�ּ� ��")] public int Amount;
    }
}

