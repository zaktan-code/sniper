using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour {

	public void Hit(Vector3 normal, Vector3 hitPosition) {
		Debug.DrawRay(hitPosition, normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
		GameObject hitGO = Instantiate(hitEffect, hitPosition, Quaternion.identity, transform);
		Destroy(hitGO, HIT_EFFECT_TIME);
		GetComponent<Rigidbody>().AddForceAtPosition(hitForce[GameManager.instance.settings.bulletForce] * normal.normalized, hitPosition, ForceMode.Impulse);
	}

	public void SetHitEffect(GameObject hitEffect) {
		this.hitEffect = hitEffect;
	}

	private const float HIT_EFFECT_TIME = 1.5f;
	private GameObject hitEffect;

	private Dictionary<BulletForce, float> hitForce = new Dictionary<BulletForce, float> {
		{ BulletForce.Low, 25f },
		{ BulletForce.Medium, 80f },
		{ BulletForce.High, 130f },
	};

	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Bullet") {
			Vector3 hitPosition = collision.GetContact(0).point;
			Bullet bullet = collision.gameObject.GetComponent<Bullet>();
			Vector3 hitNormal = bullet.Direction;
			Hit(hitNormal, hitPosition);
			Destroy(collision.collider.gameObject);
		}
	}

}
