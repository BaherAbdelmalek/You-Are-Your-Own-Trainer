using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FormExercise : MonoBehaviour {
	public Text[] AnglesInput;
	public Text[] PosesInput;
	public Text ExerciseName;
	public Text ExerciseTime;
	public Text PlayingTime;
	public static double[] Angles;
	public static double[] Poses;
	public Toggle AngleToggle;
	public Toggle PoseToggle;
	public Text Desctiption;
	public Text DescTime;


	public void formExercise(){

		if (AngleToggle.isOn)
			Getangles();
		if (PoseToggle.isOn)
			GetPoses();
		Application.LoadLevel("KinectMocapAnimator");
	}



	 public void  GetPoses(){
		SharedVariables.SetDefaultsBools ();
		SharedVariables.UsePoses = true;
		SharedVariables.FormedExerciseRecording = true;
        SharedVariables.FormExerciseName = ExerciseName.text;
        int.TryParse(ExerciseTime.text, out SharedVariables.FormExerciseTime);
		int.TryParse(PlayingTime.text, out SharedVariables.FormPlayingSeconds);
		if(!File.Exists("FormedExercises"))
		{
			using(StreamWriter writer = File.CreateText("FormedExercises"))
			{

			}
		}
		using (StreamWriter writer = File.AppendText ("FormedExercises")) {
			writer.WriteLine(ExerciseName.text);
			for(int i = 0; i < UpdateMovePose.PoseMoves.Count; i++){
				string Poses = UpdateMovePose.PoseMoves[i][0]+"";
				for (int j = 1; j < UpdateMovePose.PoseMoves[i].Count; j++) {
					Poses += "," + UpdateMovePose.PoseMoves[i][j];
			}
			writer.WriteLine (Poses);
		 }
			writer.WriteLine (ExerciseTime.text);
			writer.WriteLine (PlayingTime.text);
			writer.WriteLine (DescTime.text);

		}
		char[] delimiters = { ',' };
		using (StreamWriter writer = File.AppendText ("FormedExercisesDescription")) {
			writer.WriteLine(ExerciseName.text);
			string desc = Desctiption.text;
			string[] descs = desc.Split (delimiters);
			for (int i = 0; i < descs.Length; i++) {
				writer.WriteLine (descs[i]);
			}
		}
	}


	public void  Getangles(){
		SharedVariables.SetDefaultsBools();
		SharedVariables.FormedExerciseRecording = true;
		SharedVariables.UseAngles = true;
		SharedVariables.FormExerciseName = ExerciseName.text;
		int.TryParse(ExerciseTime.text, out SharedVariables.FormExerciseTime);
		int.TryParse(PlayingTime.text, out SharedVariables.FormPlayingSeconds);
		if(!File.Exists("FormedExercises"))
		{
			using(StreamWriter writer = File.CreateText("FormedExercises"))
			{

			}
		}
		using (StreamWriter writer = File.AppendText ("FormedExercises")) {
			 writer.WriteLine(ExerciseName.text);
			for(int i = 0;i < UpdateMove.AngleMoves.Count;i++){
				string angles =UpdateMove.AngleMoves[i][0] +"";
				for (int j = 1; j < UpdateMove.AngleMoves[i].Count; j++) {
					angles += "," + UpdateMove.AngleMoves[i][j];
			     }
			     writer.WriteLine (angles);
			}
			writer.WriteLine (ExerciseTime.text);
			writer.WriteLine (PlayingTime.text);
			writer.WriteLine (DescTime.text);
		}
		char[] delimiters = { ',' };
		using (StreamWriter writer = File.AppendText ("FormedExercisesDescription")) {
			writer.WriteLine(ExerciseName.text);
			string desc = Desctiption.text;
			string[] descs = desc.Split (delimiters);
			for (int i = 0; i < descs.Length; i++) {
				writer.WriteLine (descs[i]);
			}
		}
	}
	public void Angle_Toogle(bool val){
		if (AngleToggle.isOn)
			PoseToggle.isOn = false;
	}
	public void Form_toggle(bool val){
		if (PoseToggle.isOn)
			AngleToggle.isOn = false;
	}
}
