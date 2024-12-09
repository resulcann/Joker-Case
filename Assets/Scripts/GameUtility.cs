using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtility
{
    /// <summary>
    /// Formalization numbers ("1.23K", "1.5M")
    /// </summary>
    public static string FormatNumber(int value)
    {
        if (value >= Mathf.Pow(10,6))
        {
            return (value / Mathf.Pow(10,6)).ToString("F2") + "M";
        }
        else if (value >= Mathf.Pow(10,3))
        {
            return (value / Mathf.Pow(10,3)).ToString("F2") + "K";
        }
        return value.ToString();
    }
}
