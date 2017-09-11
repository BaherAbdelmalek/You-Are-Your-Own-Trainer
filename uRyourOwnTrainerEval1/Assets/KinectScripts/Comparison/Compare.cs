using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using Core;
using Entity;
namespace Comparison{
	public class Compare  :MonoBehaviour  {
		public string FilePath1 = "";
		public string FilePath2 = "";
		public Text PosesText;
		public Text Movestext;
		public Text AngleText;
	    private Core.Initial firstInitial;
		private Core.Initial secondInitial;
	    private DataRead dataread1;
	    private  StreamReader fileReader1;
		private  StreamReader fileReader2;
	    private  double[] results1;
		private  double[] results2;
	    private  Translation translation1;
		private  Translation translation12;
		private  Translation translation2;
		private  Translation translation22;
		private int frames1;
		private int frames2;
		int minFrames ;
		private  Pose pose1;
		private  Pose pose2;
		static  double[] similarbones;
		static  double[] similarangles;
		public  Text[] PoseTexts;
		public  Text[] AngleTexts;
		private static List<AngleInfo> TrackedJointsInfo = new List<AngleInfo>();
		private static List<int> NeededPoses = new List<int>();
		private static ArrayList TrackedJointsList;
		public Button angleCompare;
		public Button poseCompare;
		public Button TimeEvaluate;
		public int FrameStart;
		public int TransIdx;
    
	void Start()
	{
			 FilePath1 = "ExercisesCSV/" + SharedVariables.PlayingExerciseName + "Playing.CSV";
			 FilePath2 = "ExercisesCSV/" + SharedVariables.PlayingExerciseName + ".CSV";
		 frames1 = getFrames (FilePath1);
		 frames2 = getFrames (FilePath2);
		 minFrames = Math.Min (frames1, frames2); 
		 firstInitial = new Initial();
		 secondInitial = new Initial();
		 fileReader1 = new StreamReader(FilePath1);
		 fileReader2 = new StreamReader(FilePath2);
			// ReadLineFromFile1();
			// bool initial1 = getInitials1();
			//ReadLineFromFile2();
			// bool initial2 = getInitials2();
		    TrackedListInitialize ();
			ButtonHandlers ();
			GetRequiredAngles ();
			GetRequiredPoses  ();
	}


	public bool getInitials1(){
				while ((firstInitial.IsInitialFinished()) == false) {
				firstInitial.Collect(returnSkeleton1(firstInitial.GetFrameID()));
		}
			results1 = firstInitial.GetInitialResults();
			translation1 =  new Translation (results1 [0], results1 [1], results1 [2], results1 [3]);
			translation12 = new Translation (results1 [0], results1 [1], results1 [2], results1 [3]);
			return true;
	}

		public bool getInitials2(){
			while ((secondInitial.IsInitialFinished()) == false) {
				secondInitial.Collect(returnSkeleton2(secondInitial.GetFrameID()));
			}
			results2 = secondInitial.GetInitialResults();
			translation2 = new Translation (results2 [0], results2 [1], results2 [2], results2 [3]);
			translation22 = new Translation (results2 [0], results2 [1], results2 [2], results2 [3]);
			return true;
		}

		public Skeleton returnSkeleton1(int f){
			Entity.joint[] joints = new Entity.joint[20];
			for (int i = 0; i < 20; i++) {
				joints [i] = new Entity.joint (new position (0, 0, 0), 0, 0);
			}
			int t = -1;
			for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++) {
				Entity.joint j = ReadLineFromFile1();
				if (j != null) {
					if (i == 0)
						t = j.time;	
					if (j.Frame == f) {
						joints [SharedVariables.TrackedJoints[i]] = new Entity.joint (j.Position, j.type, j.type);
					} else {
						//Debug.Log ("frame: " + j.Frame + " real frame: " + f);
						joints [SharedVariables.TrackedJoints[i]] = new Entity.joint (new position (0, 0, 0), 0, 0);
					}
				}else
					joints [SharedVariables.TrackedJoints[i]] = new Entity.joint(new position(0,0,0),0,0);
			}
			Entity.Skeleton s = new Entity.Skeleton(joints);
			s.time = t;
			return s;
		}

