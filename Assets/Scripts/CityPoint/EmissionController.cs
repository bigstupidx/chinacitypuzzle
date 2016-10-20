using UnityEngine;
using System.Collections;

public class EmissionController : MonoBehaviour {

	[SerializeField] Color emissionColor;
	[SerializeField] int emissionCount;
	[SerializeField] float emissionTime;

	Renderer _renderer;

	void Awake() {
		_renderer = transform.FindChild("Cylinder").FindChild("Sphere").GetComponent<Renderer> ();
	}

	// Use this for initialization
	void Start () {
//		StartCoroutine (Execute ());
	}

	public void StartEmission() {
		StartCoroutine (Execute ());
	}
	
	private IEnumerator Execute() {
		var originalMaterial = new Material (_renderer.material);
		Color originalColor = originalMaterial.GetColor ("_EmissionColor");

		_renderer.material.EnableKeyword ("_EMISSION");
		for (int i = 0; i < emissionCount; i++) {
			
			float nowTime = 0.0f;
			// Gradually change to the specified color
			while (nowTime < emissionTime) {
				_renderer.material.SetColor ("_EmissionColor", Color.Lerp (originalColor, emissionColor, nowTime / emissionTime));
				yield return new WaitForSeconds (0.05f);
				nowTime += 0.1f;
			}

			nowTime = 0.0f;
			// Gradually change to the original color from the specified color
			while (nowTime < emissionTime) {
				_renderer.material.SetColor ("_EmissionColor", Color.Lerp (emissionColor, originalColor, nowTime / emissionTime));
				yield return new WaitForSeconds (0.05f);
				nowTime += 0.1f;
			}
		}
	}
}
