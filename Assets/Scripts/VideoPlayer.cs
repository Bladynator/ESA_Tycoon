using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour 
{
#if UNITY_ANDROID || UNITY_IPHONE
    
#else
     public MovieTexture introvideo;
#endif


    void Start()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        PlayVideo("Assets/intro video.ogv");


#else
        GetComponent<RawImage>().texture = introvideo;
     AudioSource aud = GetComponent<AudioSource>();
        aud.clip = introvideo.audioClip;
        aud.Play();
        introvideo.Play();
#endif

    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IPHONE

#else
     if (!introvideo.isPlaying)
        {
            SceneManager.LoadScene("_Main");
        }
#endif

    }

    void PlayVideo(string videoPath)
    {
        StartCoroutine(PlayVideoCoroutine(videoPath));
    }

    IEnumerator PlayVideoCoroutine(string videoPath)
    {
        Handheld.PlayFullScreenMovie(videoPath, Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("_Main");
    }
}
