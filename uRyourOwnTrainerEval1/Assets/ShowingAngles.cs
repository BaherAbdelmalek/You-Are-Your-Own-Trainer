using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Entity;
using System;

public class ShowingAngles : MonoBehaviour {
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;
	[Tooltip("Current joint position in Kinect coordinates (meters).")]
	public Vector3 jointPosition;
	public Text[] RecordedAngles;
	public Text[] PlayingAngles;
	private AngleVector[] anglesvectors;
	private string FilePath;
	private  StreamReader fileReader;
	private int RecordedCounter;
	private int PlayingCounter;
	private static ArrayList TrackedJointsList;
	public GameObject RecorededAnglesText;
	public GameObject PlayingAnglesText;
	public Image RecodedAnglesImage;
	public Image PlayingAnglesImage;
	private int TimesIndex;
	private static bool show;
	private static Entity.Skeleton Skeleton;
	public Text MoveText;
	public int FrameStart;
	private bool FirstTime;
	// Use this for initialization
	void Start () {
		if (SharedVariables.Playing) {
			FilePath = "ExercisesCSV/" + SharedVariables.PlayingExerciseName + ".CSV";
			fileReader = new StreamReader (FilePath);
			SetAngleElements ();
			ReadLineFromFile();
			for (int i = 0; i < (FrameStart * SharedVariables.TrackedJoints.Count); i++) {
				Skeleton =  returnSkeleton(i);
			}
			RecordedCounter = FrameStart;
			PlayingCounter = 0;
			TrackedJointsList = new ArrayList();
			for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++) {
				TrackedJointsList.Add(SharedVariables.TrackedJoints[i]);
			}
			if(TrackedJointsList.Contains(0))
				TrackedJointsList.Add(0);
			if(TrackedJointsList.Contains(2))
				TrackedJointsList.Add(2);
			TimesIndex = 0;
			while(!FirstTime) {
				Skeleton = returnSkeleton(RecordedCounter);
				RecordedCounter++;
				if(Skeleton.time == SharedVariables.AngleTransistionsTimes [TimesIndex])
				{
					ShowRecordedAngles (Skeleton);
					MoveText.text = "Move: " + TimesIndex + " on second: " +
						SharedVariables.AngleTransistionsTimes [TimesIndex];
					TimesIndex++;
					show = false;
					FirstTime = true;
				}
			}
		}

	}

	// Update is called once per frame
	void Update () {
		double SecondPassed = (Time.timeSinceLevelLoad);
		if (KinectFbxRecorder.ready && SharedVariables.Playing &&
			(SecondPassed < (SharedVariables.PlayingExerciseTime  + KinectFbxRecorder.StartTime)) 
			&& SecondPassed > (KinectFbxRecorder.StartTime))
		{
			if(!show){
				Skeleton = returnSkeleton(RecordedCounter);
				RecordedCounter++;
			}
			if (TimesIndex < SharedVariables.AngleTransistionsTimes.Count &&
				Skeleton.time == SharedVariables.AngleTransistionsTimes [TimesIndex]  ) {
				show = true;
				if ((SecondPassed - (KinectFbxRecorder.StartTime +10)) >= (Skeleton.time)) {
					MoveText.text = "Move: " + TimesIndex + " on second: " +
						SharedVariables.AngleTransistionsTimes [TimesIndex];
					ShowRecordedAngles (Skeleton);
					TimesIndex++;
					show = false;
				}
			}
		}
		if (SecondPassed > (SharedVariables.PlayingExerciseTime + KinectFbxRecorder.StartTime) && FirstTime) {
			TimesIndex = 0;
			FirstTime = false;
			FilePath = "ExercisesCSV/" + SharedVariables.PlayingExerciseName + ".CSV";
			fileReader = new StreamReader (FilePath);
			SetAngleElements ();
			ReadLineFromFile();
			for (int i = 0; i < (FrameStart * SharedVariables.TrackedJoints.Count); i++) {
				Skeleton =  returnSkeleton(i);
			}
			RecordedCounter = FrameStart;
			while(!FirstTime) {
				Skeleton = returnSkeleton(RecordedCounter);
				RecordedCounter++;
				if(Skeleton.time == SharedVariables.AngleTransistionsTimes [TimesIndex])
				{
					ShowRecordedAngles (Skeleton);
					MoveText.text = "Move: " + TimesIndex + " on second: " +
						SharedVariables.AngleTransistionsTimes [TimesIndex];
					TimesIndex++;
					show = false;
					FirstTime = true;
				}
			}
			FirstTime = false;
		}
		if (KinectFbxRecorder.ready && SharedVariables.Playing &&
			(SecondPassed < ((2*SharedVariables.PlayingExerciseTime)+ 10 + KinectFbxRecorder.StartTime))
			&& SecondPassed > (KinectFbxRecorder.StartTime + SharedVariables.PlayingExerciseTime + 10)) {
			if(!show){
				Skeleton = returnSkeleton(RecordedCounter);
				RecordedCounter++;
			}
			if (TimesIndex < SharedVariables.AngleTransistionsTimes.Count &&
				Skeleton.time == SharedVariables.AngleTransistionsTimes [TimesIndex]  ) {
				show = true;
				if ((SecondPassed - (KinectFbxRecorder.StartTime +10)) >= (Skeleton.time)) {
					MoveText.text = "Move: " + TimesIndex + " on second: " +
						SharedVariables.AngleTransistionsTimes [TimesIndex];
					ShowRecordedAngles (Skeleton);
					TimesIndex++;
					show = false;
				}
			}
			if (PlayingCounter % 5 == 0)
			{
				ShowPlayingAngles();
			}
			PlayingCounter++;
		}

		if((SecondPassed > ((SharedVariables.PlayingExerciseTime*2) + 10 + KinectFbxRecorder.StartTime)) && SharedVariables.Playing)
			RemoveAngleElements();
	}

	void ShowRecordedAngles(Entity.Skeleton s){
		double SecondPassed = (Time.timeSinceLevelLoad);
		bool Type0 = true;
		bool Type2 = true;
		for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++) {
			int joint = SharedVariables.TrackedJoints [i];
			bool needed = CheckAngleType (joint);
			int type = 0;
			if (joint == 0)
			if (Type0) 
				Type0 = false;
			else
				type = 1;
			if (joint == 2)
			if (Type2) 
				Type2 = false;
			else
				type = 1;
			if (needed) {
				AngleInfo	angleInfo = GetAngleInfo (joint, type);
				Entity.position x1 = s.joints [angleInfo.x].GetPosition ();
				Entity.position y1 = s.joints [joint].GetPosition ();
				Entity.Vector xy1 = new Entity.Vector (x1, y1).Normalize ();
				Entity.position x2 = s.joints [joint].GetPosition ();
				Entity.position y2 = s.joints [angleInfo.y].GetPosition ();
				Entity.Vector xy2 = new Entity.Vector (x2, y2).Normalize ();
				double cosAng = Math.Acos (dotVector (xy1,
					xy2));
				double angle = cosAng * (180 / Math.PI);
				RecordedAngles [angleInfo.TextBox].text =""+ angle.ToString("F1");
			}
		}
	}
	void ShowPlayingAngles(){
		double SecondPassed = (Time.timeSinceLevelLoad);
		bool Type0 = true;
		bool Type2 = true;
		if (SecondPassed > 10) {
			KinectManager manager = KinectManager.Instance;
			if (manager && manager.IsInitialized ()) {
				if (manager.IsUserDetected (playerIndex)) {
					long userId = manager.GetUserIdByIndex (playerIndex);
					for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++) {
						int joint = SharedVariables.TrackedJoints [i];
						int type = 0;
						if (joint == 0)
						if (Type0)
							Type0 = false;
						else
							type = 1;
						if (joint == 2)
						if (Type2)
							Type2 = false;
						else
							type = 1;
						bool needed = CheckAngleType (joint);
						if (needed) {
							AngleInfo angleInfo = GetAngleInfo (joint, type); 
							if (manager.IsJointTracked (userId, joint) && manager.IsJointTracked (userId, angleInfo.x)
								&& manager.IsJointTracked (userId, angleInfo.y)) {

								Vector3 jointPosX1 = manager.GetJointPosition (userId, angleInfo.x);
								Entity.position x1 = new position (jointPosX1.x, jointPosX1.y, jointPosX1.z);
								Vector3 jointPosY1 = manager.GetJointPosition (userId, joint);
								Entity.position y1 = new position (jointPosY1.x, jointPosY1.y, jointPosY1.z);
								Entity.Vector xy1 = new Entity.Vector (x1, y1).Normalize ();
								Vector3 jointPosX2 = manager.GetJointPosition (userId, joint);
								Entity.position x2 = new position (jointPosX2.x, jointPosX2.y, jointPosX2.z);
								Vector3 jointPosY2 = manager.GetJointPosition (userId, angleInfo.y);
								Entity.position y2 = new position (jointPosY2.x, jointPosY2.y, jointPosY2.z);
								Entity.Vector xy2 = new Entity.Vector (x2, y2).Normalize ();

								double cosAng = Math.Acos (dotVector (xy1,xy2));
								double angle = cosAng * (180 / Math.PI);
								PlayingAngles[angleInfo.TextBox].text = "" + angle.ToString("F1");
							} else 
								PlayingAngles [angleInfo.TextBox].text = "X";
						}
					}

				} else
					Debug.Log ("Not Detected");
			}
		}

	}

	public Skeleton returnSkeleton(int f){
		Entity.joint[] joints = new Entity.joint[20];
		int t = -1;
		for (int i = 0; i < 20; i++) {
			joints [i] = new Entity.joint (new position (0, 0, 0), 0, 0);
		}
		for(int i = 0; i < SharedVariables.TrackedJoints.Count;i++){
			Entity.joint j = ReadLineFromFile();
			if (j != null) {
				if (i == 0)
					t = j.time;	
				if (j.Frame == f && j.type == SharedVariables.TrackedJoints[i]) {
					joints [SharedVariables.TrackedJoints[i]] = new Entity.joint (j.Position, j.type, j.type);
				} else {
					joints [i] = new Entity.joint (new position (0, 0, 0), 0, 0);
				}
			}else
				joints [i] = new Entity.joint(new position(0,0,0),0,0);
		}
		Entity.Skeleton s = new Entity.Skeleton(joints);
		s.time = t;
		return s;
	}

	private Entity.joint ReadLineFromFile()
	{
		if (fileReader == null) {
			Debug.LogError("the file can not be accessed");
			return null;
		}
		// read a line
		string sPlayLine = fileReader.ReadLine();
		if (sPlayLine == null) {
			return (new Entity.joint(new position(0,0,0),0,0));
		}
		// extract the unity time and the body frame

		char[] delimiters = { ',' };
		string[] sLineParts = sPlayLine.Split(delimiters);
		if(sLineParts.Length == 6)
		{
			double d;
			double.TryParse (sLineParts [0], out d);
			int t = Convert.ToInt32 (d);
			int type = Skeleton.jointString(sLineParts[1]);
			float xAxis;
			float yAxis;
			float zAxis;
			float.TryParse(sLineParts[2], out xAxis);
			float.TryParse(sLineParts[3], out yAxis);
			float.TryParse(sLineParts[4], out zAxis);
			position p = new position(xAxis,yAxis,zAxis);
			int f;
			int.TryParse (sLineParts [5], out f);
			Entity.joint j = new joint(p,type,f,t);
			return j;

		}else
			return (new Entity.joint(new position(0, 0, 0), 0, 0));
	}
	public class AngleVector{
		public int x1;
		public int y1;
		public int x2;
		public int y2;
		public double angle;
		public	AngleVector(int X1,int Y1,int X2,int Y2){
			x1 = X1;
			y1 = Y1;
			x2 = X2;
			y2 = Y2;
		}
	}
	public static double  dotVector(Vector a , Vector b ){
		return((a.getX()*b.getX())+(a.getY()*b.getY())+(a.getZ()*b.getZ()));
	}

	public bool CheckAngleType(int a){
		switch (a){
		case(1):
			return(TrackedJointsList.Contains (0) && TrackedJointsList.Contains (2));
		case(2):
			return(TrackedJointsList.Contains (3) && TrackedJointsList.Contains (4)) ||(TrackedJointsList.Contains (3) && TrackedJointsList.Contains (8)) ;
		case (4):
			return(TrackedJointsList.Contains (2) && TrackedJointsList.Contains (5));
		case(5):
			return(TrackedJointsList.Contains (4) && TrackedJointsList.Contains (6));
		case(6):
			return(TrackedJointsList.Contains (5) && TrackedJointsList.Contains (7));
		case(8):
			return(TrackedJointsList.Contains (2) && TrackedJointsList.Contains (9));
		case(9):
			return(TrackedJointsList.Contains (8) && TrackedJointsList.Contains (10));
		case(10):
			return(TrackedJointsList.Contains (9) && TrackedJointsList.Contains (11));
		case(0):
			return(TrackedJointsList.Contains (1) && TrackedJointsList.Contains (12)) || (TrackedJointsList.Contains (1) && TrackedJointsList.Contains (16));
		case(12):
			return(TrackedJointsList.Contains (0) && TrackedJointsList.Contains (13));
		case(13):
			return(TrackedJointsList.Contains (12) && TrackedJointsList.Contains (14));
		case(14):
			return(TrackedJointsList.Contains (13) && TrackedJointsList.Contains (15));
		case(16):
			return(TrackedJointsList.Contains (0) && TrackedJointsList.Contains (17));
		case(17):
			return(TrackedJointsList.Contains (16) && TrackedJointsList.Contains (18));
		case(18):
			return(TrackedJointsList.Contains (17) && TrackedJointsList.Contains (19));
		}
		return false;
	}

	public AngleInfo GetAngleInfo(int a ,int Type){ 	 		
		switch (a){
		case(1):
			return(new AngleInfo(0,2,12));
		case(2):
			if (Type == 0)
				return new AngleInfo (3, 4, 13);
			else
				return new AngleInfo (3, 8, 14);
		case (4):
			return new AngleInfo (2, 5, 0);
		case(5):
			return new AngleInfo (4, 6, 1);
		case(6):
			return new AngleInfo (5, 7, 15);
		case(8):
			return new AngleInfo (2, 9, 2);
		case(9):
			return new AngleInfo (8, 10, 3);
		case(10):
			return new AngleInfo (9, 11, 16);
		case(0):
			if (Type == 0)
				return new AngleInfo (1, 12, 4);
			else
				return new AngleInfo (1, 16, 8);
		case(12):
			return new AngleInfo (0, 13, 5);
		case(13):
			return new AngleInfo (12, 14, 6);
		case(14):
			return new AngleInfo (13, 15, 7);
		case(16):
			return new AngleInfo (0, 17, 9);
		case(17):
			return new AngleInfo (16, 18, 10);
		case(18):
			return new AngleInfo (17, 19, 11);
		}
		return new AngleInfo (0, 0, 0);
	}

	public class AngleInfo{
		public int x;
		public int y;
		public int TextBox;
		public AngleInfo(int a,int b, int t){
			x = a;
			y = b;
			TextBox = t;

		}
	}
	void SetAngleElements(){
		RecodedAnglesImage.gameObject.SetActive(true);
		PlayingAnglesImage.gameObject.SetActive(true);
		RecorededAnglesText.gameObject.SetActive(true);
		PlayingAnglesText.gameObject.SetActive(true);
		MoveText.gameObject.SetActive (true);
	}
	void RemoveAngleElements()
	{
		RecodedAnglesImage.gameObject.SetActive(false);
		PlayingAnglesImage.gameObject.SetActive(false);
		RecorededAnglesText.gameObject.SetActive(false);
		PlayingAnglesText.gameObject.SetActive(false);
		MoveText.gameObject.SetActive (false);
	}
}