		public Skeleton returnSkeleton2(int f){
			Entity.joint[] joints = new Entity.joint[20];
			for (int i = 0; i < 20; i++) {
				joints [i] = new Entity.joint (new position (0, 0, 0), 0, 0);
			}
			int t = -1;
			for (int i = 0; i <SharedVariables.TrackedJoints.Count; i++) {
				Entity.joint j = ReadLineFromFile2();
				if (j != null) {
					if (i == 0)
						t = j.time;	
					if (j.Frame == f) {
						joints [SharedVariables.TrackedJoints[i]] = new Entity.joint (j.Position, j.type, j.Frame);
					} else {
						//Debug.Log ("frame: " + j.Frame + " real frame: " + f);
						joints [SharedVariables.TrackedJoints[i]] = new Entity.joint (new position (0, 0, 0), 0, 0);
					}
				}else
					joints [SharedVariables.TrackedJoints[i]] = new Entity.joint(new position(0,0,0),0,0);
			}
			Entity.Skeleton s = new Entity.Skeleton(joints);
			s.time = t;
			return s;
		}
		private Entity.joint ReadLineFromFile1()
		{
			if (fileReader1 == null) {
				Debug.LogError("the file can not be accessed");
				return null;
			}
			// read a line
			string sPlayLine = fileReader1.ReadLine();
			if (sPlayLine == null) {
				return (new Entity.joint(new position(0,0,0),0,0));
			}
			// extract the unity time and the body frame
			char[] delimiters = { ',' };
			string[] sLineParts = sPlayLine.Split(delimiters);
			int type = 0;
			int.TryParse(sLineParts[1],out type);
			double d;
			double.TryParse (sLineParts [0], out d);
			int t = Convert.ToInt32 (d);
			float xAxis;
			float yAxis;
			float zAxis;
			float.TryParse(sLineParts[2], out xAxis);
			float.TryParse(sLineParts[3], out yAxis);
			float.TryParse(sLineParts[4], out zAxis);
			position p = new position(xAxis,yAxis,zAxis);
			int f;
			int.TryParse (sLineParts [5], out f);
			//Debug.Log (f);
//			if(f > 120)
//				Debug.Log (sLineParts[1]+" frame: "+f+" x: "+xAxis +" y: "+yAxis);
			Entity.joint j = new joint(p,type,f,t);
			return j;
		}
		private Entity.joint ReadLineFromFile2()
		{
			if (fileReader2 == null) {
				Debug.LogError("the file can not be accessed");
				return null;
			}
			// read a line
			string sPlayLine = fileReader2.ReadLine();
			if (sPlayLine == null) {
				return (new Entity.joint(new position(0,0,0),0,0));
			}
			// extract the unity time and the body frame
			char[] delimiters = { ',' };
			string[] sLineParts = sPlayLine.Split(delimiters);
			int type = 0;
			int.TryParse(sLineParts[1],out type);
			double d;
			double.TryParse (sLineParts [0], out d);
			int t = Convert.ToInt32 (d);
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
		}
		public static double  dotVector(Vector a , Vector b ){
			return((a.getX()*b.getX())+(a.getY()*b.getY())+(a.getZ()*b.getZ()));
		}

