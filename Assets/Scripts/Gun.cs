using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject ammo;
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material frostMaterial;

    public GunType gunType;
    public enum GunType
	{
        Frost, 
        Fire,
	}

	private void Start()
	{
        float tempRandom = Random.Range(0, 10);

		if (tempRandom % 2 == 0)
		{
            gunType = GunType.Fire;
            ammo.GetComponent<MeshRenderer>().material = fireMaterial;
        }
		else
		{
            gunType = GunType.Frost;
            ammo.GetComponent<MeshRenderer>().material = frostMaterial;
        }

        
	}

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

        bullet.GetComponent<Bullet>().bulletType = gunType;

		switch (gunType)
		{
			case GunType.Frost:
                bullet.GetComponent<MeshRenderer>().material = frostMaterial;
                break;
			case GunType.Fire:
                bullet.GetComponent<MeshRenderer>().material = fireMaterial;
                break;
			default:
				break;
		}

        GameManager.bullets.Add(bullet);
	}
}
