using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Rewired;
using System.Collections.Generic;

public class UIManagerScript : MonoBehaviour
{
    public GameObject defaultButton;
    public GameObject defaultQuitButton;

    GameObject UI_ConfirmQuit;
    GameObject UI_Toggle_Multiplayer, UI_Toggle_Tutorial;

    public static bool Mode_Multiplayer;

    public static bool Tutorial = true;

    public string[] Characters;
    int Player1 = 1;
    int Player2 = 0;
    int Player3 = 0;
    int Player4 = 0;

    Vector2 CurrentPlayer1Pos;
    Vector2 CurrentPos;

    private GameObject UI_Start_Menu, UI_Modussen;
    private Text UI_Select_Modus;

    public GameObject[] UI_Button_Player, playerJoins;
    private Player[] player = new Player[4]; // individual players
    private int[] playerID = { 0, 1, 2, 3 };

    private List<string> modussen = new List<string>(); //list of all the play modussen
    private int arrayModusNumber = 0;

    private bool chooseModus = false, selectGameModus = false, player1SelectCharacter = false, inCredits = false;

    private bool player2Joined = false, player3Joined = false, player4Joined = false; // can other players join

    private float creditsRollTimer = 0;

    GameObject UI_HighScore, UI_Button_Help_And_Options, UI_Credits, UI_Main_Developers, UI_OtherPeople;
    AudioSource music;
    public static GameObject[] Levels;

    void Awake()
    {

    }

    void Start()
    {
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        music.ignoreListenerVolume = true;

        Levels = Resources.LoadAll<GameObject>("Prefabs/Levels");
        GUIGameScript.TotalLevels = Levels.Length;

        Time.timeScale = 1;
        Time.fixedDeltaTime = .02f;

        UI_ConfirmQuit = GameObject.FindWithTag("UI_ConfirmQuit");
        UI_ConfirmQuit.SetActive(false);

        //UI_Toggle_Multiplayer = GameObject.FindWithTag ("UI_Toggle_Multiplayer");

        GUIGameScript.Char_Player1 = Characters[Player1];
        GUIGameScript.Char_Player2 = null;
        GUIGameScript.Char_Player3 = null;
        GUIGameScript.Char_Player4 = null;

        for (int i = 0; i < player.Length; i++)
        {
            player[i] = ReInput.players.GetPlayer(i);
        }

        UI_Start_Menu = GameObject.Find("Panel_Start_Menu").gameObject;
        UI_Toggle_Tutorial = GameObject.Find("UI_Toggle_Tutorial").gameObject;
        UI_Modussen = GameObject.Find("Modussen").gameObject;
        UI_Select_Modus = GameObject.Find("UI_Text_Modussen").GetComponent<Text>();
        UI_Modussen.SetActive(false);
        UI_HighScore = GameObject.Find("UI_Highscores").gameObject;
        UI_Button_Help_And_Options = GameObject.Find("Help_Option_Screen").gameObject;
        UI_Button_Help_And_Options.SetActive(false);
        UI_Main_Developers = GameObject.Find("Panel_Main Developers").gameObject;
        UI_OtherPeople = GameObject.Find("Panel_Other People").gameObject;
        UI_Credits = GameObject.Find("Credits_Screen").gameObject;
        UI_Credits.SetActive(false);
        

        modussen.Add("Arcade"); // add the names of the game modussen to the list
        modussen.Add("Story");
        modussen.Add("Time Trial");
        modussen.Add("Coin");
        modussen.Add("Co-Op"); // Normal multiplayer
        modussen.Add("Meppen");
        modussen.Add("Hard Core");
        modussen.Add("Hell");
        modussen.Add("Wipe-Out");
        modussen.Add("Catch The Door");
        modussen.Add("Down/Up");

        EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Start_Game"));
    }

    void Update()
    {
        GameObject.FindWithTag("UI_Text_PersonalHighscore").GetComponent<Text>().text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore");


        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.DeleteAll();
        }
        ChooseCharacters();
        if (player1SelectCharacter) { EventSystem.current.SetSelectedGameObject(UI_Button_Player[0]); }

        if (player2Joined) { GUIGameScript.Char_Player2 = Characters[Player2]; } else { GUIGameScript.Char_Player2 = null; }
        if (player3Joined) { GUIGameScript.Char_Player3 = Characters[Player3]; } else { GUIGameScript.Char_Player3 = null; }
        if (player4Joined) { GUIGameScript.Char_Player4 = Characters[Player4]; } else { GUIGameScript.Char_Player4 = null; }

