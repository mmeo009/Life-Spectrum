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
        /// All : ��� ����
        /// Infancy : ���Ʊ�
        /// Adolescence : �ֻ���
        /// Youth : û��
        /// MiddleAge : �߳�
        /// Elderly : ������ �����(*�߿�*������ �̻���)
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

