using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;
    private float speed = 10;
    public bool dead = false;
    public bool dying = false;
    private bool testEnemy = false;
    private Gun.GunType killedWith;
    [SerializeField] private GameObject matBody;
    private Material mat;
    [SerializeField] private Material fireDeathPrefab;
    [SerializeField] private Material frostDeathPrefab;
    private Material fireDeath;
    private Material frostDeath;

    //Deaths
    private float disolveFire = 0;
    private float disolveFrost = 1;


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
		if (!dead && !dying)
		{
            Movement(transform.forward, speed);
		}
		else if (dead)
		{
            GameManager.DestroyDeadEnemies();
        }
        else if (dying)
		{
            GetComponent<Animator>().enabled = false;
            Dying();
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

            killedWith = collision.gameObject.GetComponent<Bullet>().bulletType;

            dying = true;
		}

        if (collision.gameObject.tag == "Squad_Unit")
        {
            collision.gameObject.GetComponent<SquadUnity>().dead = true;
            dead = true;
        }
    }

    private void Dying()
	{
		switch (killedWith)
		{
			case Gun.GunType.Frost:

                //new mat
                mat = new Material(frostDeathPrefab);

                matBody.GetComponent<SkinnedMeshRenderer>().material = mat;

                disolveFrost += 2 * (Time.deltaTime * -1);

                matBody.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_ErvaRange", disolveFrost);

                if (disolveFrost <= -1)
                {
                    dead = true;    
                }

                break;
			case Gun.GunType.Fire:
                //new mat
                mat = new Material(fireDeathPrefab);

                matBody.GetComponent<SkinnedMeshRenderer>().material = mat;

                disolveFire += 1 * Time.deltaTime;

                matBody.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_disolve", disolveFire);

                if (disolveFire >= 1 )
				{             
                    dead = true;
				}

				break;
			default:
				break;
		}
	}
}
