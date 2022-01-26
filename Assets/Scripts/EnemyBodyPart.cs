using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour {

	public void Hit(Vector3 normal, Vector3 hitPosition) {
		//Debug.DrawRay(hitPosition, normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
		GetComponent<Rigidbody>().AddForceAtPosition(hitForce[GameManager.instance.settings.bulletForce] * normal.normalized, hitPosition, ForceMode.Impulse);
	}

	private Dictionary<BulletForce, float> hitForce = new Dictionary<BulletForce, float> {
		{ BulletForce.Low, 10f },
		{ BulletForce.Medium, 50f },
		{ BulletForce.High, 100f },
		{ BulletForce.Insane, 200f },
	};

	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Bullet") {
			Vector3 hitPosition = collision.GetContact(0).point;
			Vector3 hitNormal = collision.GetContact(0).normal;
			Hit(hitNormal, hitPosition);
			Destroy(collision.collider.gameObject);
		}
	}

}
