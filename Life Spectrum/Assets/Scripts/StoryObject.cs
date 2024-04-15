using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LifeSpectrum;

[CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
public class StoryObject : ScriptableObject
{
    [Tooltip("�� ���丮�� ��ȣ")]
    public int storyNum;

    [Tooltip("�� ���丮�� Ÿ��Ʋ �̸�")]
    public string storyName;

    [Tooltip("�� ���丮�� �̻��� ����")]
    public Enums.StoryStangeDegree stangeDegree;

    [Tooltip("�̾߱� ����")]
    [TextArea (10,10)]
    public string story;

    [Tooltip("ù��° ������")]
    public Way leftWay = new Way();

    [Tooltip("�ι�° ������")]
    public Way rightWay = new Way();

    [System.Serializable]
    public class Way
    {
        [Tooltip("������ ����")]
        [TextArea(10, 10)]
        public string wayText;

        [Tooltip("�߰� Ȥ�� ���ҵǴ� �ɷ�ġ")]
        public List<StatData> statDatas = new List<StatData>();

        [Tooltip("�߰��Ǵ� ���ۿ�")]
        public List<EventData> eventDatas = new List<EventData>();
    }

    [System.Serializable]
    public class StatData
    {
        [Tooltip("�ɷ�ġ")]
        public Enums.PlayerStats playerStat;
        [Tooltip("�߰���")]
        [Range(int.MinValue, int.MaxValue)]
        public float statAmount;
    }

    [System.Serializable]
    public class EventData
    {
        [Tooltip("���ۿ�")]
        public Enums.AdditionalEvent adEvent;
        [Tooltip("���ӽð�")]
        [Range(int.MinValue, int.MaxValue)]
        public float eventAmount;
    }

}
