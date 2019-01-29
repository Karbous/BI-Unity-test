using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableEnableButton : MonoBehaviour
{
    public void EnableButton()
    {
        GetComponent<Button>().interactable = true;
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }
}
