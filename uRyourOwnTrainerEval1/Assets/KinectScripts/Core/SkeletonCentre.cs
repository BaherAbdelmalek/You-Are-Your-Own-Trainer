using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

namespace Core{
public class SkeletonCentre {

	const int JOINTNUMBER = 20;

	public struct Position
	{
		public double x;
		public double y;
		public double z;
	};


	/// <summary>
	/// defin a center position
	/// </summary>
	private Position center;

	public SkeletonCentre(Skeleton skeleton)
	{
		SkeletonJoints sj = new SkeletonJoints(skeleton);

		///the sum value x,y,z of all joints in one skeleton
		for (int i = 0; i < JOINTNUMBER; i++)
		{
			this.center.x += sj.GetJoints()[i].GetPosition().X;
			this.center.y += sj.GetJoints()[i].Position.Y;
			this.center.z += sj.GetJoints()[i].Position.Z;
		}

		///average
		this.center.x /= JOINTNUMBER;
		this.center.y /= JOINTNUMBER;
		this.center.z /= JOINTNUMBER;
	}


	/// <summary>
	/// add a skeleton center, and calcuate the new average center
	/// </summary>
	/// <param name="skeleton">the next skeleton data</param>
	/// <returns>the average center in the past period time</returns>
	public void SumCenter(Skeleton skeleton)
	{
		Position c = new Position();

		SkeletonJoints nextSJ = new SkeletonJoints(skeleton);

		///the sum value x,y,z of all joints in one skeleton
		for (int i = 0; i < JOINTNUMBER; i++)
		{
			c.x += nextSJ.GetJoints()[i].Position.X;
			c.y += nextSJ.GetJoints()[i].Position.Y;
			c.z += nextSJ.GetJoints()[i].Position.Z;
		}

		///average
		c.x /= JOINTNUMBER;
		c.y /= JOINTNUMBER;
		c.z /= JOINTNUMBER;

		///Sum
		this.center.x += c.x;
		this.center.y += c.y;
		this.center.z += c.z;

	}

	public Position GetSumCentre()
	{
		return this.center;
	}
 }
}


