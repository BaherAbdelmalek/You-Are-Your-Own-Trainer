using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity 
{
	public class joint  {

		public position Position;
		public int type;
		public int Frame;
		public int time;

		public joint(position P , int t ,int f){
			this.Position = P;
			this.type = t;
			this.Frame = f;
		}
		public joint(position P , int t ,int f,int Time){
			this.Position = P;
			this.type = t;
			this.Frame = f;
			this.time = Time;
		}
		public position GetPosition(){
			return this.Position;
		}
		public int GetFrame(){
			return this.Frame;
		}
		public int Gettype(){
			return this.type;
		}
	}



}