		public double GetValueOfVector(Vector vector)
		{
			return Math.Sqrt(vector.getX() * vector.getX() + vector.getY() * vector.getY() + vector.getZ() * vector.getZ());
		}
		public position[] getpos(Skeleton s ){
			Entity.position[] positions = new position[20];
			for (int i = 0; i < 20; i++) {
				Entity.position p = s.joints[i].GetPosition();
				positions [i] = p; 
			}
			return positions;
		}
		public  void ComparePoses(){
			fileReader1 = new StreamReader (FilePath1);
			fileReader2 = new StreamReader (FilePath2);
			ReadLineFromFile1 ();
			ReadLineFromFile2 (); 
			for (int i = 0; i < (FrameStart*SharedVariables.TrackedJoints.Count); i++) {
				ReadLineFromFile1 ();
				ReadLineFromFile2 ();
			}
			double CBS = 0;
			double ABS = 0;
			similarbones = new double[20];
			for(int i = FrameStart ;i < minFrames ;i++) {
				double bs = 0;
				Skeleton s1 = returnSkeleton1(i);
				//position[] p1 = translation1.GetNewCoordinates(s1);
				position[] p1 = getpos(s1);
				SkeletonVector sv1= new SkeletonVector(p1);
				Entity.Vector[] bones1 = sv1.GetSkeletonVectors();
				position HC1 = p1[0];
				Entity.Vector Y1 = new Entity.Vector (HC1, new position (HC1.X, HC1.Y+1, HC1.Z));
				Skeleton s2 = returnSkeleton2(i);
				//position[] p2 = translation2.GetNewCoordinates(s2);
				position[] p2 = getpos(s2);
				position HC2 = p2 [0]; 
				Entity.Vector Y2 = new Entity.Vector (HC2, new position (HC2.X, HC2.Y+1, HC2.Z));
				SkeletonVector sv2= new SkeletonVector(p2);
				Entity.Vector[] bones2 = sv2.GetSkeletonVectors();
				for (int k = 0; k < NeededPoses.Count; k++) {
					Comparison.Quaternions Q = new Comparison.Quaternions ();
					Entity.Quaternion q1 = Q.GetQuaternionByVectors(bones1[NeededPoses[k]],Y1);
					Entity.Quaternion q2 = Q.GetQuaternionByVectors(bones2[NeededPoses[k]],Y2);
					Entity.Quaternion qinv = q1.Reciprocal();
					//Debug.Log(qinv.getX()+ " "+ qinv.getY()+" " +qinv.getZ()+" " +qinv.getW());
					Entity.Quaternion qp = times (qinv,q2);
					double n = mag(qp);
					Debug.Log (n);
						if (n <= 0.25){
						 bs++;
						Debug.Log (similarbones[NeededPoses[k]] + " " + n);
						similarbones[NeededPoses[k]]++;
						}
				}
				CBS = (bs / NeededPoses.Count) * 100;
				Debug.Log (CBS);
				ABS += CBS;
			}
			for (int i = 0; i < NeededPoses.Count; i++) {
				similarbones [NeededPoses[i]] /=(minFrames - FrameStart);
				similarbones [NeededPoses[i]] *= 100; 
			}
			ABS = (ABS / (minFrames - FrameStart));
			Debug.Log ("ABS : " + ABS);
			PosesText.text = "Avgerage Poses Score: " + ABS.ToString("F3") + "%";
			PosesAverage ();
		}
     	public void CompareMoves(){
			double CJS = 0;
			double AJS  =  0;
			Skeleton sPre1 = returnSkeleton1(121);
			position[] pre1 = getpos(sPre1);
			//position[] pre1 = translation1.GetNewCoordinates(sPre1);
			Skeleton sPre2 = returnSkeleton2(121);
			position[] pre2 = getpos(sPre2);
			//position[] pre2 = translation2.GetNewCoordinates(sPre2);
	    for (int i = 141; i < 232; i += 20) {
				for (int j = 0; j < 19; j++) {
					returnSkeleton1 (j);
					returnSkeleton2 (j);
				}
				double js = 0;
				Debug.Log (i);
				Debug.Log ("pre, joint: " + 0 + " " + pre1[0].X+ " " + pre1[0].Y + " " + pre1[0].Z);
				Debug.Log ("pre2, joint: " + 0 + " " + pre2[0].X+ " " + pre2[0].Y + " " + pre2[0].Z);
				Skeleton sCur1 = returnSkeleton1(i);
				position[] cur1 = translation12.GetNewCoordinates(sCur1);
				Displacement d1 = new Displacement (pre1,cur1);
				Skeleton sCur2 = returnSkeleton2(i);
				position[] cur2 = translation22.GetNewCoordinates(sCur2);
				Displacement d2 = new Displacement (pre2,cur2);
				Entity.Vector[] v1 = d1.GetAllVectors();
				Entity.Vector[] v2 = d2.GetAllVectors();
				for (int k = 0; k < 19; k++) {
					Entity.Vector m1 =   v1[k].Normalize();
					Entity.Vector m2 =   v2[k].Normalize();
					if (GetValueOfVector (m1) != 0 && GetValueOfVector (m2) != 0) {
						double cos = Math.Acos (dotVector (v1[k].Normalize(),
							             v2[k].Normalize()));
						double angle = cos * (180 / Math.PI);
						Debug.Log (" angle: " + angle);
						if (angle <= 11)
							js++;
					} else {
						js++;
					}
				}
				for (int k = 0; k < 20; k++) {
					pre1 [k] = cur1 [k];
					pre2 [k] = cur2 [k];
				}
				CJS = (js / 20) * 100;
				AJS  += CJS;	

			}
			AJS  = (AJS /(Math.Min(frames1,frames2)-121))*100 ;
			Debug.Log ("AJS : " +AJS );

		}

