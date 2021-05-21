using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotToCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    public Camera cm;
    public Evolution ev;

    void Start()
    {
        ev = FindObjectOfType<Evolution>();
        txt.text = transform.parent.name;
        int index = int.Parse(txt.text.Replace("Car", ""));
        if (index == 0)
        {
            txt.text += "\n第一个体";
        }
        else if (index < ev.populationSize / 4)
        {
            txt.text += "\n精英个体";
        }
        else
        {
            txt.text += "\n杂交个体";
        }
        cm = Camera.main;
        txt.color = transform.parent.GetComponent<Car>().mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cm.transform.rotation;
    }
}
