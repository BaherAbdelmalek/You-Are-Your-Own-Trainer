using UnityEngine;
//using Windows.Kinect;

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// KinectGestures is utility class that processes programmatic Kinect gestures
/// </summary>
public class KinectGestures : MonoBehaviour
{

	/// <summary>
	/// This interface needs to be implemented by all Kinect gesture listeners
	/// </summary>
	public interface GestureListenerInterface
	{
		/// <summary>
		/// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="userIndex">User index</param>
		void UserDetected(long userId, int userIndex);
		
		/// <summary>
		/// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="userIndex">User index</param>
		void UserLost(long userId, int userIndex);
		
		/// <summary>
		/// Invoked when a gesture is in progress.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <param name="userIndex">User index</param>
		/// <param name="gesture">Gesture type</param>
		/// <param name="progress">Gesture progress [0..1]</param>
		/// <param name="joint">Joint type</param>
		/// <param name="screenPos">Normalized viewport position</param>
		void GestureInProgress(long userId, int userIndex, Gestures gesture, float progress, 
		                       KinectInterop.JointType joint, Vector3 screenPos);

		/// <summary>
		/// Invoked if a gesture is completed.
		/// </summary>
		/// <returns><c>true</c>, if the gesture detection must be restarted, <c>false</c> otherwise.</returns>
		/// <param name="userId">User ID</param>
		/// <param name="userIndex">User index</param>
		/// <param name="gesture">Gesture type</param>
		/// <param name="joint">Joint type</param>
		/// <param name="screenPos">Normalized viewport position</param>
		bool GestureCompleted(long userId, int userIndex, Gestures gesture,
		                      KinectInterop.JointType joint, Vector3 screenPos);

		/// <summary>
		/// Invoked if a gesture is cancelled.
		/// </summary>
		/// <returns><c>true</c>, if the gesture detection must be retarted, <c>false</c> otherwise.</returns>
		/// <param name="userId">User ID</param>
		/// <param name="userIndex">User index</param>
		/// <param name="gesture">Gesture type</param>
		/// <param name="joint">Joint type</param>
		bool GestureCancelled(long userId, int userIndex, Gestures gesture, 
		                      KinectInterop.JointType joint);
	}
	

	/// <summary>
	/// The gesture types.
	/// </summary>
	public enum Gestures
	{
		None = 0,
	}
	
	
	/// <summary>
	/// Programmatic gesture data container.
	/// </summary>
	public struct GestureData
	{
		public long userId;
		public Gestures gesture;
		public int state;
		public float timestamp;
		public int joint;
		public Vector3 jointPos;
		public Vector3 screenPos;
		public float tagFloat;
		public Vector3 tagVector;
		public Vector3 tagVector2;
		public float progress;
		public bool complete;
		public bool cancelled;
		public List<Gestures> checkForGestures;
		public float startTrackingAtTime;
	}
	

	/// <summary>
	/// Gets the list of gesture joint indexes.
	/// </summary>
	/// <returns>The needed joint indexes.</returns>
	/// <param name="manager">The KinectManager instance</param>
	public virtual int[] GetNeededJointIndexes(KinectManager manager)
	{
		return new int[0];
	}
	

	// estimate the next state and completeness of the gesture
	/// <summary>
	/// estimate the state and progress of the given gesture.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="gestureData">Gesture-data structure</param>
	/// <param name="timestamp">Current time</param>
	/// <param name="jointsPos">Joints-position array</param>
	/// <param name="jointsTracked">Joints-tracked array</param>
	public virtual void CheckForGesture(long userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
	{
	}

}
