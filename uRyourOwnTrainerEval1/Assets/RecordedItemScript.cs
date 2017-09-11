using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordedItemScript : MonoBehaviour {

	private string Name;
	public Text ButtonText;
	public Text RecordedName;
	public Text FormedName;
	private Button _button;
	public void Start(){
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() =>
			{
				SharedVariables.PlayType = 1;
				RecordedName.text = Name;

			});
	}
	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
	public void Button_Click()
	{
		RecordedName.text = Name;
		FormedName.text = "Please choose Formed Exercise";

	}
}
