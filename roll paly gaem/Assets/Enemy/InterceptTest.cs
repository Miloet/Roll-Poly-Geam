using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptTest : MonoBehaviour
{
    Transform player;
    Rigidbody2D playerSpeed;

    Rigidbody2D projectileRB;
    public GameObject prefab;
    public float speed = 15;
    public Vector2 LastInput;

    public float spread = 0.2f;

    public GameObject point1;
    public GameObject point2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerSpeed = player.GetComponent<Rigidbody2D>();

        StartCoroutine(Fire());
    }


    Vector2 intercept;
    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        float relativeSpeed = speed - playerSpeed.velocity.magnitude;

        float time = distance / relativeSpeed;

        intercept = (Vector2)player.position + playerSpeed.velocity * time;
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            for (int i = 0; i < 3; i++)
            {
                ShootBullet();
                yield return new WaitForSeconds(.1f);
            }


            yield return new WaitForSeconds(.5f);

            for (int i = 0; i < 6; i++)
                ShootBullet(1, 1f);
            yield return new WaitForSeconds(2f);
        }

        
    }


    private Vector2 BulletSpread(float custom = -1f)
    {
        if(custom != -1f) return new Vector2((Random.value * 2 - 0.5f) * custom, (Random.value * 2 - 0.5f) * custom);
        return new Vector2((Random.value * 2 - 0.5f) * spread, (Random.value*2 - 0.5f) * spread);
    }

    private void ShootBullet(float CustomSpread = -1f, float change = 0.5f)
    {
        Vector2 spread = BulletSpread(CustomSpread);

        GameObject projectile = Instantiate(prefab);
        projectileRB = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (Vector2.Lerp(intercept, player.position, change) + spread - (Vector2)transform.position).normalized;

        projectileRB.velocity = direction * speed;
        projectile.transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        Destroy(projectile, 1);
    }
}
