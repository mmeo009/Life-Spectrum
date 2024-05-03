using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace LIFESPECTRUM
{
    [CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
    public class StoryObject : ScriptableObject
    {
        [Header("�̸� �ؽ�Ʈ")] [TextArea(3, 10)] public string titleText;
        [Header("�̾߱� �ؽ�Ʈ")] [TextArea(10, 10)] public string storyText;
        [JsonIgnore] [Header("�̹��� ��������Ʈ")] public Material image = null;
        [Header("���丮 ���� ���� ����")] public List<StatMin> statMins = new List<StatMin>();
        [Header("���� ���丮�� �����ϴ���")] public bool hasPreviousStory = false;
        [Header("�ƹ� �������� ���� ���丮����")] public bool hasntAnyOptions = false;
        [Header("�ð��� �帣�� �ʴ� ���丮����")] public bool timeDoesntPass = false;
        [JsonIgnore] [Header("���� ���丮")] public StoryObject nextStory;
        [Header("��������(ALL������ �ּ� 2��)")] public List<Option> options = new List<Option>();
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
        [Header("�ɼ� �ؽ�Ʈ")][TextArea(10, 10)] public string optionText;
        [Header("�ɼ� ���� �ؽ�Ʈ")] [TextArea(10, 10)] public string optionDetailText;
        [Header("����(��������) ��������")] public bool isPositive;
        [Header("���� ���� ���丮")] public StoryObject nextStory;
        [Header("�����ϴ� ����")] public Enums.Age age;
        [Header("�ɼǿ��� �ٲ� ����")] public List<Stat> stats = new List<Stat>();
        [Header("�ɼǿ��� �����ϴ� �����")] public List<Debuff> debuffs = new List<Debuff>();
    }
    [System.Serializable]
    public class Stat
    {
        [Header("�ٲ� ����")] public Enums.PlayerStats StatType;
        [Header("�ٲ� ������ �ִ� ����")] public bool isMaxAmount;
        [Header("�ٲ� ��")] public int amount;
        [Header("�ٲ� ���")] public Enums.ActonMethod Method;
    }
    [System.Serializable]
    public class StatMin
    {
        [Header("���� ����")] public Enums.PlayerStats StatType;
        [Header("�����ϴ� ����")] public Enums.Age age;
        [Header("�ּ� ��")] public int Amount;
    }
    [System.Serializable]
    public class Debuff
    {
        [Header("����� �̸�")] public string debuffName;
        [Header("����� Ÿ��")] public Enums.DebuffType debuffType;
        [Header("����� ���� �ð�")] public int amountOfTime;
        [Header("�ð� �� �ٲ� ����")] public List<Stat> stat = new List<Stat>();
    }
}

