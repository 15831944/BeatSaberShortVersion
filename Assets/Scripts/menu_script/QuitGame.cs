using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class QuitGame : MonoBehaviour {
    private string logoutUrl = "http://www.hustimis.cn:8014/Log.svc/Logout";
    IEnumerator POSTDATA(string url, string data)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] dataPost = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(dataPost);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 7;
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        while (!request.isDone) { }
        Debug.Log("Result: " + request.downloadHandler.text);
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//用于退出运行
#else
            GLOBAL_PARA.LogoutInfo logoutInfo = new GLOBAL_PARA.LogoutInfo();
            string logoutData = JsonConvert.SerializeObject(logoutInfo);
            StartCoroutine(POSTDATA(logoutUrl, logoutData));
            Application.Quit();
#endif
    }
}
