using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FormedExerciseslist : MonoBehaviour {

	public Dropdown FormedExercises;
	private string SelectedExercise;
	private StreamReader fileReader = new StreamReader("FormedExercises") ;
	List<string> Exercises = new List<string>() { "Please choose the Exercise "};
	// Use this for initialization
	void Start () {
		ReadLineFromFile();
		PopulateList();
	}
	public void DropDownHandler(int index)
	{
		SelectedExercise = Exercises[index];
		SharedVariables.FormExerciseName = SelectedExercise;

	}

	// Update is called once per frame
	void PopulateList () {

		FormedExercises.AddOptions(Exercises);
	}
	private void ReadLineFromFile()
	{
		if (fileReader == null) {
			Debug.LogError ("the file can not be accessed");
		} else {
			// read a line
			while (true) {
				string line = fileReader.ReadLine();
				if (line == null) {
					break;
				}
				char[] delimiters = { ',' };
				string[] lines = line.Split (delimiters);

				if (lines.Length == 1) {
					Exercises.Add (line);
					fileReader.ReadLine ();
					fileReader.ReadLine ();
				}
			}
		}
	}
}
