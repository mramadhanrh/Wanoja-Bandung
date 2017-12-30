using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public static CameraFollow Instance;

    public SetCamera myCamera;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;

    Vector3 originalPos;

    bool isShaking;

    public GameObject arrow;

    public bool showArrow;

    void Awake()
    {
        Instance = this;
        myCamera = new SetCamera(gameObject);

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

	// Use this for initialization
	void Start () {
        arrow.SetActive(false);
        originalPos = new Vector3(myCamera.targetPosition.x, myCamera.targetPosition.y, -10) - new Vector3(myCamera.moveDistance + 0.2f, myCamera.targetPosition.y, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (showArrow)
            arrow.SetActive(true);
        else
            arrow.SetActive(false);

        myCamera.StartInterpolates();

        if (Input.GetKeyDown(KeyCode.O))
            myCamera.AllowToMove();

        if(isShaking)
        {
            ShakeScreen();
        }
	}

    void StopShaking()
    {
        isShaking = false;
        originalPos = new Vector3(myCamera.targetPosition.x, myCamera.targetPosition.y, -10) - new Vector3(myCamera.moveDistance + 0.2f, myCamera.targetPosition.y, 0);
        originalPos.z = -10;
        camTransform.localPosition = originalPos;
    }

    void ShakeScreen()
    {
        originalPos = new Vector3(myCamera.targetPosition.x, myCamera.targetPosition.y, -10) - new Vector3(myCamera.moveDistance + 0.2f, myCamera.targetPosition.y, 0);
        originalPos.z = -10;
        Vector2 _vector2 = (Vector2)originalPos + Random.insideUnitCircle * shakeAmount;
        Vector3 _vector3 = new Vector3(_vector2.x, _vector2.y, -10);
        _vector3.z = -10;
        camTransform.localPosition = _vector3;
    }

    public void StartShaking()
    {
        isShaking = true;
        Invoke("StopShaking", shakeDuration);
    }
}

[System.Serializable]
public class SetCamera
{
    GameObject myCamera;

    public Vector3 targetPosition;

    [Range(0, 10)]
    public float camSpeed = 3f;

    [Range(0, 20)]
    public float moveDistance = 16f;

    public bool isAbleToMove;

    int multiplier = 1;

    public SetCamera(GameObject _myCamera)
    {
        isAbleToMove = false;
        myCamera = _myCamera;
        SetTarget();
    }

    void LerpCamera()
    {
        myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, targetPosition, camSpeed * Time.deltaTime);
    }

    void SetTarget()
    {
        targetPosition = new Vector3(moveDistance * multiplier + 0.2f,
                                     myCamera.transform.position.y,
                                     myCamera.transform.position.z);        
        multiplier++;
    }

    bool isNearTarget()
    {
        if (myCamera.transform.position.x >= (targetPosition.x - 0.2f))
        {
            return true;
        }
        return false;
    }

    public void StartInterpolates()
    {
        if (isAbleToMove)
        {
            if (!isNearTarget())
                LerpCamera();
            else
            {
                SetTarget();
                isAbleToMove = false;
            }
        }
    }

    public void AllowToMove()
    {
        Debug.Log("called");
        isAbleToMove = true;
    }
}
