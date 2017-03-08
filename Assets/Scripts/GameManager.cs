using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

// Enum for scenes allowed in the project to be managed.
public enum SceneType : byte
{
    LOADING = 0,    // Loading scene used for level scenes preloading, not yet implemented.
    MENU = 1,       // Main menu scene, before playing the game.
    LEVEL1 = 2		// Level 1 scene.
}

// Enum for types of menu inside menu transictions. Should be refactored in a tree gerarchy (maybe?).
public enum MenuType : byte
{
    MENU_MAIN = 0,
    MENU_OPTIONS = 1,
    MENU_PAUSE = 2,
    MENU_GAMEOVER = 3
}

/*
	GameManager object has accountability to manage control flow of scenes and general behaviors.
	"Ported from richard project, needs to be refactored"
	-- Everything commented out at the moment is because we want to disable it until we have need to use them back again.
*/
public class GameManager
{
    // Singleton pattern.
    private static class SingletonLoader
    {
        public static readonly GameManager instance = new GameManager();
    }
    public static GameManager getInstance()
    {
        return SingletonLoader.instance;
    }

    //public AudioClip MusicSound,ClickSound;
    //public Texture background,blackbg;
    //public AudioSource m_MusicSource, m_SoundSource;
    public Settings m_Settings;     // Singleton object for managing settings.
    public bool IsShownMainMenu;
    private SceneType active_scene;  // Estabilishes which active scene is currently on quickly.
    private ThreadsafeDictionary<SceneType, string> scenes_strings;
    private SceneType scene_in_loading_stage;
    private ThreadsafeDictionary<string, Texture> textures;
    public Font rendering_font;     // Rendering font for GUI operations.

    // Note that this array matches enum values inside MenuTypes thanks by using same indexes, be careful.
    public readonly string[] MenuNames = new string[]{
        "Main Menu",		// LAYER 1.
		"Options Menu",		// As above.
		"Pause Menu",		// As above.
		"Game Over Menu"	// LAYER 2.
	};

    public MenuType active_menu { get; set; }
    public bool IsMenuActive { get; set; }
    public readonly GUI.WindowFunction[] MenuFunctions;
    public bool toggleGUI;

    private int fps;

    private int wumpa_counter=11,box_counter=23;
    /*
	public void setMusic(AudioClip ac){
		if(m_MusicSource != null)
		{
			m_MusicSource.clip = ac;
			m_MusicSource.Play();
		}
	}

	public void playSound(AudioClip sound){
		if(m_SoundSource != null) {
			m_SoundSource.PlayOneShot(sound);
		}
	}
	*/

    private int GUI_infobar_var_slide_factor;

    // GameManager constructor.
    public GameManager()
    {
        MenuFunctions = new GUI.WindowFunction[] {
            MainMenu,
            OptionsMenu,
            PauseMenu,
            GameOverMenu
        };
        scenes_strings = new ThreadsafeDictionary<SceneType, string>()
            .chained_Add(SceneType.MENU, "scene_preload")
            .chained_Add(SceneType.LEVEL1, "scene1");
        m_Settings = Settings.getInstance(); // Initialize settings object if not present and save reference to it.
        toggleGUI = true;
        GUI_infobar_var_slide_factor = -100;
        GUIHelper.setGUIStyle("GUI_infobar_label",new GUIStyle());
    }

    public void setActiveScene(SceneType new_active_scene){
        this.active_scene = new_active_scene;
    }

    public void setActiveMenu(MenuType new_active_menu){
        this.active_menu = new_active_menu;
    }

    public void setTextures(ThreadsafeDictionary<string, Texture> new_textures_dictionary){
        this.textures = new_textures_dictionary;
    }

    public void setRenderingFont(Font new_rendering_font){
        this.rendering_font = new_rendering_font;
        GUIHelper.getGUIStyle("GUI_infobar_label").font = this.rendering_font;
    }

    // Main Menu constructor.
    private void MainMenu(int id)
    {
        GUILayout.Label("Test");
        if (GUILayout.Button("Start Game"))
        {
            active_scene = SceneType.LOADING;
            Debug.Log("Start game calling...");

            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackbg);
            SceneManager.LoadScene(this.scenes_strings.Get(SceneType.LEVEL1));
            SceneManager.sceneLoaded += callback_onLoadScene;
        }

        if (GUILayout.Button("Options"))
        {
            //m_SoundSource.PlayOneShot(ClickSound);
            active_menu = MenuType.MENU_OPTIONS;
            // Code for options
        }

