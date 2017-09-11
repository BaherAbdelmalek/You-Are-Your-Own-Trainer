using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonScript : MonoBehaviour {

	private string Name;
	public Text ButtonText;
	//public Text RecordedExerciseName;
	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
	public void Button_Click()
	{
	}
}