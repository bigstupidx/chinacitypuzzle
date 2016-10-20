using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

public class GameManager
{

	private static GameManager instanse;

	// score
	public ReactiveProperty<int> Score { get; private set; }
	// Ball falling interval
	public float duration = 1.0f;
	// The number of types of city ball to drop in a single game
	public int dropCityCount = 5;
	// The number of consecutive correct answer
	public ReactiveProperty<int> ComboCount { get; private set; }

	// Point which serves as a reference to be added
	int originalPoint = 100;
	// Configured to use the name of the city
	[System.NonSerialized] public bool useCapitals = false;
	// Configured to use to mix the name of the provinces and cities
	[System.NonSerialized] public bool mixCapitalAndCity = false;
	// List of the index of the list of cities to drop in a single game
	[System.NonSerialized] public List<int> inPlayCityNums;
	// Or during game play?
	public bool inPlay = false;

	[System.NonSerialized] static int[] BeginnerModeScores = new int[] { 1000, 4000, 10000 };
	[System.NonSerialized] static int[] EasyModeScores = new int[] { 1000, 3000, 6000, 10000, 20000 };
	[System.NonSerialized] static int[] NormalModeScores = new int[] { 1000, 3000, 6000, 10000, 20000, 50000 };
	public int[] modeScores = EasyModeScores;
	[System.NonSerialized] static float[] ComboPow = new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };

	GameModeEnum _gameMode;
	public GameModeEnum GameMode {
		get {
			if (_gameMode == null) {
				_gameMode = GameModeEnum.Easy;
			}
			return _gameMode;
		} 
		set{ _gameMode = value; }
	}

	private GameManager ()
	{
		
	}

	public static GameManager Instanse {
		get {
			if (instanse == null) {
				instanse = new GameManager ();
				instanse.Score = new IntReactiveProperty (0);
				instanse.ComboCount = new IntReactiveProperty (0);
				instanse._gameMode = GameModeEnum.Easy;
				instanse.duration = 2.0f;
				instanse.dropCityCount = 5;
				instanse.inPlay = false;
			}
			return instanse;
		}
	}


	public ReadOnlyReactiveProperty<int> CurrentComboCount {
		get {
			return ComboCount.ToReadOnlyReactiveProperty ();
		}
	}

	//	public ReadOnlyReactiveProperty<int> CurrentScore {
	//		get {
	//			return Score.ToReadOnlyReactiveProperty();
	//		}
	//	}

	/// <summary>
	/// Conversion from the score in percent
	/// </summary>
	/// <value>The current score per.</value>
	public ReadOnlyReactiveProperty<float> CurrentScorePer {
		get {
			return Score.Select (score => ScoreToRankPer (score)).ToReadOnlyReactiveProperty ();
		}
	}

	/// <summary>
	/// To initialize a variable of GameManager
	/// </summary>
	public void initialize ()
	{
		instanse.Score = new IntReactiveProperty (0);
		instanse.duration = 2.0f;
		instanse.dropCityCount = 5;
		instanse.ClearComboCount ();
		instanse.inPlay = false;
		useCapitals = false;
		mixCapitalAndCity = false;
	}

	/// <summary>
	/// Game mode(学生:Easy、老師:Normal、神仙、Hard(Unimplemented))
	/// </summary>
	/// <param name="mode">Mode.</param>
	public void SetGameMode (GameModeEnum mode)
	{
		_gameMode = mode;
		// initialize
		useCapitals = false;
		mixCapitalAndCity = false;
		// モードごとの初期化
		switch (mode) {
		case GameModeEnum.Beginner:
			duration = 3.5f;
			dropCityCount = 3;
			modeScores = BeginnerModeScores;
			break;
		case GameModeEnum.Easy:
			duration = 2.8f;
			dropCityCount = 5;
			modeScores = EasyModeScores;
			break;
		case GameModeEnum.EasyHigh:
			duration = 2.8f;
			dropCityCount = 5;
			useCapitals = true;
			modeScores = EasyModeScores;
			break;
		case GameModeEnum.Normal:
			duration = 2.0f;
			dropCityCount = 7;
			mixCapitalAndCity = true;
			modeScores = NormalModeScores;
			break;
		case GameModeEnum.NormalHigh:
			duration = 2.0f;
			dropCityCount = 7;
			mixCapitalAndCity = true;
			modeScores = NormalModeScores;
			break;
		case GameModeEnum.Hard:
			duration = 1.0f;
			dropCityCount = 10;
			modeScores = NormalModeScores; // TODO:I want to implement a number of hard mode
			break;
		default:
			duration = 2.0f;
			dropCityCount = 2;
			modeScores = EasyModeScores;
			break;
		}
	}

	/// <summary>
	/// Set whether the flag is in the game
	/// </summary>
	/// <param name="mode">If set to <c>true</c> mode.</param>
	public void SetInPlay (bool mode)
	{
		inPlay = mode;
	}

	/// <summary>
	/// Set the state at the time the game is over
	/// </summary>
	public void GameOver ()
	{
		inPlay = false;
	}

	/// <summary>
	/// Return now game mode
	/// </summary>
	/// <returns>The mode name.</returns>
	public string GetModeName ()
	{

		switch (_gameMode) {
		case GameModeEnum.Beginner:
			return "入門";
		case GameModeEnum.Easy:
			return "学生";
		case GameModeEnum.EasyHigh:
			return "学生(中级)";
		case GameModeEnum.Normal:
			return "老師";
		case GameModeEnum.NormalHigh:
			return "老師(中级)";
		case GameModeEnum.Hard:
			return "神仙";
		default:
			return "????";
		}
	}

	/// <summary>
	/// Return grade name
	/// </summary>
	/// <returns>The grade name.</returns>
	public string GetGradeName ()
	{
		int rankIndex = GetIndexOfModeScore (this.Score.Value);
		if (_gameMode == GameModeEnum.Beginner) {
			return GetBeginnerGradeName (rankIndex);
		} else if (_gameMode == GameModeEnum.Easy || _gameMode == GameModeEnum.EasyHigh) {
			return GetEasyGradeName (rankIndex);
		} else if (_gameMode == GameModeEnum.Normal || _gameMode == GameModeEnum.NormalHigh) {
			return GetNormalGradeName (rankIndex);
		}

		// Return the ???? other than student and teacher mode for now
		return "????";
	}

	/// <summary>
	/// Return the grade name of the introductory mode
	/// </summary>
	/// <returns>The beginner grade name.</returns>
	/// <param name="rankIndex">Rank index.</param>
	string GetBeginnerGradeName (int rankIndex)
	{

		if (rankIndex == 0) {
			return "一级";
		} else if (rankIndex == 1) {
			return "二级";
		} else if (rankIndex >= 2) {
			return "合格!";
		}

		return "????";
	}


	/// <summary>
	/// Return the grade name of the student mode
	/// </summary>
	/// <returns>The easy grade name.</returns>
	/// <param name="rankIndex">Rank index.</param>
	string GetEasyGradeName (int rankIndex)
	{

		if (rankIndex == 0) {
			return "一级";
		} else if (rankIndex == 1) {
			return "二级";
		} else if (rankIndex == 2) {
			return "三级";
		} else if (rankIndex == 3) {
			return "四级";
		} else if (rankIndex == 4) {
			return "五级";
		} else if (rankIndex >= 5) {
			return "六级";
		}

		return "????";
	}

	/// <summary>
	/// Return the grade name of teacher mode
	/// </summary>
	/// <returns>The normal grade name.</returns>
	/// <param name="rankIndex">Rank index.</param>
	string GetNormalGradeName (int rankIndex)
	{
		if (rankIndex == 0) {
			return "一级";
		} else if (rankIndex == 1) {
			return "二级";
		} else if (rankIndex == 2) {
			return "三级";
		} else if (rankIndex == 3) {
			return "四级";
		} else if (rankIndex == 4) {
			return "五级";
		} else if (rankIndex == 5) {
			return "六级";
		} else if (rankIndex == 6) {
			return "将星级";
		} else if (rankIndex >= 7) {
			return "探花";
		}

		return "????";
	}

	/// <summary>
	/// Get a percentage of the progress of the current rank in the 0.0 to 1.0
	/// </summary>
	/// <returns>The to rank per.</returns>
	/// <param name="score">Score.</param>
	public float ScoreToRankPer (int score)
	{
		// Set the upper limit and the lower limit score of the current rank
		int rankIndex = GetIndexOfModeScore (score);
		//Debug.Log ("rankIndex = " + rankIndex);
		// Set the lower limit
		int scoreLowLimit = 0;
		// Set the lower limit unless the lowest rank
		if (rankIndex > 0) {
			scoreLowLimit = modeScores [rankIndex - 1];
		}

		// Set the upper limit
		int scoreHighLimit = modeScores [rankIndex];
		// Setting an upper limit unless reached the upper limit
		if (rankIndex < (modeScores.Length - 1)) {
			scoreHighLimit = modeScores [rankIndex];
		}

		// upper limit - lower limit = NextRankScore
		int nextRankScore = scoreHighLimit - scoreLowLimit;
		// now score - lower limit = NowScoreInRank
		int nowScoreInRank = Score.Value;
		if (rankIndex > 0) {
			nowScoreInRank = Score.Value - scoreLowLimit;
		}

		return (float)nowScoreInRank / (float)nextRankScore;
	}

	/// <summary>
	/// Returns the current rank in the integer starting from 0 from points
	/// </summary>
	/// <returns>0から始まる整数</returns>
	/// <param name="score">点数</param>
	int GetIndexOfModeScore (int score)
	{
		for (int index = 0; index < modeScores.Length; index++) {
			if (score < modeScores [index]) {
				return index;
			}
		}

		return modeScores.Length - 1;
	}

	/// <summary>
	/// Make a list of the Index of the ball of the city to be displayed in the game
	/// </summary>
	public void CreateInPlayCity ()
	{
		inPlayCityNums = new List<int> ();
		int nowAddedNum = 0;
		while (nowAddedNum < GameManager.Instanse.dropCityCount) {
			int cityIndex = Random.Range (0, CityGenerator.Instance.GetList ().Count);
			if (!inPlayCityNums.Contains (cityIndex)) {
				//Debug.Log ("createInPlayCity index = " + cityIndex);
				inPlayCityNums.Add (cityIndex);
				nowAddedNum++;
			}
		}
	}



	/// <summary>
	/// For debug. It specifies the index of the list of cities in the manual
	/// </summary>
	/// <param name="list">List.</param>
	public void CreateInPlayCityByIndex (List<int> list)
	{
		inPlayCityNums = list;
	}

	/// <summary>
	/// Return index of the city (the number) to random
	/// </summary>
	/// <returns>The rondom city number.</returns>
	public int GetRondomCityNum ()
	{
		int randomNum = Random.Range (0, GameManager.Instanse.dropCityCount);
		return inPlayCityNums [randomNum];
	}

	/// <summary>
	/// It returns a list of cities that have been used in the game
	/// </summary>
	/// <returns>The city list in maingame.</returns>
	public List<City> GetCityListInMaingame ()
	{
		List<City> list = new List<City> ();
		foreach (int index in inPlayCityNums) {
			list.Add (CityGenerator.Instance.GetCityByIndex (index));
		}

		return list;
	}

	/// <summary>
	/// Adding a combo number
	/// </summary>
	public void IncrementComboCount ()
	{
		ComboCount.Value++;
	}

	/// <summary>
	/// Clear combo number
	/// </summary>
	public void ClearComboCount ()
	{
		ComboCount.Value = 0;
	}

	/// <summary>
	/// Adding the score
	/// </summary>
	public void AddScore ()
	{
		Score.Value += GetPoint ();
	}
		
	/// <summary>
	/// Clear points
	/// </summary>
	public void ClearScore ()
	{
		Score.Value = 0;
	}

	/// <summary>
	/// It returns a summing point in consideration of the magnification
	/// </summary>
	/// <returns>The point.</returns>
	int GetPoint ()
	{
		float addingPoint = 0.0f;
		if (ComboCount.Value > 0 && ComboCount.Value <= ComboPow.Length) {
			// Calculation to get the magnification from the combo number
			addingPoint = originalPoint * ComboPow [ComboCount.Value - 1]; 
		} else if (ComboCount.Value > ComboPow.Length) {
			// 最大の倍率で計算
			addingPoint = originalPoint * ComboPow [ComboPow.Length - 1];
		} else {
			addingPoint = originalPoint;
		}

		return (int)addingPoint;  
	}
}
