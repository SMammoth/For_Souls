using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Collections;

public abstract class CharacterControllerBase : MonoBehaviour
{
	public int playerId;
	private Player player;

	public Sprite ClockPickup;
    bool bombExploding = false;

	[Header("Speed and acceleration")]
	public float accel;
	public int maxspeed;
	int Base_maxspeed;

	[Header("Jumping")]
	public float Jumpforce;
	public int JumpAdjust;
	public bool grounded = false;
	bool landed = false;
	public Transform groundCheck;
	public float groundRadius;
	float landRadius = .3f;
	public LayerMask whatIsGround;
    public float trampolineForce;

	[Header("Sliding")]
	public int SlideAdjust;

	public Transform SlideCheck;
	bool slidingUnder = false;
	float slideRadius = .2f;

	[Header("Sounds")]
	public AudioClip Jump;
	public AudioClip Scream;
	public AudioClip Slide;
	public AudioClip Coin;
	public AudioClip Clock;

	bool Sliding;
	float SlidingTimer;

	int RandomLevelNR;

	[Header("Input")]
	public string InputWalk; 	//Horizontal
	public string InputJump; 	//Jump
	public string InputSlide; 	//slide

	GameObject[] Players;
	Rigidbody2D MyBody2D;

	public static bool Controllable;

	CircleCollider2D PlayerCircleCollider;
	PolygonCollider2D PlayerBoxCollider;

	Animator PlayerAnimator;

	Vector2 StartPosition;

	bool Upgrade;


	void awake ()
	{

	}

