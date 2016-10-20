using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoBall : MonoBehaviour {
	// Unselected of ball color
	[SerializeField] Color nonSelectedColor;
	// The color of the selected ball
	[SerializeField] Color centerColor;

	// City information associated with the ball
	City _city;
	public City City{get{return _city;} 
		set{_city = value;
			shortText.text = _city.shortName;}}

	// Move the original position
	public Vector3 fromPosition;
	// Destination position
	public Vector3 toPosition;

	// True if the ball should be in the center
	bool isCenter;

	// True if the move
	bool _isMoving;
	public bool isMoving { get { return _isMoving; } }

	// For Pinyin display hold
	GameObject pinyinPopup;

	// Abbreviated text
	Text shortText;

	void Awake() {
		shortText = transform.FindChild ("Canvas").transform.FindChild ("ChineseText").gameObject.GetComponent<Text>();
	}

	/// <summary>
	/// The first time you view the city information scene, performs the initialization of the ball only to be displayed in the front row
	/// </summary>
	public void initializeCenter() {
		isCenter = true;
		CreateShortPinyin ();
	}

	/// <summary>
	/// To color for selecting a ball
	/// </summary>
	public void SetCenterColor() {
		_SetCenterColor ();
	}

	/// <summary>
	/// To color for the unselected ball
	/// </summary>
	public void SetNonSelectedColor() {
		_SetNonSelectedColor ();
	}

	/// <summary>
	/// Move the ball to the center
	/// </summary>
	/// <param name="toPos">To position.</param>
	public void MoveToCenter(Vector3 toPos) {
		isCenter = true;
		Move (toPos);
		// Change the color
		_SetCenterColor();
	}

	/// <summary>
	/// Move the ball to the outside
	/// </summary>
	/// <param name="toPos">To position.</param>
	public void MoveToOut(Vector3 toPos) {
		isCenter = false;
		Move (toPos);
		// Change the color
		_SetNonSelectedColor();
	}

	/// <summary>
	/// Set the color of the front of the ball
	/// </summary>
	void _SetCenterColor() {
		gameObject.GetComponent<Renderer> ().material.SetColor("_SpecColor", centerColor);
	}

	/// <summary>
	/// Set the color of the ball other than the most front
	/// </summary>
	void _SetNonSelectedColor() {
		gameObject.GetComponent<Renderer> ().material.SetColor("_SpecColor", nonSelectedColor);
	}

	/// <summary>
	/// To move the ball
	/// </summary>
	/// <param name="toPos">To position.</param>
	void Move(Vector3 toPos) {
		fromPosition = transform.position;
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", toPos,
			"easeType", iTween.EaseType.easeInOutCubic,
			"loopType", "none",
			"time", 2.0f,
			"onstart", "StartMove",
			"onstarttarget", gameObject,
			"oncomplete", "EndMove",
			"oncompletetarget", gameObject
		));
	}

	/// <summary>
	/// Set that it is a moving state of the ball
	/// </summary>
	void StartMove() {
		if (!isCenter) {
			DestroyShortPinyin ();
		}
		_isMoving = true;
	}

	/// <summary>
	/// Set that it is a movement end state of the ball
	/// </summary>
	void EndMove() {
		if (isCenter) {
			CreateShortPinyin ();
		}
		_isMoving = false;
	}

	void CreateShortPinyin() {
		var pos = transform.position;
		pos.x -= transform.localScale.x / 4;
		pos.y += transform.localScale.y / 4;
		pinyinPopup = Instantiate (Resources.Load ("Prefabs/PopupTextGenKeep"), pos, Quaternion.identity) as GameObject;
		var popupScript = pinyinPopup.GetComponent<PopupTextGenForKeep> ();
		popupScript.Popup (_city.shortNamePinyin);
	}

	void DestroyShortPinyin() {
		if (pinyinPopup != null) {
			var popupScript = pinyinPopup.GetComponent<PopupTextGenForKeep> ();
			popupScript.DestroyKeepText ();
		}
	}
}
