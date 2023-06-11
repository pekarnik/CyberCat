using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerLogic : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float time;
    [Header("Tentacles")]
    [SerializeField] private GameObject tentaclesGameObject;
    [SerializeField] private Sprite[] tentacles;
    private Image tentaclesGameObjectImage;

    private int prevTime;
    private float newTime;

    void Start()
    {
        timerText.text = time.ToString();
        prevTime = (int)time;
        newTime = time;

        tentaclesGameObjectImage = tentaclesGameObject.GetComponent<Image>();
    }

    void Update()
    {
        time -= Time.deltaTime;
        newTime = Mathf.Round(time);

        // Updating timer in scene
        if(newTime < prevTime)
        {
            prevTime = (int)newTime;
            timerText.text = prevTime.ToString();

            // Tentacles
            if (prevTime == 15)
            {
                tentaclesGameObject.SetActive(true);
            }

            if (prevTime < 15 && prevTime >= 0)
            {
                tentaclesGameObjectImage.sprite = tentacles[prevTime];
            }
        }

    }
}
