using UnityEngine;
using System.Collections;

public class MoveText : MonoBehaviour {
	public bool IsFinish = false;

	public void OnMoveTextAnimationFinish() {
		this.IsFinish = true;
	}
}
