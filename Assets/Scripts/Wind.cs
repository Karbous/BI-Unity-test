using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] PlantStats plantStats;
    [SerializeField] Slider windSpeedSlider;
    [SerializeField] Slider windDirectionSlider;

    private void Start()
    {
        plantStats.waitForCatchFire = windSpeedSlider.maxValue - (windSpeedSlider.value - windSpeedSlider.minValue);
        plantStats.windDirection = Quaternion.Euler(new Vector3(0, windDirectionSlider.value, 0)) * Vector3.forward;
    }

    public void RotateWindBall(Slider slider)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, slider.value, transform.eulerAngles.z);
    }

    public void ChangeWindSpeed(Slider slider)
    {
        plantStats.waitForCatchFire = windSpeedSlider.maxValue - (slider.value - windSpeedSlider.minValue);
        plantStats.ChangeWind();
    }

    public void ChangeWindDirection(Slider slider)
    {
        plantStats.windDirection = Quaternion.Euler(new Vector3(0, slider.value, 0)) * Vector3.forward;
        plantStats.ChangeWind();
    }
}
