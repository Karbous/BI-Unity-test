using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] Stats stats;

    public Stats.PlantState plantState = Stats.PlantState.green;

    IEnumerator burnCoroutine;
    IEnumerator spreadFireCoroutine;
     
   
    private void OnEnable()
    {
        stats.OnWindChange += RestartFireSpreading;
        stats.OnSimulationChange += SimulationChanged;
    }

    private void RestartFireSpreading(float windSpeed, Vector3 windDirection)
    {
        if (stats.simulationRunning && plantState == Stats.PlantState.onFire && spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
            spreadFireCoroutine = SpreadFire();
            StartCoroutine(spreadFireCoroutine);
        }
    }

    private void SimulationChanged()
    {
        if (stats.simulationRunning && plantState == Stats.PlantState.onFire)
        {
            StartBurning();
        }
        else
        {
            StopBurning();
        }
    }

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

    private void StopBurning()
    {
        if (plantState == Stats.PlantState.onFire && spreadFireCoroutine != null && burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            StopCoroutine(spreadFireCoroutine);
        }
    }

    public void Extinguish()
    {
        ResetPlantState(Stats.PlantState.burned, stats.black);
    }

    IEnumerator Burn()
    {
        yield return new WaitForSeconds(stats.burningTime);
        if (spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
        }
        ChangePlantState(stats.black, Stats.PlantState.burned);
    }
    IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(stats.waitForCatchFire);
        RaycastHit[] plantsToCatchFire = Physics.SphereCastAll(
            (transform.position + transform.localScale.x * stats.windDirection),
            stats.sphereCastRadius,
            stats.windDirection,
            stats.burnDistance,
            stats.fireColliderMask
            );
        for (int i = 0; i < plantsToCatchFire.Length; i++)
        {
            Plant plantHit = plantsToCatchFire[i].collider.gameObject.GetComponentInParent<Plant>();
            if (plantHit.plantState == Stats.PlantState.green)
            {
                plantHit.SetOnFire();
            }
        }
    }

    public void ResetToGreen()
    {
        ResetPlantState(Stats.PlantState.green, stats.green);
    }
      

    private void ResetPlantState(Stats.PlantState plantState, Color color)
    {
        if (burnCoroutine != null && spreadFireCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            StopCoroutine(spreadFireCoroutine);
        }
        ChangePlantState(color, plantState);
    }
       
    private void ChangePlantState(Color newColor, Stats.PlantState newState)
    {
        GetComponent<Renderer>().material.color = newColor;
        plantState = newState;
    }

    
    private void OnDisable()
    {
        stats.OnWindChange -= RestartFireSpreading;
        stats.OnSimulationChange -= SimulationChanged;
    }
}
