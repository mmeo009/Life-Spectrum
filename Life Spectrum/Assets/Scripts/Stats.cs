using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    [System.Serializable]
    public class Stats
    {
        //======================기본 스탯=====================
        public int statLife;                    // 생명력 스탯
        public int statFullness;                // 포만감 스탯
        public int statFeel;                    // 행복도 스탯
        public int money;                       // 돈
        //======================추가 스탯=====================
        public bool isFoodPoison;               // 식중독에 걸렸는가
        public int foodPoison;                  // 식중독 지속 턴
        public bool hasCold;                    // 감기에 걸렸는가
        public int cold;                        // 감기 지속 턴
        public bool getHurt;                    // 다쳤는가
        public int hurt;                        // 회복까지 걸리는 시간
    }
}

