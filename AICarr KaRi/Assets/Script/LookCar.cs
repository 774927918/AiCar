using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCar : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            Car c = target.GetComponent<Car>();
            int index = c.nextIndex / 2 * 2;
            transform.position = GameObject.Find(index.ToString()).transform.position + Vector3.up * 0.1f;
            transform.LookAt(target);
        }
    }
}
