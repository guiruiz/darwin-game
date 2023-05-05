using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataDisplay : MonoBehaviour
{
    public Boat boat;
    public Wind wind;
    public SailingForces sailingForces;

    void OnGUI()
    {
        GenerateLabels(new string[] {
            "Wind Angle " + wind.rotation,
            "Wind Speed " + wind.speed,
            "",
            "Hull Angle " + boat.hullRotation,
            "Sail Angle " + boat.mastRotation,
            "Speed " + boat.GetSpeed(),
            "",
            "Thrust " + sailingForces.resultDirection
        });
    }

    private void GenerateLabels(string[] labels)
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        Font font = GUI.skin.font;
        style.font = GUI.skin.font;
        style.fontStyle = FontStyle.Bold;


        int padding = 20;
        for (int i = 0; i < labels.Length; i++)
        {
            int topPadding = i * padding;
            string text = labels[i];
            Rect position = new Rect(25, topPadding, 200, 40);

            GUI.Label(position, text, style);
            DrawTextOutline(position, text, style, Color.black, Color.white);
        }
    }

    public void DrawTextOutline(Rect position, string text, GUIStyle style, Color outColor, Color inColor)
    {
        var s = style;
        style.normal.textColor = outColor;
        position.x--;
        GUI.Label(position, text, style);
        position.x += 2;
        GUI.Label(position, text, style);
        position.x--;
        position.y--;
        GUI.Label(position, text, style);
        position.y += 2;
        GUI.Label(position, text, style);
        position.y--;
        style.normal.textColor = inColor;
        GUI.Label(position, text, style);
        style = s;
    }
}
