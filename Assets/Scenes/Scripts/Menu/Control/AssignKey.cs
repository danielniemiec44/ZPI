using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignKey : MonoBehaviour
{
    [SerializeField]
    bool assigningMode = false;
    [SerializeField]
    GameObject defaultKeys;
    [SerializeField]
    string actionName;
    ActionKeyMap[] actionKeyMap;
    Text text;


    // Start is called before the first frame update
    void Start()
    {
        actionKeyMap = defaultKeys.GetComponent<DefaultKeys>().actionKeyMap;

        foreach(ActionKeyMap actionKey in actionKeyMap) {
            if(actionKey.action == actionName) {
                text = actionKey.keyText;
                if(PlayerPrefs.HasKey("controls." + actionName)) {
                    text.text = PlayerPrefs.GetString("controls." + actionName);
                } else {
                    text.text = actionKey.key;
                }
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void enterAssigningMode() {
        if(!assigningMode) {
            assigningMode = true;
            text.text = "<" + text.text + ">";
        }
    }


    void OnGUI()
     {
         if(assigningMode) {
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown){
                KeyCode keyCode = e.keyCode;
                text.text = keyCode.ToString();
                assigningMode = false;
            }
         }
     }
}
