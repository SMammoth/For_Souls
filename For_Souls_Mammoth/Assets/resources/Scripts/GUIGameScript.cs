using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using System.Collections;

public class GUIGameScript : MonoBehaviour 
{
	GameObject UI_GameOver;
	GameObject UI_GameOver_Home;
	GameObject UI_Pauze;
	GameObject UI_Pauze_Continue;
	GameObject UI_Text_Timer;
	GameObject UI_Text_Score;
	GameObject UI_Singleplayer;
	GameObject UI_Multiplayer;

	GameObject UI_Text_YourScore_1;
	GameObject UI_Text_YourScore_2;
	GameObject UI_Text_YourScore_3;
	GameObject UI_Text_YourScore_4;
	GameObject UI_Text_TotalScore;

	GameObject UI_InputField_PlayerName;
	GameObject UI_Button_SubmitScore;
	GameObject UI_Text_YourScore;

	public GameObject defaultButton;

	GameObject MusicObject;

	//loading levels
	public static int TotalLevels;

	int CurrentLevel_Max;
	public Sprite[] Backgrounds;

	Object CurrentLevel;
	int RandomLevelNR;
	public static int LastLevelNR;

	//loading Player(s)
	public static string Char_Player1;
	public static string Char_Player2;
	public static string Char_Player3;
	public static string Char_Player4;

	public static int Score_Player1;
	public static int Score_Player2;
	public static int Score_Player3;
	public static int Score_Player4;
	public static int TotalScore;
    public float HighScore;

	public string InputCharacters;
	public int SelectedChar;

	public string PlayerName;
	public int SelectedPlayerChar;

	public AudioClip Death;
	public AudioClip NewScore;

	public AudioClip Music1;
	public AudioClip Music2;
	public AudioClip Music3;
	public AudioClip Music4;
	public AudioClip Music5;
		
	public static bool Pauze;

	public static bool GameOver;

	public static float Timer;

	void Awake ()
	{
		TotalScore = Score_Player1 + Score_Player2 + Score_Player3 + Score_Player4;

		if(UIManagerScript.Tutorial == true)
		{
			if(TotalScore < 3)
			{
				CurrentLevel = Instantiate (Resources.Load ("Prefabs/TutorialLevels/Tutorial_" + TotalScore, typeof(GameObject)), new Vector2 (0, 0), Quaternion.identity);
			}
			else
			{
				GenerateLevel();
			}
		}
		else
		{
			GenerateLevel ();
		}

						

		
		MusicObject = GameObject.FindWithTag ("Music");

		if(TotalScore == 5)
		{
			MusicObject.GetComponent<AudioSource>().clip = Music2; MusicObject.GetComponent<AudioSource>().Play();
		}

		if(TotalScore == 15)
		{
			MusicObject.GetComponent<AudioSource>().clip = Music3; MusicObject.GetComponent<AudioSource>().Play();
		}

		if(TotalScore == 35)
		{
			MusicObject.GetComponent<AudioSource>().clip = Music4; MusicObject.GetComponent<AudioSource>().Play();
		}

		if(TotalScore == 50)
		{
			MusicObject.GetComponent<AudioSource>().clip = Music5; MusicObject.GetComponent<AudioSource>().Play();
		}
	}

