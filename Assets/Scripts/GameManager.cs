using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject unit_Prefab;

    private GameObject shooter;

    private List<GameObject> squad = new List<GameObject>();
    private List<GameObject> enemiesActive = new List<GameObject>();
    public static List<GameObject> bullets = new List<GameObject>();

    [SerializeField] private List<Transform> squadTransfroms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        StartSquadUnits(5);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Mouse pos ray cast " + GetMousePosInScene());

		if (squad.Count!= 0)
		{
            SaquadAim();

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
                Shoot(shooter.GetComponent<SquadUnity>());
			}
        }
    }

	#region Start Game
	//max 5
	private void StartSquadUnits(int numberOfElements)
	{
        int NumberOfStartElements = 1;

        NumberOfStartElements = (numberOfElements > squadTransfroms.Count) ? squadTransfroms.Count - 1 :  numberOfElements;

        for (int i = 0; i < NumberOfStartElements; i++)
		{
            squad.Add(GameObject.Instantiate(unit_Prefab, squadTransfroms[i]));
		}
	}

    #endregion

    #region Squad Mechanics
    private Vector3 GetMousePosInScene()
	{
        Vector3 mousePos = new Vector3();

        //Ray cast 
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit raycastHit))
		{
            mousePos = raycastHit.point;
		}

        return mousePos;
	}

    private void SaquadAim()
	{
        Vector3 target = GetMousePosInScene();

        //get height waist from the squad 
        target.y = (squad.Count != 0) ? squad[0].transform.position.y : 1;

        //Debug.Log(target)

        //get the unit that will aim 
        shooter = GetTheClosestUnitFromTarget(target);

        //Shooter
        shooter.transform.LookAt(target);
    }

    private GameObject GetTheClosestUnitFromTarget(Vector3 target)
	{
        float temp = 99999;
        GameObject tempObjt = null;

        if (squad.Count != 0)
		{       
			foreach (var unit in squad)
			{
				if (unit != null)
				{
                    float distance = Vector3.Distance(target, unit.transform.position);

                    if (temp > distance)
                    {
                        temp = distance;
                        tempObjt = unit;
                    }
                }
                
			}
		}
        return tempObjt;
    }

    private void Shoot(SquadUnity saquadUnity)
	{
        saquadUnity.shoot();

    }
	#endregion

	#region bullets mechanics

    public static void DestroyDeadBullets()
	{
        List<GameObject> deadBullets = new List<GameObject>();

        //get the dead bullets
		for (int i = 0; i < bullets.Count; i++)
		{
			if (bullets[i].GetComponent<Bullet>().dead)
			{
                deadBullets.Add(bullets[i]);
            }
		}

		for (int i = 0; i < deadBullets.Count; i++)
		{
            bullets.Remove(deadBullets[i]);

            Destroy(deadBullets[i]);
		}
	}

    #endregion

    #region Enemies Mechanics

    //spawn controller 


    //depois criar um prefab de inimigo e por a escolher o target mais próximo e ir na direção dele basicamente 

    #endregion
}
