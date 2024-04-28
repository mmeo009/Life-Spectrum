using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
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
            Multiply,
            Divide,
            Set
        }
        /// <summary>
        /// Type_A : 자연 생성 스토리
        /// Type_B : 나이, 연령대에 따라서 등장하는 스토리
        /// Type_C : 스토리 선택지에 따라 새롭게 발견되는 스토리
        /// Type_D : 특정 스탯의 수치에 따라 등장하는 스토리.
        /// </summary>
        public enum StoryType
        {
            Type_A,
            Type_B,
            Type_C,
            Type_D
        }

        /// <summary>
        /// All : 모든 나이
        /// Infancy : 유아기
        /// Adolescence : 애새기
        /// Adulthood : 어른
        /// Elderly : 용진이 동년배
        /// </summary>
        public enum Age
        {
            All,
            Infancy, 
            Adolescence, 
            Adulthood, 
            Elderly
        }
        public enum StoryStangeDegree
        {
            Ordinary,
            Strange,
            Terrible,
            Morbid,
            Disaster
        }
    }
}

