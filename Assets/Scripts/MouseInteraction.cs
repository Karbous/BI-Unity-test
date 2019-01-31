using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    enum ButtonState { fire, add, remove }
    ButtonState activeButton = ButtonState.fire;

    [SerializeField] GameObject fireText;
    [SerializeField] GameObject addText;
    [SerializeField] GameObject removeText;

    [SerializeField] Plants plants;
    [SerializeField] Stats stats;
    Plant clickedPlant;
    RaycastHit hit;
    Ray ray;


    public void SwitchMode()
    {
        switch (activeButton)
        {
            case ButtonState.fire:
                activeButton = ButtonState.add;
                fireText.SetActive(false);
                addText.SetActive(true);
                removeText.SetActive(false);
                break;
            case ButtonState.add:
                activeButton = ButtonState.remove;
                fireText.SetActive(false);
                addText.SetActive(false);
                removeText.SetActive(true);
                break;
            case ButtonState.remove:
                activeButton = ButtonState.fire;
                fireText.SetActive(true);
                addText.SetActive(false);
                removeText.SetActive(false);
                break;
            default:
                Debug.LogError("Invalid mouse interaction button state!");
                break;
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if ((activeButton == ButtonState.fire || activeButton == ButtonState.remove) && Physics.Raycast(ray, out hit, 1000, stats.plantMask))
            {
                clickedPlant = hit.collider.gameObject.GetComponent<Plant>();

                if (activeButton == ButtonState.fire)
                {
                    if (clickedPlant.plantState == Stats.PlantState.green)
                    {
                        clickedPlant.SetOnFire();
                    }
                    else if (clickedPlant.plantState == Stats.PlantState.onFire)
                    {
                        clickedPlant.Extinguish();
                    }
                }
                else if (activeButton == ButtonState.remove)
                {
                    plants.ClearPlant(clickedPlant.gameObject);
                }
            }
            else if (activeButton == ButtonState.add && Physics.Raycast(ray, out hit, 1000, stats.terrainMask))
            {
                plants.AddPlant(hit.point);
            }
        }
    }

}
