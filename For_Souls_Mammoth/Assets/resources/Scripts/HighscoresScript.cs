using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class HighscoresScript : MonoBehaviour 
{
	const string privateCode = "8d8uW9_Gj0KXDe2fXLWngQkwfTuY3JLUS7cu5Bf0Qqpw";
	const string publicCode = "54afb7fd6e51b615d8cfaf09";
	const string webURL = "http://dreamlo.com/lb/";

	GameObject UI_Text_Highscore;
	GameObject UI_Highscores;

    GameObject startMenu;

	public Highscore[] highscoresList;

	void Awake ()
	{
		DownloadHighscores ();
	}

	void Start()
	{
		if(Application.loadedLevelName == "MenuScene")
		{
			UI_Text_Highscore = GameObject.FindWithTag ("UI_Text_Highscore");

			UI_Highscores = GameObject.FindWithTag ("UI_Highscores");
			UI_Highscores.SetActive (false);

            startMenu = GameObject.Find("Panel_Start_Menu").gameObject;
		}
	}

	//adding new highscores
	public void AddNewHighscore (string username, int score)
	{
		StartCoroutine (UploadNewHighscore (username, score));
	}

	IEnumerator UploadNewHighscore(string username, int score)
	{
		WWW www = new WWW (webURL + privateCode + "/add/" + WWW.EscapeURL (username) + "/" + score);
		yield return www;

		if(string.IsNullOrEmpty(www.error))
		{
			print ("Upload Succesful");
		}
		else
		{
			print ("Error Uploading: " + www.error);
		}
	}

	//downloading highscores
	public void DownloadHighscores()
	{
		StartCoroutine (DownloadHighscoresFromDatabase ());
	}

	IEnumerator DownloadHighscoresFromDatabase()
	{
		WWW www = new WWW (webURL + publicCode + "/pipe/");
		yield return www;
		
		if(string.IsNullOrEmpty(www.error))
		{
			FormatHighscores(www.text);
		}
		else
		{
			print ("Error Downloading: " + www.error);
		}
	}


	void FormatHighscores(string textStream)
	{
		string[] entries = textStream.Split (new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		highscoresList = new Highscore[entries.Length];

		for(int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split (new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username, score);

			//print (highscoresList[i].username + ": " + highscoresList[i].score);
		}
		if(UI_Text_Highscore != null)
		{
			UI_Text_Highscore.GetComponent<Text> ().text = 	"1. " + highscoresList [0].username + " " + highscoresList [0].score + "\n" +
															"2. " + highscoresList [1].username + " " + highscoresList [1].score + "\n" +
															"3. " + highscoresList [2].username + " " + highscoresList [2].score + "\n" +
															"4. " + highscoresList [3].username + " " + highscoresList [3].score + "\n" +
															"5. " + highscoresList [4].username + " " + highscoresList [4].score;				
		}
	}

	public void ShowHighscores()
	{
		if(UI_Highscores.activeSelf == true)
		{
			UI_Highscores.SetActive(false);
		}
		else
		{
            startMenu.SetActive(false);
			UI_Highscores.SetActive(true);
			GameObject.FindWithTag("UIManager").GetComponent<HighscoresScript>().DownloadHighscores();
            EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Back_HighScore"));
		}
	}
}

public struct Highscore
{
	public string username;
	public int score;

	public Highscore(string _username, int _score)
	{
		username = _username;
		score = _score;
	}
}
