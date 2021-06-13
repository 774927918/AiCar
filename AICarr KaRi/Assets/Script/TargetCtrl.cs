
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
    float i;
    public  Evolution Ev;
   
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
            //txt.text = c.name + "   车速:" + Mathf.RoundToInt(c.curSpeed * 3.6f / 0.6f) + "   圈数:" + c.quanshu + "    得分:" + c.fit;
            if (i >= 1)
            {
                txt.text = c.name + "   スピード:" + Mathf.RoundToInt(c.curSpeed * 3.6f / 0.6f) + "   何周か走った:" + c.quanshu + "    得点:" + c.fit + "運行時間" + (Ev.t_t / 60).ToString("0.00") + "分" + "進化回数" + Ev.junnkaikaisu;
                //-----下面的保持不动------
                i = 0;
            }
            
        }
        else
        {
            txt.text = "";
        }
        i += Time.deltaTime;
    }
}
