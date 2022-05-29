using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;
    private float speed = 10;
    public bool dead = false;
    private bool testEnemy = true;

    // Start is called before the first frame update
    void Start()
    {

        if (testEnemy) Debug.Log("Squad count -> " + GameManager.squad.Count);

		if (GameManager.squad.Count != 0)
		{

            target =  GetTarget();
            target.transform.position = new Vector3(target.transform.position.x, 1, target.transform.position.z);

            //roatate for target
            transform.LookAt(target.transform);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
		if (!dead)
		{
            Movement(transform.forward, speed);
		}
		else
		{
            GameManager.DestroyDeadEnemies();
        }
    }

    private GameObject GetTarget()
	{
        float distance = 9999;
        GameObject target = null;

		foreach (var unit in GameManager.squad)
		{
			if (distance > Vector3.Distance(unit.transform.position, this.transform.position))
			{
                target = unit;

            }
		}

        return target;
	}

    private void Movement(Vector3 direction, float speed)
    {
        this.transform.position += direction * Time.deltaTime * speed;
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (testEnemy) Debug.Log("Collision Tag-> " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Bullet")
		{
            collision.gameObject.GetComponent<Bullet>().dead = true;
            dead = true;
		}

        if (collision.gameObject.tag == "Squad_Unit")
        {
            collision.gameObject.GetComponent<SquadUnity>().dead = true;
            dead = true;
        }
    }
}
