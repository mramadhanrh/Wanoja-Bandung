using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Entity Info", menuName = "Create Entity/Entity Info", order = 1)]
public class EntityInfo : ScriptableObject
{
    public string myName;
    public StatusAttribute myAttribute;
    public Status myStatus;
    public bool isCharacter;

    public void SetCharacter()
    {
        if (isCharacter)
        {
            myAttribute = new StatusAttribute();
            if(PlayerPrefs.HasKey("agillity"))
                myAttribute = getAttribute();
        }

        myStatus = new Status(myAttribute);
    }

    public void SetAttribute()
    {
        PlayerPrefs.SetInt("agillity", myAttribute.agillity);
        PlayerPrefs.SetInt("stregth", myAttribute.stregth);
        PlayerPrefs.SetInt("intellegence", myAttribute.intellegence);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("agillity") + " " + PlayerPrefs.GetInt("stregth") + " " + PlayerPrefs.GetInt("intellegence"));
    }

    StatusAttribute getAttribute()
    {
        int _agi = PlayerPrefs.GetInt("agillity");
        int _str = PlayerPrefs.GetInt("stregth");
        int _int = PlayerPrefs.GetInt("intellegence");

        StatusAttribute _myAttribute = new StatusAttribute();
        _myAttribute.agillity = _agi;
        _myAttribute.stregth = _str;
        _myAttribute.intellegence = _int;

        return _myAttribute;
    }

    //public void AttributeSetter(string StrAttribute, int value = 1)
    //{
    //    if (StrAttribute == "Agility")
    //    {
    //        myAttribute.agillity += value;
    //    }
    //    else if (StrAttribute == "Strength")
    //    {
    //        myAttribute.stregth += value;
    //    }
    //    else if (StrAttribute == "Intellegence")
    //    {
    //        myAttribute.intellegence += value;
    //    }
    //    SaveAttribute();
    //}

    //void SaveAttribute()
    //{
    //    PlayerPrefs.SetInt("Strength", myAttribute.stregth);
    //    PlayerPrefs.SetInt("Agility", myAttribute.agillity);
    //    PlayerPrefs.SetInt("Intellegence", myAttribute.intellegence);
    //    Debug.Log(myAttribute.stregth + " " + myAttribute.intellegence + " " + myAttribute.agillity);
    //    PlayerPrefs.Save();
    //}

}
