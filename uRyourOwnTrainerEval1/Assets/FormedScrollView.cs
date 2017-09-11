using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class FormedScrollView : MonoBehaviour {

	public GameObject Button_Template;
	private StreamReader fileReader = new StreamReader("FormedExercises") ;
	List<string> Exercises = new List<string>();

	// Use this for initialization
	void Start () {
		ReadLineFromFile();
		foreach(string str in Exercises)
		{
			GameObject go = Instantiate(Button_Template) as GameObject;
			go.SetActive(true);
			FormedItemScript TB = go.GetComponent<FormedItemScript>();
			TB.SetName(str);
			go.transform.SetParent(Button_Template.transform.parent);

		}

	}
	private void ReadLineFromFile()
	{
		if (fileReader == null)
		{
			Debug.LogError("the file can not be accessed");
		}
		// read a line
		int  flag = 2;
		while(true)
		{
			string sPlayLine = fileReader.ReadLine();
			if (sPlayLine == null)
			{
				break;
			}
			char[] delimiters = { ',' };
			string[] sLineParts = sPlayLine.Split(delimiters);
			if (sLineParts.Length == 1) {
				if (flag == 2) {
					Exercises.Add (sLineParts [0]);
					flag = 0;
				}
				else
					flag ++;
			}
		}
	}
	//	public void ButtonClicked(string str)
	//	{
	//		Debug.Log(str + " button clicked.");
	//
	//	}
}
