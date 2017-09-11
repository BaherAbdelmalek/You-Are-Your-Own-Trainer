using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMove : MonoBehaviour {

	private static int idx;
	public static List<List<int>> AngleMoves = new List<List<int>> ();
	public Text[] Angles;
	public InputField[] inputs;
	public Text Move;
	public bool addJump;
	void Start(){
		idx = 0;
	}

	public void UpdateClick(){
		AngleMoves.Add(new List<int> ());
		if (addJump) {
			for (int i = 0; i < Angles.Length; i++)
				AngleMoves [idx].Add (-1);
			addJump = false;
		} else {
			for (int i = 0; i < Angles.Length; i++) {
				Debug.Log ("i: " + i);
				if (Angles [i].text == "") {
					Debug.Log ("i: " + i);
					AngleMoves [idx].Add (-1);
				} else {
					int ang;
					int.TryParse (Angles [i].text, out ang);
					AngleMoves [idx].Add (ang);
				}
				inputs [i].text = "";
			}
		}
		idx++;
		Move.text ="Move: "+ idx;
	}
	public void AddAngleJump(){
		addJump = true;
	}

}
