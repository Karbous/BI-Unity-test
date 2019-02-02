using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    /*
    This method is responsible for the mouse action, when plant or terrain is clicked, depending on mouse action mode selected.
    Because I have to distinguish between plant and terrain collider, I cannot use OnMouseDown method and I have to create my own method with custom raycast.
    */

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


    // changing state and text on button when clicked
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
                MouseActionOnPlant();
            }
            else if (activeButton == ButtonState.add && Physics.Raycast(ray, out hit, 1000, stats.terrainMask))
            {
                plants.AddPlant(hit.point);
            }
        }
    }

    private void MouseActionOnPlant()
    {
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
}
