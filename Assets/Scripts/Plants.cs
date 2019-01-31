using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class Plants : MonoBehaviour
{
    float plantSize = 1f;
    float terrainSizeX = 100f;
    float terrainSizeZ = 100f;


    [SerializeField] Stats stats;
    [SerializeField] Terrain terrain;
    [SerializeField] GameObject plantPrefab;
    //[SerializeField] SimulationButtonManager fireSimulationButton;
    [SerializeField] Transform plantsOnTerrainParent;
    [SerializeField] Transform plantsPoolParent;

    List<GameObject> plantsOnTerrain = new List<GameObject>();
    List<GameObject> plantsPool = new List<GameObject>();


    private void Start()
    {
        terrainSizeX = terrain.terrainData.bounds.max.x;
        terrainSizeZ = terrain.terrainData.bounds.max.z;
        plantSize = plantPrefab.GetComponent<Renderer>().bounds.max.x;
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
                InstantiateNewPlant(PlantRandomPosition());
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


    
    private void InstantiateNewPlant(Vector3 plantPosition)
    {
        GameObject newPlant = Instantiate(plantPrefab, plantPosition, Quaternion.identity, plantsOnTerrainParent);
        plantsOnTerrain.Add(newPlant);
        newPlant.transform.SetParent(plantsOnTerrainParent);
    }

    private void UsePlantsFromPool(ref int plantsToBeGenerated)
    {
        while (plantsPool.Count > 0 && plantsToBeGenerated > 0)
        {
            AddPlantFromPool(PlantRandomPosition());
            plantsToBeGenerated--;
        }
    }

    public void ClearPlants()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            plantsOnTerrain[i].transform.SetParent(plantsPoolParent);
            plantsOnTerrain[i].GetComponent<Plant>().ResetToGreen();
            plantsOnTerrain[i].SetActive(false);
        }
        plantsPool.AddRange(plantsOnTerrain);
        plantsOnTerrain.Clear();
    }

    public void ClearPlant(GameObject plant)
    {
        plant.transform.SetParent(plantsPoolParent);
        plant.GetComponent<Plant>().ResetToGreen();
        plant.SetActive(false);
        plantsPool.Add(plant);
        plantsOnTerrain.Remove(plant);
        if (plantsOnTerrain.Count <= 0)
        {
            //fireSimulationButton.OnForrestClear();
        }
    }

    public void AddPlant(Vector3 plantPosition)
    {
        if (plantsOnTerrain.Count <= 0)
        {
            //fireSimulationButton.OnForrestGenerate();
        }

        if (plantsPool.Count > 0)
        {
            AddPlantFromPool(plantPosition);
        }
        else
        {
            InstantiateNewPlant(plantPosition);
        }
    }

    private void AddPlantFromPool(Vector3 plantPosition)
    {
        plantsPool[0].SetActive(true);
        plantsPool[0].transform.position = plantPosition;
        plantsPool[0].transform.SetParent(plantsOnTerrainParent);
        plantsOnTerrain.Add(plantsPool[0]);
        plantsPool.RemoveAt(0);
    }

    public void SetRandomPlantsOnFire()
    {
        if (plantsOnTerrain.Count > 0)
        {
            int plantSetOnFire = plantsOnTerrain.Count * stats.percentOfPlantsToFire / 100;
            if (plantSetOnFire <= 0)
            {
                plantsOnTerrain[0].GetComponent<Plant>().SetOnFire();
            }
            else
            {
                List<int> randomIndexes = new List<int>();
                while (randomIndexes.Count < plantSetOnFire)
                {
                    int newIndex = UnityEngine.Random.Range(0, plantsOnTerrain.Count);
                    if (!randomIndexes.Contains(newIndex))
                    {
                        randomIndexes.Add(newIndex);
                    }
                }
                for (int i = 0; i < randomIndexes.Count; i++)
                {
                    plantsOnTerrain[randomIndexes[i]].GetComponent<Plant>().SetOnFire();
                }
            }
        }
    }


    private Vector3 PlantRandomPosition()
    {
        RaycastHit terrainSurface;

        Vector3 newPlantPosition = new Vector3(
            UnityEngine.Random.Range(plantSize / 2, terrainSizeX - (plantSize / 2)),
            100f,
            UnityEngine.Random.Range(plantSize / 2, terrainSizeZ - (plantSize / 2))
            );
        if (Physics.Raycast(newPlantPosition, Vector3.down, out terrainSurface, 1000, stats.terrainMask))
        {
            newPlantPosition.y = terrainSurface.point.y + (plantSize / 2);
        }
        return newPlantPosition;
    }
}
