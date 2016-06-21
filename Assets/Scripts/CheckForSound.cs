using UnityEngine;
using System.Collections;
using System;

public class CheckForSound : MonoBehaviour 
{
    public string musicType;

	void Start () 
	{
        GetComponent<AudioSource>().mute = Convert.ToBoolean(PlayerPrefs.GetInt(musicType));
	}
}
