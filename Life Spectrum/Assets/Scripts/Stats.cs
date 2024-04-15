using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeSpectrum
{
    [System.Serializable]
    public class Stats
    {
        //======================�⺻ ����=====================
        public int statLife;                    // ����� ����
        public int statFullness;                // ������ ����
        public int statFeel;                    // �ູ�� ����
        public int money;                       // ��
        //======================�߰� ����=====================
        public bool isFoodPoison;               // ���ߵ��� �ɷȴ°�
        public int foodPoison;                  // ���ߵ� ���� ��
        public bool hasCold;                    // ���⿡ �ɷȴ°�
        public int cold;                        // ���� ���� ��
        public bool getHurt;                    // ���ƴ°�
        public int hurt;                        // ȸ������ �ɸ��� �ð�
    }
}

