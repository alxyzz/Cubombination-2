using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModuleDistance : MonoBehaviour
{

    public enum MDState
    {
        Waiting,
        Playing,
        Paused
    }
    public MDState _state = MDState.Playing;
    #region ParentSneakPeek

    [SerializeField] VISUAL_CRITICAL_WARNING warning;
    //[SerializeField] UnityEvent doOnActivation;
    //[SerializeField] UnityEvent doOnFulfillment;
    [SerializeField] TextMeshProUGUI txt_Distance;
    [SerializeField] TextMeshProUGUI txt_TargetDistance;
    [SerializeField] TextMeshProUGUI txt_ValidationCountdown;
    [SerializeField] TextMeshProUGUI txt_TimeToExplode;
    float timeToExplode;
    float currentTime;

    [SerializeField] float HandMovementFactor;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    [SerializeField] Sprite OK_HAND, HAND;
    [SerializeField] Image HeightMarker;
    [SerializeField] Image img_Hand;


    void Start()
    {
        // Debug.LogError("TEST");
        timeToExplode = GameManager.Instance.SETTING_DISTANCE_TIME_TO_EXPLODE;
        Init();
    }

    public void Restart()
    {
        StartCoroutine(StartCooldown());
    }
    private IEnumerator StartCooldown()
    {
        _state = MDState.Waiting;
        timeWeHaveBeenCorrect = 0;

        txt_ValidationCountdown.gameObject.SetActive(false);
        txt_ValidationCountdown.text = "";
        //selfImage.color = inactiveColor;
        yield return new WaitForSeconds(waitDuration); // Wait for 10 seconds
       // selfImage.color = activeColor;
        Init();
        _state = MDState.Playing;

    }

    float _SensorDistance
    {
        get
        {
            return (float)ArduinoIO.Instance._Distance;
        }
    }


    #endregion

    #region properties
    private void Update()
    {

        

        switch (_state)
        {
            case MDState.Waiting:
                warning.Stop();
                break;
            case MDState.Playing:
                warning.Begin();
                Process();
                break;
            case MDState.Paused:
                warning.Stop();

                //we do nothing.
                break;
            default:
                break;
        }
    }

    void Process()
    {
        txt_Distance.text = _SensorDistance.ToString();

        UpdateTiming();
        UpdateDistance();
        UpdateHand();





        void UpdateTiming()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeToExplode)
            {

                GameManager.Instance.Explode();
            }
            txt_TimeToExplode.text = currentTime.ToString("F");
            if (correct)
            {
                if (timeWeHaveBeenCorrect > timeToValidate)
                {
                    StartCoroutine(StartCooldown());

                }
              
                txt_ValidationCountdown.gameObject.SetActive(true);
                txt_ValidationCountdown.text = (timeToValidate -timeWeHaveBeenCorrect).ToString("F");
                   timeWeHaveBeenCorrect += Time.deltaTime;
                
            }
            else
            {
                txt_ValidationCountdown.gameObject.SetActive(false);
                txt_TimeToExplode.gameObject.SetActive(true);
                txt_TimeToExplode.text = timeLeftToExplosion.ToString("F");
            }
            
        }
        void UpdateDistance()
        {

            if (correct) //checks if we are close enough to sweet spot
            {
                timeWeHaveBeenCorrect += Time.deltaTime;
                img_Hand.sprite = OK_HAND;
                if (timeWeHaveBeenCorrect >= timeToValidate)
                {
                    timeWeHaveBeenCorrect = 0;
                    StartCoroutine(StartCooldown());
                    return;
                }
            }
            else
            {
                img_Hand.sprite = HAND;
               
            }


        }
        void UpdateHand()
        {


            //ArduinoIO.Instance._Distance
            HeightMarker.rectTransform.sizeDelta = new Vector2(HeightMarker.rectTransform.sizeDelta.x, _SensorDistance);
            img_Hand.transform.localPosition = new Vector3(0, Mathf.Clamp((_SensorDistance* HandMovementFactor), 0, 200), 0);
            Debug.LogWarning(" img_Hand.transform.localPosition = new Vector3(0, _SensorDistance, 0); => _SensorDistance is equal to " + _SensorDistance);




            
        }
    }

    float waitDuration
    {
        get
        {
            return GameManager.Instance.SETTING_DISTANCE_COOLDOWN_PERIOD;
        }
    }
    float timeToValidate
    {
        get
        {
            return GameManager.Instance.SETTING_DISTANCE_VALIDATION_TIME;
        }
    }

    bool correct
    {
        get
        {
            if (Mathf.Abs(ArduinoIO.Instance._Distance - SweetSpot) < MarginOfError)
            {
                return true;
            }
            else return false;

            //if (Mathf.Abs(HeightMarker.rectTransform.sizeDelta.y - SweetSpot) < MarginOfError)
            //{
            //    return true;
            //}
            //else return false;
        }
    }

   float arduinoDistance
    {
        get
        {
            object value;

            if (ArduinoIO.Instance.sensorData.TryGetValue("distance", out value))
            {
                return System.Single.Parse((string)value);
            }
            else return 0f;
        }
    }
    #endregion


    #region variables
    float timeWeHaveBeenCorrect;//how much time we've been within the Good Range. resets if we leave
    int SweetSpot;
    int MarginOfError = 10;//max distance from place
    int maxRecognizedDistanceFromCube = 40;
    float timeLeftToExplosion;
    #endregion
    #region references



    #endregion


    #region methods
    void Init()
    {
        timeLeftToExplosion = GameManager.Instance.SETTING_DISTANCE_TIME_TO_EXPLODE;
        SweetSpot = Random.Range(1, maxRecognizedDistanceFromCube);

        Debug.LogWarning("Generated random sweet spot for the distance module.");
        txt_TargetDistance.text = SweetSpot.ToString();
    }
   

   

    #endregion


    //what do we do here?
    //we check the inverse percentage of how close the hand is to the arduino controller and sync it to the hand sprite
    //if it's close enough to the target, we are valid and just gotta wait there





}
