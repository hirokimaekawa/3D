using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

public class EyeCode : MonoBehaviour {

	// 最初flagがtrueになっている。
	private bool flag = true;

	// どちらもfalseで初期化されている。
	//private bool startFlag = false;
	private bool mflag = false;

	private int state = 0;
	private string fileName;
	private string hogeString;
	private int count = 1;
	//private int i = 0;
	StringBuilder data = new StringBuilder();
	float time = 0;
	//int blink = 0;
	private string filePath;
	float time2 = 0;
	public GameObject sphere;
	//public float deg;
	//private float rad;

	// Use this for initialization
	private void Start()
	{
		//rad = deg * (Mathf.PI / 180);
	}

	// 固定秒数で呼ばれる処理。
	private void FixedUpdate()
	{
		time = Time.realtimeSinceStartup - time2;
		Vector3 leftVec = FoveInterface.GetLeftEyeVector();
		Vector3 rightVec = FoveInterface.GetRightEyeVector();
		//if(FoveInterface.CheckEyesClosed() == Fove.Managed.EFVR_Eye.Neither)
		//{
		//    blink = 0;
		//}
		//if(FoveInterface.CheckEyesClosed() == Fove.Managed.EFVR_Eye.Left)
		//{
		//    blink = 1;
		//}
		//if(FoveInterface.CheckEyesClosed() == Fove.Managed.EFVR_Eye.Right)
		//{
		//    blink = 2;
		//}
		//if(FoveInterface.CheckEyesClosed() == Fove.Managed.EFVR_Eye.Both)
		//{
		//    blink = 3;
		//}


		if (state == 1 && flag == true)
		{
			// 1回目spaceを押した時に処理に入る。

			UnityEngine.Debug.Log("初期化の処理。");

			DateTime dtNow = DateTime.Now;
			fileName = TitleLoading.inputValue + count;
			count++;
			data.Append("time,left x,left y,left z,right x,right y,right z,\n");

			// 反転する　→ falseになる
			flag = !flag;

			time2 = Time.realtimeSinceStartup;
		}
		else if(state == 1)
		{

			UnityEngine.Debug.Log("データを追加している。");


			// 上のifが呼ばれたこちらがずっと呼ばれる。
			data.Append(time + "," + leftVec.x + "," + leftVec.y + "," + leftVec.z + "," +
				rightVec.x + "," + rightVec.y + "," + rightVec.z + "," + "\n");
		}
		else if (state == 2 && flag == false)
		{
			UnityEngine.Debug.Log("データを書き出している。");
			//エクセルが開いたままだとIOExcepionのエラーが出現する
			filePath = "C:\\Users\\Yu Kume\\Desktop\\MeasureData\\blink\\" + fileName + ".csv";
			WriteCsv(filePath, data);
			//print("write");
			UnityEngine.Debug.Log("write");
			data = new StringBuilder();
			// 反転させている　→ trueにする
			flag = !flag;
		}
	}

	public void StartTracking(){
		mflag = !mflag;
		state = 1;
	}

	public void FinishTracking(){
		mflag = !flag;
		state = 2;
	}

	// Update is called once per frame
	//void Update () {

	// スペースキーを押した時
	//if (Input.GetKeyDown(KeyCode.Space))
	//{
	// 最初にスペースキーを押した時にここに入る
	// 開始
	//if (mflag == false && startFlag == false)
	//{
	// mflagをtrueにしている。
	//mflag = !mflag;

	// startFlagもtrueにしている。
	//startFlag = !startFlag;

	// stateを1にしている。
	//state = 1;
	//}
	// 一時停止
	//else if(mflag == true)
	//{
	// 2回目押した時。

	//mflag = !mflag;
	//state = 2;
	//sphere.transform.position = new Vector3(5 * Mathf.Tan(rad) * Mathf.Cos(Mathf.PI / 4 * i), 5 * Mathf.Tan(rad) * Mathf.Sin(Mathf.PI / 4 * i), 5f);
	//プログラムがその行を実行した後に１増加させます。
	//i++;
	//}
	// 再開
	//else if(mflag == false)
	//{
	// 3回目押した時。

	//mflag = !mflag;
	//state = 1;
	//}
	//}
	//}

	private void WriteCsv(string filepath, StringBuilder data)
	{
		StreamWriter sw;
		sw = new StreamWriter(filepath, false);
		sw.WriteLine(data);
		sw.Flush();
		sw.Close();
	}
}