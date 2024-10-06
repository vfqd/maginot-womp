using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class FloatParameter : ScriptableEnum
{
    public float startingValue;
    public float value;

    public void ResetValue()
    {
        value = startingValue;
    }
    
    public static implicit operator float(FloatParameter value) 
    {
        return value.value;
    }

    public void AddValue(float amount)
    {
        value += amount;
    }

    public void MultiplyValue(float perc)
    {
        value *= perc;
    }
}
