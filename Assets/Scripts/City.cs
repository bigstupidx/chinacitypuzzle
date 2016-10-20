using UnityEngine;
using System.Collections;

public class City {
	int _index;
	string _name;
	string _shortName;
	string _capitalName;
	string _capitalTitle;
	string _namePinyin;
	string _shortNamePinyin;
	string _capitalNamePinyin;
	float _capitalPosX;
	float _capitalPosY;
	float _capitalSize;
	float _capitalCameraX;
	float _capitalCameraY;
	float _capitalCameraSize;
	string _description1;
	string _description2;

	public int index { get { return _index; } }
	public string name { get { return _name; } }
	public string shortName { get { return _shortName; } }
	public string capitalName { get { return _capitalName; } }
	public string capitalTitle { get { return _capitalTitle; } }
	public string namePinyin { get { return _namePinyin; } }
	public string shortNamePinyin { get { return _shortNamePinyin; } }
	public string capitalNamePinyin { get { return _capitalNamePinyin; } }
	public float capitalPosX { get { return _capitalPosX; } }
	public float capitalPosY { get { return _capitalPosY; } }
	public float capitalSize { get { return _capitalSize; } }
	public float capitalCameraX { get { return _capitalCameraX; } }
	public float capitalCameraY { get { return _capitalCameraY; } }
	public float capitalCameraSize { get { return _capitalCameraSize; } }
	public string description1 { get { return _description1; } }
	public string description2 { get { return _description2; } }

	public City() {
		_index = 0;
		_name = "";
		_shortName = "";
		_capitalName = "";
		_capitalTitle = "";
		_namePinyin = "";
		_shortNamePinyin = "";
		_capitalNamePinyin = "";
		_capitalPosX = 0.0f;
		_capitalPosY = 0.0f;
		_capitalSize = 0.0f;
		_capitalCameraX = 0.0f;
		_capitalCameraY = 0.0f;
		_capitalCameraSize = 0.0f;
		_description1 = "";
		_description2 = "";
	}

	public City(int index, string name, string sname, string capName, string capTitle, string namePy, string snamePy, string capNamePy
		, float capPosX, float capPosY, float capPosSize, float capCameraX, float capCameraY, float capCameraSize
		, string desc1, string desc2) {

		_index = index;
		_name = name;
		_shortName = sname;
		_capitalName = capName;
		_capitalTitle = capTitle;
		_namePinyin = namePy;
		_shortNamePinyin = snamePy;
		_capitalNamePinyin = capNamePy;
		_capitalPosX = capPosX;
		_capitalPosY = capPosY;
		_capitalSize = capPosSize;
		_capitalCameraX = capCameraX;
		_capitalCameraY = capCameraY;
		_capitalCameraSize = capCameraSize;
		_description1 = desc1;
		_description2 = desc2;
	}
}