		public  void compareAngles(){
			fileReader1 = new StreamReader (FilePath1);
			fileReader2 = new StreamReader (FilePath2);
			ReadLineFromFile1 ();
			ReadLineFromFile2 (); 
			for (int i = 0; i < (FrameStart*SharedVariables.TrackedJoints.Count); i++) {
				ReadLineFromFile1 ();
				ReadLineFromFile2 ();
			}
			double avg = 0;
			similarangles = new double[20];
			for (int i = FrameStart; i < minFrames ; i++) {
				Skeleton s1 = returnSkeleton1(i);
			    //	position[] p1 = translation1.GetNewCoordinates(s1);
				position[] p1 = getpos(s1);
				SkeletonVector sv1= new SkeletonVector(p1);
				Skeleton s2 = returnSkeleton2(i);
				//position[] p2 = translation2.GetNewCoordinates(s2);
				position[] p2 = getpos(s2);
				SkeletonVector sv2= new SkeletonVector(p2);
				double eqAng = 0;
				for (int k = 0; k < TrackedJointsInfo.Count; k++) {
					Entity.Vector v11 = sv1.GetSkeletonVectors () [TrackedJointsInfo[k].x];
					Entity.Vector v12 = sv1.GetSkeletonVectors () [TrackedJointsInfo[k].y];
					double cos1 = Math.Acos (dotVector (v11.Normalize(),
						v12.Normalize()));
					double angle1 = cos1 * (180 / Math.PI);
					Entity.Vector v21 = sv2.GetSkeletonVectors () [TrackedJointsInfo[k].x];
					Entity.Vector v22 = sv2.GetSkeletonVectors () [TrackedJointsInfo[k].y];
					double cos2 = Math.Acos (dotVector(v21.Normalize(),v22.Normalize()));
					double angle2 = cos2 * (180 / Math.PI);
					Debug.Log ("angle1: " + angle1 + " angle2: " + angle2 +" "+ Math.Abs (angle1 - angle2));
					if (Math.Abs (angle1 - angle2) < 15){
						eqAng++;
						similarangles [TrackedJointsInfo[k].j]++;
					}
				}
				eqAng = (eqAng / TrackedJointsInfo.Count) * 100;
				Debug.Log (eqAng);
				avg += eqAng;
			}
			for (int i = 0; i < TrackedJointsInfo.Count; i++) {
				similarangles [TrackedJointsInfo[i].j] /=(minFrames - FrameStart);
				similarangles[TrackedJointsInfo[i].j] *= 100; 
			}
			avg = avg / (minFrames - FrameStart);
			Debug.Log (avg);
			AnglesAverage ();
			AngleText.text = "Average angles score: " + avg.ToString("F3") + "%";
		}

