﻿using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width, height, speed, spawnDelay, waves;
    bool isMovingRight = true;
    float xmin, xmax;
    int enemies;
    public static bool spawnBoss = false;


    void Start()
    {
        float zDistance = transform.position.z - Camera.main.transform.position.z;
        xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).x;
        xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zDistance)).x;

        enemies = 0;
    }

    void SpawnEnemies()
    {
        enemies += 1;
        Transform position = NextFreePosition();
        if (position)
        {
            GameObject enemy = Instantiate(enemyPrefab, position.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = position;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnEnemies", spawnDelay);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if ((transform.position.x - width * 0.5f) < xmin)
        {
            isMovingRight = true;
        }
        else if ((transform.position.x + width * 0.5f) > xmax)
        {
            isMovingRight = false;
        }

        if (AllEnemiesAreDead() && (spawnBoss))
        {
            if (enemies < waves * transform.childCount)
            {
                SpawnEnemies();
            }
        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform Position in transform)
        {
            if (Position.childCount == 0)
            {
                return Position;
            }
        }
        return null;
    }

    bool AllEnemiesAreDead()
    {
        foreach (Transform Position in transform)
        {
            if (Position.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
}
