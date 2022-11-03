using System;
using UnityEngine;

namespace TemporaryGameCompany
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        // Default Constructor.
        public FloatReference() 
        { }

        // Constructor for constants.
        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        // Getter for value.
        public float Value
        {
            get { return UseConstant ? ConstantValue : 
                                    Variable.Value; }
            set { Variable.Value = value; }
        }

        // Implicit conversion to float.
        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}