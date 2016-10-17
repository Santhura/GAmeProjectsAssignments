using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Lives = 3;
    public static int Score = 0;
    public static int BlocksAlive;
    public static GameState CurrentGameState = GameState.Start;

    public static BallScript Ball;
    private GameObject[] blocks;

    public GameObject youWonOrLostText;
    GameObject[] allParticles;

    public BoxCollider2D victoryBox;

    public enum GameState
    {
        Start,
        Playing,
        Won,
        LostALife,
        LostAllLives
    }
    Text statusText;

    public static AudioSource audio, win;
    private AudioSource lose, backgroundAudio;
    public AudioClip winSound;

    private GameObject[] amountOfBalls;

    void Awake()
    {
        audio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        win = GameObject.Find("Win Sound effect").GetComponent<AudioSource>();
        lose = GameObject.Find("DeadSound effect").GetComponent<AudioSource>();
        backgroundAudio = GameObject.Find("Background music").GetComponent<AudioSource>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        Ball = GameObject.Find("Ball").GetComponent<BallScript>();
        statusText = GameObject.Find("Status").GetComponent<Text>();
        BlocksAlive = blocks.Length;
    }

    // Use this for initialization
    void Start()
    {
        
    }


    private bool InputTaken()
    {
        return Input.touchCount > 0 || Input.GetMouseButtonDown(0);
    }


    // Update is called once per frame
    void Update()
    {
        statusText.text = string.Format("Lives: {0}  Score: {1}", Lives, Score);

        switch (CurrentGameState)
        {
            case GameState.Start:
                if (InputTaken())
                {
                    CurrentGameState = GameState.Playing;
                    ResetBalls();
                }
                break;
            case GameState.Playing:
                break;
            case GameState.Won:
                for (int i = 0; i < amountOfBalls.Length; i++)
                {
                    amountOfBalls[i].GetComponent<BallScript>().StopBall();
                }
                youWonOrLostText.SetActive(true);
                youWonOrLostText.GetComponent<Text>().text = "You won!! \nClick mouse to play again!!";
                if (InputTaken())
                {
                    backgroundAudio.UnPause();
                    RemoveAllParticles();
                    youWonOrLostText.SetActive(false);
                    victoryBox.enabled = false;
                    Restart();
                    ResetBalls();
                    //Ball.StartBall();
                    BlocksAlive = blocks.Length;
                    PaddleScript.hasPowerUp = false;
                    CurrentGameState = GameState.Playing;
                }
                break;
            case GameState.LostALife:
                if (InputTaken())
                {
                    RemoveAllParticles();
                    youWonOrLostText.SetActive(false);
                    ResetBalls();
                    // Ball.StartBall();

                    CurrentGameState = GameState.Playing;
                }
                break;
            case GameState.LostAllLives:
                youWonOrLostText.SetActive(true);
                youWonOrLostText.GetComponent<Text>().text = "You Lost!! \nClick mouse to play again!!";
                if (InputTaken())
                {
                    backgroundAudio.UnPause();
                    RemoveAllParticles();
                    youWonOrLostText.SetActive(false);
                    Restart();
                    ResetBalls();
                    //Ball.StartBall();
                    BlocksAlive = blocks.Length;
                    PaddleScript.hasPowerUp = false;
                    CurrentGameState = GameState.Playing;
                }
                break;
            default:
                break;
        }
        if (BlocksAlive <= 0)
        {
            CurrentGameState = GameState.Won;
            backgroundAudio.Pause();
            victoryBox.enabled = true;
        }

    }

    private void ResetBalls()
    {
        amountOfBalls = GameObject.FindGameObjectsWithTag("Ball");
        
        for (int i = 0; i < amountOfBalls.Length; i++)
        {
            amountOfBalls[i].GetComponent<BallScript>().StartBall();
        }
        if (amountOfBalls.Length > 1)
            Destroy(amountOfBalls[1]);
    }

    private void RemoveAllParticles()
    {
        allParticles = GameObject.FindGameObjectsWithTag("Particle brick");
        for (int i = 0; i < allParticles.Length; i++)
        {
            Destroy(allParticles[i]);
        }
    }

    private void Restart()
    {
        foreach (var item in blocks)
        {
            item.SetActive(true);
            item.GetComponent<BlockScript>().InitializeColor();
        }
        Lives = 3;
        Score = 0;
    }

  
    public void DecreaseLives(int ballNumber)
    {
        amountOfBalls = GameObject.FindGameObjectsWithTag("Ball");
        if (Lives > 0 && amountOfBalls.Length <= 1)
        {
            Lives--;
            StopBall();
        }
        else
        {
            Destroy(amountOfBalls[ballNumber]);
        }

        if(Lives == 0)
        {
            backgroundAudio.Pause();
            CurrentGameState = GameState.LostAllLives;
            StopBall();
            lose.Play();
        }
        else
        {
            youWonOrLostText.SetActive(true);
            StopBall();
            youWonOrLostText.GetComponent<Text>().text = "Lost a life.\nClick mouse to continue";
            CurrentGameState = GameState.LostALife;
        }
        
    }

    private void StopBall()
    {
        for (int i = 0; i < amountOfBalls.Length; i++)
        {
            amountOfBalls[i].GetComponent<BallScript>().StopBall();
        }
    }
}
