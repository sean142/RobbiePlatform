using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReset : MonoBehaviour
{
   
    void Start()
    {
        GameManager.instance.gameTime += Time.deltaTime;

    }

    void Update()
    {
        //GameManager.instance.gameTime = Time.deltaTime;

        // UIManager.UpdateTimeUI(GameManager.instance.gameTime + Time.time);
    }
}
