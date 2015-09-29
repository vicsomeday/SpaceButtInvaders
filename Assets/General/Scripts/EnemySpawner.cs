using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab, bossPrefab, boss;
	public float width, height, speed, spawnDelay, waves;
	bool isMovingRight = true;
	float xmin, xmax;
    int enemies, bosses;


	void Start () {
		float zDistance = transform.position.z - Camera.main.transform.position.z;
		xmin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,zDistance)).x;
		xmax = Camera.main.ViewportToWorldPoint(new Vector3(1,0,zDistance)).x;

        enemies = 0;
        bosses = 0;
		SpawnEnemies();
	}
	
	void SpawnEnemies(){
        enemies += 1;
		Transform position = NextFreePosition();
		    if (position) {
			    GameObject enemy = Instantiate(enemyPrefab, position.transform.position, Quaternion.identity) as GameObject;
			    enemy.transform.parent = position;
		    }
		    if (NextFreePosition()){
			    Invoke ("SpawnEnemies", spawnDelay);
		    }
    }

    void SpawnBoss()
    {
        bosses = 1;
        boss = Instantiate(bossPrefab, new Vector3(0,20,4), Quaternion.identity) as GameObject;
    }

    public void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
	
	// Update is called once per frame
	void Update () {
		if (isMovingRight) {
			transform.position += Vector3.right*speed*Time.deltaTime;
		} else {
			transform.position += Vector3.left*speed*Time.deltaTime;
		}
		
		if ((transform.position.x - width*0.5f) < xmin) {
			isMovingRight = true;
		} else if ((transform.position.x + width*0.5f) > xmax) {
			isMovingRight = false;
		}
		
		if (AllEnemiesAreDead()) {
            if (enemies <= waves*transform.childCount) {
                SpawnEnemies();
            } else if (bosses == 0) {
                SpawnBoss();
            }
		}
	}
	
	Transform NextFreePosition(){
		foreach(Transform Position in transform){
			if(Position.childCount == 0){
				return Position;
			}
		}
		return null;
	}
	
	bool AllEnemiesAreDead(){
		foreach(Transform Position in transform){
			if(Position.childCount > 0){
				return false;
			}
		}
		return true;
	}

}