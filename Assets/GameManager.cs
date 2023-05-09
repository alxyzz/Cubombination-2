using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{

    [SerializeReference] GameObject Losegame;


    #region singleton
    [HideInInspector]
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager singleton was null when called.");
            }
            return _instance;
        }

    }

    private static GameManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    #region variables

    bool isPlaying; //if true, time counts down
    int seconds;//if it reaches the time to win, we win


    
    #endregion



    #region settings
    [Header("Settings")]

    public int TIME_TO_WIN; //affects starting TIME_AVAILABLE

    public float SETTING_DISTANCE_COOLDOWN_PERIOD; //period after winning a module where it chills
    public float SETTING_LIGHT_COOLDOWN_PERIOD; //period after winning a module where it chills
    public float SETTING_KNOCK_COOLDOWN_PERIOD; //period after winning a module where it chills

    public float SETTING_DISTANCE_VALIDATION_TIME; //time we need to work on the module to make it cool down
    public float SETTING_LIGHT_SECONDS_TO_CORRECTLY_INTERACT_WITH; //time we need to work on the module to make it cool down
    public float SETTING_TOUCH_SECONDS_TO_TOUCH; //time we need to work on the module to make it cool down

    public float SETTING_DISTANCE_TIME_TO_EXPLODE; //time until explosion
    public float SETTING_LIGHT_TIME_TO_EXPLODE;//time until explosion
    public float SETTING_TOUCH_TIME_TO_EXPLODE; //time until explosion

    public float SETTING_TOUCH_MIN_TEMPCHANGE_INTERVAL = 1;  //time we need to work on the module to make it cool down
    public float SETTING_TOUCH_MAX_TEMPCHANGE_INTERVAL = 5; //time we need to work on the module to make it cool down

    #endregion

    #region references


    [SerializeField] ModuleDistance md;
    [SerializeField] ModuleLight ml;
    [SerializeField] ModuleTouch MyPerc;



    #endregion
    #region methods

    public void OnClickStart()
    {
        md.Restart();
        // ml._state = paused;
        MyPerc.StartOver();
    }

    public void Explode()
    {
        Debug.LogError("Exploded.");
        md._state = ModuleDistance.MDState.Paused;
       // ml._state = paused;
        MyPerc._state  = ModuleTouch.MPState.Paused;
        Losegame.SetActive(true);

    }



    void Update()
    {



        if (md._state != ModuleDistance.MDState.Waiting && MyPerc._state != ModuleTouch.MPState.Waiting)
        {
            Debug.LogWarning("Both modules are unpaused. Light should light up.");
            ArduinoIO.Instance._SerialPort.Write("L");
        }

    }
    
    #endregion
}



 