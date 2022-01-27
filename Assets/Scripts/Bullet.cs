using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 StartPosition { get; private set; }

	public Vector3 Direction {
		get {
			return transform.position - StartPosition;
		}
	}

	private void Start() {
		StartPosition = transform.position;
	}

}
