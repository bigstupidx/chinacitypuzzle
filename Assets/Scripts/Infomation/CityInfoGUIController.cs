using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityInfoGUIController : MonoBehaviour {

	Text cityPinyin;
	Text cityName;
	Text capitalTitle;
	Text capitalName;
	Text capitalPinyin;

	// 
	void Awake() {
		cityPinyin = GameObject.Find ("CityPinyin").GetComponent<Text>();
		cityName = GameObject.Find ("CityName").GetComponent<Text>();
		capitalTitle = GameObject.Find ("CapitalTitle").GetComponent<Text>();
		capitalName = GameObject.Find ("CapitalName").GetComponent<Text>();
		capitalPinyin = GameObject.Find ("CapitalPinyin").GetComponent<Text>();
	}

	/// <summary>
	/// To configure each information of the city to the panel
	/// </summary>
	/// <param name="city">City.</param>
	public void SetCity(City city) {
		cityPinyin.text = city.namePinyin;
		cityName.text = city.name;
		capitalTitle.text = city.capitalTitle;
		capitalName.text = city.capitalName;
		capitalPinyin.text = city.capitalNamePinyin;
	}


}
