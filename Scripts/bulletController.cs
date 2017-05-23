using UnityEngine;
using System.Collections;

public class bulletController : MonoBehaviour {

    GunScript gunscript;    //creates gunscript object to get components from other class
    int dmg;    //bullet damage, taken from gunscript.
 
    void Start()
    {
        gunscript = GameObject.FindGameObjectWithTag("BulletSpawn").GetComponent<GunScript>(); //grab information from gunscript.
        dmg = gunscript.gunDamage; //assign dmg to gunDamge set in gunscript.
    }

    void OnCollisionEnter2D(Collision2D collide) //when spawned bullet hits enemy character, destroy bullet object and remove health from enemy character.
    {
        if (collide.gameObject.tag == "Enemy" || collide.gameObject.name == "Enemy(Clone)")
        {
            collide.gameObject.GetComponent<EnemyHealth>().DamageEnemy(dmg); //calls damage enemy method in enemyhealth class
            Destroy(gameObject);
        }

        if (collide.gameObject.tag == "Wall") //if bullet hits wall gameobject, destroy bullet too.
        {
            Destroy(gameObject);
        }
    }
}
