using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeEffect : MonoBehaviour
{
    public UnityAction DoOnComplete;
    

    public void WinModule()
    {
        DoOnComplete.Invoke();
    }
}
