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
        // reads the min and max wind speed value from Stats
        windSpeedSlider.maxValue = stats.maxWindSpeed;
        windSpeedSlider.minValue = stats.minWindSpeed;

        // reset the wind speed and direction to default values
        stats.waitForCatchFire = CalculateWaitForCatchFire(windSpeedSlider);
        stats.windDirection = CalculateWindDirection(windDirectionSlider);
    }

    public void RotateWindArrow(Slider slider)
    {
        windArrow.transform.eulerAngles = new Vector3(windArrow.transform.eulerAngles.x, windArrow.transform.eulerAngles.y,  - 135 - slider.value);
    }

    public void ChangeWindSpeed(Slider slider)
    {
        stats.waitForCatchFire = CalculateWaitForCatchFire(slider);
        // I decided that the change in wind speed will not invoke OnWindChange event 
        // - the waitForCatchFire variable will be changed, but the fire spreading coroutine will not be resetted
        //stats.ChangeWind();
    }

    public void ChangeWindDirection(Slider slider)
    {
        stats.windDirection = CalculateWindDirection(slider);
        // when the wind direction is changed by the slider, it invokes the OnWindChange event
        stats.ChangeWind();
    }

    private float CalculateWaitForCatchFire (Slider windSpeedSlider)
    {    
        // e.g. if min wind speed is 1 and max wind speed is 5, it means that it will take 5s for plants to catch fire with min wind speed and 1s with max wind speed 
        return windSpeedSlider.maxValue - (windSpeedSlider.value - windSpeedSlider.minValue);
    }

    private Vector3 CalculateWindDirection(Slider windDirectionSlider)
    {
        // since fire spreading is simplified in 2D (XZ plane), we are reading only rotation in Y axis
        return Quaternion.Euler(new Vector3(0, windDirectionSlider.value, 0)) * Vector3.forward;
    }
}
