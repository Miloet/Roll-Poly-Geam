using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public GameObject checker;
    public static GameObject s_checker;

    public static GameObject defaultSpawnpoint;

    public float spawnTime;
    static float buffer = 3;
    static int greed = 0;

    static int times;

    public enum Enemy
    {
        Shooter,
        Sprinter,
    }

    public GameObject[] enemies;

    public class EnemyGroup
    {
        public Enemy type;
        public int amount;

        EnemyGroup(Enemy e, int n)
        {
            type = e;
            amount = n;
        }
    }


    static Camera cam;
    static Collider2D wallCollider;
    static Collider2D decoWallCollider;
    private void Start()
    {
        wallCollider = GameObject.Find("Grid/Walls").GetComponent<Collider2D>();
        decoWallCollider = GameObject.Find("Grid/DecoWall").GetComponent<Collider2D>();
        cam = Camera.main;
        defaultSpawnpoint = gameObject;
    }

    private void Update()
    {
        if (spawnTime <= 0)
        {
            Spawn(Enemy.Shooter, 3 + greed);
            spawnTime = 5;
        }
        else spawnTime -= Time.deltaTime;
    }


    public void SpawnGroup(params EnemyGroup[] groups)
    {
        foreach(EnemyGroup group in groups)
        {
            Spawn(group.type, group.amount);
        }
    }
    public void SpawnGroup(Vector2 origin, params EnemyGroup[] groups)
    {
        foreach (EnemyGroup group in groups)
        {
            Spawn(group.type, origin, group.amount);
        }
    }

    public void Spawn(Enemy enemy, int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            var g = Instantiate(enemies[(int)enemy]);
            times = 0;
            g.transform.position = GetValidPosition();
        }
    }
    public void Spawn(Enemy enemy, Vector2 origin, int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            var g = Instantiate(enemies[(int)enemy]);

            Vector2 random = new Vector2(Random.Range(-buffer, buffer), Random.Range(-buffer, buffer))/2f;

            g.transform.position = origin + random;
        }
    }



    public static Vector2 GetValidPosition()
    {
        times = 0;

        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        Vector2 b = new Vector2(buffer,buffer);
        Vector2 camMin = new Vector2(cam.transform.position.x - camWidth / 2, cam.transform.position.y - camHeight / 2) - b;
        Vector2 camMax = new Vector2(cam.transform.position.x + camWidth / 2, cam.transform.position.y + camHeight / 2) + b;
        Vector2 randomPos = new Vector2();
        
        do
        {
            if (RandomBool())
            {
                randomPos.x = Random.Range(camMin.x, camMax.x);
                if (RandomBool()) randomPos.y = camMax.y;
                else randomPos.y = camMin.y;
            }
            else
            {
                randomPos.y = Random.Range(camMin.y, camMax.y);
                if (RandomBool()) randomPos.x = camMax.x;
                else randomPos.x = camMin.x;
            }
            times++;

            if(times > 10)
            {
                print(times);
                Debug.LogWarning("Enemy has no valid spawn position");

                return defaultSpawnpoint.transform.position;
            }

        } while (wallCollider.OverlapPoint(randomPos) || decoWallCollider.OverlapPoint(randomPos));

        // If the position is valid, return it
        return randomPos;
    }


    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 0;
    }

    public static void GreedUp()
    {
        greed++;
    }
}