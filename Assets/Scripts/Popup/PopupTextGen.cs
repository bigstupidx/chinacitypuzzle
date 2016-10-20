using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupTextGen : MonoBehaviour {

	// One character width
	public float PopupTextWidth = 0.01f;

	public void SetParams() {

	}

	public void Popup(string text) {
		StartCoroutine (Execute (text));
	}

	/// <summary>
	/// popup
	/// </summary>
	private IEnumerator Execute(string PopupString)
	{
		var pos = this.gameObject.transform.position;
		// Displayed on the front
		pos = new Vector3(pos.x, pos.y, pos.z - 1.0f);
		var texts = new List<MoveText> ();

		var root = new GameObject ();
		var canvasGroup = root.AddComponent<CanvasGroup> ();

		transform.position = pos;
		root.transform.SetParent (transform.root.gameObject.transform);
		Vector3 rootPos = root.transform.localPosition;
		root.transform.localPosition = Vector3.zero;

		// Position for character
		Vector3 charLocalPos = new Vector3(0.0f, 0.0f, 0.0f);
		foreach (var s in PopupString) {
			var obj = new GameObject ();
			//obj.transform.position = pos;
			obj.transform.SetParent (root.transform);
			obj.transform.localPosition = charLocalPos;

			// It generates one character at a time
			var valueText = (GameObject)Instantiate (Resources.Load("Prefabs/MoveText"), pos, Quaternion.identity);
			var textComp = valueText.GetComponent<Text> ();
			textComp.text = s.ToString ();
			valueText.transform.SetParent (obj.transform);
			valueText.transform.localPosition = Vector3.zero;
			texts.Add( valueText.GetComponent<MoveText>() );

			// Wait 0.03 seconds (suitable)
			yield return new WaitForSeconds (0.03f);

			// next position
			//pos.x += this.PopupTextWidth;
			charLocalPos.x += this.PopupTextWidth;
		}

		// wait (suitable)
		while (!texts.TrueForAll( t => t.IsFinish )) {
			yield return new WaitForSeconds (0.1f);
		}

		// fade out
		for (int n=9; n>=0; n--) {
			canvasGroup.alpha = n / 10.0f;
			yield return new WaitForSeconds (0.01f);
		}

		// destory
		Destroy (root);
		Destroy (gameObject);
	}
}
