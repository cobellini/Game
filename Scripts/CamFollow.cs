//Script will find the playable character and set camera on it, then each update will keep finding and placing it on it -- basically will make the camera follow the players movements 

using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{

    GameObject player;  //create a gameobject
    
    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player");    //assign player gameobject to the gameobject with tag "player"
        cameraPlayerFollow();       //call follow method.
    }

    void Update()
    {        
        cameraPlayerFollow();       //call follow method each game tick
    }

    void cameraPlayerFollow()   //method to follow player
    {
        Vector3 newposition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);  //get players current position and assign it to new position vector
        transform.position = newposition;   // assign position to the returned values of newposition.
    }
}
 