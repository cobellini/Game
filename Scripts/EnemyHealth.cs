using UnityEngine;
using System.Collections;
using Rand = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{

    public int MaxHealth; //enemy character max health
    public int CurrentHealth;   //enemy character current health
    public RandomIntGen setHealth = new RandomIntGen(100, 200);

    void Start()
    {      
        MaxHealth = setHealth.Random; //set enemy health to random value
        CurrentHealth = MaxHealth;   //Set current health to max health generated above    
    }

    void Update()
    {
        if (CurrentHealth < 1)  //if health is less than 1, destroy object.
        {
            Destroy(gameObject);
        }    
    }

    public void DamageEnemy(int damage) //method to remove health based on bulletdmg (bulletcontroller)
    {
        CurrentHealth -= damage;      
    }
}
