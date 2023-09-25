using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager: MonoBehaviour
{

	struct CollisionEvent
	{
		internal Train train;
	}

	const byte howManyEvents = 3;
	public CameraColor cameraColor;
	public LevelIndicator levelIndicator;
	public ScrollingController sc;
	public Rope[] ropes;
	public TheCenter theCenter;
	public RectTransformParameters coinCountRect;
	public Transform TrainPrefab;
	public Transform EnemyPrefab;
	public Transform CoinPrefab;
	Text CoinCountText;

	internal float offset;
	internal Sprite trainHighlited;
	internal Sprite trainSprite;
	internal RoadLoader.Road road;
	internal Train train;

	MainMenuManager mainMenuManager;
	CollisionEvent[] collisionEvents;

	bool wasThereATouch;
	internal bool isGameStart = false;
	byte collisionEventsCount;
	byte trainsCount;

	int coinCount;
	//int score;


	internal byte TrainsCount {
		get
		{
			return trainsCount;
		}
		set
		{
			trainsCount = value;
			if (trainsCount == 0)
			{
				GameStateChange(0);
			}
		}
	}

	internal int CoinCount {
		get
		{
			return coinCount;
		}
		set
		{
			coinCount = value;
			CoinCountText.text = CoinCount + "$";
			//CoinCountText.GetComponentInParent<Animator>().SetTrigger("start");
			mainMenuManager.parametersSaveLoad.CoinCount = coinCount;
			//mainMenuManager.Progress(LeaderBordMoney, coinCount);
		}
	}

	void Start()
	{
		//Time.timeScale = 0.5f;

		CoinCountText = coinCountRect.GetComponent<Text>();
		RoadLoader roadLoader = new RoadLoader();
		mainMenuManager = GameObject.FindGameObjectWithTag("MMM").GetComponent<MainMenuManager>();
		mainMenuManager.levelManager = this;
		trainSprite = mainMenuManager.notActive;
		trainHighlited = mainMenuManager.active;
		offset = (trainSprite.textureRect.xMax / trainSprite.pixelsPerUnit) / 2;
		road = roadLoader.GetRoad();

		CoinCount = mainMenuManager.parametersSaveLoad.CoinCount;
		collisionEvents = new CollisionEvent[howManyEvents];
		collisionEventsCount = 0;
		wasThereATouch = false;

		{
			JointMotor2D motor = ropes[0].hj.motor;
			motor.motorSpeed = road.speedOfMotor;
			ropes[0].hj.motor = motor;

			for (int i = 1; i < ropes.Length; i++)
			{
				ropes[i].hj.motor = motor;
			}
		}

		theCenter.toNextLevel = road.levels[mainMenuManager.parametersSaveLoad.Level];

		CreateTrains();
	}

	public void ChangeTrains()
	{
		foreach (Rope rope in ropes)
		{
			if (rope.train)
			{
				if (rope.trainOnRope)
				{
					rope.train.ActivateGoOut();
				}
				else
				{
					StartCoroutine(rope.train.StopReturnAndDie());
					rope.train.FreePlace();
				}
			}
		}

		trainSprite = mainMenuManager.notActive;
		trainHighlited = mainMenuManager.active;
		offset = (trainSprite.textureRect.xMax / trainSprite.pixelsPerUnit) / 2;
		CreateTrains();
	}

	private void CreateTrains()
	{
		AddTrain(new Vector2(4, 5));
		AddTrain(new Vector2(-4, 5));
	}

	IEnumerator SpawnCoin(float time)
	{
		while (isGameStart)
		{
			yield return new WaitForSeconds(time);
			Vector3 positionOfCoin = new Vector3(
				(Random.Range(-1, 1) < 0 ? -4 : 4),
				(Random.Range(-1, 1) < 0 ? -4 : 4), 1);
			Coin coin = Instantiate(CoinPrefab, positionOfCoin, Quaternion.identity).GetComponent<Coin>();

			coin.maxSize = road.MaxSizeOfCoin;
			coin.speed = road.speedOfEnemy;
			coin.levelManager = this;
			coin.targetRect = coinCountRect;
		}
	}

	internal void AddTrain(Vector2 position)
	{
		TrainsCount++;
		Train train2;
		train2 = Instantiate(TrainPrefab, position, Quaternion.identity).GetComponent<Train>();
		train2.levelManager = this;
		train2.theCenter = theCenter;
		train2.transform.Rotate(Vector3.forward, Vector2.SignedAngle(train2.transform.up, train2.rb.position.normalized));
		train2.ChangeSprite(trainHighlited);
		train2.isFly = true;
		train2.indexOfTrainSprite = mainMenuManager.indexOfTrainSprite;
		train2.StartReturnToOrbit();
	}

	internal void AddTrainInCenter()
	{
		Train train2;
		float angle = Random.Range(0f, 360f);
		train2 = Instantiate(TrainPrefab, Vector2.zero, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<Train>();
		train2.levelManager = this;
		train2.theCenter = theCenter;
		//train2.transform.Rotate(Vector3.forward, Vector2.SignedAngle(train2.transform.up, train2.rb.position.normalized));
		train2.ChangeSprite(trainHighlited);
		train2.isFly = true;
		train2.indexOfTrainSprite = mainMenuManager.indexOfTrainSprite;
		train2.rb.velocity = train2.transform.up * 5f;
		train2.speedAtDeparture = train2.rb.velocity * 2;
	}

	internal void LostTrainsFromCentre(int count)
	{
		float deltaAngle = 360f / count;
		Train train2;
		for (float angle = 0; count > 0; count--)
		{
			angle += deltaAngle;
			train2 = Instantiate(TrainPrefab, Vector2.zero, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<Train>();
			train2.levelManager = this;
			train2.theCenter = theCenter;
			train2.ChangeSprite(trainHighlited);
			train2.isFly = true;
			train2.rb.velocity = train2.transform.up * 5f;
			train2.speedAtDeparture = Vector2.zero;
		}
	}

	void ActivateLastTrain()
	{
		train = train.lastTrain;
		if (train)
			train.ChangeSprite(trainHighlited);
	}

	IEnumerator SpawnEnemy(float time)
	{
		while (isGameStart)
		{
			int sign;
			if (Random.Range(-1, 1) < 0)
			{
				sign = -1;
			}
			else
			{
				sign = 1;
			}
			Enemy buff = Instantiate(EnemyPrefab, new Vector2(Random.Range(-3f, 3f), 5f * sign), Quaternion.identity).GetComponentInParent<Enemy>();
			buff.levelManager = this;
			buff.maxSize = road.MaxSizeOfEnemy;
			buff.speed = road.speedOfEnemy;
			yield return new WaitForSeconds(time);
		}
	}
	internal void Click()
	{
		if (isGameStart)
		{
			if (Time.timeScale != 0)
			{
				if (train)
				{
					wasThereATouch = true;
				}
			}
		}
		else
		{
			isGameStart = true;
			sc.ChangeState(true);
			StartCoroutine(SpawnEnemy(road.enemyTimerDef));
			StartCoroutine(SpawnCoin(road.deltaTimeToSpawnCoin));
		}


	}

	private void LateUpdate()
	{
		EndEvents();
	}

	void EndEvents()
	{
		if (collisionEventsCount > 0)
		{
			if (collisionEventsCount > 1)
			{ // если враг коснулся более одного поезда (вроде, давно писал, прокомментировать только щяс решил)
			  //ищем самого близкого к центру поезда, которого коснулся враг
				Train bufferTrain1 = null; //место для хранения ссылки на этот поезд
				for (int i = 0, y = road.MaxRoatingTrainsCount; i < collisionEventsCount; i++)
				{
					if (collisionEvents[i].train.rope)
						if (collisionEvents[i].train.rope.ropeIndex < y)
						{
							bufferTrain1 = collisionEvents[i].train;
							y = bufferTrain1.rope.ropeIndex;
						}
				}

				if (bufferTrain1.lastTrain)
				{
					train = bufferTrain1.lastTrain;
					train.ChangeSprite(trainHighlited);
				}
				// уничтожаем все следующие поезда после найденного
				Train buffTrain;
				do
				{
					buffTrain = bufferTrain1.nextTrain;
					bufferTrain1.FreePlace();
					Destroy(bufferTrain1.gameObject);
					Destroy(bufferTrain1);
					bufferTrain1 = buffTrain;
					TrainsCount--;
				} while (bufferTrain1);

			}
			else if (collisionEvents[collisionEventsCount - 1].train == train)
			{//если враг коснулся крайнего поезда 
				ActivateLastTrain();
				collisionEvents[collisionEventsCount - 1].train.FreePlace();
				Destroy(collisionEvents[collisionEventsCount - 1].train.gameObject);
				TrainsCount--;
			}
			else
			{//если враг коснулся ОДНОГО поезда, но этот поезд НЕ КРАЙНИЙ
			 //Ищим предидущий поезд, чтобы его активировать, а если не находим,
			 //то значит это был поселедний и игра должна перезапуститься 
				if (collisionEvents[collisionEventsCount - 1].train.lastTrain)
				{
					train = collisionEvents[collisionEventsCount - 1].train.lastTrain;
					train.ChangeSprite(trainHighlited);
				}
				//уничтожаем все  поезда идущие после задетого
				Train buffTrain;
				do
				{
					buffTrain = collisionEvents[collisionEventsCount - 1].train.nextTrain;
					collisionEvents[collisionEventsCount - 1].train.FreePlace();
					Destroy(collisionEvents[collisionEventsCount - 1].train.gameObject);
					Destroy(collisionEvents[collisionEventsCount - 1].train);
					collisionEvents[collisionEventsCount - 1].train = buffTrain;
					TrainsCount--;
				} while (collisionEvents[collisionEventsCount - 1].train);
			}

			collisionEventsCount = 0;
			collisionEvents = new CollisionEvent[howManyEvents];
		}
		else
		{
			if (wasThereATouch)
			{
				train.ActivateFly();
			}
		}
		wasThereATouch = false;
	}

	public void TrainDie(Train train)
	{
		train.FreePlace();
		TrainsCount--;
	}

	/// <summary>
	/// 0 - Restart, 1 - LevelUp, Def - Restart
	/// </summary>
	/// <param name="nextScreenCode"></param>
	internal void GameStateChange(byte nextScreenCode)
	{
		Save();
		Time.timeScale = 1f;
		switch (nextScreenCode)
		{
			default:
			case 0:
				sc.ChangeState(false);
				isGameStart = false;
				theCenter.TrainsCountSetZero();
				ChangeTrains();
				StartCoroutine(cameraColor.ChangeColor(Vector2.zero));
				break;
			case 1:
				mainMenuManager.parametersSaveLoad.Level++;
				StartCoroutine(levelIndicator.LevelUpAnimation());
				sc.ChangeState(false);
				isGameStart = false;
				theCenter.TrainsCountSetZero();
				ChangeTrains();
				StartCoroutine(cameraColor.ChangeColor(new Vector2(0, 4.5f)));
				break;
		}
	}


	internal void AddCollisionEvent(Train train)
	{
		collisionEvents[collisionEventsCount].train = train;
		collisionEventsCount++;
	}

	private void Save()
	{
		//if (mainMenuManager.parametersSaveLoad.Best < score)
		//{
		//	mainMenuManager.newBest = true;
		//	mainMenuManager.parametersSaveLoad.Best = score;
		//	/*if(score <= 1000 && score >= 200)
		//		mainMenuManager.GetAchive(achivNewcomer, score/1000);
		//	if (score <= 3000 && score >= 1000)
		//		mainMenuManager.GetAchive(achivExperienced, score / 3000);
		//	if (score <= 10000 && score >= 3000)
		//		mainMenuManager.GetAchive(achivCrazy, score / 10000);
		//	mainMenuManager.Progress(LeaderBordScore, score);*/
		//}
	}


	private void OnApplicationPause(bool pause)
	{
		Save();
	}

	private void OnDestroy()
	{
		Save();
		mainMenuManager.newBest = false;
	}

	internal Rope GetFreeRope()
	{
		for (int i = 0; i < ropes.Length; i++)
		{
			if (ropes[i].imFree)
			{
				return ropes[i];
			}
		}
		return null;
	}
}
