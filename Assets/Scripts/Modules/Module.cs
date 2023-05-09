using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Module : MonoBehaviour
{
    public bool isSolved = false;
    [SerializeField] private bool isWaiting = false;
    float waitDuration;
    [SerializeField] UnityEvent doOnActivation;
    [SerializeField] UnityEvent doOnFulfillment;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    Image selfImage;

    void Start()
    {
        selfImage = GetComponent<Image>();
    }

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
        selfImage.color = inactiveColor;
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        selfImage.color = activeColor;
        isSolved = false;
        isWaiting = false;
    }
}
