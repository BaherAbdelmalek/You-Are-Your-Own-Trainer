using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMovePose : MonoBehaviour {

	private static int idx;
	public static List<List<int>> PoseMoves = new List<List<int>> ();
	public Text[] Poses;
	public Text Move;
	public static bool AddJump;
	public InputField[] inputs;
	void Start(){
		idx = 0;
	}

	public void UpdateClick(){
		PoseMoves.Add (new List<int> ());
		if (AddJump) {
			for (int i = 0; i < Poses.Length; i++)
				PoseMoves [idx].Add (-1);
			AddJump = false;
		} else {
			for (int i = 0; i < Poses.Length; i++) {
				if (Poses [i].text == "")
					PoseMoves [idx].Add (-1);
				else {
					int ang;
					int.TryParse (Poses [i].text, out ang);
					PoseMoves [idx].Add (ang);
				}
				inputs [i].text = "";
			}
		}
		idx++;
		Move.text = "Move: " + idx;
	}

	public void AddPoseJump(){
		AddJump = true;
	}
}