	// Use this for initialization
	public virtual void Start () 
	{
		player = ReInput.players.GetPlayer(playerId);

		StartPosition = transform.position;
		Base_maxspeed = maxspeed;

		Upgrade = false;

		Sliding = false;

		Controllable = true;

		MyBody2D = gameObject.GetComponent<Rigidbody2D> ();

		PlayerCircleCollider = GetComponent<CircleCollider2D> ();
		PlayerBoxCollider = GetComponent<PolygonCollider2D> ();

		PlayerAnimator = GetComponent<Animator> ();

		//no collision with the other players
		Players = GameObject.FindGameObjectsWithTag ("Player");
		foreach(GameObject Player in Players)
		{
			Physics2D.IgnoreCollision (gameObject.GetComponent<PolygonCollider2D>(), Player.GetComponent<CircleCollider2D>(), true);
			Physics2D.IgnoreCollision (gameObject.GetComponent<CircleCollider2D>(), Player.GetComponent<CircleCollider2D>(), true);

			Physics2D.IgnoreCollision (gameObject.GetComponent<PolygonCollider2D>(), Player.GetComponent<PolygonCollider2D>(), true);
			Physics2D.IgnoreCollision (gameObject.GetComponent<CircleCollider2D>(), Player.GetComponent<PolygonCollider2D>(), true);
		}
		
		PlayerCircleCollider.enabled = true;
		PlayerBoxCollider.enabled = false;
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        landed = Physics2D.OverlapCircle(groundCheck.position, landRadius, whatIsGround);
        slidingUnder = Physics2D.OverlapCircle(SlideCheck.position, slideRadius, whatIsGround);

        if (!bombExploding)
        {
            //spawn landing particle
            if (landed && MyBody2D.velocity.y < -.04f)
            {
                this.transform.FindChild("Particle_Land").GetComponent<ParticleSystem>().Emit(1);
            }


            if (Controllable) //if controllable
            {
                //turn on animator and physics 
                PlayerAnimator.speed *= Time.timeScale;
                MyBody2D.isKinematic = false;

                if (player.GetAxis("Move Horizontal") < -.9f || player.GetNegativeButton("Move Horizontal"))
                {
                    if (MyBody2D.velocity.x > -maxspeed)
                    {
                        //unparent when parented to a moving platform
                        if (transform.parent != null)
                        {
                            transform.parent = null;
                        }

                        transform.localScale = new Vector3(-10, 10, 1);


                        MyBody2D.velocity = new Vector2(-accel * maxspeed, MyBody2D.velocity.y);
                    }
                }
                else if (player.GetAxis("Move Horizontal") > .9f || player.GetButton("Move Horizontal"))
                {
                    if (MyBody2D.velocity.x < maxspeed)
                    {
                        //unparent when parented to a moving platform
                        if (transform.parent != null)
                        {
                            transform.parent = null;
                        }

                        transform.localScale = new Vector3(10, 10, 1);

                        MyBody2D.velocity = new Vector2(accel * maxspeed, MyBody2D.velocity.y);
                    }
                }

                //JUMP
				if (!slidingUnder && grounded && player.GetButtonDown("Jump"))
				{
                    //play animation jump
                    PlayerAnimator.SetTrigger("Jump");
                    //play Particle
                    this.transform.FindChild("Particle_Jump").GetComponent<ParticleSystem>().Emit(1);

                    //Play Sound
                    GetComponent<AudioSource>().clip = Jump;
                    GetComponent<AudioSource>().Play();
                   
                    //add force for jump
                    MyBody2D.velocity = new Vector2(MyBody2D.velocity.x, 0);
                    MyBody2D.AddForce(new Vector2(0, Jumpforce));

                    maxspeed = JumpAdjust;

                    //set hitboxes to normal
                    PlayerCircleCollider.enabled = true;
                    PlayerBoxCollider.enabled = false;

                    SlidingTimer = 0;
                    Sliding = false;
                }

				if(player.GetButtonUp("Jump"))
				{
					if(MyBody2D.velocity.y > 0)
					{
						float bla = MyBody2D.velocity.y;
						bla = bla / 1.5f;

						MyBody2D.velocity = new Vector2(MyBody2D.velocity.x, bla);
					}
				}

                //SLIDE
                if (!Sliding && grounded && player.GetButtonDown("Duck"))
                {
                    //play animation slide start
                    PlayerAnimator.SetTrigger("StartSlide");
                    //Play Sound
                    GetComponent<AudioSource>().clip = Slide;
                    GetComponent<AudioSource>().Play();

                    //set hitboxes to sliding
                    PlayerCircleCollider.enabled = false;
                    PlayerBoxCollider.enabled = true;

                    Sliding = true;
                }

                //if sliding, start timer
                if (Sliding)
                {
                    SlidingTimer += Time.deltaTime;

                    maxspeed = SlideAdjust;
                }

                if (grounded && !Sliding)
                {
                    maxspeed = Base_maxspeed;
                }

                //when sliding and sliding timer is still going and not under something
                if (Sliding && SlidingTimer > 1 && !slidingUnder)
                {
                    PlayerAnimator.SetTrigger("EndSlide");

                    //set hitboxes to normal
                    PlayerCircleCollider.enabled = true;
                    PlayerBoxCollider.enabled = false;

                    SlidingTimer = 0;
                    Sliding = false;
                }
            }
            else //if not controllable
            {
                //turn off animator and physics
                //PlayerAnimator.speed = 0;
                MyBody2D.isKinematic = true;
            }

            if (GUIGameScript.GameOver == false)
            {
                if (player.GetButtonDown("Start"))
                {
                    Camera.main.GetComponent<GUIGameScript>().PauzeButton();
                }
            }

            if (MyBody2D.velocity.x < .1f && MyBody2D.velocity.x > -.1f)
            {
                PlayerAnimator.SetBool("Idle", true);
            }
            else
            {
                PlayerAnimator.SetBool("Idle", false);
            }
        }
    }