	void Start () 
    {
		CreateCharacters ();

		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;

		UI_GameOver = GameObject.FindWithTag ("UI_GameOver");
		UI_GameOver_Home = GameObject.FindWithTag ("UI_GameOver_Home");

		UI_Text_YourScore = GameObject.FindWithTag ("UI_Text_YourScore");
		UI_InputField_PlayerName = GameObject.FindWithTag ("UI_InputField_PlayerName");

		//UI_Button_SubmitScore = GameObject.FindWithTag ("UI_Button_SubmitScore");
		//UI_Button_SubmitScore.SetActive (false);

		UI_Singleplayer = GameObject.FindWithTag ("UI_Singleplayer");
		UI_Multiplayer = GameObject.FindWithTag ("UI_Multiplayer");

		if (Char_Player2 == null && Char_Player3 == null && Char_Player4 == null)
		{
			UI_Singleplayer.SetActive(true);
			UI_Multiplayer.SetActive(false);
			GameObject.Find ("Image_Player1").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player1) as Sprite;
		}
		else
		{
			UI_Singleplayer.SetActive(false);
			UI_Multiplayer.SetActive(true);
			GameObject.Find ("Image_Player1").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player1) as Sprite;
		}


		UI_GameOver.SetActive (false);

		UI_Pauze = GameObject.FindWithTag ("UI_Pauze");
		UI_Pauze_Continue = GameObject.FindWithTag ("UI_Pauze_Continue");
		UI_Pauze.SetActive (false);

		UI_Text_Timer = GameObject.FindWithTag ("UI_Text_Timer");
		UI_Text_Score = GameObject.FindWithTag ("UI_Text_Score");

		UI_Text_YourScore_1 = GameObject.FindWithTag ("UI_Text_YourScore_1");
		UI_Text_YourScore_2 = GameObject.FindWithTag ("UI_Text_YourScore_2");
		UI_Text_YourScore_3 = GameObject.FindWithTag ("UI_Text_YourScore_3");
		UI_Text_YourScore_4 = GameObject.FindWithTag ("UI_Text_YourScore_4");
		UI_Text_TotalScore = GameObject.FindWithTag ("UI_Text_TotalScore");

		HighScore = PlayerPrefs.GetFloat("Highscore");

		InputCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		SelectedChar = 0;
				
		PlayerName = "___";
		SelectedPlayerChar = 0;

 	    Pauze = false;
		GameOver = false;
	}

	void Update () 
    {
		//decimals on the clock after 5 seconds
		if(Timer > 5)
		{
			UI_Text_Timer.GetComponent<Text> ().text = "" + (int)Timer;
		}
		else if(Timer > 0 && Timer <= 5)
		{
			UI_Text_Timer.GetComponent<Text> ().text = "" + System.Math.Round(Timer, 2);
         //   InvokeRepeating("ScoreScale", .1f, .3f);
		}

		if(UI_Multiplayer.activeSelf == true)
		{
			TotalScore = Score_Player1 + Score_Player2 + Score_Player3 + Score_Player4;

			UI_Text_YourScore_1.GetComponent<Text>().text = "" + Score_Player1;
			UI_Text_YourScore_2.GetComponent<Text>().text = "" + Score_Player2;
			UI_Text_YourScore_3.GetComponent<Text>().text = "" + Score_Player3;
			UI_Text_YourScore_4.GetComponent<Text>().text = "" + Score_Player4;
			UI_Text_TotalScore.GetComponent<Text>().text = "" + TotalScore;
		}
		else
		{
			UI_Text_Score.GetComponent<Text> ().text = "" + Mathf.RoundToInt(Score_Player1);
		}

		if(Timer > 0)
		{
			Timer -= Time.deltaTime;
		}
		else if(Timer <= 0)
		{
			if(MusicObject)
			{
            	MusicObject.GetComponent<AudioSource>().mute = true;
			}

			Timer = 0;
			GameOver = true;
		}

		//start ticking during the last 5 seconds
		if(Mathf.RoundToInt(Timer) == 4 && GameObject.FindWithTag("Music_Clock").GetComponent<AudioSource>().isPlaying == false)
		{
			GameObject.FindWithTag("Music_Clock").GetComponent<AudioSource>().Play();
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PauzeButton();
		}

		if(GameOver)
		{
			CharacterControllerBase.Controllable = false;
			Time.timeScale = 0;

			if(UI_GameOver.activeSelf == false)
			{
				Analyse_EndLevel_Completed(GameObject.FindWithTag("Level").name, 0);
				
				UI_GameOver.GetComponent<AudioSource>().clip = Death;
				UI_GameOver.GetComponent<AudioSource>().playOnAwake = true;

				if(Score_Player1 > PlayerPrefs.GetInt("Highscore"))
				{
					PlayerPrefs.SetInt("Highscore", Score_Player1);
				}

				EventSystem.current.SetSelectedGameObject(UI_GameOver_Home);


				UI_GameOver.SetActive (true);
			}

			UI_Text_YourScore.GetComponent<Text>().text = "Your Score\n" + Score_Player1;
		}

		if(Pauze)
		{
			GameObject.FindWithTag("Music_Clock").GetComponent<AudioSource>().Pause ();
		}
	}

	public void Analyse_EndLevel_Completed(string LevelName, float Completed)
	{
		GA.API.Design.NewEvent ("Level:Completion:" + LevelName, Completed); 
	}

	void CreateCharacters()
	{
		GameObject Player = Instantiate (Resources.Load ("Prefabs/Characters/" + Char_Player1, typeof(GameObject)), GameObject.FindWithTag("SpawnPoint").transform.localPosition, Quaternion.identity) as GameObject;
		Player.name = "Player1";
		Player.GetComponent<CharacterControllerBase>().playerId = 0;
		GameObject.Find ("Image_Player1").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player1) as Sprite;
		
		if (Char_Player2 != "" && Char_Player2 != null)
		{
			GameObject Player2 = Instantiate (Resources.Load ("Prefabs/Characters/" + Char_Player2, typeof(GameObject)), GameObject.FindWithTag("SpawnPoint").transform.position, Quaternion.identity) as GameObject;
			Player2.name = "Player2";
			Player2.GetComponent<CharacterControllerBase>().playerId = 1;
			GameObject.Find ("Image_Player2").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player2) as Sprite;
		}
		if (Char_Player3 != "" && Char_Player3 != null)
		{
			GameObject Player3 = Instantiate (Resources.Load ("Prefabs/Characters/" + Char_Player3, typeof(GameObject)), GameObject.FindWithTag("SpawnPoint").transform.position, Quaternion.identity) as GameObject;
			Player3.name = "Player3";
			Player3.GetComponent<CharacterControllerBase>().playerId = 2;
			GameObject.Find ("Image_Player3").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player3) as Sprite;
		}
		if (Char_Player4 != "" && Char_Player4 != null)
		{
			GameObject Player4 = Instantiate (Resources.Load ("Prefabs/Characters/" + Char_Player4, typeof(GameObject)), GameObject.FindWithTag("SpawnPoint").transform.position, Quaternion.identity) as GameObject;
			Player4.name = "Player4";
			Player4.GetComponent<CharacterControllerBase>().playerId = 3;
			GameObject.Find ("Image_Player4").transform.FindChild("Image_Head").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/Characters/Character_" + Char_Player4) as Sprite;
		}
	}

	void GenerateLevel()
	{
		if(UIManagerScript.Tutorial)
		{
			CurrentLevel_Max = 5 + (((TotalScore - 3) / 5) * 5); 
		}
		else
		{
			CurrentLevel_Max = 5 + ((TotalScore / 5) * 5); 
		}


		if(CurrentLevel_Max > TotalLevels)
		{
			CurrentLevel_Max = TotalLevels;
		}

		//generate new random level
		RandomLevelNR = Random.Range(1, CurrentLevel_Max + 1);
		//RandomLevelNR = Random.Range(14, 15);
		
		if(Char_Player2 == "" && Char_Player3 == "" && Char_Player4 == "")
		{
			do
			{
				RandomLevelNR = Random.Range(1, CurrentLevel_Max + 1);
				//RandomLevelNR = Random.Range(10, 15);
				
			}while(LastLevelNR == RandomLevelNR);
		}
		else
		{
			do
			{
				RandomLevelNR = Random.Range(1, CurrentLevel_Max + 1);
				//RandomLevelNR = Random.Range(10, 15);
				
			}while(LastLevelNR == RandomLevelNR || RandomLevelNR == 13);
		}
		
		
		CurrentLevel = Instantiate (UIManagerScript.Levels[RandomLevelNR - 1], new Vector2 (0, 0), Quaternion.identity);

		CurrentLevel.name = "Level" + RandomLevelNR;
		
		LastLevelNR = RandomLevelNR;
	}

    public void Restart() 
    {
        Timer = 10;
        Score_Player1 = 0;
        Score_Player2 = 0;
        Score_Player3 = 0;
        Score_Player4 = 0;

        MusicObject.GetComponent<AudioSource>().clip = Music1; 
        MusicObject.GetComponent<AudioSource>().mute = false;
        MusicObject.GetComponent<AudioSource>().Play();

        Application.LoadLevel("GameScene");
    }

	public void ShowUploadButton(string Content)
	{
		print (Content);

		if(Content != null)
		{
			UI_Button_SubmitScore.SetActive(true);
		}
		else if(Content == null)
		{
			UI_Button_SubmitScore.SetActive(false);
		}
	}

	public void UploadHighscore()
	{
		Camera.main.GetComponent<HighscoresScript>().AddNewHighscore (PlayerName, Score_Player1);

		UI_InputField_PlayerName.GetComponent<InputField> ().interactable = false;

		Destroy(GameObject.FindWithTag("UI_Button_SubmitScore"));
	}

	public void LoadScene(string SceneName)
	{
		Score_Player1 = 0;
		Score_Player2 = 0;
		Score_Player3 = 0;
		Score_Player4 = 0;
		Destroy(MusicObject);
		Application.LoadLevel(SceneName);
	}

	public void PauzeButton()
	{
		if(!Pauze && !GameOver)
		{
			CharacterControllerBase.Controllable = false;

			Time.timeScale = 0;
			UI_Pauze.SetActive (true);

			EventSystem.current.SetSelectedGameObject(UI_Pauze_Continue);

			Pauze = true;
		}
		else if(Pauze && !GameOver)
		{
			CharacterControllerBase.Controllable = true;

			Time.timeScale = 1;
			UI_Pauze.SetActive (false);

			if(Mathf.RoundToInt(Timer) <= 4)
			{
				GameObject.FindWithTag("Music_Clock").GetComponent<AudioSource>().Play();
			}

			Pauze = false;
		}
	}
    void ScoreScale()
    {
        float randomScale = Random.Range(1.3f, 1.5f);
        UI_Text_Timer.transform.localScale = new Vector3(randomScale, randomScale, 0);
    }
}
