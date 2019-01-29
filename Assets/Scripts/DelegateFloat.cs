using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Delegate with float input parameter")]
public class DelegateFloat : ScriptableObject
{
    public delegate void OnValueChangeDelegate(float value);
    public event OnValueChangeDelegate OnValueChange;

    public void ChangeValue(float value)
    {
        OnValueChange?.Invoke(value);
    }
}