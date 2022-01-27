using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	[SerializeField]
	private Transform[] views;

	public void ToggleView() {
		SetView(view == views.Length - 1 ? view = 0 : ++view);
	}

	private byte view = 0;

	private void SetView(int viewNum) {
		transform.position = views[viewNum].position;
		transform.rotation = views[viewNum].rotation;
	}

}
