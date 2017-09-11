using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using Core;

namespace Comparison{

        
		class Displacement
		{

			/// <summary>
			/// the number of skeleton joints
			/// </summary>
			const int JOINTNUMBER = 20;

			/*
         * for current position
         * the joints position in current skeleton stream
        */
			private position[] curPoistions = new position[JOINTNUMBER];


			/*
         * for previous position
         * previous positions which was used to compare with the current positions
        */
			private position[] prePoistions = new position[JOINTNUMBER];


			/// <summary>
			/// Calculate vectors of displacement between two skeleton
			/// </summary>
			/// <param name="preSkeleton">the previous skeleton positions</param>
			/// <param name="curSkeleton">the current skeleton positions</param>
			public Displacement(position[] preSkeleton, position[] curSkeleton)
			{
			Debug.Log ("preskeleton, joint: " + 0 + " " + preSkeleton[0].X+ " " + preSkeleton[0].Y + " " + preSkeleton[0].Z);
			Debug.Log ("curskeleton, joint: " + 0 + " " + curSkeleton[0].X+ " " + curSkeleton[0].Y + " " + curSkeleton[0].Z);

				this.prePoistions = preSkeleton;
				this.curPoistions = curSkeleton;


			}

			public Entity.Vector[] GetAllVectors()
			{
				Entity.Vector[] results = new Entity.Vector[JOINTNUMBER];
				//position p = new position(0, 0, 0);
				//Console.WriteLine(c.getX());
				for (int i = 0; i < JOINTNUMBER; i++)
				{
				//Debug.Log ("prepos: " + this.prePoistions [0].X + " " + this.prePoistions [0].Y);
				//Debug.Log ("curpos: " + this.curPoistions [0].X + " " + this.curPoistions [0].Y);

					results[i] = new Entity.Vector(this.prePoistions[i], this.curPoistions[i]);

				}

				return results;
			}
		}

  }

