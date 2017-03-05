using UnityEngine;

/*
    Utilities for GUI system.
 */
public class GUIHelper{
    // Calculates the necessary size for inserting a string inside the GUI.
    public static Vector2 getPreferredLabelSize(string str)
    {
        return GUI.skin.label.CalcSize(new GUIContent(str));
    }

    public static void drawTextureFullSize(Vector2 position, Texture texture){
        GUI.DrawTexture(new Rect(position.x, position.y, texture.width, texture.height), texture);
    }
}