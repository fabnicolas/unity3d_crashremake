using UnityEngine;

public class GameManagerKeeper : MonoBehaviour
{
    private GameManager GM;

    [System.Serializable]
    public class TextureField : InspectorField<string, Texture> { }

    [SerializeField]
    public TextureField[] texture_list;

	public Font rendering_font;

    /*public AudioClip MusicSound,ClickSound;
	public Texture background,blackbg;
	public Font rendering_font;*/

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GM = GameManager.getInstance();
        /*
		GM.MusicSound = this.MusicSound;
		GM.ClickSound = this.ClickSound;
		GM.background = this.background;
		GM.blackbg = this.blackbg;
		

		GM.m_MusicSource = transform.FindChild ("Music").GetComponent<AudioSource> ();
		GM.m_SoundSource = transform.FindChild ("Sound").GetComponent<AudioSource> ();
		

		GM.m_Settings.Load (GM.m_MusicSource, GM.m_SoundSource);

		GM.m_MusicSource.clip = GM.MusicSound;
		GM.m_MusicSource.Play ();
		*/

        ThreadsafeDictionary<string, Texture> new_textures_dictionary = new ThreadsafeDictionary<string, Texture>();
        foreach (TextureField texture_data in texture_list)
        {
            new_textures_dictionary.Add(texture_data.key, texture_data.value);
        }
		GM.setActiveScene(SceneType.MENU);
		GM.setActiveMenu(MenuType.MENU_MAIN);
		GM.setTextures(new_textures_dictionary);
		GM.setRenderingFont(rendering_font);
    }

    void OnGUI()
    {
        if (GM != null) GM.call_onGUI();
    }

    private bool isShowGUIExtraCoroutineRunning = false;
    void Update()
    {
        if (Input.GetButtonDown("Info") && !isShowGUIExtraCoroutineRunning)
        {
            StartCoroutine(
                GM.showGUIExtra(this, (bool coroutine_status) =>
                    {
                        isShowGUIExtraCoroutineRunning = coroutine_status;
                    }
                )
            );
        }
    }

}
