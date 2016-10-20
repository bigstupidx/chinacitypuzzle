using UnityEngine;
using System.Collections;

namespace ZhStg.GameMain
{
	public class SphereGenerator : MonoBehaviour
	{

		public float duration = 2.0f;
		float genTime = 0.0f;

		GameMainController gameMainController;

		void Awake ()
		{
			gameMainController = GameObject.Find ("GameControllers").GetComponent<GameMainController> ();
		}

		// Use this for initialization
		void Start ()
		{
			duration = GameManager.Instanse.duration;
		}
	
		// Update is called once per frame
		void Update ()
		{
			// Create Ball
			if (GameManager.Instanse.inPlay) {
				genTime += Time.deltaTime;
				if (genTime > duration) {
					// Adjust the position at which the ball comes out
					Vector3 generatorPos = transform.position;

					// Call create ball
					gameMainController.CreateCityBall (generatorPos);
					genTime = 0.0f;
				}
			}
		}
	}
}