        if (!Application.isWebPlayer && !Application.isEditor)
        {
            if (GUILayout.Button("Exit"))
            {
                Application.Quit();
            }
        }
    }

    private void OptionsMenu(int id)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Music volume");
        //m_Settings.MusicVolume = GUILayout.HorizontalSlider(m_Settings.MusicVolume, 0.0f, 1.0f);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sound volume");
        //m_Settings.SoundVolume = GUILayout.HorizontalSlider(m_Settings.SoundVolume, 0.0f, 1.0f);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset high score"))
        {
            m_Settings.HighScore = 0; // Reset to 0
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Back"))
        {
            m_Settings.Save(); // Save settings
            //m_SoundSource.PlayOneShot(ClickSound); // *click* sound
            active_menu = MenuType.MENU_MAIN; // "Back" button returns from this menu to MainMenu
        }
    }
    private void PauseMenu(int id) { }
    private void GameOverMenu(int id) { }

    const int Width = 300, Height = 100;
    public void call_onGUI()
    {

        if (active_scene == SceneType.MENU)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
            string str_highscore = "Highscore: " + m_Settings.HighScore;
            Vector2 highscoreTextDim = GUIHelper.getPreferredLabelSize(str_highscore);
            GUI.Label(new Rect(Screen.width - highscoreTextDim.x, 0, highscoreTextDim.x, 100), str_highscore);

            Rect windowRect = new Rect(
                                  ((Screen.width - Width) / 2),
                                  ((Screen.height - Height) / 2),
                                  Width,
                                  Height
                              );
            GUILayout.Window(0, windowRect, MenuFunctions[(byte)active_menu], MenuNames[(byte)active_menu]);
            //GUI.Label (new Rect (0, 0, 100, 100), "It's working! Lol");
        }
        else if (active_scene == SceneType.LOADING)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackbg);
        }
        else if (active_scene == SceneType.LEVEL1)
        {
            if(toggleGUI){
                GUIHelper.drawLabel(new Rect(Screen.width-70, 0, 20, 20), "FPS: "+fps, Color.white, null, 15);  // Render FPS counter.
                GUI.DrawTexture(new Rect(20, 20+GUI_infobar_var_slide_factor, 60, 60), textures.Get("texture_wumpa"));  // Render wumpa.
                GUIHelper.drawLabel(new Rect(20+60, 40+GUI_infobar_var_slide_factor, 100, 100), wumpa_counter.ToString(), Color.white, "GUI_infobar_label", 32);    // Render wumpa counter.
                GUI.DrawTexture(new Rect(200, 20+GUI_infobar_var_slide_factor, 60, 60), textures.Get("texture_crate")); // Render box.
                GUIHelper.drawLabel(new Rect(200+60, 40+GUI_infobar_var_slide_factor, 100, 100), box_counter.ToString(), Color.white, "GUI_infobar_label", 32);     // Render box counter.
                GUIHelper.drawLabel(new Rect(100, Screen.height - 200, 100, 100), "SAMPLE TEXT", Color.white, "GUI_infobar_label", 32); // TODO delete...
                GUIHelper.drawLabel(new Rect(100, Screen.height - 50, 100, 100), "SAMPLE TEXT", "GUI_infobar_label");   // TODO delete...
            }
        }
    }

    /*
        Callback used when next scene is fully loaded.
     */
    void callback_onLoadScene(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Start game called!");
        active_scene = SceneType.LEVEL1;
    }

    /*
        This method shows and hides a wumpa fruit in crash-style way (when user presses triangle).
        The key to call this function is declared inside the Keeper (MonoBehaviour).

        We must pass the caller reference in order to execute coroutines.

        In this particular case, GUI_slide_factor is used inside the GameManager to determine the position of the textures on Y axis.
     */
    public IEnumerator showGUIExtra(MonoBehaviour caller, Action<bool> callback_onCoroutineStatusChanged){
        callback_onCoroutineStatusChanged(true);

        GUI_infobar_var_slide_factor=-100;  // Initial position.
        int initial_value=GUI_infobar_var_slide_factor;
        // We call a coroutine for scrolling from -100 to 0 in 0.4 secs.
        yield return caller.StartCoroutine(
            CoroutineHelper.executeInTime(0.4f, (float w)=>{
                GUI_infobar_var_slide_factor=initial_value+(int)Mathf.Lerp(0,100,w);    // GUI_slide_factor will increase from -100 to 0.
            })
        );
        // Wait 2 seconds, enough to make user show the menu. It's not really framerate-indipendent, but that doesn't matter.
        yield return new WaitForSeconds(2f);

        // We call a coroutine from scrolling to the reached value.
        yield return caller.StartCoroutine(
            CoroutineHelper.executeInTime(0.4f, (float w)=>{
                GUI_infobar_var_slide_factor=-(int)Mathf.Lerp(0,100,w);  // GUI_slide_factor will decrease from 0 to -100. Initial_value is already 0, so it won't count.
            })
        );

        callback_onCoroutineStatusChanged(false);
    }

    public IEnumerator showFPS() {
		while(true){
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(1.0f);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;
 
			// Update it.
			fps = Mathf.RoundToInt(frameCount / timeSpan);
		}
	}
}
