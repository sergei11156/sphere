using UnityEngine;

public class RoadLoader
{

	public struct Road
	{
		//public byte trainsCountOnStart;
		public byte MaxRoatingTrainsCount;
		//public byte MaxTrainsCount;
		public float deltaTimeToSpawnCoin;
		public float enemyTimerDef;
		public float MaxSizeOfEnemy;
		public float speedOfEnemy;
		public float MaxSizeOfCoin;
		public float speedOfMotor;
		public byte[] levels;
	}
	Road road;
	internal Road GetRoad()
	{
		//Debug.Log();
		return JsonUtility.FromJson<Road>(Resources.Load("road").ToString());
	}
}