	public virtual void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.tag == "EndLevel" && gameObject.tag == "Player")
		{
			GameObject.Find("FadeOut").GetComponent<Animator>().SetTrigger("fade");

			Analyse_EndLevel_CompletionTime(GameObject.FindWithTag("Level").name, GUIGameScript.Timer);
			Analyse_EndLevel_Upgrade(GameObject.FindWithTag("Level").name, Upgrade);
			Analyse_EndLevel_Completed(GameObject.FindWithTag("Level").name, 100);

			if(gameObject.name == "Player1")
			{
				GUIGameScript.Score_Player1 += 1;
			}
			else if(gameObject.name == "Player2")
			{
				GUIGameScript.Score_Player2 += 1;
			}
			else if(gameObject.name == "Player3")
			{
				GUIGameScript.Score_Player3 += 1;
			}
			else if(gameObject.name == "Player4")
			{
				GUIGameScript.Score_Player4 += 1;
			}

			Destroy(coll.gameObject);

			if(Upgrade)
			{
				GUIGameScript.Timer = 12;
			}
			else 
			{
				GUIGameScript.Timer = 10;
			}

			Application.LoadLevel(Application.loadedLevel);
		}	

		if (coll.tag == "Upgrade")
		{
			if(GameObject.FindWithTag ("UI_Multiplayer") != null)
			{
				if(gameObject.name == "Player1")
				{
					GUIGameScript.Score_Player1 += 1;
				}
				else if(gameObject.name == "Player2")
				{
					GUIGameScript.Score_Player2 += 1;
				}
				else if(gameObject.name == "Player3")
				{
					GUIGameScript.Score_Player3 += 1;
				}
				else if(gameObject.name == "Player4")
				{
					GUIGameScript.Score_Player4 += 1;
				}
			}
			else
			{
				Upgrade = true;
				GameObject.FindWithTag("UI_Image_Upgrade").GetComponent<Image>().sprite = ClockPickup;
			}

			Destroy(coll.gameObject);
		}

		if(coll.tag == "Coin")
		{
			GetComponent<AudioSource>().clip = Coin;
			GetComponent<AudioSource>().Play();

			Destroy(coll.gameObject);
		}

		if (coll.tag == "Spike")
		{
			Camera.main.GetComponent<CamShakeSimple>().Shake();
			Object Death_Part1 = Instantiate (Resources.Load ("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
			Object Death_Part_Skull = Instantiate (Resources.Load ("Prefabs/Particles/Particle_Skull", typeof(GameObject)), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 3), Quaternion.identity);


			GetComponent<AudioSource>().clip = Scream;
			GetComponent<AudioSource>().Play();

			//Back to the start of the level!
			transform.position = StartPosition;
			Object Death_Part2 = Instantiate (Resources.Load ("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
		}
	
	}

	public virtual void Analyse_EndLevel_Completed(string LevelName, float Completed)
	{
		GA.API.Design.NewEvent ("Level:Completion:" + LevelName, Completed); 
	}
	public virtual void Analyse_EndLevel_CompletionTime(string LevelName, float Timer)
	{
		GA.API.Design.NewEvent ("Level:Completion:Time:" + LevelName, 10 - Timer); 
	}
	public virtual void Analyse_EndLevel_Upgrade(string LevelName, bool Pickup)
	{
		if(Pickup)
		{
			GA.API.Design.NewEvent ("Level:Upgrade:" + LevelName, 100); 
		}
		else
		{
			GA.API.Design.NewEvent ("Level:Upgrade:" + LevelName, 0); 
		}

	}

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        //Throwback if bomb explodes
        if (coll.transform.tag == "Bomb")
        {
            bombExploding = true;
            StartCoroutine(WaitForSeconds(1));
        }

        //Add force upwards if platform is a trampoline
        if (coll.transform.tag == "TrampolinePlatform")
        {
            MyBody2D.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, trampolineForce));
        }

        //Death if hit by bullet
        if (coll.transform.tag == "Bullet")
        {
            Camera.main.GetComponent<CamShakeSimple>().Shake();
            Object Death_Part1 = Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
            Object Death_Part_Skull = Instantiate(Resources.Load("Prefabs/Particles/Particle_Skull", typeof(GameObject)), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 3), Quaternion.identity);

            GetComponent<AudioSource>().clip = Scream;
            GetComponent<AudioSource>().Play();

            //Back to the start of the level!
            transform.position = StartPosition;
            Object Death_Part2 = Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);

            Destroy(coll.gameObject);
        }
    }

	public virtual void OnCollisionStay2D (Collision2D coll) 
	{
		//parent player to moving platform when colliding with one
		//makes sure the player moves with the platform
		if(coll.transform.tag == "MovingPlatform")
		{
			transform.parent = coll.transform;
		}
		else
		{
			transform.parent = null;
		}

        //Speed up if platform is a speedy platform
        if (coll.transform.tag == "SpeedyPlatform")
        {
            maxspeed *= 2;
        }
	}

    IEnumerator WaitForSeconds(float resetTimer)
    {
        yield return new WaitForSeconds(resetTimer);
        bombExploding = false;
    }
}
