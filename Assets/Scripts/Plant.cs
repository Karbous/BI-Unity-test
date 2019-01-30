using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] PlantStats plantStats;

    PlantStats.PlantState plantState = PlantStats.PlantState.green;

    IEnumerator burnCoroutine;
    IEnumerator spreadFireCoroutine;
     
   
    private void OnEnable()
    {
        plantStats.OnWindChange += WindChanged;
    }

    private void WindChanged(float windSpeed, Vector3 windDirection)
    {
        //restart fire spreading
    }


    public void SetOnFire()
    {
        plantState = PlantStats.PlantState.onFire;
        ChangeColor(plantStats.red);
        burnCoroutine = Burn();
        spreadFireCoroutine = SpreadFire();
        StartCoroutine(burnCoroutine);
        StartCoroutine(spreadFireCoroutine);
    }

    IEnumerator Burn()
    {
        yield return new WaitForSeconds(plantStats.burningTime);
        if (spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
        }
        ChangeColor(plantStats.black);
        plantState = PlantStats.PlantState.burned;
    }
    IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(plantStats.waitForCatchFire);
        RaycastHit[] plantsToCatchFire = Physics.SphereCastAll(transform.position, plantStats.sphereCastRadius, plantStats.windDirection, plantStats.burnDistance, plantStats.plantMask);
        for (int i = 0; i < plantsToCatchFire.Length; i++)
        {
            Plant plantHit = plantsToCatchFire[i].collider.gameObject.GetComponent<Plant>();
            if (plantHit.plantState == PlantStats.PlantState.green)
            {
                plantHit.SetOnFire();
            }
        }
    }




    public void ResetStateAndColor()
    {
        if (burnCoroutine != null && spreadFireCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            StopCoroutine(spreadFireCoroutine);
        }
        plantState = PlantStats.PlantState.green;
        ChangeColor(plantStats.green);
    }

    public void ChangeColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    
    private void OnDisable()
    {
        plantStats.OnWindChange -= WindChanged;
    }
}
