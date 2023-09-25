using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParametersSaveLoad
{

	public struct Parameters
	{
		public string version;
		public int coinCount;
		public int best;
		public byte level;
	}

	Parameters parameters;

	public ParametersSaveLoad()
	{
		Debug.Log(Application.persistentDataPath + "/parameters.json");
		if (File.Exists(Application.persistentDataPath + "/parameters.json"))
		{
			parameters = JsonUtility.FromJson<Parameters>(File.ReadAllText(Application.persistentDataPath + "/parameters.json"));
		} else
		{
			parameters = JsonUtility.FromJson<Parameters>(Resources.Load("parameters").ToString());
			SaveAll();
		}
	}

	internal Parameters Parameterss {
		get
		{
			return parameters;
		}
		set
		{
			parameters = value;
			SaveAll();
		}
	}

	internal string Version {
		get
		{
			return parameters.version;
		}
		set
		{
			parameters.version = value;
			SaveAll();
		}
	}

	internal int CoinCount {
		get
		{
			return parameters.coinCount;
		}
		set
		{
			parameters.coinCount = value;
			SaveAll();
		}
	}

	internal int Best {
		get
		{
			return parameters.best;
		}
		set
		{
			parameters.best = value;
			SaveAll();
		}
	}

	internal byte Level {
		get
		{
			return parameters.level;
		}
		set
		{
			parameters.level = value;
			SaveAll();
		}
	}

	void SaveAll()
	{
		StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + "/parameters.json");
		streamWriter.Write(JsonUtility.ToJson(parameters, true));
		streamWriter.Close();
	}
}