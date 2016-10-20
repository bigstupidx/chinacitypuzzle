using UnityEngine;
using System.Collections;

public class CityMarkController : MonoBehaviour {

	private static CityMarkController instanse;
	GameObject mark;

	private CityMarkController(){
		
	}

	public static CityMarkController Instanse {
		get {
			if (instanse == null) {
				instanse = GameObject.Find("MapUI").GetComponent<CityMarkController>();
			}
			return instanse;
		}
	}

	// To view the city mark
	public void SetMark(Vector3 pos) {
		if (mark != null) {
			Destroy (mark);
		}

		mark = Instantiate (Resources.Load ("Prefabs/CityMarker"), pos, Quaternion.identity) as GameObject;
		mark.transform.SetParent (transform);
		mark.transform.localScale *= Mathf.Abs(pos.z / 5.0f);
		mark.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
		mark.GetComponent<EmissionController> ().StartEmission ();
	}
}
