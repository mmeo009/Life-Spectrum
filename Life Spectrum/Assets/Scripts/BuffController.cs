using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LIFESPECTRUM;
using System.Diagnostics;

public class BuffController : MonoBehaviour
{
    public Image StatImage;
    public TMP_Text remainingTimes;
    public GameObject BuffIcon;
    public GameObject DebuffIcon;

    public Debuff thisObjectDebuff;

    public void LoadMyState(Debuff debuff)
    {
        thisObjectDebuff = debuff;

        var statType = debuff.stat[0].StatType;
        StatImage.sprite = Resources.Load<Sprite>($"Materials/Stat/Stat_{statType.ToString()}");

        if (debuff.stat[0].amount > 0)
        {
            BuffIcon.SetActive(true);
            DebuffIcon.SetActive(false);
        }
        else
        {
            BuffIcon.SetActive(false);
            DebuffIcon.SetActive(true);
        }

        ChangeTime();
    }

    public void ChangeTime()
    {
        if (thisObjectDebuff.debuffType == Enums.DebuffType.PerSec)
        {
            remainingTimes.text = thisObjectDebuff.amountOfTime + "Sec";
        }
        else
        {
            remainingTimes.text = thisObjectDebuff.amountOfTime + "Year";
        }

    }
}
