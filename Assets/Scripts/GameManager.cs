using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;

	public Settings settings;

	public void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance == this) {
			Destroy(gameObject);
		}
	}

}
