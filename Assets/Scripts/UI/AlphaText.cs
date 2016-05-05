/// <summary>
/// This script use to fade GUI
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlphaText : MonoBehaviour {
	
	public float speedFade;
	private float count;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//Fade in-out press start
		count += speedFade * Time.deltaTime;

        List<Text> textList = new List<Text>(GetComponentsInChildren<Text>());
        foreach (Text text in textList)
        {
            Color color = text.color;
            color.a = Mathf.Sin(count);
            text.color = color;
        }

		//GetComponent<GUITexture>().color = new Color(0.5f,0.5f,0.5f,Mathf.Sin(count)*0.5f);
	
	}
}
