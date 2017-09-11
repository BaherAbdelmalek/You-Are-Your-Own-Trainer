using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ScrollView : MonoBehaviour {

	public GameObject Button_Template;
	private StreamReader fileReader = new StreamReader("Exercises") ;
	List<string> Exercises = new List<string>();

	// Use this for initialization
	void Start () {
		ReadLineFromFile();
		foreach(string str in Exercises)
		{
			GameObject go = Instantiate(Button_Template) as GameObject;
			go.SetActive(true);
			RecordedItemScript TB = go.GetComponent<RecordedItemScript>();
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
		while(true)
		{
			string sPlayLine = fileReader.ReadLine();
			if (sPlayLine == null)
			{
				break;
			}
			Exercises.Add(sPlayLine);
			fileReader.ReadLine();
			fileReader.ReadLine();
			fileReader.ReadLine();
		}
	}

}
