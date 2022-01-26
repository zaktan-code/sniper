using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings_", menuName = "SettingsAsset")]
public class Settings : ScriptableObject {

	public float bulletSpeed = 25f;
	public float playerSpeed = 5f;
	public float shootingTimeout = 0.5f;
	public BulletForce bulletForce = BulletForce.Medium;
	public float bulletMaxDistance = 10f;

}
