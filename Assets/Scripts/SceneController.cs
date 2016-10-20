using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController {

	private static SceneController instanse;

	private SceneController () {

	}

	public static SceneController Instanse {
		get {
			if (instanse == null) instanse = new SceneController();
			return instanse;
		}
	}

	/// <summary>
	/// To Title Scene
	/// </summary>
	public void ChangeTitle() {
		// Initialize GameManager
		GameManager.Instanse.initialize();
		// To Title
		SceneManager.LoadScene("Title");
	}

	/// <summary>
	/// To main game scene
	/// </summary>
	public void ChangeMainGame() {
		// stop bgm
		SoundManager.Instance.StopBGM ();
		// set volume
		SoundManager.Instance.volume.BGM = 1.0f;
		// To MainScene
		SceneManager.LoadScene("GameMain");
	}

	/// <summary>
	/// To result scene
	/// </summary>
	public void ChangeResult() {
		SoundManager.Instance.PlayBGM ((int)BGM.Result);
		SceneManager.LoadScene("Result");
	}

	/// <summary>
	/// Move to the city information scene
	/// </summary>
	public void ChangeCityInfo() {
		SceneManager.LoadScene("CityInfo");
	}

	/// </summary>
	public void ReturnResult () {
		SceneManager.LoadScene ("Result");
	}

	public void ChangeCredit() {
		SceneManager.LoadScene ("Credit");
	}
}
