using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LIFESPECTRUM
{
    public class Enums
    {
        public enum PlayerStats
        {
            Intelligence,
            Strength,
            Personality,
            Money,
            Age
        }
        public enum ActonMethod
        {
            Add,
            Subtract,
            Set
        }
        /// <summary>
        /// All : 모든 나이
        /// Infancy : 유아기
        /// Adolescence : 애새기
        /// Youth : 청년
        /// MiddleAge : 중년
        /// Elderly : 용진이 동년배(*중요*용진이 이상형)
        /// </summary>
        public enum Age
        {
            All,
            Infancy, 
            Adolescence,
            Youth,
            MiddleAge,
            Elderly
        }
        public enum DebuffType
        {
            PerSec,
            PerYear
        }
    }
}

