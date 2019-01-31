using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationButtonManager : MonoBehaviour
{
    [SerializeField] GameObject startFireText;
    [SerializeField] GameObject stopFireText;
    [SerializeField] Stats stats;

    private void Start()
    {
        stats.simulationRunning = false;
        UpdateText();
    }
    
    public void OnButtonClick()
    {
        stats.ChangeSimulation();
        UpdateText();
    }

    private void UpdateText()
    {
        startFireText.SetActive(!stats.simulationRunning);
        stopFireText.SetActive(stats.simulationRunning);
    }
}
