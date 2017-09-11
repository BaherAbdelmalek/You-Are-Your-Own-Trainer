using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormedItemScript : MonoBehaviour {

	private string Name;
	public Text ButtonText;
    public Text FormedName;
	public Text RecordedName;
	private Button _button;
	public void Start(){
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() =>
			{
				SharedVariables.PlayType = 2;
				FormedName.text = Name;

			});
	}
	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
	public void Button_Click()
	{
		FormedName.text = Name;
		RecordedName.text = "Please Choose Recorded Exercise";
	}
}
