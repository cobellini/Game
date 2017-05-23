using UnityEngine;
using System.Collections;

public class PlayerRotateToCursor : MonoBehaviour {

    Vector3 mouseposition; //create new vector holding mouseposition
    Camera cam; //create camera holder
    Rigidbody2D rig;    //rigidbody holder

	void Start () {
        rig = GetComponent<Rigidbody2D>(); //assign holder to rigidbody on playable character
        cam = Camera.main;  //assign camera hold to main camera
	}
	
	
	void Update () {
        rotatetoCamera(); //call method each game tick
	}

    void rotatetoCamera()
    {
        mouseposition = cam.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z-cam.transform.position.z)); //set mouse position to world space position of mouse
        rig.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mouseposition.y - transform.position.y), (mouseposition.x - transform.position.x)) * Mathf.Rad2Deg - 90); //rotate the character object based on the position of the mouse.
    }

}
