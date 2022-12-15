using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : List<GameObject>
{
    public GameObject poolPrefab;

    public GameObject GetFromPool()
    {
        GameObject newGO = GetPooledSquare();

        if (newGO == null)
        {
            newGO = GameObject.Instantiate(poolPrefab);
            this.Add(newGO);
        } else
        {
            newGO.SetActive(true);
        }

        return newGO;
    }

    GameObject GetPooledSquare()
    {
        //Loop squares
        for (int i = 0; i < this.Count; i++)
        {
            //See if square is inactive in scene
            if (!this[i].activeInHierarchy)
            {
                return this[i];
            }
        }

        //No inactive squares found
        return null;
    }
}
