using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] Stats stats;
    [SerializeField] Slider windSpeedSlider;
    [SerializeField] Slider windDirectionSlider;
    [SerializeField] Image windArrow;

    private void Start()
    {
        windSpeedSlider.maxValue = stats.maxWindSpeed;
        windSpeedSlider.minValue = stats.minWindSpeed;
        stats.waitForCatchFire = windSpeedSlider.maxValue - (windSpeedSlider.value - windSpeedSlider.minValue);
        stats.windDirection = Quaternion.Euler(new Vector3(0, windDirectionSlider.value, 0)) * Vector3.forward;
    }

    public void RotateWindArrow(Slider slider)
    {
        windArrow.transform.eulerAngles = new Vector3(windArrow.transform.eulerAngles.x, windArrow.transform.eulerAngles.y,  - 135 - slider.value);
    }

    public void ChangeWindSpeed(Slider slider)
    {
        stats.waitForCatchFire = windSpeedSlider.maxValue - (slider.value - windSpeedSlider.minValue);
        stats.ChangeWind();
    }

    public void ChangeWindDirection(Slider slider)
    {
        stats.windDirection = Quaternion.Euler(new Vector3(0, slider.value, 0)) * Vector3.forward;
        stats.ChangeWind();
    }
}
