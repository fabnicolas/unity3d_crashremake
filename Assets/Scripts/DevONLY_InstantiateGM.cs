using UnityEngine;
using System.Collections;

public class DevONLY_InstantiateGM : MonoBehaviour {
	void Awake()
	{
		/*
		 * This gameobject is exactly the __app inside the preloader scene;
		 * so if the loaded scene in Unity3D is exactly the preloader, check will be != null, so we don't have to preload it.
		 * Else if the loaded scene is NOT the preloader, it does NOT have the __app object, so
		 * check=null and so we will preload... the preloader.
		 * This script is marked as DevONLY; to activate it properly, set script executioner order in a way that this is the
		 * first script to be executed. REMOVE IT ON RELEASE!
		*/
		GameObject check = GameObject.Find("__app"); 
		if(check==null){
			UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
		}
	}
}
