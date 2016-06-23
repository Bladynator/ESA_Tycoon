using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToTycoon : MonoBehaviour 
{
    float level;
    [SerializeField]
    Image barToFill;
    [SerializeField]
    GameObject objectToMove;
    [SerializeField]
    GameObject[] positions;

    public void Start()
    {
        level = PlayerPrefs.GetInt("Level");
        barToFill.fillAmount = (float)(level / 10);
        objectToMove.transform.position = positions[(int)level - 1].transform.position;
    }
}
