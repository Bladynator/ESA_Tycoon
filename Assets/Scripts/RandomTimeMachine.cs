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

    public override void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            int number = Random.Range(34, 38);
            dialogs.ActivateTalking(number);
        }
    }
}
