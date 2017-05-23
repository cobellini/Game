using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    Text ammo;  //create ammo object to make ui
    string devider = " / "; //tring to make ui look nice
    public GameObject bullet;   //bullet gameobject
    public float speed = 0.1f;  //speed bullet travels.
    public int gunDamage, magSize, magKeeper;   //gun attributes that will be determines
    public RandomIntGen gunpicker = new RandomIntGen(0, 75);   //value that will assign gun enum
    public Guntype gun; //enum
    public int lastShot = 0;    //value of last shot taken, used for firerate.
    public float fireRate;  //firerate value
    public bool reloading = false;  //boolean to see if the player is reloading
    public float reloadTimer = 3.0f;    //reload timer
    public float shotTimer; //shooting timer
    public int assignedVal;
  
    public enum Guntype //enum to know what gun is used
    {
        Shotgun, Pistol, Machine_Gun
    }

    void Start()
    {
        
        Gunrandomer();  //create gun attribute values

        magKeeper = magSize;    //set max magazine value to magsize determined by gunrandomer
        ammo = GameObject.Find("Canvas/AmmoCounter").GetComponent<Text>();      //set ui 
        ammo.text = magSize.ToString() + devider + magKeeper; //set ui values
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            if (magSize > 0 && !reloading)   //check if the player is not reloading or bullets isnt 0.
            {
                if (gun == Guntype.Shotgun) //if guntype enum is shotgun, all shotgun shooting method, else regular shooting.
                {
                    Sshoot();
                }
                else
                {
                    shoot();
                }
            }
            } else if (magSize == 0)    //if mag is 0, auto reload.
            {
                StartCoroutine(Reload());
            }
        }
    

    void shoot()    //shooting method for pistol/machinegun
    {
        if (Time.time > fireRate + lastShot)    //if enough time has passed since last shot + defined firerate.
         {
            magSize--;  //reduce magazine size by 1
            Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));   //set target to current mouse position(point to shoot)
            Vector3 myPos = new Vector2(transform.position.x, transform.position.y);    //get current player position.
            Vector3 direction = target - myPos; //set direction to target - player positon
            direction.Normalize(); //set direction from int to float.
            GameObject projectile = (GameObject)Instantiate(bullet, myPos, Quaternion.identity); //create bullet gameobject.
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * speed); //add force to the bullet in the direction set * speed defined.
            lastShot = (int) Time.time; //cast last shot as an int and real time.
            ammo.text = magSize.ToString() + devider + magKeeper; //display new mag size.
          }
    }

    void Sshoot() //shotgun method to shoot. creates a 5 bullet spread instead of single bullet.
    {
        magSize--; //reduce mage size by 1
        if (Time.time > fireRate + lastShot) //if enough time has passed since last shot + defined firerate.
        {
            for (int i = 0; i < 5; i++) //same as above, but create 5 bullets instead of 1
             {       
                Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + Random.Range(-12f, 12f), Input.mousePosition.y + Random.Range(-12f, 12f), Input.mousePosition.z - Camera.main.transform.position.z));
                Vector3 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector3 direction = target - myPos;
                direction.Normalize();
                GameObject projectile = (GameObject)Instantiate(bullet, myPos, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().AddForce(direction * speed);
                lastShot = (int)Time.time;
            }
            ammo.text = magSize.ToString() + devider + magKeeper; //display new magsize.
        }
    }

        IEnumerator Reload() //reload enumerator
    {
        reloading = true; //set reloading bool to true
        yield return new WaitForSeconds(2); //wait 2 seconds(real time)
        magSize =  magKeeper; //set magsize to the max magsize defined and kept in magkeeper variable
        reloading = false;  //set reloading bool to false
    }

    public void Gunrandomer() //method that defines random wep attributes.
    {
        assignedVal = gunpicker.Random; //get random value between 0 and 75

        if (assignedVal < 25) //based on the value assigned from randomintgen, set enum to shotgun, pistol, or machine_gun
        {
            gun = Guntype.Shotgun;

        }
        else if (assignedVal >= 25 && assignedVal <= 50)
        {
            gun = Guntype.Pistol;
        }
        else
        {
            gun = Guntype.Machine_Gun;
        }

        switch (gun)    //switch to set gun attributes based on enum type.
        {
            case Guntype.Shotgun:
                fireRate = 1;
                gunDamage = 55;
                magSize = 2;

                break;

            case Guntype.Pistol:
                fireRate = 0.7f;
                gunDamage = 35;
                magSize = 12;

                break;

            case Guntype.Machine_Gun:
                fireRate = 0.2f;
                gunDamage = 20;
                magSize = 50;

                break;
        }
    }
}

