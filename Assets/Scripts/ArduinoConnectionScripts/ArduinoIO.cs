using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor.Experimental.GraphView;

public class ArduinoIO : MonoBehaviour
{
    private static ArduinoIO instance;
    public Dictionary<string, object> sensorData;

    public float _Distance
    {
        get
        {
            return distance;
        }
    }
    float distance = 0;
    private SerialPort serialPort;
    public SerialPort _SerialPort
    {
        get
        {
            return serialPort;
        }
    }
  


    //Getter for the instance
    public static ArduinoIO Instance
    {
        get
        {
            // Check if the instance is null
            if (instance == null)
            {
                // Find the instance in the scene
                instance = FindObjectOfType<ArduinoIO>();

                // If the instance is still null, create a new GameObject with the script attached
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(ArduinoIO).Name);
                    instance = singletonObject.AddComponent<ArduinoIO>();
                }

                // Ensure that the instance persists across scene changes
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }




    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        sensorData = new Dictionary<string, object>();
    }

    // Start is called before the first frame update
    void Start()
    {
        serialPort = new SerialPort("COM6", 9600);
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        
        string data = serialPort.ReadLine();
        string[] keyValuePairs = data.Split(',');

        try
        {
            foreach (string keyValuePair in keyValuePairs)
            {
                if (string.IsNullOrEmpty(keyValuePair))
                {
                    return;
                }

                string[] elements = keyValuePair.Split(':');
                string key = elements[0];
                string type = elements[1];
                string valueString = elements[2];

                //string b = "";
                //foreach (var item in elements)
                //{
                //    b += item + Environment.NewLine;
                //}




                // Parse the value based on the type
                object value;
                switch (type)
                {
                    case "f":
                        value = float.Parse(valueString);
                        break;
                    case "b":
                        if (valueString == "0")
                        {
                            value = false;
                        }
                        else
                        {
                            value = true;
                        }

                        break;
                    case "i":
                        value = Int32.Parse(valueString);
                        break;
                    default:
                        Debug.LogWarning("Unknown value type: " + type);
                        continue;
                }

                // Store the value in the dictionary
                sensorData[key] = value;
            }

        }
        catch (Exception)
        {

            throw;
        }


        
        try
        {
            distance = (float)sensorData["distance"]; // Convert the first byte to a float
            bool touch = (bool)sensorData["touchStatus"]; // Convert the first byte to a float
            int brightness = (int)sensorData["brightnessValue"]; // Convert the first byte to a float
            //Debug.Log(distance + " is the distance");
            Debug.Log(touch + " is the touch");
            //Debug.Log(brightness + " is the brightness");
        }
        catch (Exception)
        {

        }
        

    }
}
