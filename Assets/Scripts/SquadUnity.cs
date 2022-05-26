using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadUnity : MonoBehaviour
{
    [SerializeField] private GameObject gunPos_Objt;
    [SerializeField] private GameObject gun_Objt;

    private Transform gunPos;
    private Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        gunPos = gunPos_Objt.transform;
        gun = gun_Objt.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot()
	{
        gun.Shoot_BTN();
	}
}
