using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    public enum PlantState { green, onFire, burned };

    public delegate void OnWindChangeDelegate(float windSpeed, Vector3 windDirection);
    public event OnWindChangeDelegate OnWindChange;

    [Range(100, 10000)] public int maxNumberOfPlants = 100;
    [Range(1, 100)] public int percentOfPlantsToFire = 10;

    public Color green = Color.green;
    public Color red = Color.red;
    public Color black = Color.black;

    public float burningTime = 5f;
    public float burnDistance = 3f;
    public float sphereCastRadius = 1f;

    public float minWindSpeed = 1f;
    public float maxWindSpeed = 5f;



    [HideInInspector] public Vector3 windDirection = new Vector3(0, 0, 0);
    [HideInInspector] public float waitForCatchFire = 1f;

    public LayerMask fireColliderMask;


    public void ChangeWind()
    {
        OnWindChange?.Invoke(waitForCatchFire, windDirection);
    }
}
