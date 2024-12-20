using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
	private int frameCount;
    private float elapsedTime;
    private double frameRate;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
	
    public TextMeshProUGUI fpsText;
 
	void Update()
	{
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
            frameCount = 0;
            elapsedTime = 0;
        }

		fpsText.text = frameRate .ToString("F0") + " FPS";
    }
}
