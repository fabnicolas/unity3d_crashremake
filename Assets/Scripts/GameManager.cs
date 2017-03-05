using UnityEngine;
using UnityEngine.SceneManagement;

// Enum for scenes allowed in the project to be managed.
public enum SceneList : byte
{
    LOADING = 0,    // Loading scene used for level scenes preloading, not yet implemented.
    MENU = 1,       // Main menu scene, before playing the game.
    LEVEL1 = 2		// Level 1 scene.
}

// Enum for types of menu inside menu transictions. Should be refactored in a tree gerarchy (maybe?).
public enum MenuTypes : byte
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
    public SceneList active_scene;  // Estabilishes which active scene is currently on quickly.
	public ThreadsafeDictionary<SceneList,string> scenes_strings;
    public SceneList scene_in_loading_stage;


    private GUIStyle gui_style;     // GUI graphical properties.
    public Font rendering_font;     // Rendering font for GUI operations.

    // Note that this array matches enum values inside MenuTypes thanks by using same indexes, be careful.
    public readonly string[] MenuNames = new string[]{
        "Main Menu",		// LAYER 1.
		"Options Menu",		// As above.
		"Pause Menu",		// As above.
		"Game Over Menu"	// LAYER 2.
	};

    public MenuTypes ActiveMenu { get; set; }
    public bool IsMenuActive { get; set; }
    public readonly GUI.WindowFunction[] MenuFunctions;

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

	// GameManager constructor.
    public GameManager()
    {
        MenuFunctions = new GUI.WindowFunction[] {
            MainMenu,
            OptionsMenu,
            PauseMenu,
            GameOverMenu
        };
        scenes_strings = new ThreadsafeDictionary<SceneList,string>()
            .AddChain(SceneList.MENU, "scene_preload")
            .AddChain(SceneList.LEVEL1, "scene1");
        m_Settings = Settings.getInstance(); // Initialize settings object if not present and save reference to it.

        Debug.Log("GameManager in creazione...");
        //Application.runInBackground = true;	// Application can run in background.
        gui_style = new GUIStyle();         // Initialize an empty GUIStyle.
        gui_style.font = rendering_font;    // Set font taken from inspector to the gui_style object.
        Debug.Log("GameManager creato");
    }

	// Main Menu constructor.
    private void MainMenu(int id)
    {
        GUILayout.Label("Test");
        if (GUILayout.Button("Start Game"))
        {
            active_scene = SceneList.LOADING;
            Debug.Log("Start game calling...");

            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackbg);
            SceneManager.LoadScene(this.scenes_strings.Get(SceneList.LEVEL1));
            SceneManager.sceneLoaded += OnLoadSceneCallback;
        }

        if (GUILayout.Button("Options"))
        {
            //m_SoundSource.PlayOneShot(ClickSound);
            ActiveMenu = MenuTypes.MENU_OPTIONS;
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
            ActiveMenu = MenuTypes.MENU_MAIN; // "Back" button returns from this menu to MainMenu
        }
    }
    private void PauseMenu(int id) { }
    private void GameOverMenu(int id) { }

    const int Width = 300, Height = 100;
    public void call_onGUI()
    {
		
        if (active_scene == SceneList.MENU)
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
            GUILayout.Window(0, windowRect, MenuFunctions[(byte)ActiveMenu], MenuNames[(byte)ActiveMenu]);
            //GUI.Label (new Rect (0, 0, 100, 100), "It's working! Lol");
        }
        else if (active_scene == SceneList.LOADING)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackbg);
        }
        else if (active_scene == SceneList.LEVEL1)
        {
            GUI.Label(new Rect(100, Screen.height-20, 100, 100), "SAMPLE TEXT", gui_style);
        }
    }

   void OnLoadSceneCallback(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Start game called!");
        active_scene = SceneList.LEVEL1;
    }

}
