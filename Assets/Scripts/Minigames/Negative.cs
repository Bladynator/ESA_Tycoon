using UnityEngine;
using System.Collections;
using System;

public class Negative : MonoBehaviour 
{
    [SerializeField]
    Sprite[] allSprites;

	void Start()
    {
        int numberToLoad = Convert.ToInt32(Mathf.Floor(UnityEngine.Random.Range(0, allSprites.Length)));
        GetComponent<SpriteRenderer>().sprite = allSprites[numberToLoad];

    }
	
	void Update () 
	{
        transform.position -= new Vector3(0.1f, 0, 0);
        if(transform.position.x < - 14)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().hp--;
            if(other.GetComponent<Player>().hp <= 0)
            {
                GameObject.Find("Controller").GetComponent<Controller>().Ending();
            }
            Destroy(gameObject);
        }
    }
}