		public void CompareTransAngles(){
			fileReader1 = new StreamReader (FilePath1);
			fileReader2 = new StreamReader (FilePath2);
			ReadLineFromFile1 ();
			ReadLineFromFile2 (); 
			for (int i = 0; i < (FrameStart*SharedVariables.TrackedJoints.Count); i++) {
				ReadLineFromFile1 ();
				ReadLineFromFile2 ();
			 }
			bool read1 = false;
			bool read2 = false;
			int idx1 = FrameStart;
			int idx2 = FrameStart;
			Entity.joint[] joints = new Entity.joint[20];
			Entity.Skeleton s1 = new Skeleton (joints);
			Entity.Skeleton s2 = new Skeleton (joints);
			similarangles = new double[20];
			double avg = 0;
			while (TransIdx < SharedVariables.AngleTransistionsTimes.Count) {
				if (!read1) {
					s1 = returnSkeleton1 (idx1);
					idx1++;
					if (s1.time == SharedVariables.AngleTransistionsTimes [TransIdx])
						read1 = true;
				}
				if (!read2) {
					 s2 = returnSkeleton2(idx2);
					idx2++;
					if (s2.time == SharedVariables.AngleTransistionsTimes [TransIdx])
						read2 = true;
				}
				Debug.Log (s1.time + " " + s2.time);
				Debug.Log (idx1 + " " + idx2);
				if (read1 && read2) {
					position[] p1 = getpos(s1);
					SkeletonVector sv1= new SkeletonVector(p1);
					position[] p2 = getpos(s2);
					SkeletonVector sv2= new SkeletonVector(p2);
					double eqAng = 0;
					for (int k = 0; k < TrackedJointsInfo.Count; k++) {
						Entity.Vector v11 = sv1.GetSkeletonVectors () [TrackedJointsInfo[k].x];
						Entity.Vector v12 = sv1.GetSkeletonVectors () [TrackedJointsInfo[k].y];
						Debug.Log (v11.getX () + " " + v12.getX ());
						double cos1 = Math.Acos (dotVector (v11.Normalize(),
							v12.Normalize()));
						Debug.Log (cos1);
						double angle1 = cos1 * (180 / Math.PI);
						Entity.Vector v21 = sv2.GetSkeletonVectors () [TrackedJointsInfo[k].x];
						Entity.Vector v22 = sv2.GetSkeletonVectors () [TrackedJointsInfo[k].y];
						double cos2 = Math.Acos (dotVector(v21.Normalize(),v22.Normalize()));
						double angle2 = cos2 * (180 / Math.PI);
						Debug.Log ( " Trans" +  angle1 + " angle2: " + angle2 +" "+ Math.Abs (angle1 - angle2));
						if (Math.Abs (angle1 - angle2) < 10){
							eqAng++;
							similarangles[TrackedJointsInfo[k].j]++;
						}
					}
					eqAng = (eqAng / TrackedJointsInfo.Count) * 100;
					Debug.Log (eqAng);
					avg += eqAng;
					read1 = false;
					read2 = false;
					TransIdx++;
				}
					
			}
			for (int i = 0; i < TrackedJointsInfo.Count; i++) {
				similarangles [TrackedJointsInfo[i].j] /= (SharedVariables.AngleTransistionsTimes.Count);
				similarangles[TrackedJointsInfo[i].j] *= 100; 
			}
			avg = avg / ( SharedVariables.AngleTransistionsTimes.Count);
			Debug.Log (avg);
			AnglesAverage ();
			TransIdx = 0;
			AngleText.text = "Average angles score: " + avg.ToString("F3") + "%";
		}
		public void CompareTransPoses(){
			fileReader1 = new StreamReader (FilePath1);
			fileReader2 = new StreamReader (FilePath2);
			ReadLineFromFile1 ();
			ReadLineFromFile2 (); 
			double CBS = 0;
			double ABS = 0;
			for (int i = 0; i < (FrameStart*SharedVariables.TrackedJoints.Count); i++) {
				ReadLineFromFile1 ();
				ReadLineFromFile2 ();
			}
			bool read1 = false;
			bool read2 = false;
			int idx1 = FrameStart;
			int idx2 = FrameStart;
			Entity.joint[] joints = new Entity.joint[20];
			Entity.Skeleton s1 = new Skeleton (joints);
			Entity.Skeleton s2 = new Skeleton (joints);
			similarbones = new double[20];
			double avg = 0;
			while (TransIdx < SharedVariables.AngleTransistionsTimes.Count) {
				if (!read1) {
					s1 = returnSkeleton1 (idx1);
					idx1++;
					if (s1.time == SharedVariables.AngleTransistionsTimes [TransIdx])
						read1 = true;
				}
				if (!read2) {
					s2 = returnSkeleton2 (idx2);
					idx2++;
					if (s2.time == SharedVariables.AngleTransistionsTimes [TransIdx])
						read2 = true;
				}
				if (read1 && read2) {
					double bs = 0;
					//position[] p1 = translation1.GetNewCoordinates(s1);
					position[] p1 = getpos(s1);
					SkeletonVector sv1= new SkeletonVector(p1);
					Entity.Vector[] bones1 = sv1.GetSkeletonVectors();
					position HC1 = p1[0];
					Entity.Vector Y1 = new Entity.Vector (HC1, new position (HC1.X, HC1.Y+1, HC1.Z));
					//position[] p2 = translation2.GetNewCoordinates(s2);
					position[] p2 = getpos(s2);
					position HC2 = p2 [0]; 
					Entity.Vector Y2 = new Entity.Vector (HC2, new position (HC2.X, HC2.Y+1, HC2.Z));
					SkeletonVector sv2= new SkeletonVector(p2);
					Entity.Vector[] bones2 = sv2.GetSkeletonVectors();
					for (int k = 0; k < NeededPoses.Count; k++) {
						Comparison.Quaternions Q = new Comparison.Quaternions ();
						Entity.Quaternion q1 = Q.GetQuaternionByVectors(bones1[NeededPoses[k]],Y1);
						Entity.Quaternion q2 = Q.GetQuaternionByVectors(bones2[NeededPoses[k]],Y2);
						Entity.Quaternion qinv = q1.Reciprocal();
						Debug.Log(q1.getX()+ " "+ q1.getY()+" " +q1.getZ()+" " +q1.getW());
						Debug.Log(qinv.getX()+ " "+ qinv.getY()+" " +qinv.getZ()+" " +qinv.getW());
						Entity.Quaternion qp = times (qinv,q2);
						double n = mag(qp);
						Debug.Log (n);
						if (n <= 0.1){
							bs++;
							Debug.Log (similarbones[k] + " " + n);
							similarbones[NeededPoses[k]]++;
						}
					}
					CBS = (bs / NeededPoses.Count) * 100;
					ABS += CBS;
					read1 = false;
					read2 = false;
					TransIdx++;
				}
				for (int i = 0; i < NeededPoses.Count; i++) {
					similarbones [NeededPoses[i]] /=(SharedVariables.AngleTransistionsTimes.Count);
						similarbones [NeededPoses[i]] *= 100; 
				}

			}
			ABS = (ABS / (SharedVariables.AngleTransistionsTimes.Count));
			Debug.Log ("ABS : " + ABS);
			PosesText.text = "Avgerage Poses Score: " + ABS.ToString("F3") + "%";
			PosesAverage ();
			TransIdx = 0;
		}


