using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DropDownList : MonoBehaviour {
   public Dropdown dropdowm;
   private string SelectedExercise;
   private StreamReader fileReader = new StreamReader("Exercises") ;

    List<string> Exercises = new List<string>() { "Please choose the Exercise "};
    void Start () {
        ReadLineFromFile();
        PopulateList();
    }
    public void DropDownHandler(int index)
    {
        SelectedExercise = Exercises[index];
        Debug.Log(SelectedExercise);
        SharedVariables.PlayingExerciseName = SelectedExercise;

    }

    // Update is called once per frame
    void PopulateList () {
        
        dropdowm.AddOptions(Exercises);
    }
    private void ReadLineFromFile()
    {
        if (fileReader == null)
        {
            Debug.LogError("the file can not be accessed");
        }
        // read a line
        while(true)
        {
        string sPlayLine = fileReader.ReadLine();
        if (sPlayLine == null)
        {
                break;
        }
            Exercises.Add(sPlayLine);
            fileReader.ReadLine();
            fileReader.ReadLine();
        }
    }
}
