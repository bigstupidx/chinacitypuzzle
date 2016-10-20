using UnityEngine;
using System.Collections;

public class MapCameraController : MonoBehaviour {

	public float mapWidth = 19.0f;
	public float mapHeight = 20.0f;
	public float mapSize = 1.0f;

	// Coordinate units per 1%
	float unitX;
	float unitY;
	float unitSize;

	// True if the move
	bool _isMoving;
	public bool isMoving { get { return _isMoving; } }

	// Position for Moving from
	Vector3 fromLocalPos;
	// Position for Destination
	Vector3 toLocalPos;
	// Position for the city mark display
	Vector3 markPos;

	void Awake() {
		unitX = mapWidth / 100;
		unitY = mapHeight / 100;
		unitSize = mapSize;
	}

	/// <summary>
	/// Setting a map camera display position when the scene is displayed
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="size">Size.</param>
	public void SetInitialPosition(Vector3 cameraPos, Vector3 cityMarkPos) {
		cameraPos.z *= unitSize;
		cityMarkPos.z *= unitSize;
		transform.localPosition = ToOnMapPos(cameraPos);
		CityMarkController.Instanse.SetMark (ToOnMapPos(cityMarkPos));
	}

	GameObject airplane;

	/// <summary>
	/// Move the camera to the specified position
	/// </summary>
	/// <param name="perX">Per x.</param>
	/// <param name="perY">Per y.</param>
	/// <param name="size">Size.</param>
	public void MoveCamera(Vector3 pos, Vector3 markPos) {
		
		// -8 -1.5
		fromLocalPos = transform.localPosition;

		toLocalPos = ToOnMapPos(pos);

		this.markPos = ToOnMapPos(markPos);
		this.markPos.z *= unitSize;

		iTween.MoveTo(gameObject, iTween.Hash(
			"z", (toLocalPos.z - 3) * unitSize,
			"islocal", true,
			"easeType", iTween.EaseType.linear,
			"loopType", "none",
			"time", 0.8f,
			"onstart", "StartMove",
			"onstarttarget", gameObject,
			"oncomplete", "NextMove",
			"oncompletetarget", gameObject
		));

	}

	Vector3 ToOnMapPos(Vector3 pos) {
		return new Vector3 (unitX * PerToViewPosition (pos.x), unitY * PerToViewPosition (pos.y), -pos.z);
	}

	/// <summary>
	/// Setting that the camera is moving
	/// </summary>
	void StartMove() {
		_isMoving = true;
	}

	/// <summary>
	/// Moving between cities
	/// </summary>
	void NextMove() {
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", toLocalPos.x,
			"y", toLocalPos.y,
			"z", (toLocalPos.z - 3) * unitSize,
			"islocal", true,
			"easeType", iTween.EaseType.easeOutSine,
			"loopType", "none",
			"time", 1.2f,
			"oncomplete", "ZomeInMove",
			"oncompletetarget", gameObject
		));
	}

	/// <summary>
	/// The zoom up after the city has moved
	/// </summary>
	void ZomeInMove() {
		iTween.MoveTo(gameObject, iTween.Hash(
			"x", toLocalPos.x,
			"y", toLocalPos.y,
			"z", toLocalPos.z * unitSize,
			"islocal", true,
			"easeType", iTween.EaseType.easeInOutCubic,
			"loopType", "none",
			"time", 1.5f,
			"oncomplete", "EndMove",
			"oncompletetarget", gameObject
		));
	}

	/// <summary>
	/// Set the camera movement flag to Off
	/// </summary>
	void EndMove() {
		CityMarkController.Instanse.SetMark (markPos);
		_isMoving = false;
	}

	/// <summary>
	/// The calculation of the camera position
	/// </summary>
	/// <returns>The to view position.</returns>
	/// <param name="per">Per.</param>
	float PerToViewPosition(float per) {
		return per - 50.0f;
	}
}
