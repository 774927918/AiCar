using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class TargetCtrl : MonoBehaviour
{
    public SmoothFollow sf;
    public LookCar lc;
    public Text txt;
    void Update()
    {
        Car[] carArr = FindObjectsOfType<Car>();
        Car c = null;
        float f = 0;
        foreach (var item in carArr)
        {
            if (item.fit > f)
            {
                f = item.fit;
                c = item;
            }
        }
        if (c != null)
        {
            sf.target = c.transform;
            lc.target = c.transform;
            txt.text = c.name + "   车速:" + Mathf.RoundToInt(c.curSpeed * 3.6f / 0.6f) + "   圈数:" + c.quanshu + "    得分:" + c.fit;
        }
        else
        {
            txt.text = "";
        }
    }
}
