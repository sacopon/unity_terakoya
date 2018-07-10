using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	// 定数定義
	private const int MAX_ORB = 10;	// オーブ最大数
	private const int RESPAWN_TIME = 5;	// オーブが発生する秒数

	// オブジェクト参照
	public GameObject orbPrefab;	// オーブプレハブ
	public GameObject canvasGame;	// ゲームキャンバス
	public GameObject textScore;	// スコアテキスト

	// メンバ変数
	private int score = 0;			// 現在のスコア
	private int nextScore = 100;	// レベルアップまでに必要なスコア

	private int currentOrb = 0;		// 現在のオーブ数
	private DateTime lastDateTime;	// 前回オーブを生成した時間

	// Use this for initialization
	void Start () {
		currentOrb = 10;

		// 初期オーブ生成
		for (int i = 0; i < currentOrb; i++) {
			CreateOrb ();
		}

		// 初期設定
		lastDateTime = DateTime.UtcNow;

		RefreshScoreText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentOrb < MAX_ORB) {
			TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

			if (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
				while (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME)) {
					CreateNewOrb ();
					timeSpan -= TimeSpan.FromSeconds(RESPAWN_TIME);
				}
			}
		}
	}

	// 新しいオーブの生成
	public void CreateNewOrb () {
		lastDateTime = DateTime.UtcNow;
		if (currentOrb >= MAX_ORB) {
			return;
		}

		CreateOrb ();
		currentOrb++;
	}

	public void CreateOrb () {
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
		currentOrb--;
	}

	// スコアテキスト更新
	void RefreshScoreText () {
		textScore.GetComponent<Text>().text = "徳:" + score + " / " + nextScore;
	}
}
