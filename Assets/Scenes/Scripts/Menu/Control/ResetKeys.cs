using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResetKeys : MonoBehaviour
{
    [SerializeField]
    GameObject defaultKeys;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetKeys() {
        //defaultKeys.GetComponent<DefaultKeys>().actionKeyMap[0].action;

        foreach(ActionKeyMap actionKeyMap in defaultKeys.GetComponent<DefaultKeys>().actionKeyMap) {
            if(PlayerPrefs.HasKey("controls." + actionKeyMap.action))
            actionKeyMap.keyText.text = actionKeyMap.key;
        }
     }
}
