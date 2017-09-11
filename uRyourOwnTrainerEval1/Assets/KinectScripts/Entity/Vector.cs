using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entity {
public class Vector  {
	double X, Y, Z;
	/*
                /// <summary>
	/// Initialize a Vector
	/// </summary>
	/// <function>Constructor</function>
	/// <param name="p1">the first skeleton point data</param>
	/// <param name="p2">the second skeleton point data</param>
	public Vector(SkeletonPoint p1, SkeletonPoint p2)
	{
		this.X = p2.X - p1.X;
		this.Y = p2.Y - p1.Y;
		this.Z = p2.Z - p1.Z;
	}
	*/
	public Vector() { }
	/// <summary>
	/// Initialize a Vector
	/// </summary>
	/// <function>Constructor</function>
	/// <param name="p1">the first joint point data</param>
	/// <param name="p2">the second joint point data</param>
	public Vector(position p1, position p2)
	{
		this.X = p2.X - p1.X;
		//Console.WriteLine(this.getX());
		this.Y = p2.Y - p1.Y;

		this.Z = p2.Z - p1.Z;
	}

	/// <summary>
	/// Initialize a Vector
	/// </summary>
	/// <function>Constructor</function>
	/// <param name="x">the vector coordinate x </param>
	/// <param name="y">the vector coordinate y </param>
	/// <param name="z">the vector coordinate z </param>
	public Vector(double x, double y, double z)
	{
		this.X = x;

		this.Y = y;

		this.Z = z;
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

		public Vector Normalize()
    {
			Vector v;

			double key = 1;

			key = this.getX() * this.getX() + this.getY() * this.getY() +this.getZ() * this.getZ();

			if (key != 0)
			{
				key = 1 / key;
			}

			key = Math.Sqrt(key);

			v = new Vector(this.getX() * key,this.getY() * key, this.getZ()  * key);

			return v;


       }
}
	
 
}
