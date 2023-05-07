using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
//using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Module : MonoBehaviour
{
    public bool isSolved = false;
    [SerializeField] private bool isWaiting = false;
    [SerializeField] private float waitDuration = 10f;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    private void Update()
    {
        if (isSolved && !isWaiting)
        {
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        isWaiting = true;
        isSolved = true;
        this.GetComponent<Image>().color = inactiveColor;
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        this.GetComponent<Image>().color = activeColor;
        isSolved = false;
        isWaiting = false;
    }
}
