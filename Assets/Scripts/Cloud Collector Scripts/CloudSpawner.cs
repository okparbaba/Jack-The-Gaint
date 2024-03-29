﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] clouds;

    private float distanceBetweenClouds = 3f;

    private float minX, maxX;

    private float lastCloudPositionY;

    private float controlX;
    [SerializeField]
    private GameObject[] collectables;

    private GameObject player;

    void Awake()
    {
        controlX = 0;
        
        SetMinAndMaxX();
        CreateClouds();
        player = GameObject.Find("Player");
        for(int i = 0; i < collectables.Length; i++)
        {
            collectables[i].SetActive(false);
        }



    }
    // Start is called before the first frame update
    void Start()
    {
        PositionThePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetMinAndMaxX()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        maxX = bounds.x - 0.5f;
        minX = -bounds.x + 0.5f;
    }
    void Shuffle(GameObject[] arrayToShuffle)
    {
        for(int i = 0; i < arrayToShuffle.Length; i++)
        {
            GameObject temp = arrayToShuffle[i];
            int ramdom = Random.Range(i, arrayToShuffle.Length);
            arrayToShuffle[i] = arrayToShuffle[ramdom];
            arrayToShuffle[ramdom] = temp;

            
        }
    }
    void CreateClouds()
    {
        Shuffle(clouds);
        float positionY = 0f;
        for(int i=0; i < clouds.Length; i++)
        {
            Vector3 temp = clouds[i].transform.position;
            temp.y = positionY;
            temp.x = Random.Range(minX, maxX);
            if (controlX == 0)
            {
                temp.x = Random.Range(0.0f, maxX);
                controlX = 1;
            }else if(controlX==1){
                temp.x = Random.Range(0.0f, minX);
                controlX = 2;
            }
            else if (controlX == 2)
            {
                temp.x = Random.Range(1.0f, maxX);
                controlX = 3;
            }
            else if (controlX == 3)
            {
                temp.x = Random.Range(-1.0f, minX);
                controlX = 0;
            }
            lastCloudPositionY = positionY;
            clouds[i].transform.position = temp;
            positionY -= distanceBetweenClouds;
        }
    }
    void PositionThePlayer()
    {
        GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
        GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

        for(int i = 0; i< darkClouds.Length; i++)
        {
            if(darkClouds[i].transform.position.y == 0f)
            {
                Vector3 t = darkClouds[i].transform.position;
                darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x,
                    cloudsInGame[0].transform.position.y, cloudsInGame[0].transform.position.z);

                cloudsInGame[0].transform.position = t;

                
            }
        }
        Vector3 temp = cloudsInGame[0].transform.position;
        for (int i = 1; i < cloudsInGame.Length; i++)
        {
            if(temp.y < cloudsInGame[i].transform.position.y)
            {
                temp = cloudsInGame[i].transform.position;
            }
        }
        temp.y += 0.8f;
        player.transform.position = temp;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Cloud" || collision.tag == "Deadly")
        {
            if(collision.transform.position.y == lastCloudPositionY)
            {
                Shuffle(clouds);
                Shuffle(collectables);

                Vector3 temp = collision.transform.position;

                for(int i = 0; i < clouds.Length; i++)
                {
                    if (!clouds[i].activeInHierarchy)
                    {
                        if (controlX == 0)
                        {
                            temp.x = Random.Range(0.0f, maxX);
                            controlX = 1;
                        }
                        else if (controlX == 1)
                        {
                            temp.x = Random.Range(0.0f, minX);
                            controlX = 2;
                        }
                        else if (controlX == 2)
                        {
                            temp.x = Random.Range(1.0f, maxX);
                            controlX = 3;
                        }
                        else if (controlX == 3)
                        {
                            temp.x = Random.Range(-1.0f, minX);
                            controlX = 0;
                        }
                        temp.y -= distanceBetweenClouds;
                        lastCloudPositionY = temp.y;
                        clouds[i].transform.position = temp;
                        clouds[i].SetActive(true);
                        int ramdom = Random.Range(0, collectables.Length);
                        if(clouds[i].tag != "Deadly")
                        {
                            if (!collectables[ramdom].activeInHierarchy)
                            {
                                Vector3 temp2 = clouds[i].transform.position;
                                temp2.y += 0.7f;
                                if (collectables[ramdom].tag == "Life")
                                {
                                    if(PlayerScore.lifeCount < 2)
                                    {
                                        collectables[ramdom].transform.position = temp2;
                                        collectables[ramdom].SetActive(true);
                                    }
                                }
                                else
                                {
                                    collectables[ramdom].transform.position = temp2;
                                    collectables[ramdom].SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