		public int getFrames(String path){
			StreamReader fileReader = new StreamReader(path);
			if (fileReader == null) {
				Debug.LogError("the file can not be accessed");
				return -1;
			}
			string sPlayLine;
			int frames = -1;
			while(true){
			// extract the unity time and the body frame
			sPlayLine = fileReader.ReadLine() ;
			char[] delimiters = { ',' };
			string[] sLineParts = sPlayLine.Split(delimiters);
				if (sLineParts.Length == 2) {
					int.TryParse (sLineParts [1], out frames);
					return frames;
				}
			}
		}
		public void PoseCompare(){
			ComparePoses();
		}

		public void MoveCompare(){
			CompareMoves ();
		}
		public void AngleCompare(){
			compareAngles();
		}
		public void PosesAverage(){
			for (int i = 0; i < NeededPoses.Count; i++) {
				PoseTexts[NeededPoses[i]].text = similarbones[NeededPoses[i]].ToString("F2") + "%";
			}
				
		}
		public void AnglesAverage(){
			for (int i = 0; i < TrackedJointsInfo.Count; i++) {

					AngleTexts[TrackedJointsInfo[i].Textbox].text = similarangles[TrackedJointsInfo[i].j].ToString("F3") + "%";
			}
		}
		public void GetRequiredPoses(){
			for (int i = 0; i < TrackedJointsInfo.Count; i++) {
				if(!(NeededPoses.Contains(TrackedJointsInfo[i].x)))
					NeededPoses.Add(TrackedJointsInfo[i].x);
				if(!(NeededPoses.Contains(TrackedJointsInfo[i].y)))
					NeededPoses.Add(TrackedJointsInfo[i].y);
					}
			}
		public void GetRequiredAngles(){
			int type0 = 0;
			int type2 = 0;
			for (int i = 0; i < TrackedJointsList.Count; i++) {
				if ((int)TrackedJointsList[i] == 0){
					if(CheckAngleType(0))
						TrackedJointsInfo.Add(GetAngleInfo(0 , type0));
							type0 = 1;
				}
				if ((int)TrackedJointsList[i] == 2){
					if(CheckAngleType(2))
						TrackedJointsInfo.Add(GetAngleInfo(2 , type2));
						  type2 = 1;
				}
					if (CheckAngleType ((int)TrackedJointsList[i] ))
					TrackedJointsInfo.Add(GetAngleInfo((int)TrackedJointsList[i],0));
			}
		}
		void ButtonHandlers(){
			angleCompare.onClick.AddListener(() =>
				{
					compareAngles();
				});

			poseCompare.onClick.AddListener(() =>
				{
					ComparePoses();
				});
			TimeEvaluate.onClick.AddListener (() => {
				CompareTransAngles ();
				CompareTransPoses ();
			});
		}
		void TrackedListInitialize(){
			TrackedJointsList = new ArrayList();
			for (int i = 0; i < SharedVariables.TrackedJoints.Count; i++) {
				TrackedJointsList.Add(SharedVariables.TrackedJoints[i]);
			}
			if(TrackedJointsList.Contains(0))
				TrackedJointsList.Add(0);
			if(TrackedJointsList.Contains(2))
				TrackedJointsList.Add(2);
		}
		public double norm(Entity.Quaternion a) {
			return Math.Sqrt(a.getW()*a.getW() + a.getX()*a.getX() +a.getY()*a.getY() + a.getZ()*a.getZ());
		}
		public double mag(Entity.Quaternion a){
			return (Math.Abs(a.getW()-1)+Math.Abs(a.getX())+Math.Abs(a.getY())+Math.Abs(a.getZ()));
		}
		public Entity.Quaternion times(Entity.Quaternion a,Entity.Quaternion b) {
			double y0 = a.getW()*b.getW() - a.getX()*b.getX() - a.getY()*b.getY() - a.getZ()*b.getZ();
			double y1 = a.getW()*b.getX() + a.getX()*b.getW() + a.getY()*b.getZ() - a.getZ()*b.getY();
			double y2 = a.getW()*b.getY() - a.getX()*b.getZ() + a.getY()*b.getW() + a.getZ()*b.getX();
			double y3 = a.getW()*b.getZ() + a.getX()*b.getY() - a.getY()*b.getX() + a.getZ()*b.getW();
			return new Entity.Quaternion(y0, y1, y2, y3);
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
				return(new AngleInfo(1,0,1,0));
			case(2):
				if (Type == 0)
					return new AngleInfo (2,2,3,1);
				else
					return new AngleInfo (2,2,7,2);
			case (4):
				return new AngleInfo (4,3,4,3);
			case(5):
				return new AngleInfo (4,4,5,4);
			case(6):
				return new AngleInfo (6,5,6,5);
			case(8):
				return new AngleInfo (8,7, 8,6);
			case(9):
				return new AngleInfo (9,8,9,7);
			case(10):
				return new AngleInfo (10,9,10,15);
			case(0):
				if (Type == 0)
					return new AngleInfo (0,0,0,8);
				else
					return new AngleInfo (0,0,15,16);
			case(12):
				return new AngleInfo (12,11,12,9);
			case(13):
				return new AngleInfo (13,12,13,10);
			case(14):
				return new AngleInfo (14,13,14,13);
			case(16):
				return new AngleInfo (16,15,16,11);
			case(17):
				return new AngleInfo (17,16,17,12);
			case(18):
				return new AngleInfo (18,17,18,14);
			}
			return new AngleInfo (0,0, 0,17);
		}

	 public class AngleInfo{
			public int j;
			public int x;
			public int y;
			public int Textbox;
			public AngleInfo(int joint,int a,int b,int text){
				j = joint;
				x = a;
				y = b;
				Textbox = text;
			}
		}
			
	  class pair{
		 public int x;
		 public	int y;
			public pair(int X, int Y ){
				x = X;
				y = Y;
			}
		}

	}





 }


