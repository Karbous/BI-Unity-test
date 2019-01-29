using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] DelegateFloat windChangeDelegate;

    public void RotateWindBall(Slider slider)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, slider.value, transform.eulerAngles.z);
    }

    public void ChangeWindDirection(Slider slider)
    {
        windChangeDelegate.ChangeValue(slider.value);
    }

}
