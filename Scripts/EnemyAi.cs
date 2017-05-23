using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAi : MonoBehaviour
{
  
    public float moveSpeed;                     //movespeed variable
    private float range;                        //how close the player is to the enemy var
    public Transform targetTransform;           //Transform variable to attach player gameobject too
   
    public float directionChangeInterval = 1;   //how often the direction for wandering changes.
    public float maxHeadingChange = 30;         //max distance heading can be
    public float aggroRange;                    //set distance player needs to be to activate chase

    Rigidbody2D controller;
    float heading;
    Vector3 targetRotation;

    void Awake()
    {
        controller = GetComponent<Rigidbody2D>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, 0, heading);         //set heading on the z axis so it doesnt spin out of control.

        StartCoroutine(NewHeading());                               //calls coroutine method..

        aggroRange = 3f;
        targetTransform = GameObject.FindWithTag("Player").transform;   //assigns Transform variable to player tag.
    }

    void Start()
    {
        aggroRange = 5f;
       
    }



    void Update()
    {
        range = Vector2.Distance(this.transform.position, targetTransform.transform.position);  //sets range to players current position.

        if (range <= aggroRange)                        //if range is within or equal to aggro range, run seek method, otherwise enemies wander..
        {            
            Seek();
        }
        else
        {
            Wander();
        }
    }

    void Seek()                                                                     //moves enemy prefab towards players position..sets chasing bool to true.
    {        
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetTransform.transform.position, Time.deltaTime * moveSpeed);
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingProcedure();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void NewHeadingProcedure()                                                        //creates new heading 
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, 0, heading);
    }

    void Wander()                                                                  // takes current facing direction and moves towards (fix)
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        controller.AddForce(forward * moveSpeed);
    }
}