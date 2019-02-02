using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    /*
    1) The scriptable object is an efficient way to share key variables among multiple scrips, without the need of referencing all scripts to each other.
    2) It can be connected to prefab, so after instantiating prefab, the new instance doesn't have to search for the reference to this script
    (e.g. using FindObjectOfType method for each instance would be very inefficient, especially with thousands of instances).
    3) The key variables can be changed in Inspector, without the need of knowledge of the code or coding.
    */

    [Header("Random generating variables")]
    [Range(100, 10000)] public int maxNumberOfPlants = 100;     // number of trees that will be placed on terrain when "Create New Forrest" button is hit
    [Range(1, 100)] public int percentOfPlantsToFire = 10;      // % plants on terrain that will catch fire when "Start Random Fire" button is hit

    [Header("Plants color indication")]
    public Color green = Color.green;       // indicates normal green state
    public Color red = Color.red;           // indicates when plant is on fire
    public Color black = Color.black;       // indicates when plant is burned

    [Header("Fire spreading variables")]
    public float burningTime = 5f;          // it takes this time for a plant to get from OnFire state to burned state
    public float fireDistance = 3f;         // the reach of fire of burning plant, the plants beyond this distance cannot catch fire from this plant
    public float sphereCastRadius = 1f;     // the radius of sphere in SphereCast, which is used to calculate what plants will catch fire
    public float minWindSpeed = 1f;
    public float maxWindSpeed = 5f;         

    [Header("Masks to filter Raycasting")]
    public LayerMask fireColliderMask;
    public LayerMask plantMask;
    public LayerMask terrainMask;
    
    [HideInInspector] public Vector3 windDirection = new Vector3(0, 0, 0);
    [HideInInspector] public float waitForCatchFire = 1f;                   // calculated from wind speed
    [HideInInspector] public bool simulationRunning = false;                // used to turn the fire simulation on/off


    public enum PlantState { green, onFire, burned };

    // Using event system is an efficent way to broadcast information to all plants about wind change or when simulation is turned ON/OFF.

    public delegate void OnWindChangeDelegate(float windSpeed, Vector3 windDirection);
    public event OnWindChangeDelegate OnWindChange;

    public delegate void OnSimulationChangeDelegate();
    public event OnSimulationChangeDelegate OnSimulationChange;

    public void ChangeWind()
    {
        OnWindChange?.Invoke(waitForCatchFire, windDirection);
    }

    public void ChangeSimulation()
    {
        simulationRunning = !simulationRunning;
        OnSimulationChange?.Invoke();
    }
}
