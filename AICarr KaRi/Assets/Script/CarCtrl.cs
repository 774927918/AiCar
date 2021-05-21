using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        rb.centerOfMass = center;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //后轮驱动旋转
        wcArr[2].motorTorque = engine * y;
        wcArr[3].motorTorque = engine * y;
        //前轮旋转角度
        wcArr[0].steerAngle = maxAngle * x;
        wcArr[1].steerAngle = maxAngle * x;
        CheckHuaXing();
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
                tuoweil = Instantiate(tuoweiPrefab);
                tuowei2 = Instantiate(tuoweiPrefab);
                tuoweil.transform.position = wcArr[2].transform.position + Vector3.down * 0.45f;
                tuowei2.transform.position = wcArr[3].transform.position + Vector3.down * 0.45f;
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
