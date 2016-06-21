using UnityEngine;
using System.Collections;
using System;

public class CheckForSound : MonoBehaviour 
{
	void Start () 
	{
        GetComponent<AudioSource>().mute = Convert.ToBoolean(PlayerPrefs.GetInt("Music"));
	}
}
