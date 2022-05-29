using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject unit_Prefab;
    [SerializeField] private GameObject enemy_Prefab;

    private GameObject shooter;

    public static List<GameObject> squad = new List<GameObject>();
    public static List<GameObject> enemiesActive = new List<GameObject>();
    public static List<GameObject> bullets = new List<GameObject>();

    [SerializeField] private List<Transform> squadTransfroms = new List<Transform>();

    private bool readyForNewSpawn = false;
    private bool testEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        StartSquadUnits(5);

        readyForNewSpawn = true;
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

		if (readyForNewSpawn)
		{
            StartCoroutine(SpawnerControl(5f));
            //Vector2 tempPos = RandomPointInAnnulus(new Vector2(0, 0), 25f, 27f);
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

    public static void DestroyDeadUnits()
    {
        List<GameObject> deadUnits = new List<GameObject>();

        //get the dead Units
        for (int i = 0; i < squad.Count; i++)
        {
            if (squad[i].GetComponent<SquadUnity>().dead)
            {
                deadUnits.Add(squad[i]);
            }
        }

        for (int i = 0; i < deadUnits.Count; i++)
        {
            squad.Remove(deadUnits[i]);

            Destroy(deadUnits[i]);
        }
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

    private Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {
        Vector2 randomDirection = (Random.insideUnitCircle + origin).normalized;

        float  randomDistance = Random.Range(minRadius, maxRadius);

        Vector2 point = origin + randomDirection * randomDistance;

        if (testEnemies)
        {
            Debug.Log("Random pos for enemy -> " + point);
            Debug.Log("Random direction for enemy -> " + randomDirection);
            Debug.Log("max distance for enemy -> " + maxRadius);
            Debug.Log("min distance for enemy -> " + minRadius);
        }

        return point;
    }

    //spawn controller 
    private IEnumerator SpawnerControl(float secForSpawn)
    {
        //disable spawn
        readyForNewSpawn = false;

        //get random point in radius 
        Vector2 tempPos = RandomPointInAnnulus(new Vector2(0, 0), 25f, 27f);

		if (testEnemies)
		{
            Debug.Log("New pos for enemy -> " + tempPos);
		}

        //spawn
        enemiesActive.Add(GameObject.Instantiate(enemy_Prefab, new Vector3(tempPos.x, 0, tempPos.y),new Quaternion()));

        yield return new WaitForSeconds(secForSpawn); //will only call this after every 4 seconds

        //trigger 
        readyForNewSpawn = true;
    }

    public static void DestroyDeadEnemies()
    {
        List<GameObject> deadEnemies = new List<GameObject>();

        //get the dead Enemies
        for (int i = 0; i < enemiesActive.Count; i++)
        {
            if (enemiesActive[i].GetComponent<Enemy>().dead)
            {
                deadEnemies.Add(enemiesActive[i]);
            }
        }

        for (int i = 0; i < deadEnemies.Count; i++)
        {
            enemiesActive.Remove(deadEnemies[i]);

            Destroy(deadEnemies[i]);
        }
    }

    #endregion
}
