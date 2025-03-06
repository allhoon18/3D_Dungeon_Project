using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Extension
{
    static public float RoundToDecimalPlaces(this float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(value * multiplier) / multiplier;
    }

}
