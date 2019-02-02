using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    /*
    This script is responsible for the fire simulation of each plant - it can set the plant of fire and calculate if and how the fire will spread.
    Each plant has a corresponding state: green (default state, can catch fire), onFire (is burning, can spread fire), burned (cannot catch fire again)
    I used coroutines to handle catching fire and burning, in order to call methods after a certain time. I can also easily stop a start these coroutines when needed.

    The fire spreading simulation is simplified only in 2D (XZ plane). The fire spreading is calculated by SphereCastAll method.
    Each plant has a special fire collider indicating whether or not it was hit by SphereCastAll.
    The fire collider is scaled 7 time in Y axis (= 3 plant heights both above and below the plant - see Plant prefab).
    */
    
    [SerializeField] Stats stats;

    public Stats.PlantState plantState = Stats.PlantState.green;

    IEnumerator burnCoroutine;
    IEnumerator spreadFireCoroutine;

    private void OnEnable()
    {
        // suscribing methods to events
        stats.OnWindChange += RestartFireSpreading;
        stats.OnSimulationChange += SimulationChanged;
    }

    // when plant caches fire and the simulation is running, the plant will start burning and spread fire
    public void SetOnFire()
    {
        ChangePlantState(stats.red, Stats.PlantState.onFire);
        if (stats.simulationRunning)
        {
            StartBurning();
        }
    }

    private void StartBurning()
    {
        burnCoroutine = Burn();
        spreadFireCoroutine = SpreadFire();
        StartCoroutine(burnCoroutine);
        StartCoroutine(spreadFireCoroutine);
    }

    IEnumerator Burn()
    {
        // when the timer runs out, the plant stops spreading fire and is burned
        yield return new WaitForSeconds(stats.burningTime);
        if (spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
        }
        ChangePlantState(stats.black, Stats.PlantState.burned);
    }

    IEnumerator SpreadFire()
    {
        Plant plantHit = null;

        // wait time depending on wind speed
        yield return new WaitForSeconds(stats.waitForCatchFire);

        // cast sphere in direction of of wind and find all plant-fire colliders it hit
        RaycastHit[] plantsToCatchFire = Physics.SphereCastAll(
            (transform.position + transform.localScale.x * stats.windDirection),
            stats.sphereCastRadius,
            stats.windDirection,
            stats.fireDistance,
            stats.fireColliderMask
            );

        // all plants that were hit and are green will catch fire
        for (int i = 0; i < plantsToCatchFire.Length; i++)
        {
            plantHit = plantsToCatchFire[i].collider.gameObject.GetComponentInParent<Plant>();
            if (plantHit.plantState == Stats.PlantState.green)
            {
                plantHit.SetOnFire();
            }
        }
    }
    

    // called by event when simulation is turn ON/OFF, starting or stopping the coroutines
    private void SimulationChanged()
    {
        if (plantState == Stats.PlantState.onFire)
        {
            if (stats.simulationRunning)
            {
                StartBurning();
            }
            else
            {
                StopBurning();
            }
        }
    }

    private void StopBurning()
    {
        if (spreadFireCoroutine != null && burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            StopCoroutine(spreadFireCoroutine);
        }
    }

    // called by event when wind direction is changed
    private void RestartFireSpreading(float windSpeed, Vector3 windDirection)
    {
        if (stats.simulationRunning && plantState == Stats.PlantState.onFire && spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
            spreadFireCoroutine = SpreadFire();
            StartCoroutine(spreadFireCoroutine);
        }
    }

    // called when plant is removed from terrain to plant pool
    public void MovedToPool()
    {
        StopFireAndResetState(Stats.PlantState.green, stats.green);
    }

     // called when clicked on and plant is on fire (when Fire/Extinguish mode is on)
    public void Extinguish()
    {
        StopFireAndResetState(Stats.PlantState.burned, stats.black);
    }
    
    #region Helper methods
    // stops all coroutines and reset the plant state
    private void StopFireAndResetState(Stats.PlantState plantState, Color color)
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        if (spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
        }
        ChangePlantState(color, plantState);
    }

    // reset plant to certain state and coresponding color
    private void ChangePlantState(Color newColor, Stats.PlantState newState)
    {
        GetComponent<Renderer>().material.color = newColor;
        plantState = newState;
    }
    #endregion


    private void OnDisable()
    {        
        // unsuscribing methods from events
        stats.OnWindChange -= RestartFireSpreading;
        stats.OnSimulationChange -= SimulationChanged;
    }
}