using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine.SceneManagement;


public class KinectFbxRecorder : MonoBehaviour 
{
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("Whether the avatar finger orientations are allowed or not.")]
	private bool fingerOrientations = false;

	[Tooltip("Whether the avatar's feet must stick to the ground.")]
	public bool groundedFeet = false;

	[Tooltip("Smooth factor used for avatar movements and joint rotations.")]
	public float smoothFactor = 10f;

	// path to the file to load
	[Tooltip("Path to the fbx file, used to load the model.")]
	public string loadFilePath;

	[Tooltip("Path to the fbx file, used to save the model and animation.")]
	public string saveFilePath = string.Empty;

	[Tooltip("Whether to append timestamp to the save file name. If this setting is enabled, each recording will be saved to a separate fbx-file.")]
	public bool timestampSaveFileName = false;

	[Tooltip("Whether to load the model from save-to path, if the save-file exists.")]
	public bool loadSaveFileIfExists = false;

	[Tooltip("Name of the saved animation.")]
	public string animationName = string.Empty;

	[Tooltip("Unity scale factor of the loaded fbx model.")]
	public float importScaleFactor = 1f;

	[Tooltip("Output file format. Leave empty for the default format.")]
	public string outputFileFormat = string.Empty;

	[Tooltip("Maximum FPS (frame rate) for saving animation frames. The time between frames will be minimum 1/FPS seconds.")]
	public float maxFramesPerSecond = 0f;

	[Tooltip("Joint position is saved, if the distance to last position is greater than the threshold (in meters).")]
	public float jointDistanceThreshold = 0.01f;

	[Tooltip("Joint orientation is saved, if the angle to last orientation is greater than the threshold (in degrees).")]
	public float jointAngleThreshold = 0.5f;

	[Tooltip("Whether not to include the root position into the saved animation.")]
	public bool dontSaveRootPos = false;

	[Tooltip("Whether not to include the clavicle orientations into the saved animation.")]
	public bool dontSaveClavicles = false;

	[Tooltip("Joints that will not be included into saved animation.")]
	public List<KinectInterop.JointType> dontSaveJoints = new List<KinectInterop.JointType>();

	[Tooltip("UI-Text to display information messages.")]
	public UnityEngine.UI.Text infoText;

	[Tooltip("Whether to start recording, right after the scene starts.")]
	public bool recordAtStart = false;

	[Tooltip("Sound to play when the recording is started.")]
	public AudioSource soundRecStarted;

	[Tooltip("Sound to play when the recording is stopped.")]
	public AudioSource soundRecStopped;


	// the avatar object
	private GameObject avatarObject = null;

	// the recorded model
	private GameObject recordedObject = null;

	// whether it is recording at the moment
	private bool isRecording = false;

	// reference to KinectManager
	private KinectManager manager = null;

	// reference to AvatarController
	private AvatarController avatarCtrl = null;

	// initial avatar position
	private Vector3 initialPos = Vector3.zero;

	// last saved positions and rotations of the avatar joints
	private Vector3[] lastSavedPos;
	private Quaternion[] lastSavedRot;

	// local rotations of the bone transform
	private Quaternion[] alJointPreRots = null;
	private Quaternion[] alJointPostRots = null;

	// time variables used for recording
	private long liRelTime = 0;
	private float fStartTime = 0f;
	private float fCurrentTime = 0f;
	private float fMinFrameTime = 0f;
	private int iSavedFrame = 0;

	// unity scale factor
	private float fUnityScale = 0f;

	// fbx global time mode as FPS
	private float fGlobalFps = 0f;

	// fbx global scale factor
	private float fFbxScale = 0f;

	// if the fbx wrapper is available
	private bool bFbxAvailable = false;

	// if the fbx-system is successfully initialized
	private bool bFbxInited = false;

	// if the fbx-file was successfully loaded
	private bool bFbxLoaded = false;

	// if the fbx file is modified and needs to be saved
	private bool bFbxDirty = false;

	public Text Timer;
	public Text Reps;
	public Button Evaluation;
	public Button ReturnToStart;
	private int Minutes;
	private int Seconds;
	private bool RecordStarted;
	private bool RecordStopped;
	private bool PlayedStarted;
	private bool PlayedStopped;
	private bool FormExerciseStarted;
	private bool FormExerciseStopped;
	private bool FormExercisePlayingStarted;
	private bool FormExercisePlayingStoped;
	private GetJointPositionDemo GetjointPosition;
	public Button StartButton;
	public static bool ready;
	public static double StartTime;
	public GameObject KinectObject;
	public GameObject Event;
	public Image left;
	public Image right;
	public bool FormPlayingStop;
	private bool StopTimer;
	private bool RecordShow;

	// starts recording
	public bool StartRecording()
	{
		if(avatarObject == null)
			return false;

		if(isRecording)
			return false;

		isRecording = true;

		// load the fbx file
		if(!LoadInputFile())
		{
			isRecording = false;

			Debug.LogError("File could not be loaded: " + loadFilePath);
			if(infoText != null)
			{
				infoText.text = "File could not be loaded: " + loadFilePath;
			}
		}

		// check for save file name
		if(saveFilePath == string.Empty)
		{
			isRecording = false;

			Debug.LogError("No file to save.");
			if(infoText != null)
			{
				infoText.text = "No file to save.";
			}
		}

		// create the animation stack, if animation name is specified
		if(animationName != string.Empty && !MocapFbxWrapper.CreateAnimStack(animationName))
		{
			isRecording = false;

			//throw new Exception("Could not create animation: " + animationName);
			Debug.LogError("Could not create animation: " + animationName);
			if(infoText != null)
			{
				infoText.text = "Could not create animation: " + animationName;
			}
		}

		if(isRecording && animationName != string.Empty)
		{
			MocapFbxWrapper.SetCurrentAnimStack(animationName);
		}

		if(isRecording)
		{
			Debug.Log("Recording started.");
			if(infoText != null)
			{
				infoText.text = "Recording... Say 'Stop' to stop the recorder.";
			}

			if (soundRecStarted && !soundRecStarted.isPlaying) 
			{
				soundRecStarted.Play();
			}

			// get initial avatar position
			if(avatarCtrl != null)
			{
				int hipsIndex = avatarCtrl.GetBoneIndexByJoint(KinectInterop.JointType.SpineBase, false);
				Transform hipsTransform = avatarCtrl.GetBoneTransform(hipsIndex);
				initialPos = hipsTransform != null ? hipsTransform.position : Vector3.zero;
			}

			// initialize times
			fStartTime = fCurrentTime = Time.time;
			fMinFrameTime = maxFramesPerSecond > 0f ? 1f / maxFramesPerSecond : 0f;
			iSavedFrame = 0;

			// get global time mode as fps
			fGlobalFps = MocapFbxWrapper.GetGlobalFps();

			// get global scale factor
			fFbxScale = 100f / MocapFbxWrapper.GetGlobalScaleFactor();

			// Unity scale factor
			fUnityScale = importScaleFactor != 0f ? 1f / importScaleFactor : 1f;
		}

		return isRecording;
	}


	// stops recording
	public void StopRecording()
	{
		if(isRecording)
		{
			if (soundRecStopped && !soundRecStopped.isPlaying) 
			{
				soundRecStopped.Play();
			}

			SaveOutputFileIfNeeded();
			isRecording = false;

			Debug.Log("Recording stopped.");
			if(infoText != null)
			{
				infoText.text = "Recording stopped.";
			}
		}

		if(infoText != null)
		{
			infoText.text = "Say: 'Record' to start the recorder.";
		}
	}


	// returns if file recording is in progress at the moment
	public bool IsRecording()
	{
		return isRecording;
	}


	// ----- end of public functions -----


	// loads the specified fbx file
	private bool LoadInputFile()
	{
		if(!bFbxInited)
			return false;

		if(!SaveOutputFileIfNeeded())
			throw new Exception("Could not save the modified file to: " + saveFilePath);

		string sLoadFbxFile = loadFilePath;
		if (loadSaveFileIfExists && saveFilePath != string.Empty && File.Exists (saveFilePath)) 
		{
			sLoadFbxFile = saveFilePath;
		}

		if(sLoadFbxFile != string.Empty)
			bFbxLoaded = MocapFbxWrapper.LoadFbxFile(sLoadFbxFile, false);
		else
			bFbxLoaded = false;

		if (avatarCtrl && manager && manager.IsInitialized ()) 
		{
			// get the pre- and post-rotations
			int jointCount = manager.GetJointCount ();

			lastSavedPos = new Vector3[jointCount + 2];
			lastSavedRot = new Quaternion[jointCount + 2];

			for (int j = 0; j < (jointCount + 2); j++) 
			{
				lastSavedPos [j] = Vector3.zero;
				lastSavedRot [j] = Quaternion.identity;
			}

			alJointPreRots = new Quaternion[jointCount + 2];
			alJointPostRots = new Quaternion[jointCount + 2];

			for (int j = 0; j < jointCount; j++) 
			{
				int boneIndex = avatarCtrl.GetBoneIndexByJoint ((KinectInterop.JointType)j, false);
				Transform jointTransform = avatarCtrl.GetBoneTransform (boneIndex);

				alJointPreRots [j] = GetJointPreRot (jointTransform);
				alJointPostRots [j] = GetJointPostRot (jointTransform);
			}

			// get special transforms
			int specIndex = avatarCtrl.GetSpecIndexByJoint (KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder, false);
			Transform specTransform = avatarCtrl.GetBoneTransform (specIndex);
			alJointPreRots [jointCount] = GetJointPreRot (specTransform);
			alJointPostRots [jointCount] = GetJointPostRot (specTransform);

			specIndex = avatarCtrl.GetSpecIndexByJoint (KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder, false);
			specTransform = avatarCtrl.GetBoneTransform (specIndex);
			alJointPreRots [jointCount + 1] = GetJointPreRot (specTransform);
			alJointPostRots [jointCount + 1] = GetJointPostRot (specTransform);
		} 
		else 
		{
			bFbxLoaded = false;
		}

		bFbxDirty = false;

		return bFbxLoaded;
	}


	// saves the specified fbx file
	private bool SaveOutputFile()
	{
		if(saveFilePath == string.Empty)
		{
			saveFilePath = loadFilePath;
		}

		string _saveFilePath = timestampSaveFileName ? AddTimestampToSaveFileName(saveFilePath) : saveFilePath;

		// recorded animator controller
		int iP = _saveFilePath.LastIndexOf('/');
		string animatorFileName = (iP >= 0 ? _saveFilePath.Substring(0, iP) : "Assets") + "/RecordedAnim.controller";
		string animatorMetaName = (iP >= 0 ? _saveFilePath.Substring(0, iP) : "Assets") + "/RecordedAnim.controller.meta";

		// delete the old csv file
		if(_saveFilePath != string.Empty && File.Exists(_saveFilePath))
		{
			File.Delete(_saveFilePath);

			string saveMetaFilePath = _saveFilePath + ".meta";
			if (File.Exists(saveMetaFilePath)) 
			{
				File.Delete(saveMetaFilePath);
			}

			if (File.Exists(animatorFileName)) 
			{
				File.Delete(animatorFileName);
			}

			if (File.Exists(animatorMetaName)) 
			{
				File.Delete(animatorMetaName);
			}

			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
			#endif
		}

		if(_saveFilePath != string.Empty)
		{
			if (MocapFbxWrapper.SaveFbxFile(_saveFilePath, outputFileFormat, true, true, false)) 
			{
				// copy the meta file path, too
				string loadMetaFilePath = loadFilePath + ".meta";
				string saveMetaFilePath = _saveFilePath + ".meta";

				if (File.Exists(loadMetaFilePath)) 
				{
					File.Copy(loadMetaFilePath, saveMetaFilePath, true);
				}

				#if UNITY_EDITOR
				UnityEditor.AssetDatabase.Refresh(); //.ImportAsset(saveFilePath);

				// create animator controller with single clip
				AnimationClip animationClip = GetRecordedAnimationClip(_saveFilePath);
				if (animationClip)
					UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPathWithClip(animatorFileName, animationClip);
				else
					UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(animatorFileName);

				UnityEditor.AssetDatabase.Refresh (); //.ImportAsset(saveFilePath);
				#endif

				bFbxDirty = false;

				// play the animation
				//				AnimationPlayer animPlayer = AnimationPlayer.Instance;
				//				if (animPlayer) 
				//				{
				//					animPlayer.PlayAnimationClip(_saveFilePath, animationName);
				//				} 
				//				else
				{
					// load the recorded model
					InstatiateRecordedModel(_saveFilePath);
				}

				return true;
			}
			else 
			{
				Debug.LogError("Could not save mo-cap to: " + _saveFilePath);
			}
		}

		return false;
	}


	// saves the fbx file, if it was modified
	private bool SaveOutputFileIfNeeded()
	{
		if(bFbxDirty)
		{
			return SaveOutputFile();
		}

		return true;
	}

	// adds time stamp to the save file name
	private string AddTimestampToSaveFileName(string saveFilePath)
	{
		if (saveFilePath == string.Empty)
			return string.Empty;

		int iP = saveFilePath.LastIndexOf('.');
		string _saveFilePath = iP >= 0 ? saveFilePath.Substring(0, iP) : saveFilePath;

		_saveFilePath += "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".fbx";

		return _saveFilePath;
	}


	// returns the saved animation clip
	private AnimationClip GetRecordedAnimationClip(string saveFilePath)
	{
		if (saveFilePath == string.Empty)
			return null;

		#if UNITY_EDITOR
		// get the objects in the saved file
		UnityEngine.Object[] fbxObjects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(saveFilePath);

		if(fbxObjects != null)
		{
			foreach(UnityEngine.Object fbxObject in fbxObjects)
			{
				if(fbxObject is AnimationClip && fbxObject.name == animationName)
				{
					AnimationClip fbxAnimClip = (AnimationClip)fbxObject;
					return fbxAnimClip;
				}
			}
		}
		#endif

		return null;
	}

	// loads the avatar object from the input fbx file
	private bool InstatiateLiveReplay()
	{
		if(loadFilePath == string.Empty)
			return false;

		#if UNITY_EDITOR
		UnityEngine.Object avatarModel = UnityEditor.AssetDatabase.LoadAssetAtPath(loadFilePath, typeof(UnityEngine.Object));
		if(avatarModel == null)
			return false;

		if(avatarObject != null) 
		{
			GameObject.Destroy(avatarObject);
		}

		avatarObject = (GameObject)GameObject.Instantiate(avatarModel);
		if(avatarObject != null)
		{
			if (SharedVariables.Playing)
				avatarObject.transform.position = new Vector3(-2f, 0f, -0.17f);// Vector3.zero;
			else
				avatarObject.transform.position = new Vector3(-1.1f, 0f, -0.17f);

			avatarObject.transform.rotation =  Quaternion.identity;

			AvatarController ac = avatarObject.AddComponent<AvatarController>();
			ac.playerIndex = playerIndex;

			ac.mirroredMovement = false;
			ac.verticalMovement = true;

			ac.smoothFactor = smoothFactor;
			ac.fingerOrientations = fingerOrientations;
			ac.groundedFeet = groundedFeet;

			ac.Awake();

			KinectManager km = KinectManager.Instance;

			if(km)
			{
				if(km.IsUserDetected())
				{
					ac.SuccessfulCalibration(km.GetPrimaryUserID());
				}

				km.avatarControllers.Clear();
				km.avatarControllers.Add(ac);
			}
		}

		return true;
		#else
		return false;
		#endif
	}


	// loads the recorded model from the output fbx file
	private bool InstatiateRecordedModel(string saveFilePath)
	{
		if(saveFilePath == string.Empty)
			return false;

		#if UNITY_EDITOR
		UnityEngine.Object recordedModel = UnityEditor.AssetDatabase.LoadAssetAtPath(saveFilePath, typeof(UnityEngine.Object));
		if(recordedModel == null)
			return false;

		if(recordedObject != null) 
		{
			GameObject.Destroy(recordedObject);
		}

		// instantiate the recorded model
		recordedObject = (GameObject)GameObject.Instantiate(recordedModel);
		if(recordedObject != null)
		{
			recordedObject.transform.position = new Vector3(-10f, -1f, -100f); // Vector3.zero;
			recordedObject.transform.rotation =  Quaternion.identity;

			// recorded animator controller
			int iP = saveFilePath.LastIndexOf('/');
			string animatorFileName = (iP >= 0 ? saveFilePath.Substring(0, iP) : "Assets") + "/RecordedAnim.controller";

			if(File.Exists(animatorFileName))
			{
				UnityEditor.Animations.AnimatorController animatorController = UnityEditor.AssetDatabase.LoadAssetAtPath(animatorFileName, typeof(UnityEngine.Object)) 
					as UnityEditor.Animations.AnimatorController;

				Animator animator = recordedObject.GetComponent<Animator>();
				animator.applyRootMotion = false;

				if(animator && animatorController)
				{
					animator.runtimeAnimatorController = animatorController;
				}
			}

			AnimationPlayer animPlayer = recordedObject.AddComponent<AnimationPlayer>();
			animPlayer.PlayAnimationClip(saveFilePath, animationName);

		}

		return true;
		#else
		return false;
		#endif
	}


	// returns pre-rotation of a joint (fbx node)
	private Quaternion GetJointPreRot(Transform jointTransform)
	{
		if(jointTransform != null)
		{
			string sJointName = jointTransform.name;
			Vector3 vPreRot = Vector3.zero;
			MocapFbxWrapper.GetNodePreRot(sJointName, ref vPreRot);

			Quaternion qPreRot = Quaternion.identity;
			MocapFbxWrapper.Rot2Quat(ref vPreRot, ref qPreRot);

			qPreRot.y = -qPreRot.y;
			qPreRot.z = -qPreRot.z;

			return qPreRot;
		}

		return Quaternion.identity;
	}


	// returns post-rotation of a joint (fbx node)
	private Quaternion GetJointPostRot(Transform jointTransform)
	{
		if(jointTransform != null)
		{
			string sJointName = jointTransform.name;
			Vector3 vPostRot = Vector3.zero;
			MocapFbxWrapper.GetNodePostRot(sJointName, ref vPostRot);

			Quaternion qPostRot = Quaternion.identity;
			MocapFbxWrapper.Rot2Quat(ref vPostRot, ref qPostRot);

			qPostRot.y = -qPostRot.y;
			qPostRot.z = -qPostRot.z;

			return qPostRot;
		}

		return Quaternion.identity;
	}


	// saves needed animation frames at the current save-time
	private bool SaveAnimFrame(int jointIndex, Transform jointTransform, StringBuilder sbDebug)
	{
		if(jointTransform == null)
			return false;

		string sJointName = jointTransform.name;
		bool bSuccess = true;

		float fSaveTime = (float)iSavedFrame / fGlobalFps;

		if(jointIndex == 0 && !dontSaveRootPos)
		{
			// save avatar position
			Vector3 currentPos = jointTransform.position - initialPos;
			currentPos.x = -currentPos.x;
			currentPos.y += initialPos.y;

			float distToLast = (currentPos - lastSavedPos[jointIndex]).magnitude;
			if (distToLast >= jointDistanceThreshold) 
			{
				lastSavedPos[jointIndex] = currentPos;

				if (sbDebug != null) 
				{
					sbDebug.AppendFormat ("{0}:{1:F2} ", jointIndex, distToLast);
				}

				Vector3 vJointPos = currentPos * fUnityScale * fFbxScale;
				bSuccess &= MocapFbxWrapper.SetAnimCurveTrans(sJointName, fSaveTime, ref vJointPos);
			}
		}

		// save joint orientation
		Quaternion qJointRot = jointTransform.localRotation;
		float angleToLast = Quaternion.Angle(lastSavedRot[jointIndex], qJointRot);

		if (angleToLast >= jointAngleThreshold) 
		{
			lastSavedRot[jointIndex] = qJointRot;

			if (sbDebug != null) 
			{
				sbDebug.AppendFormat ("{0}:{1:F0} ", jointIndex, angleToLast);
			}

			if(alJointPreRots != null && alJointPostRots != null)
			{
				Quaternion qJointPreRot =  alJointPreRots[jointIndex];
				Quaternion qJointPostRot = alJointPostRots[jointIndex];

				qJointRot = Quaternion.Inverse(qJointPreRot) * qJointRot * Quaternion.Inverse(qJointPostRot);
			}

			qJointRot.x = -qJointRot.x;
			qJointRot.w = -qJointRot.w;

			Vector3 vJointRot = Vector3.zero;
			MocapFbxWrapper.Quat2Rot(ref qJointRot, ref vJointRot);

			bSuccess &= MocapFbxWrapper.SetAnimCurveRot(sJointName, fSaveTime, ref vJointRot);
		}

		if(iSavedFrame == 0)
		{
			// save the node scale in the first frame only
			Vector3 vJointScale = jointTransform.localScale;
			MocapFbxWrapper.SetAnimCurveScale(sJointName, 0f, ref vJointScale);
		}

		return bSuccess;
	}


	void Awake()
	{
		try 
		{
			// ensure the fbx wrapper is available
			bool bNeedRestart = false;
			if(MocapFbxWrapper.EnsureFbxWrapperAvailability(ref bNeedRestart))
			{
				bFbxAvailable = true;

				if(bNeedRestart)
				{
					//KinectInterop.RestartLevel(gameObject, "MF");
					return;
				}
			}
			else
			{
				throw new Exception("Fbx-Unity-Wrapper is not available!");
			}
		} 
		catch (Exception ex) 
		{
			Debug.LogError(ex.Message);
			Debug.LogException(ex);

			if(infoText != null)
			{
				infoText.text = ex.Message;
			}
		}
	}


	void Start()
	{

		Timer.text = "00:10";
		//DeactivateButtons ();
		InitializeSaveFilePath ();
		InitializeLoadFilePath ();
		Timer.gameObject.SetActive(false);
		StartTime = 0;
		ready = false;
		//Debug.Log (SharedVariables.RecordingExercise);q
		ReturnToStart.onClick.AddListener (() => {
			SharedVariables.SetDefaultsBools();
			SharedVariables.SetDefaultStrings();
			SharedVariables.SetDefaultStrings();
			DefaultBools();
			Application.LoadLevel("GameStart");
		});
		Evaluation.onClick.AddListener (() => {
			SharedVariables.SetDefaultsBools();
			DefaultBools();
			Application.LoadLevel ("Evaluation");
		});
		StartButton.onClick.AddListener (() => {
			Event.GetComponent<KinectInputModule>().enabled = false;
			ready = true;
			StartButton.gameObject.SetActive(false);
			Timer.gameObject.SetActive(true);
			disableHands ();
			StartTime = (Time.timeSinceLevelLoad);
		});

		// check if fbx-wrapper is available
		if (!bFbxAvailable)
			return;
		try 
		{

			{
				manager = KinectManager.Instance;
			}

			if(!manager)
			{
				throw new Exception("KinectManager not found, probably not initialized. See the log for details");
			}

			if(!bFbxInited)
			{
				bFbxInited = MocapFbxWrapper.InitFbxWrapper();

				if(!bFbxInited)
				{
					throw new Exception("Fbx wrapper could not be initialized.");
				}
			}

		} 
		catch (Exception ex) 
		{
			Debug.LogError(ex.Message);
			Debug.LogException(ex);

			if(infoText != null)
			{
				infoText.text = ex.Message;
			}
		}
	}


	void Update () 
	{

		RecordingUpdate ();
		Minutes = (int)((Time.timeSinceLevelLoad-(StartTime+13)) / 60f);
		Seconds = ((int)((Time.timeSinceLevelLoad-(StartTime+13)) % 60f));
		double SecondPassed = (Time.timeSinceLevelLoad);
		double s = (10+StartTime) - (SecondPassed);

		if ( ready && SecondPassed > (10 + StartTime) && SharedVariables.FormedExerciseRecording && !FormExerciseStarted) {
			FormExerciseStarted = true;
			InstantiateAvatar();
			Timer.text = "Start Recording";
			Invoke("DisableTimer", 3f);
			StartRecording();
			RecordStarted = true;
		}
		if (FormExerciseStarted && SecondPassed > (SharedVariables.FormExerciseTime+StartTime+10) && SharedVariables.FormedExerciseRecording && !FormExerciseStopped) {
			FormExerciseStopped = true;
			StopRecording();
			GameObject.Destroy(recordedObject);
			GameObject.Destroy(avatarObject);
			ReturnToStart.gameObject.SetActive(true);
			StopTimer = true;
			Event.GetComponent<KinectInputModule>().enabled = true;			
			Invoke("DisableTimer", 2f);
			enableHands ();
		}
		if(ready && SecondPassed > (10 +StartTime) && SharedVariables.FormExercisePlay && !FormExercisePlayingStarted )
		{
			FormExercisePlayingStarted = true;
			Timer.text = "Recorded Exercise Started";
			InstatiateRecordedModel(saveFilePath);
			Invoke("DisableTimer", 2f);
		}
		if (FormExercisePlayingStarted  && SecondPassed > (SharedVariables.FormExerciseTime+10+StartTime) && SharedVariables.FormExercisePlay && !FormExercisePlayingStoped)
		{
			FormExercisePlayingStoped = true;
			Timer.gameObject.SetActive(true);
			Timer.text = "Start Playing";
			GameObject.Destroy(recordedObject);
			InstantiateAvatar();
			Invoke("DisableTimer", 3f);
		}
		if( SharedVariables.FormExercisePlay && SecondPassed > (SharedVariables.FormPlayingSeconds+SharedVariables.FormExerciseTime+10+StartTime) && !FormPlayingStop){
			FormPlayingStop = true;
			GameObject.Destroy(avatarObject);
			ReturnToStart.gameObject.SetActive(true);
			StopTimer = true;
			Reps.text = "Final Number of Reps: " + ExerciseTrack.ExercisePerformed;
			Invoke("DisableTimer", 2f);

		}

		if (ready && SecondPassed > (10+StartTime) && SharedVariables.RecordingExercise && !RecordStarted  )
		{
			RecordStarted = true;
			Timer.text = "Start Recording";
			InstantiateAvatar();
			RecordStarted = true;
			Invoke("DisableTimer", 3f);
			StartRecording ();
		}

		if (SharedVariables.RecordingExercise && RecordStarted && !RecordStopped && SecondPassed > (SharedVariables.RecordedExerciseTime +10+StartTime)) {
			RecordStopped = true;
			StopRecording ();
			Debug.Log("stopped");
			GameObject.Destroy(avatarObject);
			GameObject.Destroy(recordedObject, SharedVariables.RecordedExerciseTime);
			ReturnToStart.gameObject.SetActive(true);
			StopTimer = true;
			Event.GetComponent<KinectInputModule>().enabled = true;
			Invoke("DisableTimer", 2f);
			enableHands ();
		}

		if (ready && SharedVariables.Playing && SecondPassed > StartTime  && !RecordShow) {
			RecordShow = true;
			Timer.text = "Recorded Exercise Started";
			InstatiateRecordedModel(saveFilePath);
			Invoke("DisableTimer", 3f);
		}
		if (ready && SharedVariables.Playing && SecondPassed > (StartTime+SharedVariables.PlayingExerciseTime+10) && !PlayedStarted) {
			PlayedStarted = true;
			Timer.text = "Start Playing";
			StopTimer = true;
			InstantiateAvatar();
			InstatiateRecordedModel(saveFilePath);
			Invoke("DisableTimer", 3f);
		}
		if (SharedVariables.Playing && SecondPassed > ((SharedVariables.PlayingExerciseTime*2) + 10 + StartTime) && PlayedStarted && !PlayedStopped) {
			PlayedStopped = true;
			GameObject.Destroy(avatarObject);
			GameObject.Destroy(recordedObject);
			Evaluation.gameObject.SetActive(true);
			Event.GetComponent<KinectInputModule>().enabled = true;
			Invoke("DisableTimer", 2f);
			enableHands();
		}

		if (ready && SecondPassed < (10+StartTime) && Timer != null)
			Timer.text = Minutes.ToString("00") + ":" + s.ToString("00");

		if (!SharedVariables.FormExercisePlay && ready && ! StopTimer) {
			if (SharedVariables.Playing && SecondPassed > (3 + StartTime)) {
				EnableTimer ();
				Timer.text = Minutes.ToString ("00") + ":" + Seconds.ToString ("00");
			}else {
				if (SecondPassed > (13 + StartTime)) {
					EnableTimer ();
					Timer.text = Minutes.ToString ("00") + ":" + Seconds.ToString ("00");
				}
			}
		}
		if (SharedVariables.Playing) {
			if (SecondPassed > (StartTime + SharedVariables.PlayingExerciseTime) && !StopTimer) {
				EnableTimer ();
				Timer.text = "00" + ":" + (StartTime + 10 + SharedVariables.PlayingExerciseTime - SecondPassed).ToString ("00");
			} else {
				if(SecondPassed < (StartTime + 10 + (SharedVariables.PlayingExerciseTime*2))){
					int M = (int)((Time.timeSinceLevelLoad-(SharedVariables.PlayingExerciseTime+StartTime+13)) / 60f);
					int S = ((int)((Time.timeSinceLevelLoad-(SharedVariables.PlayingExerciseTime+StartTime+13)) % 60f));
					EnableTimer ();
					Timer.text = M.ToString ("00") + ":" + S.ToString ("00");
				}
			}
		}

		if( SharedVariables.FormExercisePlay && ready && SecondPassed > (13+ StartTime + SharedVariables.FormExerciseTime)){
			double RemainSeconds = (SharedVariables.FormPlayingSeconds +13+SharedVariables.FormExerciseTime+StartTime)-SecondPassed;
			if(RemainSeconds >= 0){
				int Rmins = (int)(RemainSeconds / 60f);
				int RSeconds = ((int)(RemainSeconds% 60f));
				EnableTimer ();
				Timer.text = Rmins.ToString("00") + ":" + RSeconds.ToString("00");
			}	
		}	


	}

	void OnDestroy()
	{
		// check if fbx-wrapper is available
		if(!bFbxAvailable)
			return;

		// finish recording, if needed
		isRecording = false;

		// save the fbx file, if needed
		if(!SaveOutputFileIfNeeded())
		{
			//Debug.LogError("Could not save the modified fbx to: " + saveFilePath);
		}

		// terminate the fbx wrapper
		if(bFbxInited)
		{
			MocapFbxWrapper.TermFbxFrapper();
			bFbxInited = false;
		}
	}

	public void StartEvaluation()
	{
		Application.LoadLevel("Evaluation");
	}
	public void ReturnToGameStart()
	{
		Application.LoadLevel("GameStart");
	}
	private void InstantiateAvatar(){
		if(InstatiateLiveReplay() && avatarObject)
		{
			avatarCtrl = avatarObject.gameObject.GetComponent<AvatarController>();
		}
		else
		{
			if(loadFilePath == string.Empty)
				throw new Exception("Please specify fbx-file to load.");
			else
				throw new Exception("Cannot instantiate avatar from file: " + loadFilePath);
		}
	}
	private void InitializeSaveFilePath(){
		if (SharedVariables.RecordingExercise)
			saveFilePath = "Assets/Models/RecordedExercises/" + ""  + SharedVariables.RecordedExerciseName+".fbx";
		if (SharedVariables.Playing)
			saveFilePath = "Assets/Models/RecordedExercises/" + "" + SharedVariables.PlayingExerciseName + ".fbx";
		if (SharedVariables.FormedExerciseRecording)
			saveFilePath = "Assets/Models/FormedExercises/" + "" + SharedVariables.FormExerciseName+".fbx";
		if(SharedVariables.FormExercisePlay)
			saveFilePath = "Assets/Models/FormedExercises/"+"" + SharedVariables.FormExerciseName+".fbx";
	}
	private void InitializeLoadFilePath(){
		if (SharedVariables.RecordingExercise || SharedVariables.FormedExerciseRecording)
			loadFilePath = "Assets/Models/LiveReplay.fbx";
		else
			loadFilePath = "Assets/Models/Passive_Marker_Man 1.fbx";
	}
	private void DeactivateButtons(){
		Evaluation.gameObject.SetActive(false);
		ReturnToStart.gameObject.SetActive(false);
	}
	void DisableTimer()
	{
		Timer.gameObject.SetActive(false);
	}
	void EnableTimer()
	{
		Timer.gameObject.SetActive(true);
	}
	void DefaultBools()
	{
		RecordStarted = false;
		RecordStopped = false;
		PlayedStarted = false;
		PlayedStopped = false;
		FormExerciseStarted = false;
		FormExerciseStopped = false;
		FormExercisePlayingStarted = false;
		FormExercisePlayingStoped = false;
	}
	private void RecordingUpdate(){
		if(isRecording && animationName != string.Empty)
		{
			// save the body frame, if any
			if(manager && manager.IsInitialized())
			{
				long userId = manager.GetUserIdByIndex(playerIndex);
				long liFrameTime = manager.GetBodyFrameTimestamp();

				if(userId != 0 && avatarCtrl && liFrameTime != liRelTime &&
					Time.time >= (fCurrentTime + fMinFrameTime))
				{
					//Debug.Log("Saving @ time: " + Time.time + ", delta: " + (Time.time - fCurrentTime));

					liRelTime = liFrameTime;
					fCurrentTime = Time.time;

					float fRelTime = fCurrentTime - fStartTime;
					int iCurrentFrame = Mathf.FloorToInt(fRelTime * fGlobalFps);

					if(infoText)
					{
						infoText.text = string.Format("Recording @ {0:F3}... Say 'Stop' to stop the recorder.", fRelTime);
					}

					int jointCount = manager.GetJointCount();
					//bool bSavedOnce = false;

					StringBuilder sbBuf = new StringBuilder();
					const char delimiter = ',';

					//Quaternion qRootRotation = Quaternion.identity;
					//int iSavedFrame1 = iSavedFrame;

					while(iSavedFrame <= iCurrentFrame)
					{
						StringBuilder sbDebug = null; // new StringBuilder();

						if (sbDebug != null) 
						{
							sbDebug.AppendFormat ("{0} - ", iSavedFrame);
						}

						// save properties of all joints
						for(int j = 0; j < jointCount; j++)
						{
							if(dontSaveJoints.Count > 0 && dontSaveJoints.Contains((KinectInterop.JointType)j))
								continue;

							int boneIndex = avatarCtrl.GetBoneIndexByJoint((KinectInterop.JointType)j, false);
							Transform jointTransform = avatarCtrl.GetBoneTransform(boneIndex);

							if(jointTransform != null)
							{
								bool bSuccess = SaveAnimFrame(j, jointTransform, sbDebug);

								if(!bSuccess)
								{
									string sMessage = string.Format("Could not save animation frame at time: {0:F3}", fRelTime);

									infoText.text = sMessage;
									Debug.LogError(sMessage);
								}

								bFbxDirty = bSuccess;
							}
						}

						// save properties of the special joints
						if (!dontSaveClavicles) 
						{
							int specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderLeft, KinectInterop.JointType.SpineShoulder, false);
							Transform specTransform = avatarCtrl.GetBoneTransform(specIndex);
							bFbxDirty |= SaveAnimFrame(jointCount, specTransform, sbDebug);

							specIndex = avatarCtrl.GetSpecIndexByJoint(KinectInterop.JointType.ShoulderRight, KinectInterop.JointType.SpineShoulder, false);
							specTransform = avatarCtrl.GetBoneTransform(specIndex);
							bFbxDirty |= SaveAnimFrame(jointCount + 1, specTransform, sbDebug);
						}

						if (sbDebug != null) 
						{
							Debug.Log(sbDebug.ToString ());
						}

						//bSavedOnce = true;
						iSavedFrame++;
					}

					// remove the last delimiter
					if(sbBuf.Length > 0 && sbBuf[sbBuf.Length - 1] == delimiter)
					{
						sbBuf.Remove(sbBuf.Length - 1, 1);
					}

				}
			}
		}

	}
	public void eval(){
		Application.LoadLevel ("Evaluation");
	}
	public void enableHands(){
		left.gameObject.SetActive (true);
		right.gameObject.SetActive (true);

	}
	public void disableHands(){
		left.gameObject.SetActive (false);
		right.gameObject.SetActive (false);
	}
}

