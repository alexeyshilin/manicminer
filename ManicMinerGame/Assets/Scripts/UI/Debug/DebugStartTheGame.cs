using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class DebugStartTheGame : MonoBehaviour
{
    public string theGame = "Room Store Test";

    void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener( new UnityEngine.Events.UnityAction(
        ()=>
        {
            PlayerPrefs.SetInt("_room", 0);
            PlayerPrefs.SetInt("_score", 0);
            UnityEngine.SceneManagement.SceneManager.LoadScene(theGame);
        }
        ));
    }

}
