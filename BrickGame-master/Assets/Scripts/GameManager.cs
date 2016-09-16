using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int Lives = 3;
    public static int Score = 0;
    public static int BlocksAlive;
    public static GameState CurrentGameState = GameState.Start;

    public static BallScript Ball;
    private GameObject[] blocks;

    public GameObject youWonOrLostText;

    public enum GameState
    {
        Start,
        Playing,
        Won,
        LostALife,
        LostAllLives
    }
    Text statusText;

    public static AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        blocks = GameObject.FindGameObjectsWithTag("Block");
        Ball = GameObject.Find("Ball").GetComponent<BallScript>();
        statusText = GameObject.Find("Status").GetComponent<Text>();
        BlocksAlive = blocks.Length;
    }


    private bool InputTaken()
    {
        return Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space);
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
                    Ball.StartBall();
                }
                break;
            case GameState.Playing:
                break;
            case GameState.Won:
                Time.timeScale = 0;
                youWonOrLostText.SetActive(true);
                youWonOrLostText.GetComponent<Text>().text = "You won!! \nPress Space to play again!!";
                if (InputTaken())
                {
                    Time.timeScale = 1;
                    youWonOrLostText.SetActive(false);
                    Restart();
                    Ball.StartBall();
                    BlocksAlive = blocks.Length;
                    CurrentGameState = GameState.Playing;
                }
                
                break;
            case GameState.LostALife:
                if (InputTaken())
                {
                    youWonOrLostText.SetActive(false);
                    Ball.StartBall();
                    CurrentGameState = GameState.Playing;
                }
                break;
            case GameState.LostAllLives:
                youWonOrLostText.SetActive(true);
                youWonOrLostText.GetComponent<Text>().text = "You Lost!! \nPress Space to play again!!";
                if (InputTaken())
                {
                    youWonOrLostText.SetActive(false);
                    Restart();
                    Ball.StartBall();
                    BlocksAlive = blocks.Length;
                    CurrentGameState = GameState.Playing;
                }
                break;
            default:
                break;
        }
        if (BlocksAlive <= 0)
        {
            CurrentGameState = GameState.Won;
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

  
    public void DecreaseLives()
    {
        if (Lives > 0)
            Lives--;

        if(Lives == 0)
        {
            CurrentGameState = GameState.LostAllLives;
        }
        else
        {
            youWonOrLostText.SetActive(true);
            youWonOrLostText.GetComponent<Text>().text = "Lost a life. Press Space to continue";
            CurrentGameState = GameState.LostALife;
        }
        Ball.StopBall();
    }

   
}
