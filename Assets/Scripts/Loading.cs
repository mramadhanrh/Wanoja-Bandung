using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

    public static Loading Instance;
    Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Transition"))
            {
                LoadingLoadScene();
                animator.SetTrigger("Out");
            }
        }
	}

    public void LoadingLoad()
    {
        animator.Play("Transition");
    }

    void LoadingLoadScene()
    {
        SceneManager.LoadScene(GameData.sceneTarget);
    }
}
