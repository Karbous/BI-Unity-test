using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Plants : MonoBehaviour
{
    [SerializeField] Stats stats;
    float plantSize = 1f;
    float terrainSize = 100f;

    [SerializeField] Terrain terrain;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] LayerMask terrainMask;
    [SerializeField] Transform plantsOnTerrainParent;
    [SerializeField] Transform plantsPoolParent;

    List<GameObject> plantsOnTerrain = new List<GameObject>();
    List<GameObject> plantsPool = new List<GameObject>();


    private void Start()
    {
        terrainSize = terrain.terrainData.bounds.max.x;
        plantSize = plantPrefab.transform.localScale.x;
    }

    public void GeneratePlants()
    {
        int plantsToBeGenerated = stats.maxNumberOfPlants;

        if (plantsOnTerrain.Count > 0)
        {
            ClearPlants();
        }

        UsePlantsFromPool(ref plantsToBeGenerated);

        if (plantsToBeGenerated > 0)
        {
            for (int i = 0; i < plantsToBeGenerated; i++)
            {
                InstantiateNewPlant();
            }
        }
    }

    /*
    public void GeneratePlantsWithDestroy()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            Destroy(plantsOnTerrain[i]);
        }
        for (int i = 0; i < stats.maxNumberOfPlants; i++)
        {
            InstantiateNewPlant();
        }
    }
    */


    
    private void InstantiateNewPlant()
    {
        GameObject newPlant = Instantiate(plantPrefab, PlantRandomPosition(), Quaternion.identity, plantsOnTerrainParent);
        plantsOnTerrain.Add(newPlant);
        newPlant.transform.SetParent(plantsOnTerrainParent);
    }

    private void UsePlantsFromPool(ref int plantsToBeGenerated)
    {
        while (plantsPool.Count > 0 && plantsToBeGenerated > 0)
        {
            plantsPool[0].SetActive(true);
            plantsPool[0].transform.position = PlantRandomPosition();
            plantsPool[0].transform.SetParent(plantsOnTerrainParent);
            plantsOnTerrain.Add(plantsPool[0]);
            plantsPool.RemoveAt(0);
            plantsToBeGenerated--;
        }
    }

    public void ClearPlants()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            plantsOnTerrain[i].transform.SetParent(plantsPoolParent);
            plantsOnTerrain[i].GetComponent<Plant>().ResetStateAndColor();
            plantsOnTerrain[i].SetActive(false);
        }
        plantsPool.AddRange(plantsOnTerrain);
        plantsOnTerrain.Clear();
    }


    public void SetPlantsOnFire()
    {
        if (plantsOnTerrain.Count > 0)
        {
            int plantSetOnFire = plantsOnTerrain.Count * stats.percentOfPlantsToFire / 100;
            for (int i = 0; i < plantSetOnFire; i++)
            {
                plantsOnTerrain[i].GetComponent<Plant>().SetOnFire();
            }
        }
    }

    public void StopFireSimulation()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            plantsOnTerrain[i].GetComponent<Plant>().ResetStateAndColor();
        }

    }

    private Vector3 PlantRandomPosition()
    {
        RaycastHit terrainSurface;

        Vector3 newPlantPosition = new Vector3(
            UnityEngine.Random.Range(plantSize / 2, terrainSize - (plantSize / 2)),
            100f,
            UnityEngine.Random.Range(plantSize / 2, terrainSize - (plantSize / 2))
            );
        Physics.Raycast(newPlantPosition, Vector3.down, out terrainSurface, 1000, terrainMask);
        newPlantPosition.y = terrainSurface.point.y + (plantSize / 2);

        return newPlantPosition;
    }
}
