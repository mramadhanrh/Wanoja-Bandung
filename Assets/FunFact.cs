using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FunFact : MonoBehaviour {

    TextMeshProUGUI myText;

    [Header("Fact Collection")]
    public string[] factString;

	// Use this for initialization
	void Start () {
        myText = GetComponent<TextMeshProUGUI>();
        myText.SetText(factString[Random.Range(0, factString.Length)]);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}