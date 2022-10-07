using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public List<GameObject> maps;
    public List<GameObject> placers;
    public List<float> roundOrder;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject clipBoard;
    public GameObject clipBoard2;
    public GameObject title;
    public float roundEndOffset;
    public GameObject text1;
    public GameObject text2;
    private bool waitingForClick = false;
    private bool waitingForMapSelect = true;
    private bool waitingForGameStart = false;
    public List<GameObject> players;
    public List<Vector3> playerSpawnPoints;
    public GameObject enemySpawnController;
    public bool roundOver = false;
    private bool fadeTitle = false;
    public Button left;
    public Button right;
    public Button select;
    public float mapSelector = 0;
    public int currentMap = 0;
    public bool boarding = false;
  

    [SerializeField]
    private float score1 = 0;
    [SerializeField]
    private float score2 = 0;
    [SerializeField]
    private float score3 = 0;
    [SerializeField]
    private float score4 = 0;
    [SerializeField]
    private float score5 = 0;
    [SerializeField]
    private float score6 = 0;

    public struct ScoreListData
    {
        public float score;
        public string name;

        public ScoreListData(float score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        select.gameObject.SetActive(false);
        //NewRound();
        text1.GetComponent<TextMeshPro>().text = "FIRST TO 30 POINTS";
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingForClick == true && Input.GetMouseButtonDown(0))
        {
            if (!roundOver)
            {
                waitingForClick = false;
                text2.GetComponent<TextMeshPro>().enabled = true;
                NewRound();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload the current scene
            }
        }
        if (waitingForMapSelect == true && Input.GetMouseButtonDown(0))
        {
            fadeTitle = true;
            foreach(GameObject map in maps)
            {
                if(maps.IndexOf(map) == 0)
                {
                    map.transform.position = new Vector3(0f, 0f, 0f);
                }
                else
                {
                    map.transform.position = new Vector3(44f, 0f, 0f);
                }
            }
            waitingForMapSelect = false;
            leftDoor.GetComponent<door>().MoveOut();
            rightDoor.GetComponent<door>().MoveOut();
            StartCoroutine(WaitForDoorsOpen());
        }
        if (waitingForGameStart == true && Input.GetMouseButtonDown(0))
        {
            maps[currentMap].gameObject.GetComponentInChildren<Rigidbody2D>().transform.localScale = new Vector3(1f, 1f, 1f);
            waitingForGameStart = false;
            NewRound();
        }
        if(fadeTitle == true)
        {
            title.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, title.GetComponent<SpriteRenderer>().color.a - 0.05f);
            if(title.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                fadeTitle = false;
            }
        }
    }

    IEnumerator WaitForDoorsOpen()
    {
        yield return new WaitForSeconds(1f);
        right.gameObject.SetActive(true);
        select.gameObject.SetActive(true);
    }

    public void LeftMap()
    {
        maps[currentMap].GetComponent<MapMovement>().Move(44f);
        maps[currentMap - 1].GetComponent<MapMovement>().Move(0f);
        if(currentMap == 1)
        {
            left.gameObject.SetActive(false);
        }
        right.gameObject.SetActive(true);
        currentMap -= 1;
    }

    public void RightMap()
    {
        maps[currentMap].GetComponent<MapMovement>().Move(-44f);
        maps[currentMap + 1].GetComponent<MapMovement>().Move(0f);
        if (currentMap == maps.Count - 2)
        {
            right.gameObject.SetActive(false);
        }
        left.gameObject.SetActive(true);
        currentMap += 1;
    }

    public void SelectMap()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        select.gameObject.SetActive(false);
        leftDoor.GetComponent<door>().MoveIn();
        rightDoor.GetComponent<door>().MoveIn();
        StartCoroutine(WaitForDoorsStart());
    }

    IEnumerator WaitForDoorsStart()
    {
        yield return new WaitForSeconds(1f);
        clipBoard2.GetComponent<clipBoard>().MoveIn();
        yield return new WaitForSeconds(1f);
        maps[currentMap].gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        maps[currentMap].gameObject.GetComponentInChildren<Rigidbody2D>().transform.localScale = new Vector3(1.1f, 1f, 1f);       
        waitingForGameStart = true;
    }

    public void PlayerDied(float playerNumb)
    {
        /*
        foreach (var x in roundOrder)
        {
            Debug.Log(x.ToString() + " in list");
        }
        */
        if (!roundOrder.Contains(playerNumb))
        {
            roundOrder.Add(playerNumb);
            //Debug.Log(playerNumb);
            if (roundOrder.Count == 6)
            {
                Debug.Log("assigning scores");
                AssignScores();
                    
            }
            if (roundOrder.Count == 5)
            {
                Debug.Log("final player");
                FinalPlayer();
            }
        }

    }

    public void AssignScores()
    {
        boarding = true;
        lock (roundOrder)
        {
            foreach (float i in roundOrder)
            {
                float index = roundOrder.IndexOf(i); //Conveniently, this is also the score we need to add
                switch (i)
                {
                    case 1:
                        score1 += index;
                        break;
                    case 2:
                        score2 += index;
                        break;
                    case 3:
                        score3 += index;
                        break;
                    case 4:
                        score4 += index;
                        break;
                    case 5:
                        score5 += index;
                        break;
                    case 6:
                        score6 += index;
                        break;
                }
            }

            placers[0].GetComponent<Placer>().WriteScore(score1);
            placers[1].GetComponent<Placer>().WriteScore(score2);
            placers[2].GetComponent<Placer>().WriteScore(score3);
            placers[3].GetComponent<Placer>().WriteScore(score4);
            placers[4].GetComponent<Placer>().WriteScore(score5);
            placers[5].GetComponent<Placer>().WriteScore(score6);

            StartCoroutine(WaitForAnimation());
        }

    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(roundEndOffset);
        Animation();
    }

    void Animation()
    {
        leftDoor.GetComponent<door>().MoveIn();
        rightDoor.GetComponent<door>().MoveIn();
        StartCoroutine(WaitForDoorsClose());
    }

    IEnumerator WaitForDoorsClose()
    {
        yield return new WaitForSeconds(1f);
        clipBoard.GetComponent<clipBoard>().MoveIn();
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        StartCoroutine(BoardUp());
    }


    public void FinalPlayer()
    {
        var Spawner = GameObject.Find("EnemySpawnController(Clone)");
        Destroy(Spawner);
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            switch (obj.name)
            {
                case "Brick(Clone)":
                    obj.GetComponent<BrickBehaviour>().Die();
                    break;
                case "Bouncer(Clone)":
                    obj.GetComponent<Bouncer>().Die();
                    break;
                case "BouncerGun(Clone)":
                    obj.GetComponent<bounceGun>().Die();
                    break;
                case "Bullet(Clone)":
                    obj.GetComponent<BulletMovement>().Die();
                    break;
                case "Incind(Clone)":
                    obj.GetComponent<IncindBehaviour>().Die();
                    break;
                case "Incind_spikes(Clone)":
                    obj.GetComponent<SpikeBehaviour>().Die();
                    break;
                case "Missile(Clone)":
                    obj.GetComponent<missile>().Die();
                    break;
                case "Saw(Clone)":
                    obj.GetComponent<sawBlade>().Die();
                    break;
            }
        }
        var blinks = GameObject.FindGameObjectsWithTag("Warning");
        foreach (GameObject obj in blinks)
        {
            Destroy(obj);
        }
        var powerup = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject obj in powerup)
        {
            Destroy(obj);
        }
        
        StartCoroutine(RandomWait());
        

    }

    IEnumerator RandomWait()
    {
        yield return new WaitForSeconds(0.5f);
        var player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);
        player.GetComponent<AudioSource>().enabled = false;
        PlayerDied(player.GetComponent<PlayerMovement>().playerNumb);
        Debug.Log(player.GetComponent<PlayerMovement>().playerNumb.ToString() + " playerNumb");
    }

    IEnumerator BoardUp()
    {
        yield return new WaitForSeconds(0.5f);
        List<ScoreListData> scoresListData = new List<ScoreListData>();
        scoresListData.Add(new ScoreListData(score1, placers[0].name));
        scoresListData.Add(new ScoreListData(score2, placers[1].name));
        scoresListData.Add(new ScoreListData(score3, placers[2].name));
        scoresListData.Add(new ScoreListData(score4, placers[3].name));
        scoresListData.Add(new ScoreListData(score5, placers[4].name));
        scoresListData.Add(new ScoreListData(score6, placers[5].name));
        scoresListData.Sort(SortNumb);
        float i = 0;
        foreach(ScoreListData placerScore in scoresListData)
        {
            GameObject.Find(placerScore.name).GetComponent<Placer>().MovePlacer(i);
            i++;
        }
        if(scoresListData[0].score >= 30)
        {                      
            if(scoresListData[1].score >= scoresListData[0].score - 1)
            {
                text1.GetComponent<TextMeshPro>().text = "GET A LEAD OF 2 POINTS";
            }
            else
            {
                roundOver = true;
                switch (scoresListData[0].name)
                {
                    case "Placer":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: BLUE IS SUPERIOR";
                        break;
                    case "Placer 2":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: PINK IS SUPERIOR";
                        break;
                    case "Placer 3":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: GREEN IS SUPERIOR";
                        break;
                    case "Placer 4":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: YELLOW IS SUPERIOR";
                        break;
                    case "Placer 5":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: ORANGE IS SUPERIOR";
                        break;
                    case "Placer 6":
                        text1.GetComponent<TextMeshPro>().text = "CONCLUSION: RED IS SUPERIOR";
                        break;
                }
            }
        }
        StartCoroutine(WaitForPlacers());
    }

    IEnumerator WaitForPlacers()
    {
        yield return new WaitForSeconds(1f);
        text2.GetComponent<TextMeshPro>().enabled = true;
        waitingForClick = true;
    }

    private int SortNumb(ScoreListData a, ScoreListData b)
    {
        if(a.score > b.score) { return -1; }
        else if(a.score < b.score) { return 1; }
        return 0;
    }

    public void NewRound()
    {
        boarding = false;
        clipBoard.GetComponent<clipBoard>().MoveOut();
        clipBoard2.GetComponent<clipBoard>().MoveOut();
        leftDoor.GetComponent<door>().MoveOut();
        rightDoor.GetComponent<door>().MoveOut();
        roundOrder = new List<float>();
        int i = 0;
        foreach(GameObject player in players)
        {
            Instantiate(players[i], playerSpawnPoints[i], Quaternion.identity);
            i++;
        }
        Instantiate(enemySpawnController, Vector3.zero, Quaternion.identity);
        
    }

}
