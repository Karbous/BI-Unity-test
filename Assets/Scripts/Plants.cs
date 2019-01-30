using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Plants : MonoBehaviour
{
    [SerializeField] [Range(100, 10000)] int maxNumberOfPlants = 100;
    float plantSize = 1f;
    float terrainSize = 100f;
    float terrainHeight;
    [SerializeField] [Range(1, 100)] int percentOfPlantsToFire = 20;

    [SerializeField] Terrain terrain;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] LayerMask terrainMask;
    [SerializeField] Transform plantsOnTerrainParent;
    [SerializeField] Transform plantsPoolParent;

    List<GameObject> plantsOnTerrain = new List<GameObject>();
    List<GameObject> plantsPool = new List<GameObject>();

    Vector3 plantsPoolPosition = new Vector3(1000, 1000, 1000);


    private void Start()
    {
        terrainSize = terrain.terrainData.bounds.max.x;
        terrainHeight = terrain.terrainData.bounds.max.y;
        plantSize = plantPrefab.transform.localScale.x;
    }

    public void GeneratePlants()
    {
        int plantsToBeGenerated = maxNumberOfPlants;

        if (plantsOnTerrain.Count > 0)
        {
            ClearPlants();
        }

        MovePlantsFromPool(ref plantsToBeGenerated);

        if (plantsToBeGenerated > 0)
        {
            for (int i = 0; i < plantsToBeGenerated; i++)
            {
                InstantiateNewPlant();
            }
        }
    }


    /*public void GeneratePlantsWithDestroy()
    {
        DestroyAllPlants();
        for (int i = 0; i < maxNumberOfPlants; i++)
        {
            InstantiateNewPlant();
        }

    }

    private void DestroyAllPlants()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            Destroy(plantsOnTerrain[i]);
        }
    }
    */

    private void MovePlantsFromPool(ref int plantsToBeGenerated)
    {
        while (plantsPool.Count > 0 && plantsToBeGenerated > 0)
        {
            MovePlant(plantsPool[0], PlantRandomPosition(), plantsOnTerrainParent);
            plantsOnTerrain.Add(plantsPool[0]);
            plantsPool.RemoveAt(0);
            plantsToBeGenerated--;
        }
    }

    public void ClearPlants()
    {
        for (int i = 0; i < plantsOnTerrain.Count; i++)
        {
            MovePlant(plantsOnTerrain[i], plantsPoolPosition, plantsPoolParent);
            plantsOnTerrain[i].GetComponent<Plant>().ResetStateAndColor();
        }
        plantsPool.AddRange(plantsOnTerrain);
        plantsOnTerrain.Clear();
    }

    private void InstantiateNewPlant()
    {
        GameObject newPlant = Instantiate(plantPrefab, PlantRandomPosition(), Quaternion.identity, plantsOnTerrainParent);
        newPlant.GetComponent<BoxCollider>().size = new Vector3(
            newPlant.GetComponent<BoxCollider>().size.x,
            2 * terrainHeight,
            newPlant.GetComponent<BoxCollider>().size.z
            );
        plantsOnTerrain.Add(newPlant);
        newPlant.transform.SetParent(plantsOnTerrainParent);
    }

    public void SetPlantsOnFire()
    {
        if (plantsOnTerrain.Count > 0)
        {
            int plantSetOnFire = plantsOnTerrain.Count * percentOfPlantsToFire / 100;
            for (int i = 0; i < plantSetOnFire; i++)
            {
                plantsOnTerrain[i].GetComponent<Plant>().SetOnFire();
            }
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

    private void MovePlant(GameObject plant, Vector3 newPosition, Transform newParent)
    {
        plant.transform.position = newPosition;
        plant.transform.SetParent(newParent);
    }
}
