using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Moving : MonoBehaviour
{
    public Vector3 StartPos, NewPos;//NewPos Установка от PZ
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer < 1.5f)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(StartPos, NewPos, Timer);
            if (Timer >= 1)//Дожим
            {
                transform.position = NewPos;
            }
        }


    }
}
