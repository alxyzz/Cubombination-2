using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleTouch : MonoBehaviour
{
    [SerializeReference] TextMeshProUGUI txt_TimeToExplode;
    float currentTime;
    float timeToExplode;

    public enum MPState
    {
        Waiting,
        RedHot,
        Cold,
        Paused
    }
    public MPState _state = MPState.RedHot;
    #region References
    [SerializeField] VISUAL_CRITICAL_WARNING warning;
    [SerializeField] List<Image> wires = new();

    [SerializeField] Color hotwire, coldwire;
    [SerializeField] TextMeshProUGUI txt_CurrentSecondsTouched;
    [SerializeField] TextMeshProUGUI txt_GoalSecondsTouched;





    #endregion 
    float secondsLeftTillValidation;
    void Start()
    {
        StartCooldown();
        txt_GoalSecondsTouched.text = GameManager.Instance.SETTING_TOUCH_SECONDS_TO_TOUCH.ToString();

    }

    private IEnumerator StartCooldown()
    {
        warning.Stop();
        txt_CurrentSecondsTouched.text = "OK";
        foreach (var item in wires)
        {
            item.color = coldwire;
        }
        _state = MPState.Waiting;
        warning.Stop();
        secondsLeftTillValidation = GameManager.Instance.SETTING_TOUCH_SECONDS_TO_TOUCH;
        yield return new WaitForSeconds(GameManager.Instance.SETTING_KNOCK_COOLDOWN_PERIOD); // Wait for 10 seconds
        txt_CurrentSecondsTouched.text = GameManager.Instance.SETTING_TOUCH_SECONDS_TO_TOUCH.ToString();
        warning.Begin();
        RandomlyChooseBetweenHotOrCold();


    }

    void FlipTemperature()
    {
        Debug.LogWarning("Flipped temperature");


        if (_state == MPState.RedHot)
        {
            _state = MPState.Cold;
            foreach (var item in wires)
            {
                item.color = coldwire;
            }
        }
        else if (_state == MPState.Cold)
            _state = MPState.RedHot;
        foreach (var item in wires)
        {
            item.color = hotwire;
        }

    }


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

    public void StartOver()
    {
        StartCoroutine(StartCooldown());
    }
   

    void RandomlyChooseBetweenHotOrCold()
    {


        int b = Random.Range(1, 10);
        Debug.LogWarning("RandomlyChooseBetweenHotOrCold.b was "+ b.ToString());
        if (b > 4)
        {
            _state = MPState.RedHot;
            foreach (var item in wires)
            {
                item.color = hotwire;
            }
            Debug.LogWarning("Randomly picked to be ");

        }
        else
        {
            _state = MPState.Cold;
            foreach (var item in wires)
            {
                item.color = coldwire;
            }
            Debug.LogWarning("Randomly picked to be cold.");

        }
        StartCoroutine(DelayedTempChange(Random.Range(GameManager.Instance.SETTING_TOUCH_MIN_TEMPCHANGE_INTERVAL, GameManager.Instance.SETTING_TOUCH_MAX_TEMPCHANGE_INTERVAL)));

    }
    IEnumerator DelayedTempChange(float b)
    {
        yield return new WaitForSecondsRealtime(b);

        FlipTemperature();
    }



    #region properties
     void Update()
    {
        switch (_state)
        {
            case MPState.Waiting:



                break;
            case MPState.RedHot:
                Process();

                break;
            case MPState.Cold:
                Process();

                break;
            case MPState.Paused:


                break;
            default:
                break;
        }



    }



    void Process()
    {



        currentTime += Time.deltaTime;
        if (currentTime >= timeToExplode)
        {

            GameManager.Instance.Explode();
        }
        txt_TimeToExplode.text = currentTime.ToString("F");

        txt_CurrentSecondsTouched.text = (secondsLeftTillValidation).ToString("F");
        if (CheckTouch())
        {
            switch (_state)
            {
                case MPState.RedHot:
                    secondsLeftTillValidation = Mathf.Clamp(secondsLeftTillValidation + (Time.deltaTime * 2), 0, 500); break;
                case MPState.Cold:
                    secondsLeftTillValidation = Mathf.Clamp(secondsLeftTillValidation - Time.deltaTime, 0, 500); break;
                default:
                    break;
            }
        }

        txt_CurrentSecondsTouched.text = (GameManager.Instance.SETTING_TOUCH_SECONDS_TO_TOUCH - secondsLeftTillValidation).ToString("F");

        if (secondsLeftTillValidation <= 0)
        {
            StartCoroutine(StartCooldown());
            return;
        }



        bool CheckTouch()
        {
            if (arduinoTouch)
            {
                return true;
            }
            else return false;
        }
    }




    #endregion
}
