using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Entity;
using Core;

namespace Comparison{

	public class Quaternions  {

		/**
         * The first vector 
         * */
		private Vector preVector;

		/**
         * The second vector 
         * */
		private Vector curVector;

//		public Quaternions(Vector v1, Vector v2){
//			preVector = Normalize (v1.getX (), v1.getY (), v1.getZ ());
//
//			curVector = Normalize (v2.getX (),v2.getY (), v2.getZ ());
//		}
//

		/// <summary>
		/// get a quaternion by two vectors
		/// </summary>
		/// <param name="joint1">the first vector </param>
		/// <param name="joint2">the second vector </param>
		public Entity.Quaternion GetQuaternionByVectors(Vector joint1, Vector joint2)
		{
			preVector = Normalize(joint1.getX(), joint1.getY(), joint1.getZ());

			curVector = Normalize(joint2.getX(), joint2.getY(), joint2.getZ());

			return CalculateQuaternion();
		}
	


		/// <summary>
		/// calculate a quaternion by two vectors
		/// </summary>
		private Entity.Quaternion CalculateQuaternion()
		{

			var angle = this.GetAngleFromTwoVector(preVector, curVector);

			var vector = this.GetNormalVector(preVector, curVector);

			angle = angle / 2;

			var W = Math.Cos(angle);

			var sin = Math.Sin(angle);

			var X = vector.getX() * sin;

			var Y = vector.getY() * sin;

			var Z = vector.getZ() * sin;

			//return new Quaternion(W, X, Y, Z);

			return Normalize(W, X, Y, Z);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		private Vector GetNormalVector(Vector a, Vector b)
		{
			Vector vector = new Vector(a.getY() * b.getZ() - a.getZ() * b.getY(), a.getZ() * b.getX() - a.getX() * b.getZ(), a.getX() * b.getY() - a.getY() * b.getX());

			var vectorValue = this.GetValueOfVector(vector);

			Vector Nvector = new Vector(0, 0, 0);
			if (vectorValue != 0)
			{
				Nvector = this.Normalize(vector.getX() / vectorValue, vector.getY() / vectorValue, vector.getZ() / vectorValue);
			}
			return Nvector;
		}

		/// <summary>
		/// calculate the angle between two angles
		/// </summary>
		/// <param name="a">one vector</param>
		/// <param name="b">another vector</param>
		/// <returns>the angle of the two angles</returns>
		private double GetAngleFromTwoVector(Vector a, Vector b)
		{
			double result = 0;

			var molecule = a.getX() * b.getX() + a.getY() * b.getY() + a.getZ() * b.getZ();

			var aValue = this.GetValueOfVector(a);

			var bValue = this.GetValueOfVector(b);

			var denominator = aValue * bValue;
			if (denominator != 0)
			{
				result = Math.Acos(molecule / denominator);
			}

			return result;
		}

		/// <summary>
		/// calculate the length of the vector
		/// </summary>
		/// <param name="vector">the vector</param>
		/// <returns>the length of the vector</returns>
		public double GetValueOfVector(Vector vector)
		{
			return Math.Sqrt(vector.getX() * vector.getX() + vector.getY() * vector.getY() + vector.getZ() * vector.getZ());
		}

		/// <summary>
		/// normalize a vectors
		/// </summary>
		/// <param name="x">the vector coordinate x </param>
		/// <param name="y">the vector coordinate y </param>
		/// <param name="z">the vector coordinate z </param>
		/// <returns>mormalized vector</returns>
		private Vector Normalize(double x, double y, double z)
		{
			Vector v;

			double key = 1;

			key = x * x + y * y + z * z;

			if (key != 0)
			{
				key = 1 / key;
			}

			key = Math.Sqrt(key);

			v = new Vector(x * key, y * key, z * key);

			return v;
		}

		public static Entity.Quaternion Normalize(double w, double x, double y, double z)
		{
			Entity.Quaternion q;

			double key = 1;

			key = w * w + x * x + y * y + z * z;

			if (key != 0)
			{
				key = 1 / key;
			}

			key = Math.Sqrt(key);

			q = new Entity.Quaternion(w * key, x * key, y * key, z * key);

			return q;
		}

		public static double  dot(Entity.Quaternion a , Entity.Quaternion b ){
			return( (a.getW() * b.getW())+(a.getX()*b.getX())+(a.getY()*b.getY())+(a.getZ()*b.getZ()));
		}

	}

}
