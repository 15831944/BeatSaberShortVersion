using UnityEngine;
using UnityEngine.UI;

public class Score_Combo : MonoBehaviour {

    public GameObject score;
    public GameObject combo;

    // Update score and combo
    void Update () {
        score.GetComponent<TextMesh>().text = GLOBAL_PARA.Game.CountScore().ToString();
        combo.GetComponent<TextMesh>().text = GLOBAL_PARA.Game.CurrentComboRecord.ToString();
    }
}
