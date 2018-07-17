using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	// 定数定義
	private const int MAX_ORB = 10;	// オーブ最大数
	private const int RESPAWN_TIME = 5;	// オーブが発生する秒数
	private const int MAX_LEVEL = 2;	// 最大お寺レベル

	// データセーブ用キー
	private const string KEY_SCORE = "SCORE";	// スコア
	private const string KEY_LEVEL = "LEVEL";	// レベル
	private const string KEY_ORB = "ORB";		// オーブ数
	private const string KEY_TIME = "TIME";		// 時間

	// オブジェクト参照
	public GameObject orbPrefab;		// オーブプレハブ
	public GameObject smokePrefab;		// 煙プレハブ
	public GameObject kusudamaPrefab;	// くす玉プレハブ
	public GameObject canvasGame;		// ゲームキャンバス
	public GameObject textScore;		// スコアテキスト
	public GameObject imageTemple;		// お寺
	public GameObject imageMokugyo;		// 木魚

	public AudioClip getScoreSE;		// 効果音:スコアゲット
	public AudioClip levelUpSE;			// 効果音:レベルアップ
	public AudioClip clearSE;			// 効果音:クリア

	// メンバ変数
	private int score = 0;			// 現在のスコア
	private int nextScore = 10;	// レベルアップまでに必要なスコア

	private int currentOrb = 0;		// 現在のオーブ数
	private int templeLevel = 0;	// 寺のレベル
	private DateTime lastDateTime;	// 前回オーブを生成した時間
	private int[] nextScoreTable = new int[] {10, 100, 150};	// レベルアップ値
	private AudioSource audioSource;	// オーディオソース

	private int numOfOrb;			// まとめて生成するオーブの数

	// Use this for initialization
	void Start () {
		// オーディオソース取得
		audioSource = this.gameObject.GetComponent<AudioSource> ();

		// 初期設定
		score = PlayerPrefs.GetInt(KEY_SCORE, 0);
		templeLevel = PlayerPrefs.GetInt(KEY_LEVEL, 0);

		nextScore = nextScoreTable[templeLevel];
		imageTemple.GetComponent<TempleManager> ().SetTemplePicture(templeLevel);
		imageTemple.GetComponent<TempleManager> ().SetTempleScale(score, nextScore);

		RefreshScoreText ();
	}

	// 寺のレベル管理
	void TempleLevelUp () {
		if (score >= nextScore) {
			if (templeLevel < MAX_LEVEL) {
				templeLevel++;
				score = 0;

				TempleLevelUpEffect ();

				nextScore = nextScoreTable [templeLevel];
				imageTemple.GetComponent<TempleManager> ().SetTemplePicture(templeLevel);
			}
		}
	}

	// レベルアップ時の演出
	void TempleLevelUpEffect () {
		GameObject smoke = (GameObject)Instantiate (smokePrefab);
		smoke.transform.SetParent(canvasGame.transform, false);
		smoke.transform.SetSiblingIndex (2);

		audioSource.PlayOneShot (levelUpSE);

		Destroy (smoke, 0.5f);
	}

	// 寺が最後まで育った時の演出
	void ClearEffect () {
		GameObject kusudama = (GameObject)Instantiate (kusudamaPrefab);
		kusudama.transform.SetParent (canvasGame.transform, false);

		audioSource.PlayOneShot (clearSE);
	}

	// Update is called once per frame
	void Update () {
		// まとめて生成するオーブがあれば生成
		while (numOfOrb > 0) {
			Invoke ("CreateNewOrb", 0.1f * numOfOrb);
			numOfOrb--;
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

		SaveGameData();
	}

	public void CreateOrb () {
		GameObject orb = (GameObject)Instantiate (orbPrefab);
		orb.transform.SetParent (canvasGame.transform, false);
		orb.transform.localPosition = new Vector3(
			UnityEngine.Random.Range(-100.0f, 100.0f),
			UnityEngine.Random.Range(-300.0f, -450.0f)
		);

		// オーブの種類を設定
		int kind = UnityEngine.Random.Range(0, templeLevel + 1);
		switch (kind) {
			case 0:
				orb.GetComponent<OrbManager> ().SetKind(OrbManager.ORB_KIND.BLUE);
				break;

			case 1:
				orb.GetComponent<OrbManager> ().SetKind(OrbManager.ORB_KIND.GREEN);
				break;

			case 2:
				orb.GetComponent<OrbManager> ().SetKind(OrbManager.ORB_KIND.PURPLE);
				break;
		}

		orb.GetComponent<OrbManager> ().FlyOrb();

		audioSource.PlayOneShot (getScoreSE);

		// 木魚アニメ再生
		AnimatorStateInfo stateInfo = imageMokugyo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.get@ImageMokugyo")) {
			// すでに再生中なら先頭から
			imageMokugyo.GetComponent<Animator>().Play(stateInfo.fullPathHash, 0, 0.0f);
		}
		else {
			imageMokugyo.GetComponent<Animator>().SetTrigger("isGetScore");
		}
	}

	// オーブ入手
	public void GetOrb (int getScore) {

		if (score < nextScore) {
			score += getScore;

			// レベルアップ値を超えないよう制限
			if (score > nextScore) {
				score = nextScore;
			}


			TempleLevelUp ();
			RefreshScoreText ();
			imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);

			// ゲームクリア判定
			if ((score == nextScore) && (templeLevel == MAX_LEVEL)) {
				ClearEffect ();
			}
		}

		currentOrb--;

		SaveGameData();
	}

	// スコアテキスト更新
	void RefreshScoreText () {
		textScore.GetComponent<Text>().text = "徳:" + score + " / " + nextScore;
	}

	// ゲームデータをセーブ
	void SaveGameData() {
		PlayerPrefs.SetInt(KEY_SCORE, score);
		PlayerPrefs.SetInt(KEY_LEVEL, templeLevel);
		PlayerPrefs.SetInt(KEY_ORB, currentOrb);
		PlayerPrefs.SetString(KEY_TIME, lastDateTime.ToBinary().ToString());

		PlayerPrefs.Save();
	}
}
