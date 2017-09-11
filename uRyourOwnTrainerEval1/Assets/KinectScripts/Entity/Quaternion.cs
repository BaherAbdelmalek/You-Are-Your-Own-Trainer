using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entity{
	public class Quaternion {

	// Use this for initialization
	//the Quaternion coefficient W 
	private double W;

	//the Quaternion coefficient X 
	private double X;

	//the Quaternion coefficient Y
	private double Y;

	//the Quaternion coefficient Z 
	private double Z;

	/// <summary>
	/// Initialize a Vector
	/// </summary>
	/// <function>Constructor</function>
	/// <param name="w">the Quaternion coefficient w </param>
	/// <param name="x">the Quaternion coefficient x </param>
	/// <param name="y">the Quaternion coefficient y </param>
	/// <param name="z">the Quaternion coefficient z </param>
	public Quaternion(double w, double x, double y, double z)
	{
		this.W = w;
		this.X = x;
		this.Y = y;
		this.Z = z;
	}

	public double getW()
	{
		return this.W;
	}

	public double getX()
	{
		return this.X;
	}

	public double getY()
	{
		return this.Y;
	}

	public double getZ()
	{
		return this.Z;
	}

	public string toString()
	{
		return "" + this.W + " + " + this.X + "i " + this.Y + "j " + this.Z + "k  ";
	}
		public Quaternion  Conjugute(){
			return (new Quaternion(W,-X,-Y,-Z));
		}
		public double  magnitude(){
			return ((double)Math.Sqrt((W*W)+(X*X)+(Y*Y)+(Z*Z)));
		}
		public Quaternion Reciprocal(){
			double d = W*W + X*X + Y*Y + Z*Z;
			return new Quaternion(W/d, -X/d, -Y/d, -Z/d);
		}

  }

}
