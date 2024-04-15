using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LifeSpectrum;

[CreateAssetMenu(fileName = "StoryCard", menuName = "ScriptableObject/StoryObject")]
public class StoryObject : ScriptableObject
{
    [Tooltip("이 스토리의 번호")]
    public int storyNum;

    [Tooltip("이 스토리의 타이틀 이름")]
    public string storyName;

    [Tooltip("이 스토리의 이상한 정도")]
    public Enums.StoryStangeDegree stangeDegree;

    [Tooltip("이야기 내용")]
    [TextArea (10,10)]
    public string story;

    [Tooltip("첫번째 선택지")]
    public Way leftWay = new Way();

    [Tooltip("두번째 선택지")]
    public Way rightWay = new Way();

    [System.Serializable]
    public class Way
    {
        [Tooltip("선택지 내용")]
        [TextArea(10, 10)]
        public string wayText;

        [Tooltip("추가 혹은 감소되는 능력치")]
        public List<StatData> statDatas = new List<StatData>();

        [Tooltip("추가되는 부작용")]
        public List<EventData> eventDatas = new List<EventData>();
    }

    [System.Serializable]
    public class StatData
    {
        [Tooltip("능력치")]
        public Enums.PlayerStats playerStat;
        [Tooltip("추가량")]
        [Range(int.MinValue, int.MaxValue)]
        public float statAmount;
    }

    [System.Serializable]
    public class EventData
    {
        [Tooltip("부작용")]
        public Enums.AdditionalEvent adEvent;
        [Tooltip("지속시간")]
        [Range(int.MinValue, int.MaxValue)]
        public float eventAmount;
    }

}
