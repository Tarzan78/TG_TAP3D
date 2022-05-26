using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;

    //Gun Specs 
    private float bulletSpeed = 25f;

    public void Shoot_BTN()
	{
        Shoot();
	}

    //Shoot Mechanic
    private void Shoot()
	{
        //spawn
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        //set specs
        bullet.GetComponent<Bullet>().SetBulletSpecs(bullet.transform.forward, bulletSpeed);

        GameManager.bullets.Add(bullet);
	}
}
