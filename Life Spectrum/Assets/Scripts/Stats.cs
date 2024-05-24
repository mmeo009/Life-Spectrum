using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LIFESPECTRUM
{
    /// <summary>
    /// statIntelligence : ���� ����
    /// statStrength : ü�� ����
    /// statPersonality : ���� ����
    /// statMoney : ��
    /// </summary>
    [System.Serializable]
    public class Stats
    {
        public int statIntelligence;
        public int statStrength;
        public int statPersonality;
        public int statMoney;
        public int maxIntelligence;
        public int maxStrength;
        public int maxPersonality;
        public int maxMoney;
        public float age;
        public string[] storyNames;
        public Stats(int intelligence, int strength, int personality, int money, int maxIntelligence, int maxStrength, int maxPersonality, int maxMoney, float age)
        {
            this.statIntelligence = intelligence;
            this.statStrength = strength;
            this.statPersonality = personality;
            this.statMoney = money;
            this.maxIntelligence = maxIntelligence;
            this.maxStrength = maxStrength;
            this.maxPersonality = maxPersonality;
            this.maxMoney = maxMoney;
            this.age = age;
        }
    }
}

