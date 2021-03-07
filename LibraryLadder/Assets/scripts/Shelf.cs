using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("du bist in meiner triggerzone");
        if(collider.TryGetComponent(out PlayerMovement player))
        {
            player.possibleShelfs.Add(this);
        }
        else
        {
            Debug.Log("aber hast kein playermovement script");
        }
    } 

}
