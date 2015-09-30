using UnityEngine;
using System.Collections;

public class EnemyShipController : MonoBehaviour {
	
	public float health, projectileSpeed, fireRate;
	public GameObject projectile;
	float chargeLevel = 0;
    public AudioClip fireSound;
	
	void Fire(){
		GameObject enemyLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector3(0,-projectileSpeed,0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	
	void Update(){
		if (chargeLevel > 100){
			Fire();
			chargeLevel = 0;
		} else {
			float random = Random.value*fireRate;
			chargeLevel = chargeLevel + Time.deltaTime*random;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Projectile laser = col.gameObject.GetComponent<Projectile>();
		if (laser) {
			laser.Hit();
			health -= laser.GetDamage();
			if (health < 0) {
				Destroy(gameObject);
			}
		}
	}
}
