using UnityEngine;
using System.Collections;
using System.IO;

public class GetJointPositionDemo : MonoBehaviour 
{
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("The Kinect joint we want to track.")]
	public KinectInterop.JointType joint = KinectInterop.JointType.HandRight;

	[Tooltip("Current joint position in Kinect coordinates (meters).")]
	public Vector3 jointPosition;

	[Tooltip("Whether we save the joint data to a CSV file or not.")]
	public bool isSaving = false;

	[Tooltip("Path to the CSV file, we want to save the joint data to.")]
	public string saveFilePath;
	
	[Tooltip("How many seconds to save data to the CSV file, or 0 to save non-stop.")]
	public double secondsToSave ;


	// start time of data saving to csv file
	private double saveStartTime = -1;
    private KinectInterop.JointType[] joints;
    private static int frame ;
    private static bool done;

    void Start()
	{
		if(SharedVariables.RecordingExercise || SharedVariables.Playing){
		InitializeVariables ();
        if (isSaving && File.Exists(saveFilePath))
		   {
			File.Delete(saveFilePath);
		    }
		}
    }

    void Update() 
	{
        double SecondPassed = (Time.timeSinceLevelLoad);

		if (KinectFbxRecorder.ready && isSaving && (SharedVariables.Playing || SharedVariables.RecordingExercise))
		{
			// create the file, if needed
			if(!File.Exists(saveFilePath))
			{
				using(StreamWriter writer = File.CreateText(saveFilePath))
				{
					// csv file header
					string sLine = "time,joint,pos_x,pos_y,poz_z,frame";
					writer.WriteLine(sLine);
				}
				InitializeSecondsToSave ();
			}
				
		}

		// get the joint position
		KinectManager manager = KinectManager.Instance;

		if(KinectFbxRecorder.ready && manager && manager.IsInitialized() && (SharedVariables.Playing || SharedVariables.RecordingExercise)
			&& isSaving && SecondPassed > (saveStartTime) && SecondPassed < (secondsToSave) )
		{
			if(manager.IsUserDetected(playerIndex))
			{

				long userId = manager.GetUserIdByIndex(playerIndex);
				for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++)
                {
					if (manager.IsJointTracked(userId, (int)(SharedVariables.TrackedJoints[i])))
				    {
                   
                        // output the joint position for easy tracking
						Vector3 jointPos = manager.GetJointPosition(userId,  (int)(SharedVariables.TrackedJoints[i]));
					    jointPosition = jointPos;
					
						using (StreamWriter writer = File.AppendText (saveFilePath)) {
							string sLine = string.Format ("{0:F3},{1},{2:F3},{3:F3},{4:F3}", (SecondPassed - saveStartTime), ((KinectInterop.JointType)(SharedVariables.TrackedJoints [i])).ToString (), jointPos.x, jointPos.y, jointPos.z) + "," + frame;
							writer.WriteLine (sLine);
						}
					}else
						using (StreamWriter writer = File.AppendText (saveFilePath)) {
							string sLine = string.Format ("{0:F3},{1},{2:F3},{3:F3},{4:F3}", (SecondPassed - saveStartTime), ((KinectInterop.JointType)(SharedVariables.TrackedJoints [i])).ToString (), 0, 0, 0) + "," + frame;
							writer.WriteLine (sLine);
						}
						
                }
				frame++;
            }

        }
		if ( (SharedVariables.Playing || SharedVariables.RecordingExercise) && 
			SecondPassed > secondsToSave && !done) {
			using (StreamWriter writer = File.AppendText (saveFilePath)) {
				string sLine = "frames," + (frame - 1);
				writer.WriteLine (sLine);
				File.Exists (saveFilePath);
			}
			done = true;
		}
			
    }
	void InitializeVariables(){
		if(SharedVariables.Playing){
			saveFilePath = "ExercisesCSV/" + SharedVariables.PlayingExerciseName + "Playing.CSV";
			secondsToSave = SharedVariables.PlayingExerciseTime;
		}
		if (SharedVariables.RecordingExercise) {
			saveFilePath = "ExercisesCSV/" + SharedVariables.RecordedExerciseName + ".CSV";
			secondsToSave = SharedVariables.RecordedExerciseTime;
		}
        done = false;
        frame = 0;
	}	
	void InitializeSecondsToSave(){
		if(SharedVariables.Playing){
			secondsToSave = SharedVariables.PlayingExerciseTime + 10 + KinectFbxRecorder.StartTime;
		}
		if (SharedVariables.RecordingExercise) {
			secondsToSave = SharedVariables.RecordedExerciseTime+ 10 + KinectFbxRecorder.StartTime;
		}
		saveStartTime = 10 + KinectFbxRecorder.StartTime;
	}
	public static int GetFrame(){
		return frame;
	}

}
