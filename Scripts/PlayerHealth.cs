/*script to create player health controller. assigns player health and checks to see if the player collides with enemy units. if health is 0, 
reload the level(with new generation), if player collides with enemy, reduce health by 25. also deals with displaying the players health
*/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Text healthDisp;    //create text object to make ui
    int maxPlayerHealth;    //players max health
    int playerCurrentHealth;    //players current health
    int DamageDealer;   //damage dealt by enemies
    string Divider = " / ";     //make the text display neater.

    void Start()
    {
        DamageDealer = 25;  //assign values to dmg
        maxPlayerHealth = 100;  //assign value to max health
        playerCurrentHealth = maxPlayerHealth;  //sets current player health to max health
        healthDisp = GameObject.Find("Canvas/Health").GetComponent<Text>(); //finds the gameobject that displays health on the canvas(ui)
        healthDisp.text = playerCurrentHealth.ToString() + Divider + maxPlayerHealth;   //displays health in ui as string.
    }

    void OnCollisionEnter2D(Collision2D collide)    //standard collision method
    {
        if (collide.gameObject.tag == "Enemy" || collide.gameObject.name == "Enemy(Clone)" && playerCurrentHealth < 0)  //check to see if the player collides with enemies, lose health  based on damagedealer(25)
        {
            playerCurrentHealth -= DamageDealer;    //remove 25 from 100 each time collision
            healthDisp.text = playerCurrentHealth.ToString() + Divider + maxPlayerHealth;   //display new health.
        }

        if (playerCurrentHealth <= 0)   //if player health goes to 0, reload level.
        {
            SceneManager.LoadScene(0);
        }
        

        

    }
}