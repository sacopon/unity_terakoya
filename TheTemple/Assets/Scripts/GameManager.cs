using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	// 定数定義
	private const int MAX_ORB = 10;	// オーブ最大数

	// オブジェクト参照
	public GameObject orbPrefab;	// オーブプレハブ
	public GameObject canvasGame;	// ゲームキャンバス
	public GameObject textScore;	// スコアテキスト

	// メンバ変数
	private int score = 0;			// 現在のスコア
	private int nextScore = 100;	// レベルアップまでに必要なスコア

	// Use this for initialization
	void Start () {
		// 初期オーブ生成
		for (int i = 0; i < MAX_ORB; i++) {
			CreateOrb ();
		}

		RefreshScoreText ();
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

	// オーブ入手
	public void GetOrb () {
		score += 1;
		RefreshScoreText ();
	}

	// スコアテキスト更新
	void RefreshScoreText () {
		textScore.GetComponent<Text>().text = "徳:" + score + " / " + nextScore;
	}
}
