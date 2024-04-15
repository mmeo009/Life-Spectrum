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
        [Tooltip("�ɷ�ġ")]
        public Enums.PlayerStats playerStat;
        [Tooltip("�߰���")]
        [Range(int.MinValue,int.MaxValue)]
        public float statAmount;
        [Tooltip("���ۿ�")]
        public Enums.AdditionalEvent adEvent;
        [Tooltip("���ӽð�")]
        [Range(int.MinValue, int.MaxValue)]
        public float eventAmount;
    }
}
