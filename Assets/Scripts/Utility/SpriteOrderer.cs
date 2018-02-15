using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderer : MonoBehaviour {

	private SpriteRenderer s;

	void Awake() {
		s = GetComponent<SpriteRenderer>();
	}

	void Update() {
		s.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
	}
}
