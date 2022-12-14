﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlCaTrAzzGames.Utilities;

public class AestheticGenerator : Singleton<AestheticGenerator>
{
    public GameObject aestheticPrefab;
    public Vector2 yRange;
    public float xDistanceInFront;

    public Vector2 sizeRange;
    public Vector2 rotationRange;

    public Vector2 creationRangeBounds;
    float currentCreationRange;
    float travelledDistance;
    float lastLocation;

    List<GameObject> aestheticSquares = new List<GameObject>();
    float aestheticBehindToRegenDistance = 15f;

    void Start()
    {
        currentCreationRange = Random.Range(creationRangeBounds.x, creationRangeBounds.y);
    }

    void Update()
    {
        if (activePlayer == null)
        {
            return;
        }

        float thisDistance = activePlayer.transform.position.x - lastLocation;
        travelledDistance += thisDistance;

        if (travelledDistance > currentCreationRange)
        {
            GenerateAesthetic();
            travelledDistance -= currentCreationRange;
        }

        lastLocation = activePlayer.transform.position.x;

        CleanAestheticSquares();
    }


    Player activePlayer;

    void GenerateAesthetic()
    {
        //Try get pooled square
        GameObject newAesthetic = GetPooledSquare();

        //No pooled squares avaliable. generate new
        if (newAesthetic == null)
        {
            newAesthetic = GameObject.Instantiate(aestheticPrefab);
            aestheticSquares.Add(newAesthetic);
            newAesthetic.transform.SetParent(transform);
        }

        currentCreationRange = Random.Range(creationRangeBounds.x, creationRangeBounds.y);
        newAesthetic.transform.position = new Vector3(lastLocation + xDistanceInFront, Random.Range(yRange.x, yRange.y), 0f);

        float size = Random.Range(sizeRange.x, sizeRange.y);
        newAesthetic.transform.localScale = new Vector3(size, size, size);

        float rot = Random.Range(rotationRange.x, rotationRange.y);
        newAesthetic.transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }

    GameObject GetPooledSquare()
    {
        //Loop squares
        for (int i = 0; i < aestheticSquares.Count; i++)
        {
            //See if square is inactive in scene
            if (!aestheticSquares[i].activeInHierarchy)
            {
                return aestheticSquares[i];
            }
        }

        //No inactive squares found
        return null;
    }

    void CleanAestheticSquares()
    {
        for (int i = 0; i < aestheticSquares.Count; i++)
        {
            if (aestheticSquares[i].transform.position.x < (activePlayer.transform.position.x - aestheticBehindToRegenDistance))
            {
                /*  Destroy(aestheticSquares[i]);
                  aestheticSquares.RemoveAt(i);*/

                aestheticSquares[i].SetActive(false);
            }
        }

    }

    public IEnumerator CleanAllAestheticSquaresAfterDelay(float delay = 0.35f)
    {
        yield return new WaitForSeconds(delay);

        CleanAllAestheticSquares();
    }

    void CleanAllAestheticSquares()
    {
        for (int i = 0; i < aestheticSquares.Count; i++)
        {
            /*Destroy(aestheticSquares[i]);
            aestheticSquares.RemoveAt(i);*/
            aestheticSquares[i].SetActive(false);
        }
    }

    public void SetActivePlayer(Player p)
    {
        activePlayer = p;
        lastLocation = activePlayer.transform.position.x;
    }
}
