using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform tr = GetComponent<Transform>();
		Vector3 pos = new Vector3(
			Random.Range(-2.0f, 2.0f),
			Random.Range(-2.0f, 2.0f));
		tr.position = pos;
	}
}
