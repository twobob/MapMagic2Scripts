using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildrenOnEnableAfterTime : MonoBehaviour
{

    public float delay = 5;

    public void EnableChildren() {

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    
    } 

    void OnEnable()
    {
        Invoke("EnableChildren", delay);

    }
}
