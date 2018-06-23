using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	Vector3 startpos;

	// Use this for initialization
	void Start () {
		startpos = GetComponent<Transform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		Transform tr = GetComponent<Transform>();
		Vector3 pos = new Vector3(
			startpos.x + Random.Range(-2.0f, 2.0f),
			startpos.y + Random.Range(-2.0f, 2.0f));
		tr.position = pos;
	}
}
