using UnityEngine;
using System.Collections;
using System.IO;
using Entity;

namespace Comparison{
public class DataRead 
{
		public string filePath = "joint_pos.csv";
		private  StreamReader fileReader = new StreamReader("joint_pos.csv"); 
  
    // Use this for initialization
    public void Intialize()
    {
        fileReader = new StreamReader(filePath);
        //ReadLineFromFile();
    }

   

		public Skeleton returnSkeleton(int f){
		//  fileReader = new StreamReader("joint_pos.csv");
			Entity.joint[] joints = new Entity.joint[20];
			for (int i = 0; i < 20; i++) {
				Entity.joint j = ReadLineFromFile();
				Debug.Log ("frame :" + f +"jframe:" +j.Frame);
				if (j != null) {
				  if (j.Frame == f) {
						Debug.Log ("ok");
						joints [i] = new Entity.joint(j.Position,j.type,j.type);
					}
					else
						joints [i] = new Entity.joint(new position(0,0,0),0,0);
				}else
					joints [i] = new Entity.joint(new position(0,0,0),0,0);
			}
			Entity.Skeleton s = new Entity.Skeleton(joints);
			return s;
		}

    // reads a line from the file
  private Entity.joint ReadLineFromFile()
    {
			if (fileReader == null) {
				Debug.Log ("here");
				return null;
			}

        // read a line
         string sPlayLine = fileReader.ReadLine();

			if (sPlayLine == null) {
				Debug.Log ("there");
				return null;
			}

        // extract the unity time and the body frame
            char[] delimiters = { ',' };
            string[] sLineParts = sPlayLine.Split(delimiters);
			int type = 0;
			int.TryParse(sLineParts[1],out type);
			float xAxis;
			float yAxis;
			float zAxis;
			float.TryParse(sLineParts[2], out xAxis);
			float.TryParse(sLineParts[3], out yAxis);
			float.TryParse(sLineParts[4], out zAxis);
			position p = new position(xAxis,yAxis,zAxis);
			int f;
			Debug.Log (sLineParts[1]+sLineParts[0]);
			int.TryParse (sLineParts [5], out f);
			Debug.Log ("frame number:" + f);
			Entity.joint j = new joint(p,type,f);
			return j;

    }
 }
}
