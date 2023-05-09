using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateIfTouch : MonoBehaviour
{
    bool arduinoTouch
    {
        get
        {
            object val;

            if (ArduinoIO.Instance.sensorData.TryGetValue("touchStatus", out val))
            {
                return (bool)val;//todo - do conversion here
            }
            else return false;
        }
    }
    Image selfimage;
    // Start is called before the first frame update
    void Start()
    {
        selfimage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        selfimage.enabled = arduinoTouch;
    }
}
