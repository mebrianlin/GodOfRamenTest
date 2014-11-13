using UnityEngine;
using System.Collections;

public delegate void TimerEventHandler(GameObject sender);

public class Timer : MonoBehaviour
{
    public event TimerEventHandler OnTimeElpased;

    /*
    private string _clockClickPath = "Sounds/count_down";
    private string _timeUpPath = "Sounds/time_up";

    private AudioClip _clockClickAudioClip;
    private AudioClip _timeUpAudioClip;

    private AudioSource _audioSource;
    */

    public double RemainingTime
    {
        get;
        private set;
    }

    //public bool Alarm = true;
    public bool Enabled = false;

    double _interval;
    public double Interval
    {
        get { return _interval; }
        set
        {
            if (value <= 0)
                _interval = 0.1;
            else if (value > System.Int32.MaxValue)
                _interval = System.Int32.MaxValue;
            else
                _interval = value;
            this.RemainingTime = this.Interval;
        }
    }

    // Use this for initialization
    void Awake()
    {
        this.Enabled = false;
        this.Interval = 0.1;
        this.RemainingTime = this.Interval;
       // this.Alarm = true;
    }

    void Start()
    {
        /*
        _clockClickAudioClip = Resources.Load(_clockClickPath) as AudioClip;
        _timeUpAudioClip = Resources.Load(_timeUpPath) as AudioClip;

        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _clockClickAudioClip;
        _audioSource.loop = true;
         */
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Enabled)
        {
            this.RemainingTime -= Time.deltaTime;

            float ratio = 1 - Mathf.Max(0.0001f, (float)(this.RemainingTime / this.Interval));
            renderer.material.SetFloat("_Cutoff", ratio);

            if (this.RemainingTime <= 0.0)
            {
                StopTimer();

                this.RemainingTime = this.Interval;
                
                //if (this.Alarm)
                //    AudioSource.PlayClipAtPoint(_timeUpAudioClip, Camera.main.transform.position);

                if (OnTimeElpased != null)
                    OnTimeElpased(this.gameObject);
            }
        }
    }

    public void StartTimer()
    {
        this.Enabled = true;
        //_audioSource.Play();
    }

    public void StopTimer()
    {
        this.Enabled = false;
        //_audioSource.Pause();
    }
}
