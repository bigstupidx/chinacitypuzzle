using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text;

public class CityGenerator {

	private static CityGenerator instance;

	List<City> cityList;

	public static CityGenerator Instance {
		get {
			if (instance == null) {
				instance = new CityGenerator ();
				instance.ReadFile ();
			}

			return instance;
		}
	}

	/// <summary>
	/// To hold as an object by reading the city information from a text file
	/// </summary>
	void ReadFile(){

		string allText = null;

		try {
			allText = ((TextAsset) Resources.Load("City", typeof(TextAsset))).text;
		} catch (Exception e){
			Debug.LogError ("error " + e.StackTrace);
		}


		allText = allText.Trim().Replace("\r", "");
		List<string> lines = allText.Split ('\n').ToList ();

		cityList = new List<City> ();

		// One line for each reading
		foreach(string row in lines) {
			if (!row.StartsWith("#") && row.Trim() != "") {
				String[] data = row.Split(',');
				// Note to properly freeze If you do not change the following int value when you add the new element
				if (data.Length == 16) {
					cityList.Add (new City (int.Parse(data [0]), data [1], data [2], data [3], data [4], data [5]
						, data [6], data [7], float.Parse(data[8]), float.Parse(data[9]), float.Parse(data[10])
						, float.Parse(data[11]), float.Parse(data[12]), float.Parse(data[13])
						, data [14], data [15]));
				}
			}
		}
	}

	/// <summary>
	/// Return a list of cities
	/// </summary>
	/// <returns>The list.</returns>
	public List<City> GetList() {
		return cityList;
	}

	/// <summary>
	/// Return the city information
	/// </summary>
	/// <returns>The city by index.</returns>
	/// <param name="index">Index.</param>
	public City GetCityByIndex(int index) {
		return cityList [index];
	}

	/// <summary>
	/// Return the city information objects from the city abbreviation
	/// </summary>
	/// <returns>The city by short name.</returns>
	/// <param name="name">Name.</param>
	public City GetCityByShortName(string name) {
		foreach (City city in cityList) {
			if (city.shortName == name) {
				return city;
			}
		}

		return new City();
	}
}
