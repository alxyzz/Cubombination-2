using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
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

    #region settings
    [Header("Settings")]

    public int SETTING_STARTING_TIME_AVAILABLE; //affects starting TIME_AVAILABLE
    public float ROTATION_DURATION; //interval over which the cube rotates as the player navigates 
    public float INTERVAL_BEAT; //period between beats
    public float TIME_UNTIL_WIN; //time to be elapsed until win. should always be longer than SETTING_STARTING_TIME_AVAILABLE. affects what number TOTAL_TIME_ELAPSED has to be to win.




    #endregion

    #region references

    [Header("References")]
    public GameObject obj_CUBE;
    public TextMeshProUGUI txt_COORDINATE_DISPLAY;
    public TextMeshProUGUI txt_CURRENT_MODULE_DESCRIPTION;
    public PostProcessVolume _volume;

    public List<CubeModule> CUBE_FACETS = new List<CubeModule>();

    #endregion


    #region private variables
    bool BEATING;
    bool ROTATING = false;
    float TARGET_Y = 0, TARGET_X = 0;
    float TIME_SINCE_BEAT;
    float TOTAL_TIME_ELAPSED;




    #endregion


    #region properties
    float rotationX
    {
        get
        {
            return obj_CUBE.transform.rotation.x;
        }
    }
    float rotationY
    {
        get
        {
            return obj_CUBE.transform.rotation.y;
        }
    }


    #endregion

    
    //6 facets - 1 top, 2 bottom, 3 left, 4 right, 5 front, 6 back
   //facet 1 - game where you gotta whack the orb when it's on one of the blue spaces













    #region methods
    private void RotateLeft()
    {
        //THE_CUBE.transform.Rotate(new Vector3(0,-90,0));
        //if (!rotating)
        //{
        //    StartCoroutine(rotater(new Vector3(0, -90, 0), THE_CUBE));

        //}


        TARGET_Y -= 90; if (TARGET_Y < -360)
        {
            TARGET_Y = 0;
        }

    }
    private void RotateRight()
    {
        //k THE_CUBE.transform.Rotate(new Vector3(0, 90, 0));
        //if (!rotating)
        //{
        //    StartCoroutine(rotater(new Vector3(0, 90, 0), THE_CUBE));

        //}

        TARGET_Y += 90; if (TARGET_Y > 360)
        {
            TARGET_Y = 0;
        }
    }
    private void RotateUp()
    {
        //THE_CUBE.transform.Rotate(new Vector3(90, 0, 0));
        //if (!rotating)
        //{
        //    StartCoroutine(rotater(new Vector3(90, 0, 0), THE_CUBE));

        //}

        TARGET_X -= 90; if (TARGET_X < -360)
        {
            TARGET_X = 0;
        }
    }

    private void RotateDown()
    {
        //THE_CUBE.transform.Rotate(new Vector3(-90, 0, 0));
        //if (!rotating)
        //{
        //    StartCoroutine(rotater(new Vector3(-90, 0, 0), THE_CUBE));

        //}

        TARGET_X += 90;
        if (TARGET_X > 360)
        {
            TARGET_X = 0;
        }
    }

    void StartRotation()
    {
        StartCoroutine(LerpFunction(Quaternion.Euler(new Vector3(TARGET_X, TARGET_Y, 0)), ROTATION_DURATION));
    }

    private void ProcessInput()
    {
        if (ROTATING == true)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateLeft();
            StartRotation();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateRight();
            StartRotation();

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateUp();
            StartRotation();

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotateDown();
            StartRotation();

        }


    }
    void UpdateFeedback()
    {
        txt_COORDINATE_DISPLAY.text = "Target X = " + TARGET_X.ToString() + "\nTarget Y = " + TARGET_Y.ToString();
    }

    void RotateCube()
    {
        //float angleX = Mathf.LerpAngle(THE_CUBE.transform.rotation.x, targetX, (Time.time +Time.deltaTime) * SPEED_ROTATION);
        //float angleY = Mathf.LerpAngle(THE_CUBE.transform.rotation.y, targetY, (Time.time + Time.deltaTime) * SPEED_ROTATION);

        //float angleX = Mathf.LerpAngle(0, targetX, (Time.time + Time.deltaTime) * SPEED_ROTATION);
        //float angleY = Mathf.LerpAngle(0, targetY, (Time.time + Time.deltaTime) * SPEED_ROTATION);
        //THE_CUBE.transform.eulerAngles = new Vector3(angleX, angleY, 0);

    }



    IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
        ROTATING = true;
        float time = 0;
        Quaternion startValue = obj_CUBE.transform.rotation;
        while (time < duration)
        {
            obj_CUBE.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obj_CUBE.transform.rotation = endValue;
        ROTATING = false;
    }



    void LoseGame()
    {

    }

    void WinGame()
    {

    }
    /// <summary>
    /// does a beat for each facet, if they lost time, everything explodes
    /// </summary>
    void DoBeat()
    {

        TOTAL_TIME_ELAPSED++;
        if (TOTAL_TIME_ELAPSED >= TIME_UNTIL_WIN)
        {

            BEATING = false;
            WinGame();
        }

        bool HasAnyFacetExploded = false;
        foreach (var item in CUBE_FACETS)
        {
            if (item.DoTick())
            {
                HasAnyFacetExploded = true;
            }
           
        }
        if (HasAnyFacetExploded)
        {
            BEATING = false;
            LoseGame();
        }
        else
        {
            DoCubeExpansion();
        }
    }

    void DoCubeExpansion()
    {
        StopCoroutine(AnimateFishEyeLens());
    }
    IEnumerator AnimateFishEyeLens()
    {

        yield return new WaitForSecondsRealtime(2f);

    }
    void ProcessBeat()
    {
        if (BEATING)
        {
            TIME_SINCE_BEAT += Time.deltaTime;
            if (TIME_SINCE_BEAT >= INTERVAL_BEAT)
            {

                TIME_SINCE_BEAT = 0;
                DoBeat();
            }
        }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        UpdateFeedback();
        ProcessBeat();
    }
}
