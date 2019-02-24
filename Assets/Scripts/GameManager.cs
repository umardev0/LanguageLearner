using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public GameObject[] optionGO;
	public GameObject menuCanvas;
	public GameObject gameCanvas;

	public GameObject nextBtn;
	public GameObject feedback;

	public Sprite normalSprite;
	public Sprite rightSprite;
	public Sprite wrongSprite;

	public Text question;

	private List<Question> begQuestionList;
	private List<Question> intQuestionList;
	private List<Question> advQuestionList;

	private List<Question> currentQuesList;
	private List<Question> activeQuesList;

	private List<string> begOptions;
	private List<string> intOptions;
	private List<string> advOptions;

	private List<string> correctFeedback;
	private List<string> wrongFeedback;

	private List<string> currentOptList;

	private int correctOption = 0;
	private bool canClick = false;
	private string questionFormat;

	public class Question
	{
		public string questionText;
		public string correctAns;
		public Question(string question, string answer)
		{
			questionText = question;
			correctAns = answer;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
//			SceneManager.LoadScene("MainMenuScene");
			gameCanvas.SetActive(false);
			menuCanvas.SetActive(true);
		}
	}

	public void StartGame()
	{
		PopulateLists();

		if(SoundManager.GetInstance().level == 1)
		{
			currentQuesList = begQuestionList;
			currentOptList = begOptions;
			questionFormat = "Pick correct word for ";
		}
		else if(SoundManager.GetInstance().level == 2)
		{
			currentQuesList = intQuestionList;
			currentOptList = intOptions;
			questionFormat = "Pick correct word for ";
		}
		else
		{
			currentQuesList = advQuestionList;
			currentOptList = advOptions;
			questionFormat = "Pick antonym for ";
		}

		activeQuesList = new List<Question>();

		feedback.SetActive(false);
		nextBtn.SetActive(false);
		LoadQuestion();

		for(int i=0; i<optionGO.Length; i++)
		{
			optionGO[i].GetComponent<Button>().image.sprite = normalSprite;
		}
	}

	public void OnHomeClick()
	{
		SoundManager.GetInstance().OnClickSound();
//		SceneManager.LoadScene("MainMenuScene");
		gameCanvas.SetActive(false);
		menuCanvas.SetActive(true);
	}

	public void OnNextClick()
	{
		SoundManager.GetInstance().OnClickSound();
		feedback.SetActive(false);
		nextBtn.SetActive(false);
		LoadQuestion();

		for(int i=0; i<optionGO.Length; i++)
		{
			optionGO[i].GetComponent<Button>().image.sprite = normalSprite;
		}
	}

	public void OnOptionClick(int option)
	{
		if(!canClick)
			return;

		SoundManager.GetInstance().OnClickSound();
		bool result = option == correctOption;

		if(!result)
		{
			optionGO[option].GetComponent<Button>().image.sprite = wrongSprite;
			SoundManager.GetInstance().wrongAns++;
			ParseManager.IncrementWrong();
			if(SoundManager.GetInstance().correctAnsInRow>1)
			{
				ParseManager.AddCorrectInRow(SoundManager.GetInstance().correctAnsInRow);
			}
			SoundManager.GetInstance().correctAnsInRow = 0;
		}
		else
		{
			ParseManager.IncrementCorrect();
			SoundManager.GetInstance().correctAns++;
			SoundManager.GetInstance().correctAnsInRow++;
		}

		optionGO[correctOption].GetComponent<Button>().image.sprite = rightSprite;


		if(SoundManager.GetInstance().FEEDBACK)
			LoadFeeback(result);

		nextBtn.SetActive(true);
		canClick = false;
	}

	private void LoadFeeback(bool result)
	{
		feedback.SetActive(true);
		if(result)
		{
			feedback.transform.Find("Text").GetComponent<Text>().text = correctFeedback[Random.Range(0, correctFeedback.Count)];
		}
		else
		{
			feedback.transform.Find("Text").GetComponent<Text>().text = wrongFeedback[Random.Range(0, wrongFeedback.Count)];
		}
	}

	private void LoadQuestion()
	{
		canClick = true;

		if(activeQuesList.Count<=0)
		{
			activeQuesList = currentQuesList.ToList();
		}

		int rand = Random.Range(0, activeQuesList.Count);
		Question ques = activeQuesList[rand];
		activeQuesList.RemoveAt(rand);

		List<string> options = new List<string>();
		options.Add(ques.correctAns);


		for(int i=0; i<3;)
		{
			bool addNew = true;
			string op = currentOptList[Random.Range(0, currentOptList.Count)];

			foreach(var item in options)
			{
				if(op.Equals(item))
					addNew = false;
			}
			if(addNew)
			{
				options.Add(op);
				i++;
			}
		}

		question.text = questionFormat + ques.questionText;

		for(int i=0; i<4; i++)
		{
			int index = Random.Range(0, options.Count);
			optionGO[i].transform.Find("Text").GetComponent<Text>().text = options[index];
			if(options[index].Equals(ques.correctAns))
			{
				correctOption = i;
			}
			options.RemoveAt(index);
		}
	}

	private void PopulateLists()
	{
		begQuestionList = new List<Question>()
		{
			new Question("white","valkoinen"),
			new Question("red","punainen"),
			new Question("blue","sininen"),
			new Question("yellow","keltainen"),
			new Question("green","vihreä"),
			new Question("black","musta"),
			new Question("orange","oranssi"),
			new Question("purple","violetti"),
			new Question("brown","ruskea"),
			new Question("grey","harmaa"),
			new Question("small","pieni"),
			new Question("big","iso/suuri"),
			new Question("hot","kuuma"),
			new Question("cold","kylmä"),
			new Question("good","hyvä"),
			new Question("bad","huono")
		};

		intQuestionList = new List<Question>()
		{
			new Question("hard","vaikea"),
			new Question("easy","helppo"),
			new Question("new","uusi"),
			new Question("old","vanha"),
			new Question("beautiful","kaunis"),
			new Question("ugly","ruma"),
			new Question("cheap","halpa"),
			new Question("expensive","kallis"),
			new Question("long","pitkä"),
			new Question("short","lyhyt"),
			new Question("clean","puhdas"),
			new Question("dirty","likainen"),
			new Question("slow","hidas"),
			new Question("fast","nopea")
		};

		advQuestionList = new List<Question>()
		{
			new Question("pieni","iso/suuri"),
			new Question("kuuma","kylmä"),
			new Question("hyvä","huono"),
			new Question("vaikea","helppo"),
			new Question("uusi","vanha"),
			new Question("kaunis","ruma"),
			new Question("halpa","kallis"),
			new Question("pitkä","lyhyt"),
			new Question("puhdas","likainen"),
			new Question("hidas","nopea")
		};

		begOptions = new List<string>()
		{
			"valkoinen",
			"punainen",
			"sininen",
			"keltainen",
			"vihreä",
			"musta",
			"oranssi",
			"violetti",
			"ruskea",
			"harmaa",
			"pieni",
			"iso/suuri",
			"kuuma",
			"kylmä",
			"hyvä",
			"huono"
		};

		intOptions = new List<string>()
		{
			"vaikea",
			"helppo",
			"uusi",
			"vanha",
			"kaunis",
			"ruma",
			"halpa",
			"kallis",
			"pitkä",
			"lyhyt",
			"puhdas",
			"likainen",
			"hidas",
			"nopea"
		};

		advOptions = new List<string>()
		{
			"iso/suuri",
			"kylmä",
			"huono",
			"helppo",
			"vanha",
			"ruma",
			"kallis",
			"lyhyt",
			"likainen", 
			"nopea"
		};

		correctFeedback = new List<string>()
		{
			"Good thinking!",
			"Well done!",
			"Good job!",
			"Excellent!",
			"Great!",
			"Perfect!", 
			"You are on fire!",
			"Keep up the good work!"				
		};

		wrongFeedback = new List<string>()
		{
			"It's OK, we all make mistakes!",
			"Nobody’s perfect!",
			"Now you know it. Good luck next time!",
			"Don’t be sad. You can do better!",
			"I'm sure you will get it next time",
			"We all learn from our mistakes"
		};
	}
}
