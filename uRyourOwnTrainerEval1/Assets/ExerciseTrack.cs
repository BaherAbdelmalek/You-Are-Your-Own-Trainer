using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Entity;
public class ExerciseTrack : MonoBehaviour {

	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("The Kinect joint we want to track.")]
	public KinectInterop.JointType joint = KinectInterop.JointType.HandRight;

	[Tooltip("Current joint position in Kinect coordinates (meters).")]
	public Vector3 jointPosition;

	[Tooltip("Whether we save the joint data to a CSV file or not.")]
	public bool isSaving = false;

	[Tooltip("Path to the CSV file, we want to save the joint data to.")]
	public string saveFilePath = "joint_pos.csv";

	[Tooltip("How many seconds to save data to the CSV file, or 0 to save non-stop.")]
	public float secondsToSave = 0f;

	// start time of data saving to csv file
	private float saveStartTime = -1f;
	private KinectInterop.JointType[] joints;
	public  static int ExercisePerformed = 0;
	private static bool Did;
	private static bool PoseCalculated;
    private AngleVector[] anglesvectors;
    public Text Reps;
	public Text[] DefinedAngles;
	public Text[] PlayingAngles;
	public Text[] DefinedPoses;
	public Text[] PlayingPoses;
	public Image DefinedAnglesImage;
	public Image PlayingAnglesImage;
	public GameObject DefinedAnglesText;
	public GameObject PlayingAnglesText;
	public Image DefinedPosesImage;
	public Image PlayingPosesImage;
	public GameObject DefinedPosesText;
	public GameObject PlayingPosesText;
	public Text PreDefined;
	public Text Player;
	public Text Move;
    private int PreVTimes;
    private int CurTimes;
	private Vector3 HeadPos;
	private bool HeadTracked;
	private static int idx;

    void Start()
	{
		if (SharedVariables.FormExercisePlay) {
			DefineAngleVector ();
			Did = false;
			PreVTimes = -10;
			CurTimes = 0;
			idx = 0;
			if (SharedVariables.PLayWithAngles)
				SetAngleElements ();
			else
				SetPoseElements ();
			
			Move.text = "Move: 0 " + SharedVariables.FormExercisesDesc[idx];
		}
	}

	void Update() 
	{
		if (SharedVariables.FormExercisePlay && KinectFbxRecorder.ready ) {
			if (!HeadTracked) {
				TrackHead ();
				HeadTracked = true;
			}
			double SecondPassed = (Time.timeSinceLevelLoad);

			if (SecondPassed > (KinectFbxRecorder.StartTime + 10) && SecondPassed < ( KinectFbxRecorder.StartTime + SharedVariables.FormExerciseTime + 10)) {
				if (SecondPassed > (SharedVariables.FormTimeShow * idx)) {
					Move.text = "Move: " + idx + " " + SharedVariables.FormExercisesDesc [idx];
					if (SharedVariables.PLayWithAngles) {
						DifineAngles ();
					}
					if (SharedVariables.PlayWithPoses) {
						DifinePoses ();
					}
					idx++;
					if (idx == SharedVariables.FormExercisesDesc.Length)
						idx = 0;
				}
			}
			if (SecondPassed > (KinectFbxRecorder.StartTime + SharedVariables.FormExerciseTime + 10)
			    && SecondPassed < (KinectFbxRecorder.StartTime + SharedVariables.FormExerciseTime + 10 + SharedVariables.FormPlayingSeconds)) {
				idx = 0;
				Move.text = "Move: 0 " + SharedVariables.FormExercisesDesc[idx];
				if ((CurTimes - PreVTimes == 10)) {
					if (SharedVariables.PLayWithAngles) {
						TrackAngles ();
						DifineAngles ();
					}
					if (SharedVariables.PlayWithPoses) {
						TrackPoses ();
						DifinePoses ();
					}
				}
				CurTimes++;
			}
		}
    }

	public void TrackPoses(){
			KinectManager manager = KinectManager.Instance;
			if (manager && manager.IsInitialized ()) {
				if (manager.IsUserDetected (playerIndex)) {
				 bool TrueExercise = true;
				 long userId = manager.GetUserIdByIndex (playerIndex);
			      	bool jump = true;
				   for (int i = 0; i < SharedVariables.FormedPoses [idx].Count; i++) {
					if (SharedVariables.FormedPoses[idx] [i] !=  -1 ) {
						jump = false;
						break;
					  }
				  }
				if (jump) {
					Move.text = "Move: " + idx + " Jump";
					TrueExercise = TrackJump();
					PoseCalculated = false;
				}else {
					Entity.position[] positions = new Entity.position[20];
					for (int i = 0; i < 20; i++) {
						if (manager.IsJointTracked (userId, i)) {
							Vector3 jointPos = manager.GetJointPosition (userId, i);
							positions [i] = new Entity.position (jointPos.x, jointPos.y, jointPos.z);
						} else
							positions [i] = new position (0, 0, 0);
					}
					SkeletonVector sv = new SkeletonVector (positions);
					Entity.Vector[] bones = sv.GetSkeletonVectors ();
					position HC = positions [0];
					Entity.Vector Y = new Entity.Vector (new position (0, 0, 0), new position (0, 1, 0));
					for (int i = 0; i < SharedVariables.FormedPoses [idx].Count; i++) {
						if (SharedVariables.FormedPoses [idx] [i] != -1) {
							double cosAng = Math.Acos (dotVector (Y.Normalize (), bones [i].Normalize ()));
							double angle = cosAng * (180 / Math.PI);
							PlayingPoses [i].text = "" + angle.ToString ("F1");
							Debug.Log (i + angle);
							if (Math.Abs (angle - SharedVariables.FormedPoses [idx] [i]) > 15)
								TrueExercise = false;
						} else
							PlayingPoses [i].text = "X";
					}
				}
						if(TrueExercise && !PoseCalculated){
									PoseCalculated = true;
					                idx++;
					       if (idx == SharedVariables.FormedPoses.Count) {
						       ExercisePerformed ++;
						       Reps.text = "Reps: " + ExercisePerformed;
						       idx = 0;
					        }
					Move.text = "Move: " + idx + " " + SharedVariables.FormExercisesDesc[idx];
					        TrueExercise = false;
						}
						if(!TrueExercise)
						   PoseCalculated = false;
				}else
					Debug.Log("User Not Detected");
			}
        PreVTimes = CurTimes;
	}
	public void TrackAngles(){
		
			KinectManager manager = KinectManager.Instance;
			bool ExerciseDone = true;
			if (manager && manager.IsInitialized ()) {
				if (manager.IsUserDetected (playerIndex)) {
					long userId = manager.GetUserIdByIndex (playerIndex);
				    bool jump = true;
				for (int i = 0; i < SharedVariables.FormedAngles [idx].Count; i++) {
					if (SharedVariables.FormedAngles [idx] [i] !=  -1 ) {
						jump = false;
						break;
					}
				 }
				if (jump) {
					Move.text = "Move: " + idx + " Jump";
					ExerciseDone = TrackJump();
					Did = false;
				}
				else {
					for (int i = 0; i < anglesvectors.Length; i++) {
						AngleVector angVec = anglesvectors [i];
						if (manager.IsJointTracked (userId, angVec.x1) && manager.IsJointTracked (userId, angVec.y1)
						    && manager.IsJointTracked (userId, angVec.x2) && manager.IsJointTracked (userId, angVec.y2)) {
					
							// output the joint position for easy tracking
							Vector3 jointPosX1 = manager.GetJointPosition (userId, angVec.x1);
							Entity.position x1 = new position (jointPosX1.x, jointPosX1.y, jointPosX1.z);
							Vector3 jointPosY1 = manager.GetJointPosition (userId, angVec.y1);
							Entity.position y1 = new position (jointPosY1.x, jointPosY1.y, jointPosY1.z);
							Entity.Vector xy1 = new Entity.Vector (x1, y1).Normalize ();
							Vector3 jointPosX2 = manager.GetJointPosition (userId, angVec.x2);
							Entity.position x2 = new position (jointPosX2.x, jointPosX2.y, jointPosX2.z);
							Vector3 jointPosY2 = manager.GetJointPosition (userId, angVec.y2);
							Entity.position y2 = new position (jointPosY2.x, jointPosY2.y, jointPosY2.z);
							Entity.Vector xy2 = new Entity.Vector (x2, y2).Normalize ();

							double cosAng = Math.Acos (dotVector (xy1, xy2));
							double angle = cosAng * (180 / Math.PI);
							if (SharedVariables.FormedAngles [idx] [i] != -1) {
								PlayingAngles [i].text = "" + angle.ToString ("F1");
								if ((Math.Abs (angle - SharedVariables.FormedAngles [idx] [i])) > 15) {
									ExerciseDone = false;
								}
							}
						} else {
							if (SharedVariables.FormedAngles [idx] [i] != -1)
								ExerciseDone = false;
						}

					}
				}
				if (ExerciseDone && !Did) {
						Did = true;
					    idx++;
					 if (idx == SharedVariables.FormedAngles.Count) {
						ExercisePerformed++;
						Reps.text = "Reps: " + ExercisePerformed;
						idx = 0;
					   }
					Move.text = "Move: " + " " + SharedVariables.FormExercisesDesc[idx];
					ExerciseDone = false;
					}
					if (!ExerciseDone)
						Did = false;
				}else
					Debug.Log ("Not Detected");
			}
        PreVTimes = CurTimes;
	}

	void DifineAngles(){

		Debug.Log ( "idx: "+ idx);
		for (int i = 0; i < SharedVariables.FormedAngles[idx].Count; i++) {
			if ( SharedVariables.FormedAngles[idx][i] == -1)
				DefinedAngles [i].text = "X";
			else
				DefinedAngles[i].text = "" + SharedVariables.FormedAngles[idx][i];
		}
	}
	public void TrackHead(){
		KinectManager manager = KinectManager.Instance;
		if (manager && manager.IsInitialized ()) {
			if (manager.IsUserDetected (playerIndex)) {
				long userId = manager.GetUserIdByIndex (playerIndex);
				if (manager.IsJointTracked (userId, 3)){
					HeadPos =  manager.GetJointPosition (userId, 3);
				}
		    }		
		}
	}
	public bool TrackJump(){
		KinectManager manager = KinectManager.Instance;
		if (manager && manager.IsInitialized ()) {
			if (manager.IsUserDetected (playerIndex)) {
				long userId = manager.GetUserIdByIndex (playerIndex);
				if (manager.IsJointTracked (userId, 3)){
					Vector3 HeadPos1 =  manager.GetJointPosition (userId, 3);
					Debug.Log (HeadPos1.y + " " + HeadPos.y); 
					if (HeadPos1.y >= HeadPos.y + 0.1)
						return true;
				}
			}		
		}
		return false;
	}
	void DifinePoses(){
		for (int i = 0; i < SharedVariables.FormedPoses[idx].Count; i++) {
			if (SharedVariables.FormedPoses[idx][i] == -1)
				DefinedPoses[i].text = "X";
			else
				DefinedPoses[i].text = "" + SharedVariables.FormedPoses[idx][i];
		}
	}
	public static double  dotVector(Vector a , Vector b ){
		return((a.getX()*b.getX())+(a.getY()*b.getY())+(a.getZ()*b.getZ()));
		}

	void SetPoseElements(){
		DefinedPosesImage.gameObject.SetActive(true);
		PlayingPosesImage.gameObject.SetActive(true);
		DefinedPosesText.gameObject.SetActive(true);
		PlayingPosesText.gameObject.SetActive(true);
		Player.gameObject.SetActive(true);
		PreDefined.gameObject.SetActive(true);
		Reps.gameObject.SetActive (true);
		Move.gameObject.SetActive (true);
	}
	void SetAngleElements(){
		DefinedAnglesImage.gameObject.SetActive(true);
		PlayingAnglesImage.gameObject.SetActive(true);
		DefinedAnglesText.gameObject.SetActive(true);
		PlayingAnglesText.gameObject.SetActive(true);
		Player.gameObject.SetActive(true);
		PreDefined.gameObject.SetActive(true);
		Reps.gameObject.SetActive (true);
		Move.gameObject.SetActive (true);
	}
	void DefineAngleVector(){
		anglesvectors = new AngleVector[12];
		anglesvectors [0] = new AngleVector (2, 4, 4, 5);
		anglesvectors [1] = new AngleVector (4, 5, 5, 6);
		anglesvectors [2] = new AngleVector (2, 8, 8, 9);
		anglesvectors [3] = new AngleVector (8, 9, 9, 10);
		anglesvectors [4] = new AngleVector (0, 1, 0, 12);
		anglesvectors [5] = new AngleVector (0, 12, 12, 13);
		anglesvectors [6] = new AngleVector (12, 13, 13, 14);
		anglesvectors [7] = new AngleVector (13, 14, 14, 15);
		anglesvectors [8] = new AngleVector (0, 1, 0, 16);
		anglesvectors [9] = new AngleVector (0, 16, 16, 17);
		anglesvectors [10] = new AngleVector (16, 17, 17, 18);
		anglesvectors [11] = new AngleVector (17, 18, 18, 19);
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
}
