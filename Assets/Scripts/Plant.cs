using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] Stats stats;

    Stats.PlantState plantState = Stats.PlantState.green;

    IEnumerator burnCoroutine;
    IEnumerator spreadFireCoroutine;
     
   
    private void OnEnable()
    {
        stats.OnWindChange += WindChanged;
    }

    private void WindChanged(float windSpeed, Vector3 windDirection)
    {
        if (spreadFireCoroutine != null && plantState == Stats.PlantState.onFire)
        {
            StopCoroutine(spreadFireCoroutine);
            spreadFireCoroutine = SpreadFire();
            StartCoroutine(spreadFireCoroutine);
        }
    }
    
    public void SetOnFire()
    {
        plantState = Stats.PlantState.onFire;
        ChangeColor(stats.red);
        burnCoroutine = Burn();
        spreadFireCoroutine = SpreadFire();
        StartCoroutine(burnCoroutine);
        StartCoroutine(spreadFireCoroutine);
    }

    IEnumerator Burn()
    {
        yield return new WaitForSeconds(stats.burningTime);
        if (spreadFireCoroutine != null)
        {
            StopCoroutine(spreadFireCoroutine);
        }
        ChangeColor(stats.black);
        plantState = Stats.PlantState.burned;
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

    public void ResetStateAndColor()
    {
        if (burnCoroutine != null && spreadFireCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            StopCoroutine(spreadFireCoroutine);
        }
        plantState = Stats.PlantState.green;
        ChangeColor(stats.green);
    }

    public void ChangeColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    
    private void OnDisable()
    {
        stats.OnWindChange -= WindChanged;
    }
}