        if (inCredits)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Back_Credits"));
            creditsRollTimer += Time.deltaTime;
            if (creditsRollTimer >= 10)
            {
                UI_Main_Developers.SetActive(false);
                UI_OtherPeople.SetActive(true);
            }
            if (creditsRollTimer >= 15)
            {
                UI_Main_Developers.SetActive(true);
                UI_OtherPeople.SetActive(false);
                creditsRollTimer = 0;
            }
        }
    }
    /// <summary>
    /// Changes the game mode to play
    /// </summary>
    public void ChangeModus()
    {
        arrayModusNumber++;
        if (arrayModusNumber >= modussen.Count) { arrayModusNumber = 0; }
        UI_Select_Modus.text = "Modussen: \n" + modussen[arrayModusNumber];
    }

    public void Multiplayer()
    {
        if (UI_Toggle_Multiplayer.GetComponent<Toggle>().isOn)
        {
            Mode_Multiplayer = true;
            UI_Toggle_Multiplayer.GetComponentInChildren<Text>().text = "MULTI PLAYER";
        }
        else
        {
            Mode_Multiplayer = false;
            UI_Toggle_Multiplayer.GetComponentInChildren<Text>().text = "SINGLE PLAYER";
        }
    }

    public void StartGame()
    {
        GUIGameScript.Timer = 10;

        Application.LoadLevel("GameScene");
    }

    public void QuitGame()
    {
        UI_ConfirmQuit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(defaultQuitButton);

      //  GameObject.Find("UI_Button_Play").GetComponent<Button>().interactable = false;
      //  GameObject.Find("UI_Button_Quit").GetComponent<Button>().interactable = false;
        //GameObject.Find("UI_Button_Leaderboard").GetComponent<Button>().interactable = false;
        UI_Start_Menu.SetActive(false);
    }

    public void ConfirmOrCancel(string ConfirmOrQuit)
    {
        if (ConfirmOrQuit == "Confirm")
        {
            Application.Quit();
        }
        else if (ConfirmOrQuit == "Cancel")
        {
            //GameObject.Find("UI_Button_Play").GetComponent<Button>().interactable = true;
          //  GameObject.Find("UI_Button_Quit").GetComponent<Button>().interactable = true;
          //  GameObject.Find("UI_Button_Leaderboard").GetComponent<Button>().interactable = true;

            UI_ConfirmQuit.SetActive(false);
            UI_Start_Menu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Start_Game"));
        }
    }

    public void CharacterSelect(int PlayerNR)
    {
        if (PlayerNR == 1)
        {
            Player1 += 1;
            if (Player1 >= Characters.Length)
            {
                Player1 = 1;
            }

            GUIGameScript.Char_Player1 = Characters[Player1];
            GameObject.Find("Player" + PlayerNR).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Characters/Character_" + Characters[Player1]);
        }
        else if (PlayerNR == 2)
        {
            Player2 += 1;

            if (Player2 >= Characters.Length)
            {
                Player2 = 0;
            }

            GUIGameScript.Char_Player2 = Characters[Player2];
            GameObject.Find("Player" + PlayerNR).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Characters/Character_" + Characters[Player2]);
        }
        else if (PlayerNR == 3)
        {
            Player3 += 1;
            if (Player3 >= Characters.Length)
            {
                Player3 = 0;
            }

            GUIGameScript.Char_Player3 = Characters[Player3];
            GameObject.Find("Player" + PlayerNR).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Characters/Character_" + Characters[Player3]);
        }
        else if (PlayerNR == 4)
        {
            Player4 += 1;
            if (Player4 >= Characters.Length)
            {
                Player4 = 0;
            }

            GUIGameScript.Char_Player4 = Characters[Player4];
            GameObject.Find("Player" + PlayerNR).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Characters/Character_" + Characters[Player4]);
        }
    }
    #region New Start Menu

    public void GoToModussen()
    {
        UI_Start_Menu.SetActive(false);
        UI_Modussen.SetActive(true);
        chooseModus = true;
        UI_Select_Modus.text = "Modussen: \n" + modussen[0];
        GameObject.Find("Player2").GetComponent<SpriteRenderer>().sprite = null;
        player2Joined = false;
        playerJoins[0].SetActive(true);
        GameObject.Find("Player3").GetComponent<SpriteRenderer>().sprite = null;
        player3Joined = false;
        playerJoins[1].SetActive(true);
        GameObject.Find("Player4").GetComponent<SpriteRenderer>().sprite = null;
        player4Joined = false;
        playerJoins[2].SetActive(true);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Modussen"));
    }
    public void BackToMenu()
    {
        UI_Start_Menu.SetActive(true);
        UI_Modussen.SetActive(false);
        chooseModus = false;
        UI_HighScore.SetActive(false);
        UI_Button_Help_And_Options.SetActive(false);
        UI_Credits.SetActive(false);
        inCredits = false;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Start_Game"));
    }

    public void HelpAndOptionMenu()
    {
        UI_Button_Help_And_Options.SetActive(true);
        UI_Start_Menu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("UI_Button_Controls").gameObject);
    }

    //Show image of the controls
    public void ControlsMenu(GameObject showControls)
    {
        showControls.SetActive(true);
    }

    private void ChooseCharacters()
    {
        if (chooseModus)
        {
            //todo idividual players can join the game and play multiplayer
            GameObject.Find("Player1").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Characters/Character_" + Characters[Player1]);

            #region Player Join
            foreach (Player p in player)
            {
                //player 2
                if (p.GetButtonDown("Jump") && p.id == 1 && !player2Joined)
                {
                    player2Joined = true;
                    playerJoins[0].SetActive(false);
                }
                else if (p.GetButtonDown("Duck") && p.id == 1)
                {
                    player2Joined = false;
                    GameObject.Find("Player2").GetComponent<SpriteRenderer>().sprite = null;
                    playerJoins[0].SetActive(true);
                }

                // player 3
                if (p.GetButtonDown("Jump") && p.id == 2 && !player3Joined)
                {
                    player3Joined = true;
                    playerJoins[1].SetActive(false);
                }
                else if (p.GetButtonDown("Duck") && p.id == 2)
                {
                    player3Joined = false;
                    GameObject.Find("Player3").GetComponent<SpriteRenderer>().sprite = null;
                    playerJoins[1].SetActive(true);
                }

                //player 4
                if (p.GetButtonDown("Jump") && p.id == 3 && !player4Joined)
                {
                    player4Joined = true;
                    playerJoins[2].SetActive(false);
                }
                else if (p.GetButtonDown("Duck") && p.id == 3)
                {
                    player4Joined = false;
                    GameObject.Find("Player4").GetComponent<SpriteRenderer>().sprite = null;
                    playerJoins[2].SetActive(true);
                }

                // Player 1 selection of a character
                if (EventSystem.current.currentSelectedGameObject == UI_Button_Player[0])
                {
                    if (p.GetButtonDown("Jump") && p.id == 0)
                    {
                        player1SelectCharacter = true;

                    }
                    else if (p.GetButtonDown("Duck") && p.id == 0)
                    {
                        player1SelectCharacter = false;
                    }
                }
                if (p.GetButtonDown("Move Horizontal") && p.id == 0 && player1SelectCharacter)
                {
                    CharacterSelect(1);
                }
                else if (p.GetNegativeButtonDown("Move Horizontal") && p.id == 0 && player1SelectCharacter)
                {
                    CharacterSelect(1);
                }

                // other players character selection
                if (p.GetButtonDown("Move Horizontal"))
                {
                    if (p.id == 1 && player2Joined)
                    {
                        CharacterSelect(2);
                    }
                    else if (p.id == 2 && player3Joined)
                    {
                        CharacterSelect(3);
                    }
                    else if (p.id == 3 && player4Joined)
                    {
                        CharacterSelect(4);
                    }
                }
                else if (p.GetNegativeButtonDown("Move Horizontal"))
                {
                    if (p.id == 1 && player2Joined)
                    {
                        CharacterSelect(2);
                    }
                    else if (p.id == 2 && player3Joined)
                    {
                        CharacterSelect(3);
                    }
                    else if (p.id == 3 && player4Joined)
                    {
                        CharacterSelect(4);
                    }
                }
            }
            #endregion
        }
    }
    public void AudioVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }

    public void CreditsButton()
    {
        creditsRollTimer = 0;
        inCredits = true;
        UI_Start_Menu.SetActive(false);
        UI_Credits.SetActive(true);
        UI_OtherPeople.SetActive(false);
        UI_Main_Developers.SetActive(true);
    }
    public void OnOffTutorial()
    {
        if (UI_Toggle_Tutorial.GetComponent<Toggle>().isOn)
        {
            Tutorial = true;
        }
        else
        {
            Tutorial = false;
        }
    }

    #endregion
}
