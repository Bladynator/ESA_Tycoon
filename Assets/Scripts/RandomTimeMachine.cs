using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RandomTimeMachine : BuildingMain 
{
    Dialogs dialogs;

    public override void Start()
    {
        dialogs = GameObject.Find("Quests").GetComponent<Dialogs>();
        //base.Start();
    }

    public override void OnMouseUp() // 27 - 32
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            int number = Random.Range(27, 33);
            dialogs.ActivateTalking(number);
        }
    }
}
