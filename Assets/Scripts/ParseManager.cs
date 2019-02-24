using System.Collections.Generic;
using UnityEngine;
using Parse;
using System;
using System.Collections;
using UnityEngine.UI;


public class ParseManager : MonoBehaviour
{
	static ParseObject GameSessionObj;

	public static void RegisterUser(string username, string password, Action<bool> callback)
	{
		var user = new ParseUser()
		{
			Username = username.ToLower(),
			Password = password
		};

		user.SignUpAsync().ContinueWith(task => {

			if(task.Exception != null)
			{
				Debug.Log("Register Exception : " + task.Exception.InnerExceptions[0].Message);
				callback(false);
            }
			else
			{
				var parseObj = new ParseObject("UserVersion");
				parseObj["userID"] = ParseUser.CurrentUser.ObjectId;
				parseObj["prototype"] = SoundManager.GetInstance().PROTOTYPE;

				parseObj.SaveAsync().ContinueWith(secondTask => {
					if (secondTask.IsFaulted || secondTask.IsCanceled)
					{
						IEnumerable<Exception> exceptions = secondTask.Exception.InnerExceptions;

						foreach (var exception in exceptions)
						{
							Debug.Log("SignUp User error : " + exception.Message);
							callback(false);
						}
					}
					else
					{
						Debug.Log("User Registeration Successful");
						CreateSession(callback);
					}
				});
            }
		});
	}

	public static void CreateSession(Action<bool> callback = null)
	{
		GameSessionObj = new ParseObject("UserSessionData");
		GameSessionObj["userID"] = ParseUser.CurrentUser.ObjectId;
		GameSessionObj["questionsAttempted"] = 0;
		GameSessionObj["correctAnswered"] = 0;
		GameSessionObj["wrongAnswered"] = 0;
		GameSessionObj["correctInRow"] = new List<int>();

		GameSessionObj.SaveAsync().ContinueWith(secondTask => {
			if (secondTask.IsFaulted || secondTask.IsCanceled)
			{
				IEnumerable<Exception> exceptions = secondTask.Exception.InnerExceptions;

				foreach (var exception in exceptions)
				{
					Debug.Log("Creating GameSessionObj error : " + exception.Message);
					if(callback!=null)
						callback(false);
				}
			}
			else
			{
				Debug.Log("GameSessionObj created successfully");
				if(callback!=null)
					callback(true);
			}
		});
	}

	public static void IncrementCorrect()
	{
		GameSessionObj.Increment("correctAnswered");
		GameSessionObj.SaveAsync().ContinueWith(secondTask => {
			if (secondTask.IsFaulted || secondTask.IsCanceled)
			{
				IEnumerable<Exception> exceptions = secondTask.Exception.InnerExceptions;

				foreach (var exception in exceptions)
				{
					Debug.Log("correctAnswered increment error : " + exception.Message);
				}
			}
			else
			{
				Debug.Log("correctAnswered incremented successfully");
			}
		});
	}

	public static void IncrementWrong()
	{
		GameSessionObj.Increment("wrongAnswered");
		GameSessionObj.SaveAsync().ContinueWith(secondTask => {
			if (secondTask.IsFaulted || secondTask.IsCanceled)
			{
				IEnumerable<Exception> exceptions = secondTask.Exception.InnerExceptions;

				foreach (var exception in exceptions)
				{
					Debug.Log("wrongAnswered increment error : " + exception.Message);
				}
			}
			else
			{
				Debug.Log("wrongAnswered incremented successfully");
			}
		});
	}

	public static void AddCorrectInRow(int count)
	{
		GameSessionObj.AddToList("correctInRow", count);
		GameSessionObj.SaveAsync().ContinueWith(secondTask => {
			if (secondTask.IsFaulted || secondTask.IsCanceled)
			{
				IEnumerable<Exception> exceptions = secondTask.Exception.InnerExceptions;

				foreach (var exception in exceptions)
				{
					Debug.Log("correctInRow add error : " + exception.Message);
				}
			}
			else
			{
				Debug.Log("correctInRow added successfully");
			}
		});
	}
}
