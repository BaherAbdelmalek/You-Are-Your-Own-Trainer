using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class StartPlaying : MonoBehaviour {
	public Toggle Recorded;
	public Toggle formed;
	private StreamReader fileReader = new StreamReader("FormedExercises") ;
	private StreamReader fileReader1 = new StreamReader("Exercises") ;
	private StreamReader fileReaderDesc = new StreamReader("FormedExercisesDescription") ;
	private Button _button;
	public Text PlayExerciseName;
	public Text FormedExerciseName;

	// Use this for initialization
	void Start()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() =>
			{

				SharedVariables.SetDefaultsBools();
				if(SharedVariables.PlayType == 1){
					SharedVariables.Playing = true;
					SharedVariables.PlayingExerciseName = PlayExerciseName.text;
					ReadLineFromFile1();
				}
				if (SharedVariables.PlayType == 2) {
					SharedVariables.FormExercisePlay = true;
					SharedVariables.FormExerciseName = FormedExerciseName.text;
					ReadLineFromFile();
					ReadDesc();
				}
				Application.LoadLevel("KinectMocapAnimator");

			});
	}

//  public void StartExercise()
//    {
//        
//		SharedVariables.SetDefaultsBools();
//		if(Recorded.isOn){
//			SharedVariables.Playing = true;
//            Debug.Log(SharedVariables.PlayingExerciseName);
//			ReadLineFromFile1();
//
//		}
//		if (formed.isOn) {
//			SharedVariables.FormExercisePlay = true;
//			ReadLineFromFile();
//		}
//        
//        Application.LoadLevel("KinectMocapAnimator");
//   }

	public void Record_Toogle(bool val){
		if (Recorded.isOn)
			formed.isOn = false;
	}
	public void Form_toggle(bool val){
		if (formed.isOn)
			Recorded.isOn = false;
	}
	private void ReadDesc(){
		if (fileReaderDesc == null) {
			Debug.LogError ("the file can not be accessed");
		} else {
			while (true) {
				string line = fileReader.ReadLine();
				if (line == null) {
					break;
				}
				if(line.Equals(FormedExerciseName.text)){
						for (int i = 0; i <  Math.Max(SharedVariables.FormedAngles.Count,SharedVariables.FormedPoses.Count); i++) 
						SharedVariables.FormExercisesDesc[i] = fileReader.ReadLine();
					break;
				}
			}
		}
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
				if (lines.Length == 1 && lines [0] == SharedVariables.FormExerciseName) {
					string angs = fileReader.ReadLine();
					string[] arr = angs.Split(delimiters);
					if (arr.Length == 12) {
						SharedVariables.PlayWithPoses = false;
						SharedVariables.PLayWithAngles = true;
						// SharedVariables.Angles = new double[12];
						int idx = 0;
						while (arr.Length > 1) {
							SharedVariables.FormedAngles.Add(new List<int> ());
							for (int i = 0; i < 12; i++) {
								int ang;
								int.TryParse (arr [i], out ang);
								SharedVariables.FormedAngles[idx].Add(ang);
							}
						
						angs = fileReader.ReadLine ();
						arr = angs.Split (delimiters);
						idx++;
					  }
					}
					if (arr.Length == 19) {
						SharedVariables.PlayWithPoses = true;
						SharedVariables.PLayWithAngles = false;
						SharedVariables.Poses = new double[19];
						int idx = 0;
						while (arr.Length > 1) {
							SharedVariables.FormedPoses.Add (new List<int> ());
							for (int i = 0; i < 19; i++) {
								int ang;
								int.TryParse (arr [i], out ang);
								SharedVariables.FormedPoses [idx].Add (ang);
							}
							angs = fileReader.ReadLine ();
							arr = angs.Split (delimiters);
							idx++;
						}
					}
					int.TryParse(arr[0],out SharedVariables.FormExerciseTime);
					int.TryParse(fileReader.ReadLine(),out SharedVariables.FormPlayingSeconds);
					int.TryParse(fileReader.ReadLine(),out SharedVariables.FormTimeShow);
				}
				// string time = fileReader.ReadLine();

			}

		}
	}

	private void ReadLineFromFile1()
	{
		if (fileReader1== null) {
			Debug.LogError ("the file can not be accessed");
		} else {
			// read a line
			while (true) {
				string line = fileReader1.ReadLine();
                if (line == null) {
					break;
				}
				char[] delimiters = { ',' };
				string[] lines = line.Split (delimiters);
				if (lines.Length == 1 && lines [0] == SharedVariables.PlayingExerciseName){
					string TJoints = fileReader1.ReadLine();
					string[] Tjointslist = TJoints.Split (delimiters);
                    SharedVariables.TrackedJoints = new List<int>();
                    for (int i = 0; i < Tjointslist.Length; i++) {
						int J;
						int.TryParse (Tjointslist [i], out J);
						SharedVariables.TrackedJoints.Add (J);
					}
					string times = fileReader1.ReadLine ();
					string[] timesarray = times.Split (delimiters);
					for (int i = 0; i < timesarray.Length; i++) {
						int a;
						int.TryParse (timesarray [i], out a);
						SharedVariables.AngleTransistionsTimes.Add (a);
					}
					int.TryParse (fileReader1.ReadLine (), out SharedVariables.PlayingExerciseTime);
				}

			}
		}
	}
}

