using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSimulationButtonManager : MonoBehaviour
{
    [SerializeField] GameObject startFireText;
    [SerializeField] GameObject stopFireText;
    [SerializeField] Plants plants;
    bool startFire = true;

    private void Start()
    {
        startFire = true;
        UpdateText();
    }

    public void OnForrestGenerate()
    {
        startFire = true;
        UpdateText();
        GetComponent<Button>().interactable = true;
    }

    public void OnForrestClear()
    {
        startFire = true;
        UpdateText();
        GetComponent<Button>().interactable = false;
    }

    public void OnFireButtonClick()
    {
        if (startFire)
        {
            startFire = false;
            UpdateText();
            plants.SetPlantsOnFire();
        }
        else
        {
            startFire = true;
            UpdateText();
            plants.StopFireSimulation();
        }
    }


    private void UpdateText()
    {
        startFireText.SetActive(startFire);
        stopFireText.SetActive(!startFire);
    }
}
