using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VISUAL_CRITICAL_WARNING : MonoBehaviour
{
    public Image img;
    public GameObject  txt_Critical;
    public GameObject txt_Chilling ;
    bool on;
    private void Start()
    {
        img = GetComponent<Image>();
    }



    public void Begin()
    {
        if (on)
        {
            return;
        }
        img.color = Color.red;
        on = true;
     txt_Chilling.SetActive(false);
      txt_Critical.SetActive(true);
        InvokeRepeating("LoopFlash", 0, 4);

    }


    public void Stop()
    {
        if (!on)
        {
            return;
        }
        on = false;

        img.color = Color.green;
       txt_Chilling.SetActive(true);
       txt_Critical.SetActive(false);
        StopAllCoroutines();
    }



    IEnumerator LoopFlash()
    {
        img.color = Color.red;
        yield return new WaitForSecondsRealtime(2);

        img.color = Color.yellow;
        yield return new WaitForSecondsRealtime(2);
    }



}
