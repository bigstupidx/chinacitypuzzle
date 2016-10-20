using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ZhStg.GameMain
{
	public class GUIController : MonoBehaviour
	{

		GameObject gameOverPanel;
		GameObject gameQuitPanel;
		GameObject readyText;
		Text cityText;
		Text rankText;
		Text comboText;
		Slider rankBar;

		Text scoreText;

		void Awake ()
		{
			gameOverPanel = GameObject.Find ("GameOver");
			gameQuitPanel = GameObject.Find ("GameQuit");
			cityText = GameObject.Find ("CityText").GetComponent<Text> ();
			readyText = GameObject.Find ("ReadyText");
			scoreText = GameObject.Find ("Score").GetComponent<Text> ();
			rankText = GameObject.Find ("RankText").GetComponent<Text> ();
			comboText = GameObject.Find ("ComboText").GetComponent<Text> ();
			rankBar = GameObject.Find ("RankBar").gameObject.GetComponent<Slider> ();
		}

		// Use this for initialization
		void Start ()
		{
			// Hide the game over panel
			gameOverPanel.SetActive (false);
			// Hide the game interruption panel
			gameQuitPanel.SetActive (false);
			// It displays the game start countdown
			readyText.SetActive (true);
			comboText.text = "";
		}

		/// <summary>
		/// To set the display state of the game over panel
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		public void SetGameOverEnable (bool state)
		{
			gameOverPanel.SetActive (state);
		}

		/// <summary>
		/// Set the city name on the city display panel
		/// </summary>
		/// <param name="name">Name.</param>
		public void SetCityText (string name)
		{
			cityText.text = name;
		}

		/// <summary>
		/// Countdown display setting at the time of the game start
		/// </summary>
		/// <param name="text">Text.</param>
		public void SetReadyText (string text)
		{
			readyText.GetComponent<Text> ().text = text;
		}


		/// <summary>
		/// Update score
		/// </summary>
		/// <param name="score">Score.</param>
		public void SetScore (int score)
		{
			scoreText.text = score.ToString ();
		}

		/// <summary>
		/// The countdown display switching of at the time of the game start
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public void SetStartCounterEnabled (bool enabled)
		{
			readyText.SetActive (enabled);
		}

		/// <summary>
		/// To view the rank name
		/// </summary>
		/// <param name="rank">Rank.</param>
		public void SetRank (string rank)
		{
			rankText.text = rank;
		}

		/// <summary>
		/// Update of the experience bar
		/// </summary>
		/// <param name="per">Per.</param>
		public void UpdateBar (float per)
		{
			rankBar.value = per;
		}

		/// <summary>
		/// To set the combo number
		/// </summary>
		/// <param name="count">Count.</param>
		public void SetComboCoount (string combo)
		{
			this.comboText.text = combo;
		}

		/// <summary>
		/// Disps the game quit panel.
		/// </summary>
		public void DispGameQuitPanel() {
			gameQuitPanel.SetActive (true);
		}

		/// <summary>
		/// Hides the game quit panel.
		/// </summary>
		public void HideGameQuitPanel() {
			gameQuitPanel.SetActive (false);
		}
	}
}
