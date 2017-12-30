using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpSceneSelect : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Splash"))
            {
                LoadScenes();
            }
        }
	}

    void LoadScenes()
    {
        if (PlayerPrefs.HasKey("firstTime"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("Story");
            PlayerPrefs.SetInt("firstTime", 1);
            PlayerPrefs.Save();
        }
    }
}
