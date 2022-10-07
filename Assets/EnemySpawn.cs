using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawn : MonoBehaviour
{
    public GameObject saw; //Reference the saw gameObject
    public GameObject bullet;
    public GameObject incind;
    public GameObject bounceGun;
    public GameObject brick;
    public GameObject missile;
    public GameObject powerupBox;
    public float initSpawnTime; //The initial time it takes for the next saw to spawn
    private float curSpawnTime; //The current time it takes for the next saw to spawn
    public float SpawnTimeInc; //How much the time between saws decreases by. The time decreases so more and more saws spawn as the game goes on
    private bool SPset = false; //bool to see if the spawn point for the next  saw has been set
    private bool hittingWall; //bool to see if the spawnpoint is overlapping a wall
    private bool nearWall;
    private float positionX; //X and Y positions for the spawn point
    private float positionY;
    private float positionX2; //X and Y positions for the spawn point
    private float positionY2;
    private float positionX3; //X and Y positions for the spawn point
    private float positionY3;
    private float positionPUX;
    private float positionPUY;
    private Vector3 pos;
    public LayerMask wall; //Referencing the layer of the wall
    public GameObject warning; //Reference the alert gameobject
    private float type;
    public float randomX;
    public float randomY;
    public float powerupTime;
    private bool powerupSPset = false;
    //public bool DoingCoroutine = false;
    // Start is called before the first frame update
    void Start()
    {
        warning.GetComponent<SpriteRenderer>().enabled = false; //Get the sprite renderer of the alert gameobject
        curSpawnTime = initSpawnTime; //current spawn time starts as the initial spawn time
        StartCoroutine(WaitForSiren());
        
        
    }

    IEnumerator WaitForSiren()
    {
        yield return new WaitForSeconds(5 - initSpawnTime);
        StartCoroutine(enemySpawn()); //Start spawning enemies
        StartCoroutine(powerupSpawn()); //Start spawning powerups
    }

    // Update is called once per frame
    void Update()
    {        
        

    }

    IEnumerator enemySpawn() //enemyspawn function
    {
        yield return new WaitForSeconds(curSpawnTime * SpawnTimeInc); //Wait for the time until next spawn
        curSpawnTime = curSpawnTime * SpawnTimeInc; //Reduce the wait time for the next spawn
        
        type = Random.Range(1, 7); //1: saw, 2: bullet, 3: incindiary, 4: bouncer, 5: brick, 6: missile
        //type = 5;
        if (type == 2)
        {
            float Sps = 0;
            while (Sps < 3) //If spawn point has not been set yet, pick random X and Y values for the spawn point within a certain range
            {                     //If this spawn point intersects a wall, this code will wait for the next Update() function to rerun the randomization
                                  //So eventually a proper spawnpoint will be created
                float allocatedX = 0;
                float allocatedY = 0;
                switch (Sps)
                {
                    case 0:
                        positionX = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                        positionY = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;

                        allocatedX = positionX;
                        allocatedY = positionY;
                        break;
                    case 1:
                        positionX2 = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                        positionY2 = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;

                        allocatedX = positionX2;
                        allocatedY = positionY2;
                        break;
                    case 2:
                        positionX3 = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                        positionY3 = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;

                        allocatedX = positionX3;
                        allocatedY = positionY3;
                        break;
                }

                hittingWall = Physics2D.OverlapCircle(new Vector2(allocatedX, allocatedY), 0.4f, wall);

                nearWall = false;
                if (nearWall == false)
                {
                    nearWall = Physics2D.OverlapCircle(new Vector2(allocatedX - 0.5f, allocatedY), 0.4f, wall);
                }
                if (nearWall == false)
                {
                    nearWall = Physics2D.OverlapCircle(new Vector2(allocatedX, allocatedY - 0.5f), 0.4f, wall);
                }
                if (nearWall == false)
                {
                    nearWall = Physics2D.OverlapCircle(new Vector2(allocatedX + 0.5f, allocatedY), 0.4f, wall);
                }
                if (nearWall == false)
                {
                    nearWall = Physics2D.OverlapCircle(new Vector2(allocatedX, allocatedY + 0.5f), 0.4f, wall);
                }

                if (hittingWall == false && nearWall == true)
                {
                    Sps += 1;
                    //Debug.Log(allocatedX);
                }
            }
            StartCoroutine(flashWarning(positionX, positionY, type)); //Spawn the alert gameobject
            StartCoroutine(flashWarning(positionX2, positionY2, type));
            StartCoroutine(flashWarning(positionX3, positionY3, type));
            /*
            Debug.Log(positionX);
            Debug.Log(positionY);
            Debug.Log(positionX2);
            Debug.Log(positionY2);
            Debug.Log(positionX3);
            Debug.Log(positionY3);
            */
            SPset = true;
        }
        else if (type == 6)
        {
            float Sps = 0;
            while (Sps < 2) //If spawn point has not been set yet, pick random X and Y values for the spawn point within a certain range
            {                     //If this spawn point intersects a wall, this code will wait for the next Update() function to rerun the randomization
                                  //So eventually a proper spawnpoint will be created
                float allocatedX = 0;
                float allocatedY = 0;
                switch (Sps)
                {
                    case 0:
                        positionX = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                        positionY = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;

                        allocatedX = positionX;
                        allocatedY = positionY;
                        break;
                    case 1:
                        positionX2 = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                        positionY2 = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;

                        allocatedX = positionX2;
                        allocatedY = positionY2;
                        break;
                }
                hittingWall = Physics2D.OverlapCircle(new Vector2(allocatedX, allocatedY), 0.9f, wall);
                if (hittingWall == false)
                {
                    Sps += 1;
                }
            }
            StartCoroutine(flashWarning(positionX, positionY, type)); //Spawn the alert gameobject
            StartCoroutine(flashWarning(positionX2, positionY2, type));
        }
        else
        {
            while (SPset == false)
            {
                positionX = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
                positionY = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;
                hittingWall = Physics2D.OverlapCircle(new Vector2(positionX, positionY), 1f, wall);
                if (hittingWall == false)
                {
                    StartCoroutine(flashWarning(positionX, positionY, type));
                    SPset = true;
                }

            }
        }
        StartCoroutine(enemySpawn()); //Restart this function
    }

    public void StartPowerup()
    {
        StartCoroutine(powerupSpawn());
    }

    IEnumerator powerupSpawn()
    {
        //DoingCoroutine = true;
        yield return new WaitForSeconds(powerupTime);
        while (powerupSPset == false)
        {
            positionPUX = Mathf.Floor(Random.Range(-randomX + 0.5f, randomX + 0.5f + 1f)) - 0.5f;
            positionPUY = Mathf.Floor(Random.Range(-randomY + 0.5f, randomY + 0.5f + 1f)) - 0.5f;
            hittingWall = Physics2D.OverlapCircle(new Vector2(positionPUX, positionPUY), 1f, wall);
            if (hittingWall == false)
            {
                Instantiate(powerupBox, new Vector2(positionPUX, positionPUY), Quaternion.identity);
                powerupSPset = true;
            }

        }
        powerupSPset = false;
        //DoingCoroutine = false;
    }

    IEnumerator flashWarning(float X, float Y, float Type)
    {
        // All this is for blinking the sprite of the alert gameobject
        Instantiate(warning, new Vector3(X, Y), Quaternion.identity);
        yield return new WaitForSeconds(warning.GetComponent<warningFlash>().blinkTime);
        if (gameObject != null)
        {
            switch (Type)
            {
                case 1:
                    Instantiate(saw, new Vector3(X, Y), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bullet, new Vector3(X, Y), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(incind, new Vector3(X, Y), Quaternion.identity);
                    break;
                case 4:
                    Instantiate(bounceGun, new Vector3(X, Y), Quaternion.identity);
                    break;
                case 5:
                    Instantiate(brick, new Vector3(X, Y), Quaternion.identity);
                    break;
                case 6:
                    Instantiate(missile, new Vector3(X, Y), Quaternion.identity);
                    break;

            }
        }
         //Spawn the saw
        SPset = false; //reset the spawnpoint bool to false

    }
}
