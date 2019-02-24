using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnClickHandler : MonoBehaviour
{
	public Sprite soundOn;
	public Sprite soundOff;
	public Button soundBtn;
	public InputField nameField;
	public GameManager gameManager;

	public GameObject namePanel;
	public GameObject waitPanel;
	public GameObject levels;
	public GameObject lessons;
	public GameObject gameCanvas;
	public GameObject menuCanvas;

	void Start()
	{
		if(PlayerPrefs.GetInt("sound", 1) == 1)
		{
			PlayerPrefs.SetInt("sound", 1);
			soundBtn.image.sprite = soundOn;
		}
		else
		{
			soundBtn.image.sprite = soundOff;
		}

		if(string.IsNullOrEmpty(PlayerPrefs.GetString("reset")))
		{
			PlayerPrefs.SetString("reset", "done");
			PlayerPrefs.SetString("name", "");
		}

		if(string.IsNullOrEmpty(PlayerPrefs.GetString("name")))
		{
			namePanel.SetActive(true);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

	public void OnSoundClick()
	{
		if(PlayerPrefs.GetInt("sound") == 1)
		{
			PlayerPrefs.SetInt("sound", 0);
			soundBtn.image.sprite = soundOff;
		}
		else
		{
			PlayerPrefs.SetInt("sound", 1);
			soundBtn.image.sprite = soundOn;
		}
		SoundManager.GetInstance().OnClickSound();
	}

	public void OnLevelCLick(int level)
	{
		SoundManager.GetInstance().OnClickSound();
		SoundManager.GetInstance().level = level;
//		SceneManager.LoadScene("GameScene");
		gameCanvas.SetActive(true);
		menuCanvas.SetActive(false);
		gameManager.StartGame();

	}

	public void OnLessonCLick(int lessonNo)
	{
		SoundManager.GetInstance().OnClickSound();
		lessons.SetActive(false);
		levels.SetActive(true);
	}

	public void OnNameOkClick()
	{
		SoundManager.GetInstance().OnClickSound();
		if(!string.IsNullOrEmpty(nameField.text))
		{
			var name = nameField.text + Random.Range(1,1000).ToString();
			PlayerPrefs.SetString("name", name);
			namePanel.SetActive(false);
			waitPanel.SetActive(true);
			ParseManager.RegisterUser(name, "123456", RegisterCallback);
		}
	}

	public void RegisterCallback(bool result)
	{
		if(result)
			waitPanel.SetActive(false);
		else
			namePanel.SetActive(true);
	}
}
