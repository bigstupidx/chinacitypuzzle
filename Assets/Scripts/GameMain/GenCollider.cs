using UnityEngine;
using System.Collections;

namespace ZhStg.GameMain
{
	public class GenCollider : MonoBehaviour
	{

		GUIController guiCtrl;

		void Awake ()
		{
			guiCtrl = GameObject.Find ("GameControllers").GetComponent<GUIController> ();
		}


		// Use this for initialization
		void Start ()
		{
			guiCtrl.SetGameOverEnable (false);
		}

		void OnTriggerEnter (Collider other)
		{
			SoundManager.Instance.PlaySE (4);
			guiCtrl.SetGameOverEnable (true);
			GameManager.Instanse.GameOver ();
		}

	}
}
