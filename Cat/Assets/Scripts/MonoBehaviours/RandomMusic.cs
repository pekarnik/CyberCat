using UnityEngine;
using TMPro;

public class RandomMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] Allmusic;
    [SerializeField] private float PlayingTimePerTrack;
    [SerializeField] private TextMeshProUGUI TrackName;

    private int amountOfTracks;
    private int currentTrackNum;
    private float timeLeft;
    private AudioSource soundPlayer;

    void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
        amountOfTracks = Allmusic.Length;
        currentTrackNum = Random.Range(0,amountOfTracks);
        NextTrack(currentTrackNum);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            NextTrack(currentTrackNum);
        }
    }

    private void NextTrack(int TrackNum)
    {
        if (currentTrackNum + 1 >= amountOfTracks)
        {
            currentTrackNum = -1;
        }
        currentTrackNum += 1;

        soundPlayer.clip = Allmusic[TrackNum];
        soundPlayer.Play();
        TrackName.SetText(Allmusic[TrackNum].name);

        timeLeft = PlayingTimePerTrack;
    }
}
