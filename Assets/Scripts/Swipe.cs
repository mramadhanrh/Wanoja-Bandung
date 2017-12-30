using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    //Enum Permission 0,1,2,4,8,... karena binary coba lihat
    //stackoverflow.com/questions/9811114/why-do-enum-permissions-often-have-0-1-2-4-values
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,

    //untuk membuat akses multiple tinggal tambahkan seperti
    // Right + Up = 2 + 4 = 6 yang hasilnya diagonal kanan atas
    LeftDown = 9,
    LeftUp = 5,
    RightDown = 10,
    RightUp = 6
}

public class Swipe : MonoBehaviour {

    private static Swipe instance;
    public static Swipe Instance { get { return instance; } }

    public SwipeDirection Direction { set; get; }

    private Vector3 touchPosition;
    public float swipeThresholdX = 10.0f;
    public float swipeThresholdY = 10.0f;

	// Use this for initialization
	void Start () {
        instance = this;	
	}
	
	// Update is called once per frame
	void Update () {
        Direction = SwipeDirection.None;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;

            if (Mathf.Abs(deltaSwipe.x) > swipeThresholdX)
            {
                //Swipe X Axis
                Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeThresholdY)
            {
                //Swipe Y Axis
                Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
	}

    //Check kalau lagi swipe atau tidak
    public bool IsSwiping(SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }

    //How to Use
    //4 Arah simetris
        //if (Swipe.Instance.IsSwiping(SwipeDirection.Right))
        //    Debug.Log("Swipe Right");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.Left))
        //    Debug.Log("Swipe Left");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.Up))
        //    Debug.Log("Swipe Up");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.Down))
        //    Debug.Log("Swipe Down");

    //4 Arah Diagonal
        //if (Swipe.Instance.IsSwiping(SwipeDirection.RightUp))
        //    Debug.Log("Diagonal Right Up");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.RightDown))
        //    Debug.Log("Diagonal Right Down");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.LeftUp))
        //    Debug.Log("Diagonal Left Up");
        //else if (Swipe.Instance.IsSwiping(SwipeDirection.LeftDown))
        //    Debug.Log("Diagonal Left Down");
}
