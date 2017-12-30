using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AIState
{
    Patrol,
    Chase
}

public enum PatrolState
{
    Normal,
    Bouncy
}

public enum AttackState
{
    Chase_Then_Attack,
    Charge_Immidiate,
    Cast_Spell
}

[CreateAssetMenu(fileName = "AI Behaviour", menuName = "Enemy/Behaviour", order = 1)]
public class AIEnemyObject : ScriptableObject
{
    [Header("State Setter")]
    public float distanceThreshold;

    [Header("Patrol Behaviour")]
    public PatrolState patrolState;
    public float speed;
    public float duration;

    [Header("Attack Behaviour")]
    public AttackState attackState;
    
    [HideInInspector]
    public GameObject spell_Object;

    [HideInInspector]
    public float forcePower;

}

#if UNITY_EDITOR
[CustomEditor(typeof(AIEnemyObject))]
public class AIEnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myScript = (AIEnemyObject)target;

        if (myScript.attackState == AttackState.Cast_Spell)
        {
            myScript.spell_Object = (GameObject)EditorGUILayout.ObjectField("Spell Object", 
                                                                            myScript.spell_Object, 
                                                                            typeof(GameObject), 
                                                                            true);
        }

        if (myScript.patrolState == PatrolState.Bouncy)
        {
            EditorGUILayout.Space();
                
            EditorGUILayout.LabelField("Force Power ", EditorStyles.boldLabel);
            myScript.forcePower = EditorGUILayout.FloatField("Force Power", myScript.forcePower);
        }

        if (myScript.attackState == AttackState.Chase_Then_Attack)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Chase Then Attack Selected Requirement : ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Chase Then Attack Parameter : " + "isCharging");
            EditorGUILayout.LabelField("Chase Then Attack Animation : " + "Charging");
        }
    }
}
#endif