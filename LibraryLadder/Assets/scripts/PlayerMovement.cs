using System.Collections;
using System.Collections.Generic;
using System;
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

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
        Vector3 playerDirection = ((Vector3.up+transform.forward*.21f)*vertical).normalized;
        distanceTravelled += horizontal *1.2f* Time.deltaTime;
       

        if (playerDirection.magnitude > 0.1f)
        {
           
            
            transform.position +=  playerDirection* speed *Time.deltaTime;
            
        }

      
           // ladder.GetComponent<Rigidbody>().AddForce(currentShelf.localToWorldMatrix*ladderDirection*speed*400* Time.deltaTime);
            ladder.transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        MoveLadderTo(currentPath.path.GetClosestPointOnPath(transform.position), rotationToShelf());
        //ladder.transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

    }

   void LadderToShelf()
   {
       if(Input.GetButton("Fire1") && possibleShelfs.Count!=0)
       {
          Shelf targetShelf = ClosestShelf();
            currentShelf = targetShelf.transform;
            if (targetShelf != null && !OnLadder)
            {
                OnLadder = true;

                ladder.parent = null;
                currentPath = targetShelf.pathCreator;
               
                transform.rotation = Quaternion.LookRotation((currentShelf.position - ladder.position).normalized, currentPath.path.GetNormalAtDistance(distanceTravelled));
                transform.position = currentPath.path.GetClosestPointOnPath(transform.position) - transform.forward*150;
                distanceTravelled = currentPath.path.GetClosestDistanceAlongPath(transform.position);
                MoveLadderTo(currentPath.path.GetClosestPointOnPath(transform.position), rotationToShelf()) ;
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

    Quaternion rotationToShelf() 
    {
        Quaternion targetRotation = Quaternion.identity;

        
        targetRotation = Quaternion.LookRotation((currentShelf.position- ladder.position ).normalized, currentPath.path.GetNormalAtDistance(distanceTravelled));


        return targetRotation;
    }


    void MoveLadderTo(Vector3 targetPosition, Quaternion targetRotation) 
    {
        ladder.position = targetPosition;
        ladder.rotation = targetRotation;
    }


   Shelf ClosestShelf()
   {
       Shelf closestShelf = null;
       float currentClosestDistance=Mathf.Infinity;
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
