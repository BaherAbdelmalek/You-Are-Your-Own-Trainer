using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedVariables {

	//Playing Exercsie variables
    public static bool Playing;
	public static string PlayingExerciseName;
	public static int PlayingExerciseTime;
	public static List<int> TrackedJoints = new List<int>();
	public static List<int> AngleTransistionsTimes= new List<int>();

	public static int PlayType;

	//Recording Exercise variables
    public static bool RecordingExercise;
	public static string RecordedExerciseName;
	public static int RecordedExerciseTime;


	//Formed Exercise Recording variables
	public static bool FormedExerciseRecording;

	//formed Exercise playing variables
	public static bool  FormExercisePlay;
	public static int   FormExerciseTime;
	public static string FormExerciseName;
	public static double[] Angles;
	public static double[] Poses;
	public static List<List<int>> FormedAngles = new List<List<int>> ();
	public static List<List<int>> FormedPoses = new List<List<int>> ();
	public static bool UseAngles;
	public static bool UsePoses;
    public static bool PLayWithAngles;
    public static bool PlayWithPoses;
	public static int FormPlayingSeconds;
	public static string[] FormExercisesDesc;
	public static int FormTimeShow;


public static void SetDefaultsBools(){
		Playing = false;
		RecordingExercise = false;
		FormedExerciseRecording = false;
		FormExercisePlay = false;
		UseAngles = false;
		UsePoses = false;
		PLayWithAngles = false;
		PlayWithPoses = false;
	}
	public static void SetDefaultStrings(){
		FormExerciseName = "";
		RecordedExerciseName = "";
	}
	public static void SetDefaultInt(){
		FormExerciseTime = 0;
		RecordedExerciseTime = 0;
	}

}
