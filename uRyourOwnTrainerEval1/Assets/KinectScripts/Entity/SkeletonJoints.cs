using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity{
public class SkeletonJoints {

	/* the number of joint points we considered
	*/
	const int JOINTCOUNT = 20;

	/*
         * use to store all Joints in one skeleton
         * */
	private Entity.joint[] jointAl = new Entity.joint[JOINTCOUNT];


	/// <summary>
	/// Initialize joints of the skeleton
	/// </summary>
	/// <function>Constructor</function>
	/// <param name="skeleton">the skeleton data</param>
	public SkeletonJoints(Skeleton skeleton)
	{

		//1
			this.jointAl[0] = skeleton.joints[0];

		//2
			this.jointAl[1] = skeleton.joints[1];

		//3
			this.jointAl[2] = skeleton.joints[2];

		//4
			this.jointAl[3] = skeleton.joints[3];

		//5
			this.jointAl[4] = skeleton.joints[4];

		//6
			this.jointAl[5] = skeleton.joints[5];

		//7
				this.jointAl[6] = skeleton.joints[6];

		//8
				this.jointAl[7] = skeleton.joints[7];

		//9
				this.jointAl[8] = skeleton.joints[8];

		//10
				this.jointAl[9] = skeleton.joints[9];

		//11
				this.jointAl[10] = skeleton.joints[10];

		//12
				this.jointAl[11] = skeleton.joints[11];

		//13
				this.jointAl[12] = skeleton.joints[12];

		//14
				this.jointAl[13] = skeleton.joints[13];

		//15
				this.jointAl[14] = skeleton.joints[14];

		//16
				this.jointAl[15] = skeleton.joints[15];

		//17
				this.jointAl[16] = skeleton.joints[16];

		//18
				this.jointAl[17] = skeleton.joints[17];

		//19
				this.jointAl[18] = skeleton.joints[18];

		//20
				this.jointAl[19] = skeleton.joints[19];

	}

	public Entity.joint[] GetJoints()
	{
		return this.jointAl;
	}
 }
}
