using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupTextGenForKeep : MonoBehaviour {

	// One character width
	float PopupTextWidth = 0.33f;
	// Pinyin display position adjustment (pinyin maximum number of characters)
	int MaxPinyinLength = 5;
	// Pinyin display position adjustment (1 characters in length)
	float TextWidth = 0.15f;


	GameObject root;

	Color TextColor = new Color(1.0f, 0.44f, 0.118f, 1.0f);

	public void SetParams() {

	}

	public void Popup(string text) {
		StartCoroutine (Execute (text));
	}

	/// <summary>
	/// PopUp
	/// </summary>
	private IEnumerator Execute(string PopupString)
	{
		var pos = this.gameObject.transform.position;
		// Displayed on the front
		pos = new Vector3(pos.x, pos.y, pos.z - 1.0f);
		var texts = new List<MoveText> ();

		root = new GameObject ();
		var canvasGroup = root.AddComponent<CanvasGroup> ();

		transform.position = pos;
		root.transform.SetParent (transform.root.gameObject.transform);
		Vector3 rootPos = root.transform.localPosition;
		root.transform.localPosition = Vector3.zero;

		// Position for character
		Vector3 charLocalPos = new Vector3((MaxPinyinLength - PopupString.Length) * this.TextWidth, 0.0f, 0.0f);
		foreach (var s in PopupString) {
			var obj = new GameObject ();
			//obj.transform.position = pos;
			obj.transform.SetParent (root.transform);
			obj.transform.localPosition = charLocalPos;

			// It generates one character at a time
			var valueText = (GameObject)Instantiate (Resources.Load("Prefabs/MoveText"), pos, Quaternion.identity);
			var textComp = valueText.GetComponent<Text> ();
			textComp.text = s.ToString ();
			textComp.color = TextColor;
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
	}

	public void DestroyKeepText() {
		// destroy
		Destroy (root);
		Destroy (gameObject);
	}
}
