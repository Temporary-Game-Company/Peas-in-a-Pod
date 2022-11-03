using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TemporaryGameCompany
{
    [CreateAssetMenu(menuName = "Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        #if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
        #endif

        public float Value; // Value of float.

        // Set value.
        public void SetValue(float value) 
        { 
            Value = value; 
        }

        public void SetValue(FloatVariable value) 
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}