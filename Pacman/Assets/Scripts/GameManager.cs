using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }


    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject pinky;
    public GameObject inky;
    public GameObject win;
    public GameObject gameOver;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject countDown;
    public AudioClip startClip;

    public Text remainText;
    public Text eatenText;
    public Text scoreText;

    int remain;
    int eaten;
    int score;
    bool gameEnd = false;

    private List<GameObject> Pacdots = new List<GameObject>();


    private void Awake()
    {
        _instance = this;
        foreach (Transform t in GameObject.Find("Maze").transform) 
        {
            Pacdots.Add(t.gameObject);
        }
        remain = Pacdots.Count;
        eaten = 0;
        score = 0;
    }

    private void Start()
    {
        SetGameState(false);
        
    }

    private void FixedUpdate()
    {
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + remain;
            eatenText.text = "Eaten:\n\n" + eaten;
            scoreText.text = "Score:\n\n" + score;
        }
        if (!pacman.activeInHierarchy && !gameEnd)
        {
            StartCoroutine(GameOver());
        }
        if(remain == 0 && !gameEnd)
        {
            StartCoroutine(GameWin());
        }
    }

    private void onStart()
    {
        AudioSource.PlayClipAtPoint(startClip, new Vector3(26,16,-5));
        startPanel.SetActive(false);
        StartCoroutine(CountingAndStart());
       

    }

    private void onEist()
    {
        Application.Quit();
    }

    IEnumerator CountingAndStart()
    {
        GameObject go = Instantiate(countDown);
        go.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        Destroy(go);
        SetGameState(true);
        Invoke("GenerateSuperDot", 8f);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    private void GenerateSuperDot()
    {
        if(Pacdots.Count > 5)
        {
            int temp = Random.Range(0, Pacdots.Count);
            Pacdots[temp].transform.localScale = new Vector3(3, 3, 3);
            Pacdots[temp].GetComponent<Pacdot>().isSuperdot = true;
        }
    }

    public void Eatdot(GameObject dot)
    {
        eaten++;
        remain -= 1;
        Pacdots.Remove(dot);
        score += 100;
    }

    public void EatSuperDot()
    {
        score += 200;
        FreezeEnemy();
        Invoke("UnfreezeEnemy", 3f);
        Invoke("GenerateSuperDot", 8f);

    }


    private void FreezeEnemy()
    {
        pacman.GetComponent<Pacman>().isSuperPacman = true;
        blinky.GetComponent<Enemy>().enabled = false;
        clyde.GetComponent<Enemy>().enabled = false;
        pinky.GetComponent<Enemy>().enabled = false;
        inky.GetComponent<Enemy>().enabled = false;
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void UnfreezeEnemy()
    {
        pacman.GetComponent<Pacman>().isSuperPacman = false;
        blinky.GetComponent<Enemy>().enabled = true;
        clyde.GetComponent<Enemy>().enabled = true;
        pinky.GetComponent<Enemy>().enabled = true;
        inky.GetComponent<Enemy>().enabled = true;
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    private void SetGameState(bool state)
    {
        pacman.GetComponent<CircleCollider2D>().enabled = state;
        pacman.GetComponent<Pacman>().enabled = state;
        blinky.GetComponent<Enemy>().enabled = state;
        clyde.GetComponent<Enemy>().enabled = state;
        pinky.GetComponent<Enemy>().enabled = state;
        inky.GetComponent<Enemy>().enabled = state;
    }

    IEnumerator GameOver()
    {
        gameEnd = true;
        Instantiate(gameOver);
        SetGameState(false);
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(3f);
        gameEnd = false;
        Restart();
    }

    IEnumerator GameWin()
    {
        gameEnd = true;
        Instantiate(win);
        SetGameState(false);
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(3f);
        gameEnd = false;
        Restart();
    }

    private void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void EatEmeny()
    {
        score += 500;
    }


}
