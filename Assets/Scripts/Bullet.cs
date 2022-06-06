using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0;
    public Vector3 direction = new Vector3();
    public bool inMovement = false;
    public bool dead = false;
    public Gun.GunType bulletType;

	// Update is called once per frame
	void Update()
    {
		if (inMovement && !dead)
		{
            Movement(direction, speed);

            dead = KillCondiction();
        }

		if (dead)
		{
            GameManager.DestroyDeadBullets();
		}
    }

    private void Movement(Vector3 direction, float speed)
	{
        this.transform.position += direction * Time.deltaTime * speed;
	}

    public void SetBulletSpecs(Vector3 direction, float speed)
	{
        this.direction = direction;
        this.speed = speed;
        inMovement = true;
	}

    private bool KillCondiction()
	{
		if (Vector3.Distance(new Vector3(0,0,0), this.transform.position) > 25)
		{
            return true;
		}
		else
		{
            return false;
		}
	}
}
