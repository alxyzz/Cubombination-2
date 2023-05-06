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


    private SerialPort serialPort;

    int distance;

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
        serialPort = new SerialPort("COM4", 9600);
        serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string data = serialPort.ReadLine();
        string[] keyValuePairs = data.Split(',');
        foreach (string keyValuePair in keyValuePairs)
        {
            // Split the key-value pair into key, type, and value
            string[] elements = keyValuePair.Split(':');
            string key = elements[0];
            string type = elements[1];
            string valueString = elements[2];

            // Parse the value based on the type
            object value;
            switch (type)
            {
                case "f":
                    value = float.Parse(valueString);
                    break;
                case "b":
                    value = bool.Parse(valueString);
                    break;
                default:
                    Debug.LogWarning("Unknown value type: " + type);
                    continue;
            }

            // Store the value in the dictionary
            sensorData[key] = value;
        }
        float sensorValue1 = (float)sensorData["Distance"]; // Convert the first byte to a float
        float sensorValue2 = (float)sensorData["Duration"];

        //message = serialPort.ReadLine();
        //distance = Int32.Parse(message);
        Debug.Log(sensorValue1+ ", " + sensorValue2);
        //Debug.Log(data);
    }
}
