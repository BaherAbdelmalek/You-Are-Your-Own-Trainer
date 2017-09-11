using UnityEngine;
using System.Collections;


namespace Entity{
	
public class Skeleton {


	public  joint[] joints;
	public int time;
	public Skeleton(joint[] j ){
			this.joints = new joint[20];
			for (int i = 0; i < 20; i++) {
				this.joints [i] = j [i];
			}
	}



	public enum JointType : int
	{
		SpineBase = 0,
		SpineMid = 1,
		Neck = 2,
		Head = 3,
		ShoulderLeft = 4,
		ElbowLeft = 5,
		WristLeft = 6,
		HandLeft = 7,
		ShoulderRight = 8,
		ElbowRight = 9,
		WristRight = 10,
		HandRight = 11,
		HipLeft = 12,
		KneeLeft = 13,
		AnkleLeft = 14,
		FootLeft = 15,
		HipRight = 16,
		KneeRight = 17,
		AnkleRight = 18,
		FootRight = 19,
		SpineShoulder = 20,
		HandTipLeft = 21,
		ThumbLeft = 22,
		HandTipRight = 23,
		ThumbRight = 24
			//Count = 25
	}
        public static int jointString(string j)
        {
            switch( j)
            {
                case "SpineBase" : return 0;
		        case   "SpineMid": return 1;
		        case  "Neck": return 2;
		        case  "Head": return 3;
		        case   "ShoulderLeft": return 4;
		        case "ElbowLeft" : return 5;
                case "WristLeft": return 6;
		        case    "HandLeft": return 7;
		        case    "ShoulderRight": return 8;
		        case    "ElbowRight": return 9;
                case   "WristRight": return 10;
                case "HandRight": return 11;
                case "HipLeft": return 12;
                case "AnkleLeft": return 14;
                case "KneeLeft": return 13;
                case "FootLeft": return 15;
                case "HipRight": return 16;
                case "KneeRight": return 17;
                case "AnkleRigh": return 18;
                case "FootRight": return 19;
                case "SpineShoulder": return 20;
                case "HandTipLeft": return 21;
                case "ThumbLeft": return 22;
                case "HandTipRight": return 23;
                case "ThumbRight": return 24;
            }
            return -1;
        }
    }
}
