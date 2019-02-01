using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeObject : MonoBehaviour
{

	public GameObject viewNextObj;
	public GameObject crossObj;
	private GameObject AdlObj;

	//生成して破棄するスクリプトの作成する必要ある
	public List<GameObject> gameobjects;
	public int showCount =0;
	public EyeCode eyeCode;

    // Start is called before the first frame update
    void Start()
    {
		ShowViewNext ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	//nullは何も入っているものがない場合　！は否定　！＝　nullではない、何か入っている
	void ShowViewNext() {
		if (AdlObj != null) {
			　
			Destroy (AdlObj);
		}
		viewNextObj.SetActive (true);
		Invoke ("ShowCross", 3.0f);
	}
	void ShowCross() {
		viewNextObj.SetActive(false);
		crossObj.SetActive(true);
		Invoke ("ShowGoods", 1.0f);
	}

	void ShowGoods() {
		//eyeCode.StartTracking ();
		crossObj.SetActive(false);
		this.AdlObj = Instantiate (gameobjects[showCount])as GameObject;
		AdlObj.transform.position = new Vector3 (0, 0, 0);

		showCount++;

		if (showCount < 10) {
			// もう一度繰り返す。
			Invoke ("ShowViewNext", 5.0f);

		}
		else{
			Destroy (AdlObj);
		}
		//まったく別　条件に関係なく呼び出される
		Invoke ("CallFinishEyeTracking", 5.0f);
		//showCount++;
	}
	void CallFinishEyeTracking (){
		//eyeCode.FinishTracking ();
	}

}

