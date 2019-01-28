using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants : MonoBehaviour
{
    [SerializeField] [Range(100, 10000)] int numberOfPlants = 100;
    [SerializeField] float plantSize = 1f;
    [SerializeField] float terrainSize = 100f;
    [SerializeField] GameObject plantPrefab;
    [SerializeField] LayerMask terrainMask;
    Transform plantsParent;

    private void Start()
    {
        plantsParent = GetComponent<Transform>();
    }


    public void GeneratePlants()
    {
        if (plantPrefab != null)
        {
            for (int i = 0; i < numberOfPlants; i++)
            {
                GameObject newPlant = InstantiateNewPlant();
                MovePlantToSurface(newPlant);

                // add plant to list of plant on terrain


            }
        }
        else
        {
            Debug.LogError("Plant prefab is missing!");
        }

    }

    private void MovePlantToSurface(GameObject newPlant)
    {
        RaycastHit terrainSurface;
        Physics.Raycast(newPlant.transform.position, Vector3.down, out terrainSurface, 1000, terrainMask);
        newPlant.transform.position = new Vector3(newPlant.transform.position.x, terrainSurface.point.y + (plantSize / 2), newPlant.transform.position.z);
    }

    private GameObject InstantiateNewPlant()
    {
        Vector3 newPlantPosition = new Vector3(
            Random.Range(plantSize / 2, terrainSize - (plantSize / 2)),
            100f,
            Random.Range(plantSize / 2, terrainSize - (plantSize / 2))
            );
        return Instantiate(plantPrefab, newPlantPosition, Quaternion.identity, plantsParent);
    }
}
