using UnityEngine;
using TMPro; //needed to use TextMeshPro text

//this scripts display the current wave number(UI)
public class WaveUI : MonoBehaviour
{
    public WaveManager waveManager; //Reference to the WaveManager.cs script and we use this to read the current wave number
    public TextMeshProUGUI waveText; //Reference to the ui text that shows the wave number

    private void Start()
    {
        if (waveManager == null)// if the WaveManager is not assigned in the inspector try to find it automatically in the scene
        {
            waveManager = Object.FindAnyObjectByType<WaveManager>();
        }

        if (waveText == null)// if the text reference is missing try to find get it from the same GameObject
        {
            waveText = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (waveManager == null || waveText == null) return; // if something is missing do nothing

        waveText.text = $"Wave {waveManager.CurrentWave}";// updates the ui text every frame
    }
}
