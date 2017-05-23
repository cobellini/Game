using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    
    float speed = 5.0f; //movement speed of player

	void Update () {
        movement(); //call movement method each game tick
	}

    void movement() //method that controls player movement
    {
        if (Input.GetKey(KeyCode.W)) //if player presses w, the will move up
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);            
        }
        if (Input.GetKey(KeyCode.A)) //if player presses a they will move left
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);            
        }
        if (Input.GetKey(KeyCode.S)) //if player presses s they will move down
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);           
        }
        if (Input.GetKey(KeyCode.D)) //if player presses d they will mve right
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);           
        }      
    }
}
