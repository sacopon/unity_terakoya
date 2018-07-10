using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour {

	// オブジェクト参照
	private GameObject gameManager;	// ゲームマネージャ

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// オーブ取得
	public void TouchOrb() {
		if (Input.GetMouseButton(0) == false) {
			return;
		}

		gameManager.GetComponent<GameManager>().GetOrb();
		Destroy(this.gameObject);
	}
}
