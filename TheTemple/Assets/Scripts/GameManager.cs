using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// 定数定義
	private const int MAX_ORB = 10;	// オーブ最大数

	// オブジェクト参照
	public GameObject orbPrefab;	// オーブプレハブ
	public GameObject canvasGame;	// ゲームキャンバス

	// Use this for initialization
	void Start () {
		// 初期オーブ生成
		for (int i = 0; i < MAX_ORB; i++) {
			CreateOrb ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateOrb() {
		GameObject orb = (GameObject)Instantiate (orbPrefab);
		orb.transform.SetParent (canvasGame.transform, false);
		orb.transform.localPosition = new Vector3(
			UnityEngine.Random.Range(-300.0f, 300.0f),
			UnityEngine.Random.Range(-140.0f, -500.0f)
		);
	}
}
