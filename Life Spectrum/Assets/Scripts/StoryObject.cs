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
        [Tooltip("능력치")]
        public Enums.PlayerStats playerStat;
        [Tooltip("추가량")]
        [Range(int.MinValue,int.MaxValue)]
        public float statAmount;
        [Tooltip("부작용")]
        public Enums.AdditionalEvent adEvent;
        [Tooltip("지속시간")]
        [Range(int.MinValue, int.MaxValue)]
        public float eventAmount;
    }
}
