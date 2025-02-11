using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSCounter : MonoBehaviour
{
    private int avgFrameRate;
    public GameObject display_Text;
 
    public void Update ()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        display_Text.GetComponent<TMPro.TextMeshProUGUI>().text = avgFrameRate.ToString() + " FPS";
    }
}