using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScene : MonoBehaviour {

    Animator animator;
    int tapCount;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Story"))
            {
                LoadScenes();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            tapCount++;
            if (tapCount >= 7)
            {
                LoadScenes();
            }
        }
	}

    void LoadScenes()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
