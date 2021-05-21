using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Agent
{
    // Start is called before the first frame update

    public float engine;//引擎马力 牵引力
    public float maxAngle;//
    public WheelCollider[] wcArr;
    public Transform[] xianshiArr;
    public Material weidengMat;//尾灯材质球

    public Rigidbody rb;//质量中心
    public Vector3 center;//坐标中心

    public GameObject tuoweiPrefab;
   private GameObject tuoweil;
    private GameObject tuowei2;

    public Material mat;//车的材质球
    public int num;
    public List<LineRenderer> lineList = new List<LineRenderer>();
    public List<float> jianceDis = new List<float>();

    public int jianceNum;
    public List<Transform> jianceList = new List<Transform>();
    public float moveTime;

    public float perAngle;//射线偏移角度
    public float seedis;//能看到最远距离
    public float curSpeed;//射线距离

    public double[] ops;

    public float moveDis;
    public int nextIndex;
    public float fit;
    public float lowSpeedTime;
    private float t;

    public int quanshu;
    void Start()
    {

        mat = new Material(Shader.Find("Legacy Shaders/Self-Illumin/Diffuse"));
        //随机颜色
        while (true)
        {
            Vector3 vc = new Vector3(Random.value, Random.value, Random.value);
            float a = Vector3.Angle(vc, Vector3.one);
            float d = Mathf.Sin(a / 180 * Mathf.PI) * vc.magnitude;
            if (d > 0.8f)
            {
                mat.color = new Color(vc.x, vc.y, vc.z);
                break;
            }
        }
        
        weidengMat.shader = Shader.Find("Legacy Shaders/Self-Illumin/Diffuse");
        //重心位置等于坐标中心
        rb.centerOfMass = center;
        //画线
        for (int i = 0; i < num; i++)
        {
            LineRenderer lr = new GameObject("line" + i).AddComponent<LineRenderer>();
            lr.transform.SetParent(transform);
            lr.transform.localPosition = Vector3.zero;
            lr.transform.localRotation = Quaternion.identity;

            lr.startWidth = 0.06f;
            lr.endWidth = 0.06f;
            lr.material = mat;
            //不投射阴影
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //不接受阴影
            lr.receiveShadows = false;
            lineList.Add(lr);
        }

        Transform[] trArr = FindObjectsOfType<Transform>();

        for (int i = 0; i < jianceNum; i++)
        {
            for (int j = 0; j < trArr.Length; j++)
            {
                if (trArr[j].name == i.ToString() && trArr[j].gameObject.layer == 10)
                {
                    jianceList.Add(trArr[j]);
                    if (i == 0)
                    {
                        jianceDis.Add(0);
                    }
                    else
                    {
                        jianceDis.Add(Vector3.Distance(jianceList[i - 1].position, jianceList[i].position));
                    }
                    break;
                }
            }
        }
        jianceDis[0] = Vector3.Distance(jianceList[0].position, jianceList[jianceNum - 1].position);
    }

    // Update is called once per frame
    void Update()
    {
        moveTime += Time.deltaTime;

        CheckHuaXing();
        curSpeed = rb.velocity.magnitude;
        SetWc();

        foreach (var item in lineList)
        {
            item.gameObject.SetActive(isDraw);
        }
    }
    void CheckHuaXing()
    {
        float angle = Vector3.Angle(rb.velocity, transform.forward);
        if (angle > 10)
        {
            HuaXing();
        }
        else
        {
            TingZhiHuaXing();          
        }

    }
    public void HuaXing()
    {
        if (tuoweil == null)
        {
            tuoweil = Instantiate(tuoweiPrefab, wcArr[2].transform.position, tuoweiPrefab.transform.rotation);
            tuowei2 = Instantiate(tuoweiPrefab, wcArr[3].transform.position, tuoweiPrefab.transform.rotation);
            tuoweil.transform.SetParent(wcArr[2].transform);
            tuowei2.transform.SetParent(wcArr[3].transform);
            tuoweil.transform.localPosition = new Vector3(0, -0.40f, 0);
            tuowei2.transform.localPosition = new Vector3(0, -0.40f, 0);
            tuoweil.GetComponent<TrailRenderer>().material.color = mat.color;
            tuowei2.GetComponent<TrailRenderer>().material.color = mat.color;
        }
    }
    public void TingZhiHuaXing()
    {
        if (tuoweil != null)
        {
            tuoweil.transform.SetParent(null);
            tuowei2.transform.SetParent(null);
            tuoweil = null;
            tuowei2 = null;
        }
    }
    public void Move(float f)
    {
        //后轮驱动旋转
        wcArr[2].motorTorque = f * engine;
        wcArr[3].motorTorque = f * engine;
        //if (Mathf.Abs(f) > 0.5f)
        //{
        //    if (f > 0)
        //    {
        //        wcArr[2].motorTorque = 1 * engine;
        //        wcArr[3].motorTorque = 1 * engine;
        //    }
        //    else
        //    {
        //        wcArr[2].motorTorque = -1 * engine;
        //        wcArr[3].motorTorque = -1 * engine;
        //    }
        //}
        //if (curSpeed / 30f < 0.3f)
        //{
        //    asrArr[1].clip = acArr[0];
        //}
        //else if (curSpeed / 30f < 0.6f)
        //{
        //    asrArr[1].clip = acArr[1];
        //}
        //else if (curSpeed / 30f < 2f)
        //{
        //    asrArr[1].clip = acArr[2];
        //}
        //if (!asrArr[1].isPlaying)
        //{
        //    asrArr[1].Play();
        //}
    }

    public void Turn(float f)
    {
        //if (Mathf.Abs(f) > 0.3f)
        {
            //wcArr[0].steerAngle = f * angle;
            //wcArr[1].steerAngle = f * angle;
            //前轮旋转角度
            wcArr[0].steerAngle = maxAngle * f;
            wcArr[1].steerAngle = maxAngle *f;
        }
    }
    public void Brake(float f)
    {
        if (f > 0)
        {
            wcArr[2].brakeTorque = engine * 2 * 1;
            wcArr[3].brakeTorque = engine * 2 * 1;
            weidengMat.color = Color.red;
        }
        else
        {
            wcArr[2].brakeTorque = 0;
            wcArr[3].brakeTorque = 0;
            weidengMat.color = new Color(0.1f, 0, 0);
        }
    }
    private void SetWc()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos;
            Quaternion rot;
            wcArr[i].GetWorldPose(out pos, out rot);
            xianshiArr[i].position = pos;
            xianshiArr[i].rotation = rot;
        }
    }
    public override double[] GetInputs()
    {
        List<double> list = new List<double>();
        Vector3 vf = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        for (int i = 1; i <= (num - 1) / 2; i++)
        {
            lineList[i].SetPosition(0, transform.position);
            lineList[num - i].SetPosition(0, transform.position);
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(-i * perAngle, Vector3.up) * vf);
            if (Physics.Raycast(ray, out hit, seedis, 1 << 8))
            {
                list.Add(Mathf.Pow(1 - hit.distance / seedis, 1));
                lineList[i].SetPosition(1, hit.point);
            }
            else
            {
                list.Add(0);
                lineList[i].SetPosition(1, transform.position + ray.direction * seedis);
            }

            RaycastHit hit2;
            Ray ray2 = new Ray(transform.position, Quaternion.AngleAxis(i * perAngle, Vector3.up) * vf);
            if (Physics.Raycast(ray2, out hit2, seedis, 1 << 8))
            {
                list.Add(Mathf.Pow(1 - hit2.distance / seedis, 1));
                lineList[num - i].SetPosition(1, hit2.point);
            }
            else
            {
                list.Add(0);
                lineList[num - i].SetPosition(1, transform.position + ray2.direction * seedis);
            }
        }
        lineList[0].SetPosition(0, transform.position);
        RaycastHit hit3;
        Ray ray3 = new Ray(transform.position, vf);
        if (Physics.Raycast(ray3, out hit3, seedis, 1 << 8))
        {
            list.Add(Mathf.Pow(1 - hit3.distance / seedis, 1));
            lineList[0].SetPosition(1, hit3.point);
        }
        else
        {
            list.Add(0);
            lineList[0].SetPosition(1, transform.position + ray3.direction * seedis);
        }
        list.Add(Mathf.Pow(curSpeed / 40f, 1));
        return list.ToArray();
    }

    public override void UseOutputs(double[] outputs)
    {
        ops = outputs;
        Move((float)outputs[0]);
        Turn((float)outputs[1]);
        Brake((float)outputs[2]);
    }

    public override void SetFitness()
    {
        float f = moveDis + jianceDis[nextIndex] - Vector3.Distance(transform.position, jianceList[nextIndex].position);
        //f *= 1000;
        if (f > fit)
        {
            t = 0;
            fit = f;
        }
        else
        {
            t += Time.deltaTime;
            if (t > lowSpeedTime)
            {
                gameObject.SetActive(false);
            }
        }
        ge.fitness = fit;
    }

    public override void ResetAgent()
    {
        quanshu = 0;
        moveTime = 0;
        t = 0;
        nextIndex = 0;
        moveDis = 0;
        fit = 0;
        ge.fitness = 0;
        transform.position = Vector3.up * 0.9f;
        transform.rotation = Quaternion.identity;
    }

    public override void Draw()
    {
        //base.Draw();
    }
    //碰到墙删除自身
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (quanshu >= 2)
        {
            //Debug.Log(name + "  " + (800000 - (System.DateTime.Now - Evolution.startTime).Ticks / 10000) / 100);
            fit += (800000 - (System.DateTime.Now - Evolution.startTime).Ticks / 10000) / 100;
            ge.fitness = fit;
            Evolution ev = FindObjectOfType<Evolution>();
            if (ev != null && !ev.isJiShi)
            {
                ev.isJiShi = true;
            }
        }

    }
}
