using UnityEngine;

/*
    Utilities for GUI system.
 */
public class GUIHelper{
    // Calculates the necessary size for inserting a string inside the GUI.
    private static ThreadsafeDictionary<string,GUIStyle> gui_styles = new ThreadsafeDictionary<string,GUIStyle>();

    public static Vector2 getPreferredLabelSize(string str)
    {
        return GUI.skin.label.CalcSize(new GUIContent(str));
    }

    public static void drawTextureFullSize(Vector2 position, Texture texture){
        GUI.DrawTexture(new Rect(position.x, position.y, texture.width, texture.height), texture);
    }

    public static void drawTextureResized(Vector2 position, Texture texture){
        GUI.DrawTexture(GUIHelper.ResizeGUI(new Rect(position.x, position.y, texture.width, texture.height)), texture);
    }

    public static void drawTextureDyn(Rect render_data, Texture texture){
        GUI.DrawTexture(GUIHelper.ResizeGUI(render_data), texture);
    }

    public static void setGUIStyle(string key, GUIStyle gui_style, bool force_rewrite = false){
        gui_styles.Add(key, gui_style, force_rewrite);
    }

    public static GUIStyle getGUIStyle(string key){
        return gui_styles.Get(key);
    }

    public static void drawLabel(Rect render_data, string text, string style=null, int fontSize = -1){
        GUIStyle guistyle;
        if(style!=null) guistyle = gui_styles.Get(style);
        else            guistyle = null;

        if(fontSize > -1){
            guistyle = new GUIStyle(guistyle);
            guistyle.fontSize = fontSize;
        }

        if(guistyle!=null){
            GUI.Label(render_data, text, guistyle);
        }else{
            GUI.Label(render_data, text);
        }
    }

    /*
        May be removed in future, took from Unity3D forum:
        http://answers.unity3d.com/questions/307330/gui-scale-guis-according-to-resolution.html
    */
    public static Rect ResizeGUI(Rect _rect)
    {
        float FilScreenWidth = _rect.width / 800;
        float rectWidth = FilScreenWidth * Screen.width;
        float FilScreenHeight = _rect.height / 600;
        float rectHeight = FilScreenHeight * Screen.height;
        float rectX = (_rect.x / 800) * Screen.width;
        float rectY = (_rect.y / 600) * Screen.height;
        
        return new Rect(rectX,rectY,rectWidth,rectHeight);
    }
}