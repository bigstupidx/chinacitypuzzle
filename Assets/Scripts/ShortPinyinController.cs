using UnityEngine;
using System.Collections;

public class ShortPinyinController : MonoBehaviour {

	float lifeTimeLimit = 3.0f;
	float lifeTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;

		if (lifeTime >= lifeTimeLimit) {
			Destroy (this.gameObject);
		}
	}
}
