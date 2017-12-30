using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ControlName
{
    Attack,
    Skill1,
    Skill2,
    Skill3,
    LeftMove,
    RightMove
}

public class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public ControlName myControl;
    bool isAsuh, isAsih;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (isAsuh)
        {
            Character.Instance.myBehaviour.StartSkillAsuh();
        }

        if (isAsih)
        {
            Character.Instance.myBehaviour.StartSkillAsih();
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Character.Instance == null)
            return;

        switch (myControl)
        {
            case ControlName.Attack:
                Control.Attack();
                break;
            case ControlName.Skill1:
                SetAsuh(true);
                break;
            case ControlName.Skill2:
                SetAsih(true);
                break;
            case ControlName.Skill3:
                SetAsah();
                break;
            case ControlName.LeftMove:
                Control.direction = -1;           
                break;
            case ControlName.RightMove:
                Control.direction = 1;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (myControl)
        {
            case ControlName.LeftMove:
                Control.direction = 0;
                break;
            case ControlName.RightMove:
                Control.direction = 0;
                break;
            case ControlName.Skill1:
                SetAsuh(false);
                Character.Instance.myBehaviour.StopSkillAsuh();
                break;
            case ControlName.Skill2:
                SetAsih(false);
                Character.Instance.myBehaviour.StopSkillAsih();
                break;
        }
    }

    void SetAsah()
    {
        Character.Instance.myBehaviour.StartSkillAsah();
    }

    void SetAsih(bool _val)
    {
        isAsih = _val;
    }

    void SetAsuh(bool _val)
    {
        isAsuh = _val;
    }
}

public class Control
{
    public static int direction { get; set; }

    public static void Attack()
    {
        Character.Instance.myBehaviour.Attack();
    }

}
