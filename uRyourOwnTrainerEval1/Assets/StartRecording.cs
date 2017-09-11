using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class StartRecording : MonoBehaviour {
    public Text ExerciseName;
    public Text trackedjoints;
	public Text ExerciseTime;
	public Text AngleTransitions;
    public string saveFilePath = "Exercises";
	// Use this for initialization
	public void StartRecord() {
        SharedVariables.RecordedExerciseName = ExerciseName.text;
		int.TryParse (ExerciseTime.text, out SharedVariables.RecordedExerciseTime);
        string Tjoints = trackedjoints.text;
        char[] delimiters = { ',' };
        string[] Jointslist =Tjoints.Split(delimiters);       
        for(int i = 0;i < Jointslist.Length;i++)
        {
            int f;
            int.TryParse(Jointslist[i], out f);
            SharedVariables.TrackedJoints.Add(f);
        }
	
		   SharedVariables.SetDefaultsBools();
		   SharedVariables.RecordingExercise = true;
        if (!File.Exists(saveFilePath))
        {
            using (StreamWriter writer = File.CreateText(saveFilePath))
            {
                // csv file header
                string sLine = ExerciseName.text;
                writer.WriteLine(sLine);
            }
        }
        else
        {
            using (StreamWriter writer = File.AppendText(saveFilePath))
            {
                // csv file header
                string sLine = ExerciseName.text;
                writer.WriteLine(sLine);
				string TrackedJoints = "" + Jointslist [0];
				for(int i = 1;i < Jointslist.Length;i++)
				{
					TrackedJoints += "," + Jointslist [i];
				}
				writer.WriteLine (TrackedJoints);
				writer.WriteLine (AngleTransitions.text);
				writer.WriteLine (ExerciseTime.text);
            }
        }
        Application.LoadLevel("KinectMocapAnimator");

    }


}
