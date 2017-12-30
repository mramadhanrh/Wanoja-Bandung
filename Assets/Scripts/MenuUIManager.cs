using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUIManager : MonoBehaviour {

    public TextMeshProUGUI coin, strength, agility, intellegence, point;
    public EntityInfo characterInfo;


	// Use this for initialization
	void Start () {
		GameData.SetAllValue();
	}
	
	// Update is called once per frame
	void Update () {
        SetTextValue();
	}

    public void SetTextValue()
    {
        coin.SetText(GameData.myRecentCoin.ToString());
        point.SetText(GameData.myPoint.ToString());
        strength.SetText(characterInfo.myAttribute.stregth.ToString());
        agility.SetText(characterInfo.myAttribute.agillity.ToString());
        intellegence.SetText(characterInfo.myAttribute.intellegence.ToString());
    }

    public void BuyPoint()
    {
        if(GameData.myRecentCoin >= 10)
        {
            GameData.DecreaseCoin(10);
            GameData.IncreasePoint();
        }
    }

    public void UpgradeAttribute(string myStatus)
    {
        if (GameData.myPoint > 0)
        {
            if (myStatus == "Agility")
            {
                characterInfo.myAttribute.agillity++;
            }
            else if (myStatus == "Strength")
            {
                characterInfo.myAttribute.stregth++;
            }
            else if(myStatus == "Intellegence")
            {
                characterInfo.myAttribute.intellegence++;
            }
            characterInfo.SetAttribute();
            GameData.DecreasePoint();
        }
    }
}
