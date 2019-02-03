using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    /*
    This script is responsible for adding and removing plants on terrain.
    It can also set random plants on terrain on fire.

    I tried several different approaches when the forest is cleared or regenerated and checked performance in Profiler:
    1 ) Destroy the plants and instantiate again.
    2 ) Disable game object when moved to pool and enable again when moved from pool (by SetActive method).
    3 ) Same as 2 ), but only disabling certain components
    4 ) Just move the plant to far location out of camera when moved to pool
    The first three options just worked poorly, with huge memory allocations and heavy CPU usage. The forth works best for me, although I think there must even a better way to do it.
    */

    // plants and terrain size
    float plantSizeX = 1f;
    float plantSizeY = 1f;
    float plantSizeZ = 1f;
    float terrainSizeX = 100f;
    float terrainSizeZ = 100f;

    [SerializeField] Stats stats;
    [SerializeField] Terrain terrain;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] Transform plantsOnTerrainParent;
    [SerializeField] Transform plantsPoolParent;

    // I use lists to track which plants on terrain and in pool
    [SerializeField] List<GameObject> plantsOnTerrain = new List<GameObject>();
    [SerializeField] List<GameObject> plantsPool = new List<GameObject>();

    // position of plant pool
    Vector3 plantsPoolPosition = new Vector3(1000, 1000, 1000);

    private void Start()
    {
        // check plant prefab and terrain size
        terrainSizeX = terrain.terrainData.bounds.max.x;
        terrainSizeZ = terrain.terrainData.bounds.max.z;
        plantSizeX = plantPrefab.GetComponent<Renderer>().bounds.max.x;
        plantSizeY = plantPrefab.GetComponent<Renderer>().bounds.max.y;
        plantSizeZ = plantPrefab.GetComponent<Renderer>().bounds.max.z;
    }

    public void GeneratePlants()
    {
        int plantsToBeGenerated = stats.maxNumberOfPlants;

        // move plants from terrain to pool
        if (plantsOnTerrain.Count > 0)
        {
            ClearPlants();
        }

        // generate plants from pool
        UsePlantsFromPool(ref plantsToBeGenerated);

        // if pool is empty and more plants need to be generated, instantiate new plants
        if (plantsToBeGenerated > 0)
        {
            for (int i = 0; i < plantsToBeGenerated; i++)
            {
                InstantiateNewPlant(PlantRandomPosition());
            }
        }
    }


    public void ClearPlants()
    {
        while (plantsOnTerrain.Count > 0)
        {
            ClearPlant(plantsOnTerrain[0]);
        }
    }

    public void ClearPlant(GameObject plant)
    {
        plant.GetComponent<Plant>().MovedToPool();      //reset to deafault state
        plant.transform.SetParent(plantsPoolParent);    //change parent
        plantsPool.Add(plant);                          // move from pool List to terrain List
        plantsOnTerrain.Remove(plant);
        plant.transform.position = plantsPoolPosition;  //move to pool location
    }


    private void UsePlantsFromPool(ref int plantsToBeGenerated)
    {
        while (plantsPool.Count > 0 && plantsToBeGenerated > 0)
        {
            AddPlantFromPool(PlantRandomPosition(), plantsPool[0]);
            plantsToBeGenerated--;
        }
    }

    private void AddPlantFromPool(Vector3 plantPosition, GameObject plant)
    {
        // similar as ClearPlant method
        plant.transform.SetParent(plantsOnTerrainParent);
        plantsOnTerrain.Add(plant);
        plantsPool.Remove(plant);
        plant.transform.position = plantPosition;
    }

    private void InstantiateNewPlant(Vector3 plantPosition)
    {
        GameObject newPlant = Instantiate(plantPrefab, plantPosition, Quaternion.identity, plantsOnTerrainParent);
        plantsOnTerrain.Add(newPlant);
        newPlant.transform.SetParent(plantsOnTerrainParent);
    }


    // called when clicked on terrain and the Add mode is on
    public void AddPlant(Vector3 plantPosition)
    {
        if (plantsPool.Count > 0)
        {
            AddPlantFromPool(plantPosition, plantsPool[0]);
        }
        else
        {
            InstantiateNewPlant(plantPosition);
        }
    }

    public void SetRandomPlantsOnFire()
    {
        if (plantsOnTerrain.Count > 0)
        {
            int plantsToSetOnFire = plantsOnTerrain.Count * stats.percentOfPlantsToFire / 100;

            // if there are too few plants on terrain, it will set fire at least 1 random plant
            if (plantsToSetOnFire <= 0)
            {
                plantsToSetOnFire = 1;
            }
            else
            {
                List<int> randomIndexes = new List<int>();
                while (randomIndexes.Count < plantsToSetOnFire)
                {
                    int newIndex = UnityEngine.Random.Range(0, plantsOnTerrain.Count);
                    if (!randomIndexes.Contains(newIndex))
                    {
                        // The script doesn't take into account whether or not the random plant is already on fire.
                        // It really doesn't matter, beacuase we don't need exact number and it add a little bit to the randomness.
                        plantsOnTerrain[newIndex].GetComponent<Plant>().SetOnFire();
                        randomIndexes.Add(newIndex);
                    }
                }
            }
        }
    }

    #region Helper methods

    // It creates temporary Vector3 with random X and Z position, and then it finds the terrain surface with raycasting to get correct Y position
    private Vector3 PlantRandomPosition()
    {
        RaycastHit terrainSurface;

        Vector3 newPlantPosition = new Vector3(
            UnityEngine.Random.Range(plantSizeX / 2, terrainSizeX - (plantSizeX / 2)),
            100f,
            UnityEngine.Random.Range(plantSizeZ / 2, terrainSizeZ - (plantSizeZ / 2))
            );
        if (Physics.Raycast(newPlantPosition, Vector3.down, out terrainSurface, 1000, stats.terrainMask))
        {
            newPlantPosition.y = terrainSurface.point.y + (plantSizeY / 2);
        }
        return newPlantPosition;
    }
    #endregion
}
