using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public CharacterController controller;
   public Transform cam;
   public float speed;

   public float turnSmoothTime = 0.1f;
   float turnSmoothVelocity;

    public List<Shelf> possibleShelfs = new List<Shelf>();
   Shelf closestShelf;


   void Update()
   {
       Move();
   }

   void Move()
   {
        float vertical= Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3( horizontal,0,vertical).normalized;

        if(direction.magnitude >0.1f)
        {
            float targetangle=Mathf.Atan2(direction.x, direction.z )*Mathf.Rad2Deg +cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetangle, ref turnSmoothVelocity,turnSmoothTime);
            transform.rotation = Quaternion.Euler(transform.rotation.x,angle,transform.rotation.z);

            Vector3 moveDir = Quaternion.Euler(0f,targetangle,0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed *Time.deltaTime);
        }


   }

   void LadderToShelf()
   {
       if(Input.GetButton("Ladder"))
       {
          Shelf targetShelf = ClosestShelf(); 
          if(targetShelf!=null)
          {
                Debug.Log("i should put my ladder here");
          }
          else
          {
              Debug.Log("no Shelf in reach");
          }
       }

   }

   Shelf ClosestShelf()
   {
       Shelf closestShelf = null;
       int currentClosestDistance=1000;
       for(int i = 0;i<possibleShelfs.Count;i++)
       {
           if(Vector3.Distance(transform.position,possibleShelfs[i].transform.position) < currentClosestDistance)
           {
               closestShelf=possibleShelfs[i];
           }

       }
       return closestShelf;
   }
}
