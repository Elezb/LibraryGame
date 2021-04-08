using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform ladder;
    public Transform playerModel;
    public float speed;
    public PathCreator currentPath;
    float distanceTravelled;
    public EndOfPathInstruction endOfPathInstruction;

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
        Vector3 playerDirection = ((Vector3.up)*vertical).normalized;
        distanceTravelled += horizontal * Time.deltaTime*.01f;
       

        if (playerDirection.magnitude > 0.1f)
        {
           
            
            transform.position +=  playerDirection* speed *Time.deltaTime;
        }

      
           // ladder.GetComponent<Rigidbody>().AddForce(currentShelf.localToWorldMatrix*ladderDirection*speed*400* Time.deltaTime);
            ladder.transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //ladder.transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        
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

                ladder.parent = null;
                currentPath = targetShelf.pathCreator;
                transform.position =currentPath.path.GetClosestPointOnPath(transform.position);
                distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
                MoveLadderTo(currentPath.path.GetClosestPointOnPath(transform.position), currentPath.path.GetRotationAtDistance(distanceTravelled)) ;
                //transform.rotation = Quaternion.LookRotation(ladder.position + transform.up * 1.94f, transform.up);
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
