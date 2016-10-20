using UnityEngine;
using System.Collections;

public class TitleGUIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayBGM ((int)BGM.Opening);
	}
}
