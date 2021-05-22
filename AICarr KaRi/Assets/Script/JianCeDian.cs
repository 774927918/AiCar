using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JianCeDian : MonoBehaviour
{
    public int index;
    // Use this for initialization
    public Car car;
    void Start()
    {
        index = int.Parse(name);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
             car = c.gameObject.GetComponent<Car>();
          
            if (car != null && index == car.jianceNum - 1 && car.nextIndex == index)
            {
                car.nextIndex = 0;
                car.moveDis += car.jianceDis[index];
                car.quanshu++;
                if (car.quanshu >= 2)
                {
                    //car.fit = 2000000;
                    car.gameObject.SetActive(false);
                }
            }
            else if (car != null && index == car.nextIndex )
            {
                car.moveDis += car.jianceDis[index];
                car.nextIndex++;
            }
        }
    }

}
