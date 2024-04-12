using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LifeSpectrum;

[CreateAssetMenu(fileName = "NewStory", menuName = "ScriptableObject/StoryObject")]
public class StoryObject : ScriptableObject
{
    public int storyNum;

    public string storyName;

    [TextArea (10,10)]
    public string story;
    public List<AdditionalStats> additionalStats = new List<AdditionalStats>();

    [System.Serializable]
    public class AdditionalStats
    {
        public Enums.PlayerStats additionalStat;
        public float amount;
    }
}
