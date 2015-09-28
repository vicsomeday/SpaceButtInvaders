using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	public float speed, projectileSpeed, fireRate, health;
	public GameObject projectile;
    public AudioClip fireSound;
	float xmin;
	float xmax;

	void Start () {
		float zDistance = transform.position.z - Camera.main.transform.position.z;
		xmin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,zDistance)).x + 0.55f;
		xmax = Camera.main.ViewportToWorldPoint(new Vector3(1,0,zDistance)).x - 0.55f;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		Projectile enemyLaser = col.gameObject.GetComponent<Projectile>();
		if (enemyLaser) {
				enemyLaser.Hit();
				health -= enemyLaser.GetDamage();
				if (health < 0) {
					Destroy(gameObject);
				}
		}
	}
	
	void fire(){
		GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("fire", 0.0000001f, fireRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("fire");
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position -= new Vector3(speed*Time.deltaTime,0,0);
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += new Vector3(speed*Time.deltaTime,0,0);
		}
		
		float clampX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(clampX, transform.position.y, transform.position.z);
	}	
}
