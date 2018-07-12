using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	// 定数定義
	private const int MAX_ORB = 10;	// オーブ最大数
	private const int RESPAWN_TIME = 1;	// オーブが発生する秒数
	private const int MAX_LEVEL = 2;	// 最大お寺レベル

	// オブジェクト参照
	public GameObject orbPrefab;		// オーブプレハブ
	public GameObject smokePrefab;		// 煙プレハブ
	public GameObject kusudamaPrefab;	// くす玉プレハブ
	public GameObject canvasGame;		// ゲームキャンバス
	public GameObject textScore;		// スコアテキスト
	public GameObject imageTemple;		// お寺

	// メンバ変数
	private int score = 0;			// 現在のスコア
	private int nextScore = 10;	// レベルアップまでに必要なスコア

	private int currentOrb = 0;		// 現在のオーブ数
	private int templeLevel = 0;	// 寺のレベル
	private DateTime lastDateTime;	// 前回オーブを生成した時間
	private int[] nextScoreTable = new int[] {10, 10, 10};	// レベルアップ値

	// Use this for initialization
	void Start () {
		currentOrb = 10;

		// 初期オーブ生成
		for (int i = 0; i < currentOrb; i++) {
			CreateOrb ();
		}

		// 初期設定
		lastDateTime = DateTime.UtcNow;
		nextScore = nextScoreTable[templeLevel];
		imageTemple.GetComponent<TempleManager> ().SetTemplePicture(templeLevel);
		imageTemple.GetComponent<TempleManager> ().SetTempleScale(score, nextScore);

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
