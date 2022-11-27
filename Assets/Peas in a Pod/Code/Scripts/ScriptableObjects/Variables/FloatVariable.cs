using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ValueChangedDelegate(float oldValue, float newValue);
namespace TemporaryGameCompany
{
    [CreateAssetMenu(menuName = "Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        #if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
        #endif

        public float Value; // Value of float.

        public ValueChangedDelegate ValueChanged;

        // Set value.
        public void SetValue(float value) 
        { 
            float old = Value;
            Value = value; 
            ValueChanged(old, Value);
        }

        public void SetValue(FloatVariable value)
        {
            float old = Value;
            Value = value.Value;
            ValueChanged(old, Value);

        }

        public void ApplyChange(float amount)
        {
            float old = Value;
            Value += amount;
            ValueChanged(old, Value);
        }

        public void ApplyChange(FloatVariable amount)
        {
            float old = Value;
            Value += amount.Value;
            ValueChanged(old, Value);
        }
    }
}