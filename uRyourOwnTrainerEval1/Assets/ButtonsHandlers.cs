using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsHandlers : MonoBehaviour {

	// Use this for initialization
 public	void StartRecorder () {
        //SceneManager.LoadScene("Start");
        Application.LoadLevel("Start");
    }

    public void StartPlaying()
    {
        // SceneManager.LoadScene("StartPlaying");
        Application.LoadLevel("StartPlaying");
    }
	public void FormExercise()
	{
		// SceneManager.LoadScene("StartPlaying");
		Application.LoadLevel("FormExercise");
	}

}
