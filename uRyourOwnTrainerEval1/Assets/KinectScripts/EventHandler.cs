using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Comparison;

public class EventHandler : MonoBehaviour {
	public Text PoseText;
	public Text MoveText;
	public Text AngleText;
	public Compare c;

	// Use this for initialization
	void Start () {
		c = new Compare ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void PoseCompare(){
		c.ComparePoses ();
	}

	public void MoveCompare(){
		c.CompareMoves ();
	}
	public void AngleCompare(){
		c.compareAngles();
	}
}
