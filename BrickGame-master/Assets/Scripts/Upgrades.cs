﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour {

    public GameObject UpgradeBallPrefab;
    private bool secondBallSpawned;
    private float totalBlocks;
    private float percentage;

    GameObject[] blocks;

    // Use this for initialization
    void Start () {
        secondBallSpawned = false;
        totalBlocks = GameManager.BlocksAlive;
        percentage = Random.Range(20,100);
        blocks = GameObject.FindGameObjectsWithTag("Block");
        RandomUpgradeBlock();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.BlocksAlive < (totalBlocks / 100) * percentage && !secondBallSpawned)
        {
            GameObject newBall = Instantiate(UpgradeBallPrefab, GameObject.Find("Ball").transform.position, Quaternion.identity) as GameObject;
            newBall.GetComponent<BallScript>().ballNumber = 1;
            secondBallSpawned = true;
        }
	}

    void RandomUpgradeBlock()
    {
        blocks[Random.Range(0, blocks.Length)].GetComponent<BlockScript>().isAPowerUp = true;
    }
}
