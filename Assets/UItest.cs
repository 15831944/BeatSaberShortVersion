using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItest : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => { Debug.Log("hahhaha"); });
    }

    // Update is called once per frame
    void Update () {
		
	}
}
