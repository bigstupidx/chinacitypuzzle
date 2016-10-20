using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ZhStg.GameMain
{
	public class CityRoulette : MonoBehaviour
	{

		GUIController guiCtrl;
		GameObject cityFrontPanel;
		public float duration = 5.0f;
		public float now = 0.0f;
		string nowCity;
		public string nowShortName;

		public Color cityColor;
		public Color captitalColor;

		enum CityNameKind
		{
			Normal,
			Short
		};

		void Awake ()
		{
			guiCtrl = GameObject.Find ("GameControllers").GetComponent<GUIController> ();
			cityFrontPanel = GameObject.Find ("CityFrontPanel");
		}

		// Use this for initialization
		void Start ()
		{
			cityFrontPanel.GetComponent<Image> ().color = cityColor;
			guiCtrl.SetCityText ("请稍等...");
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (GameManager.Instanse.inPlay) {
				now += Time.deltaTime;
				if (now > duration) {
					// Update the screen and variable in the new city information
					updateCityPanel ();
					// sound change city
					SoundManager.Instance.PlaySE (2);
					now = 0.0f;
				}
			}
		}

		/// <summary>
		/// To update the display with the data held in the province
		/// </summary>
		void updateCityPanel ()
		{
			string dispName = "";
			// Get the information of the province to be displayed in random
			int r = GameManager.Instanse.GetRondomCityNum ();
			City city = CityGenerator.Instance.GetCityByIndex (r);

			if (GetCityNameKind () == CityNameKind.Short) {
				cityFrontPanel.GetComponent<Image> ().color = captitalColor;
				dispName = city.capitalName;
			} else {
				cityFrontPanel.GetComponent<Image> ().color = cityColor;
				dispName = city.name;
			}

			this.nowCity = dispName;
			this.nowShortName = city.shortName;
			guiCtrl.SetCityText (this.nowCity);
		}

		/// <summary>
		/// Return information type of the city to be displayed in a random(Normal=province name, Short = city name in province)
		/// </summary>
		CityNameKind GetCityNameKind ()
		{
			if (GameManager.Instanse.mixCapitalAndCity) {
				int rand = Random.Range (0, 2);
				return (rand == 0) ? CityNameKind.Normal : CityNameKind.Short;
			} else if (GameManager.Instanse.useCapitals) {
				return CityNameKind.Short;
			} else {
				return CityNameKind.Normal;
			}
		}
	}
}