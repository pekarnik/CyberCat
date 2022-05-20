using UnityEngine;
using UnityEngine.UI;
 
public class SimpleTimer
{
    private float _time;
 
    private float _timeLeft = 0f;
    public bool _timerOn = false;
    public SimpleTimer(float time)
    {
        _time = time;
        _timeLeft = _time;
    }
 
    public void setOn()
    {
        _timerOn = true;
        _timeLeft = _time;
    }
 
    public void Execute()
    {
        if (_timerOn)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            } else
            {
                _timeLeft = _time;
                _timerOn = false;
            }
        }
    }
}