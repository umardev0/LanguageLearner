using UnityEngine;
using Parse;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public AudioClip clickSound;
	public GameObject internetPanel;

	public float pingInterval = 2f; //seconds
	public string PROTOTYPE = "A-Feedback";
	public bool FEEDBACK = true;

	[HideInInspector]
	public int level=0;
	[HideInInspector]
	public bool isOnline = false;
	[HideInInspector]
	public int correctAns = 0;
	[HideInInspector]
	public int correctAnsInRow = 0;
	[HideInInspector]
	public int wrongAns = 0;

	private static SoundManager instance = null;
	private WWW googleRequest;

	public static SoundManager GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start()
	{
//		Debug.unityLogger.logEnabled = false;
		if(ParseUser.CurrentUser != null)
		{
			ParseManager.CreateSession();
		}

		StartCoroutine(Ping());

//		ParseClient.Initialize(new ParseClient.Configuration {
//			ApplicationId = "WmhUZz1Rb6GZcefbNefJFp8PoT4FXxFHwocHBZpl",
//			WindowsKey = "NfKyDeh4MHS0a0FGYNg52BcaJaEiywMHltpCAyyi",
//			Server = "https://parseapi.back4app.com"
//		});
	}

	IEnumerator Ping()
	{
		while(true)
		{
			string url = "https://www.google.com";///search?q=" + Random.Range(0,10000).ToString();
			googleRequest = new WWW(url);
			yield return googleRequest;
			if(string.IsNullOrEmpty(googleRequest.error))
			{
				isOnline = true;
			}
			else
			{
				isOnline = false;
			}

			if(isOnline)
				internetPanel.SetActive(false);
			else
				internetPanel.SetActive(true);
			
			yield return new WaitForSeconds(pingInterval);
		}
	}

	public void OnClickSound()
	{
		if(PlayerPrefs.GetInt("sound") == 1)
		{
			AudioSource.PlayClipAtPoint(clickSound, transform.position);
		}
	}
}
