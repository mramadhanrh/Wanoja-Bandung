using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public float health;
    public float damage;
    public float gauge;

    public Status(StatusAttribute myAttribute)
    {
        health = getHealth(myAttribute);
        damage = getDamage(myAttribute);
        gauge = getGauge(myAttribute);
    }

    float getHealth(StatusAttribute myAttribute)
    {
        return 200f + (myAttribute.stregth * 20f);
    }

    float getDamage(StatusAttribute myAttribute)
    {
        return 10f + (myAttribute.agillity * 4f);
    }

    float getGauge(StatusAttribute myAttribute)
    {
        return 70f + (myAttribute.intellegence * 12f);
    }

}

[System.Serializable]
public class StatusAttribute
{
    public int stregth, agillity, intellegence;
}

