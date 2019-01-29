using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    enum PlantState { green, onFire, burned };  // put somewhere else?

    PlantState plantState = PlantState.green;

    [SerializeField] Color green;
    [SerializeField] Color red;
    [SerializeField] Color black;

    [SerializeField] DelegateFloat windChangeDelegate;

    [SerializeField] float burningTime = 3f;
    IEnumerator burnCoroutine;

    private void OnEnable()
    {
        // add methods to delegate
    }

    public void SetOnFire()
    {
        plantState = PlantState.onFire;
        ChangeColor(red);
        burnCoroutine = Burn();
        StartCoroutine(burnCoroutine);
        //start spreading fire
    }

    IEnumerator Burn()
    {
        yield return new WaitForSeconds(burningTime);
        ChangeColor(black);
        plantState = PlantState.burned;
    }

    private void ChangeColor(Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }

    public void ResetStateAndColor()
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        plantState = PlantState.green;
        ChangeColor(green);
    }

    private void OnDisable()
    {
        //remove methods from delegate
    }

}
