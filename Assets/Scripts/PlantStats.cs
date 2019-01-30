using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Plant Stats")]
public class PlantStats : ScriptableObject
{
    public delegate void OnWindChangeDelegate(float windSpeed, Vector3 windDirection);
    public event OnWindChangeDelegate OnWindChange;

    public enum PlantState { green, onFire, burned };

    public LayerMask plantMask;

    public Color green = Color.green;
    public Color red = Color.red;
    public Color black = Color.black;

    public float burningTime = 5f;
    public float burnDistance = 3f;
    public float sphereCastRadius = 1f;

    public Vector3 windDirection = new Vector3(0, 0, 0);
    public float waitForCatchFire = 1f;

    public void ChangeWind()
    {
        OnWindChange?.Invoke(waitForCatchFire, windDirection);
    }
}
