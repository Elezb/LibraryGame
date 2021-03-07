using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform ladder;
    public Transform playerModel;
    public float speed;
    
   

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public List<Shelf> possibleShelfs = new List<Shelf>();
    Transform currentShelf;

    bool OnLadder;


   void Update()
   {
        if (!OnLadder)
        {
            Move();
            LadderToShelf();
        }
        else 
        {
            MoveOnLadder();
        }
   }

    void Move()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);

            Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


    }

    void MoveOnLadder() 
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 playerDirection = new Vector3(0, vertical,0).normalized;
        Vector3 ladderDirection = new Vector3(horizontal, 0, 0).normalized;

        if (playerDirection.magnitude > 0.1f)
        {
            transform.position +=  playerDirection* speed *Time.deltaTime;
        }

        if (ladderDirection.magnitude > 0.1f) 
        {
            ladder.GetComponent<Rigidbody>().AddForce(currentShelf.localToWorldMatrix*ladderDirection*speed*400* Time.deltaTime);
        }
    }

   void LadderToShelf()
   {
       if(Input.GetButton("Fire1"))
       {
          Shelf targetShelf = ClosestShelf();
            currentShelf = targetShelf.transform;
            if (targetShelf != null && !OnLadder)
            {
                OnLadder = true;

                Vector3 direction = new Vector3(-Mathf.Pow(transform.position.x, 2) / targetShelf.transform.right.x, 0, transform.position.z);
                //Schnittpunkt berechnung

                ladder.parent = null;
                
                transform.position = new Vector3(targetShelf.transform.position.x + targetShelf.ladderOnShelfOffset.x, targetShelf.transform.position.y + targetShelf.ladderOnShelfOffset.y, targetShelf.transform.position.z + targetShelf.ladderOnShelfOffset.z);
                MoveLadderTo(targetShelf.transform.position + targetShelf.ladderOnShelfOffset, Quaternion.Euler(targetShelf.ladderOnShelfRotation));
                transform.parent = ladder;
                Debug.Log("i should put my ladder here");
            }
            else if (!OnLadder)
            {
                Debug.Log("no Shelf in reach");
            }
            else 
            {
                Debug.Log("I'm already on the Ladder");
            }
       }

   }

    void MoveLadderTo(Vector3 targetPosition, Quaternion targetRotation) 
    {
        ladder.position = targetPosition;
        ladder.rotation = targetRotation;
    }


   Shelf ClosestShelf()
   {
       Shelf closestShelf = null;
       float currentClosestDistance=1000;
       for(int i = 0;i<possibleShelfs.Count;i++)
       {
           if(Vector3.Distance(transform.position,possibleShelfs[i].transform.position) < currentClosestDistance)
           {
               closestShelf=possibleShelfs[i];
                currentClosestDistance = Vector3.Distance(transform.position, possibleShelfs[i].transform.position);
           }

       }
       return closestShelf;
   }
}
