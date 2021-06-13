using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    public float engine;//引擎马力 牵引力
    public float maxAngle;//
    public WheelCollider[] wcArr;
    public Transform[] xianshiArr;
    public Material weidengMat;

    public Rigidbody rb;//质量中心
    public Vector3 center;//坐标中心

    public GameObject tuoweiPrefab;
    public GameObject tuoweil;
    public GameObject tuowei2;
    public Text txt;
    public float curSpeed;//射线距离

    //public float fit;
    //public float moveDis;
    //public int jianceNum;
    //public List<float> jianceDis = new List<float>();
    //public List<Transform> jianceList = new List<Transform>();
    //public int nextIndex;
    void Start()
    {
        rb.centerOfMass = center;

        //Transform[] trArr = FindObjectsOfType<Transform>();
        //for (int i = 0; i < jianceNum; i++)
        //{
        //    for (int j = 0; j < trArr.Length; j++)
        //    {
        //        if (trArr[j].name == i.ToString() && trArr[j].gameObject.layer == 10)
        //        {
        //            jianceList.Add(trArr[j]);
        //            if (i == 0)
        //            {
        //                jianceDis.Add(0);
        //            }
        //            else
        //            {
        //                jianceDis.Add(Vector3.Distance(jianceList[i - 1].position, jianceList[i].position));
        //            }
        //            break;
        //        }
        //    }
        //}
        //jianceDis[0] = Vector3.Distance(jianceList[0].position, jianceList[jianceNum - 1].position);
    }

    // Update is called once per frame
    void Update()
    {
        curSpeed = rb.velocity.magnitude;
        //float f = moveDis + jianceDis[nextIndex] - Vector3.Distance(transform.position, jianceList[nextIndex].position);
        txt.text = "   スピード:" + Mathf.RoundToInt(curSpeed * 3.6f / 0.6f) ;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //后轮驱动旋转
        wcArr[2].motorTorque = engine * y;
        wcArr[3].motorTorque = engine * y;
        //前轮旋转角度
        wcArr[0].steerAngle = maxAngle * x;
        wcArr[1].steerAngle = maxAngle * x;
        //CheckHuaXing();
        if (Time.time > 1.5f)
        {
            CheckHuaXing();
        }
       // Invoke("CheckHuaXing", 1.2f);
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos;
            Quaternion rot;
            wcArr[i].GetWorldPose(out pos, out rot);
            xianshiArr[i].position = pos;
            xianshiArr[i].rotation = rot;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            wcArr[2].brakeTorque = engine * 5;
            wcArr[3].brakeTorque = engine * 5;
            weidengMat.color = Color.red;
        }
        else
        {
            wcArr[2].brakeTorque = 0;
            wcArr[3].brakeTorque = 0;
            weidengMat.color = new Color(30f / 255f, 0, 0);
        }
       
    }
    void CheckHuaXing()
    {
        float angle = Vector3.Angle(rb.velocity, wcArr[2].transform.forward);
        if (angle > 10)
        {
            if (tuoweil == null)
            {

                //tuoweil = Instantiate(tuoweiPrefab, wcArr[2].transform.position, tuoweiPrefab.transform.rotation);
                //tuowei2 = Instantiate(tuoweiPrefab, wcArr[3].transform.position, tuoweiPrefab.transform.rotation);
                //tuoweil.transform.localPosition = new Vector3(0, -0.2f, 0);
                //tuowei2.transform.localPosition = new Vector3(0, -0.2f, 0);
                tuoweil = Instantiate(tuoweiPrefab);
                tuowei2 = Instantiate(tuoweiPrefab);
                tuoweil.transform.position = wcArr[2].transform.position + Vector3.down * 0.24f;
                tuowei2.transform.position = wcArr[3].transform.position + Vector3.down * 0.24f;
                tuoweil.transform.SetParent(wcArr[2].transform);
                tuowei2.transform.SetParent(wcArr[3].transform);
            }
        }
        else
        {
            if (tuoweil != null)
            {
                tuoweil.transform.SetParent(null);
                tuowei2.transform.SetParent(null);
                tuoweil = null;
                tuowei2 = null;
            }
        }
    }
}
