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
        /// Type_A : �ڿ� ���� ���丮
        /// Type_B : ����, ���ɴ뿡 ���� �����ϴ� ���丮
        /// Type_C : ���丮 �������� ���� ���Ӱ� �߰ߵǴ� ���丮
        /// Type_D : Ư�� ������ ��ġ�� ���� �����ϴ� ���丮.
        /// </summary>
        public enum StoryType
        {
            Type_A,
            Type_B,
            Type_C,
            Type_D
        }

        /// <summary>
        /// All : ��� ����
        /// Infancy : ���Ʊ�
        /// Adolescence : �ֻ���
        /// Adulthood : �
        /// Elderly : ������ �����
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

