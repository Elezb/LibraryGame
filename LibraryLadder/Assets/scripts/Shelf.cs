using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Vector3 ladderOnShelfOffset;
    public Vector3 ladderOnShelfRotation;
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("du bist in meiner triggerzone");
        if(collider.TryGetComponent(out PlayerMovement player) && !player.possibleShelfs.Contains(this))
        {
            player.possibleShelfs.Add(this);
        }
        else
        {
            Debug.Log("aber hast kein playermovement script");
        }
    }

    void OnTriggerExit(Collider collider) 
    {
        Debug.Log("du verlässt in meiner triggerzone");
        if (collider.TryGetComponent(out PlayerMovement player) && player.possibleShelfs.Contains(this))
        {
            player.possibleShelfs.Remove(this);
        }
        else
        {
            Debug.Log("aber hast kein playermovement script");
        }


    }

}
