using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {
    public float delay = 3; //delay before loading a new level
    public string NewLevelString= "Main"; //what level to load
    Transform Player;
  
	void Start () {       
        StartCoroutine(LoadLevelAfterDelay(delay));      
    }

    IEnumerator LoadLevelAfterDelay(float delay) //enumerator to load ne wlevels
    {
        yield return new WaitForSeconds(delay); //wait for defined delay
        Application.LoadLevel(NewLevelString);  //load new level
    }
}
