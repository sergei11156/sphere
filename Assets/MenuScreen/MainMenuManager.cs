//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager: MonoBehaviour
{
	internal LevelManager levelManager;
	internal Sprite notActive;
	internal Sprite active;
	//public Camera mainCamera;
	internal bool isFirstStart;
	internal Text CoinCount;
	internal int randomColor;
	public Color[] colors;

	internal int indexOfTrainSprite;
	internal bool newBest;
	//internal byte indexOfPlayingRoad;
	internal ParametersSaveLoad parametersSaveLoad;
	internal bool GPSAuthorization = false;
	Item activeItem;
	internal Item ActiveItem
	{
		get{
			return activeItem;
		}
		set
		{
			activeItem = value;
			active = activeItem.activate;
			notActive = activeItem.deActivate;
			indexOfTrainSprite = activeItem.itemIndex;
		}
	}
	void Start()
	{
		//PlayerPrefs.DeleteAll();
		if (PlayerPrefs.GetInt("isFirstStrat") == 0)
		{
			isFirstStart = true;
			PlayerPrefs.SetInt("isFirstStrat", 1);
		}
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene("MainMenu");
		parametersSaveLoad = new ParametersSaveLoad();
		randomColor = (int)(Random.Range(0, colors.Length - 0.1f));
	}
	internal void NewBackgroundColor()
	{
		if(randomColor + 1< colors.Length)
		{
			randomColor++;
		}
		else
		{
			randomColor = 0;
		}
	}
	/*internal void GPS1Authorization() {
		if (!GPSAuthorization)
			Social.localUser.Authenticate((bool sucsess) => { GPSAuthorization = sucsess; });
		Debug.Log("GPSAuthorization" + GPSAuthorization);
	}
	internal void GetAchive(string id, float progress) {
		Social.ReportProgress(id, progress, (bool achive) => { });
	}
	internal void Progress(string id, int score) {
		Social.ReportProgress(id, score, (bool success) => { });
	}*/
	internal void UpdateCoin()
	{
		CoinCount.text = parametersSaveLoad.CoinCount + "$";
	}
	internal void StartLevel()
	{
		SceneManager.LoadScene("GameScreen");
	}
}
