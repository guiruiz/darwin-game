using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataDisplay : MonoBehaviour
{
    public Boat boat;
    public Wind wind;

    void OnGUI()
    {
        GenerateLabels(new string[] {
            "Wind Angle " + wind.direction,
            "Wind Speed " + wind.speed,
            "Hull Angle " + boat.hullRotation,
            "Sail Angle " + boat.mastRotation
        });
    }

    private void GenerateLabels(string[] labels)
    {
        int padding = 20;
        for (int i = 0; i < labels.Length; i++)
        {
            int topPadding = i * padding;
            GUI.Label(new Rect(25, topPadding, 200, 40), labels[i]);
        }
    }
}
