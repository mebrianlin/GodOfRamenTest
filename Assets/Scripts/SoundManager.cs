using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
		public static SoundManager instance { get; private set; }
		public AudioClip[] clips;
		private AudioSource source;

        GameObject BGMusic;

        AudioSource _ramenAudio;
        AudioSource _transitionAudio;
        bool _playTransitionSound = false;    

        string _ramenPath =  "Sounds/RamenFeedback/";
	    string _transitionPath = "Sounds/Transition/";

        string[] _upSounds = { "Up1", "Up2", "Up3", "Up4", };
        string[] _downSounds = { "Down1", "Down2", "Down3", "Down4", };
        string _finishSound = "RamenFinish";
		string[] _transitionSounds = {"Transition0","Transition1","Transition2"};

		// Use this for initialization
		void Start ()
		{
				instance = this;
				source = this.GetComponent<AudioSource> ();

                _ramenAudio = transform.Find("RamenFeedback").gameObject.GetComponent<AudioSource>();
                _transitionAudio = transform.Find("Transition").gameObject.GetComponent<AudioSource>();
                BGMusic = transform.Find("BGM").gameObject;
        }

        void Update()
        {
            if (_playTransitionSound)
            {
                if (!_transitionAudio.isPlaying)
                {
                    _playTransitionSound = false;
                    fadeInBGM();
                }
            }
        }

		public void playRamenUp ()
		{
				source.PlayOneShot (clips [0]);
		}

		public void playRamenDown ()
		{
				source.PlayOneShot (clips [1]);
		}

		public void playRamenBreak ()
		{
				source.PlayOneShot (clips [2]);
		}

		public void playIngredientGrab ()
		{
				source.PlayOneShot (clips [3]);
		}



        public void PlayRamenUpSound(int i)
        {
            if (i < _upSounds.Length)
            {
                AudioClip newClip = Resources.Load(string.Concat(_ramenPath, _upSounds[i]), typeof(AudioClip)) as AudioClip;
                //_ramenAudio.clip = newClip;
                //_ramenAudio.Play();
                _ramenAudio.PlayOneShot(newClip);
            }
        }

        public void PlayRamenDownSound(int i)
        {
            if (i < _upSounds.Length)
            {
                AudioClip newClip = Resources.Load(string.Concat(_ramenPath, _downSounds[i]), typeof(AudioClip)) as AudioClip;
                //_ramenAudio.clip = newClip;
                //_ramenAudio.Play();
                _ramenAudio.PlayOneShot(newClip);
            }
        }

        public void PlayRamenFinishSound()
        {
            AudioClip newClip = Resources.Load(string.Concat(_ramenPath, _finishSound), typeof(AudioClip)) as AudioClip;
            _ramenAudio.clip = newClip;
            _ramenAudio.Play();
        }

        /// <summary>
        /// Plays the transition sound.
        /// </summary>
        /// <param name="round"></param>
        /// <returns>The length of the sound (in seconds)</returns>
        public float PlayerTransitionSound(int round)
        {
            if (round < _transitionSounds.Length)
            {
                fadeOutBGM();
                AudioClip newClip = Resources.Load(string.Concat(_transitionPath, _transitionSounds[round]), typeof(AudioClip)) as AudioClip;
                _transitionAudio.clip = newClip;
                _transitionAudio.Play();
                _playTransitionSound = true;
                return newClip.length;
            }
            return 0.001f;
        }

        void fadeOutBGM()
        {
            //BGMusic.GetComponent<AudioSource>().volume = 0f;
            StartCoroutine("fadingBGM", 0f);
        }

        void fadeInBGM()
        {
            StartCoroutine("fadingBGM", 0.35f);
        }
    
        IEnumerator fadingBGM(float targetVolume)
        {
            float startTime = Time.realtimeSinceStartup;
            float startVolume = BGMusic.GetComponent<AudioSource>().volume;
            float elapsedTime = 0f;
            float transitionTime = 1f;

            while (elapsedTime < transitionTime)
            {
                yield return null;
                elapsedTime = Time.realtimeSinceStartup - startTime;

                float currentTime = Time.realtimeSinceStartup;

                float volume = startVolume + (targetVolume - startVolume) * elapsedTime / transitionTime;
                volume = Mathf.Clamp01(volume);

                BGMusic.GetComponent<AudioSource>().volume = volume;
            }
            /*
            Debug.Log(targetVolume);
            int count = 20;
            float step = (BGMusic.GetComponent<AudioSource>().volume - targetVolume) / count;
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                float volume = BGMusic.GetComponent<AudioSource>().volume;
                //volume -= 1f / (float)count;
                volume -= step;
                volume = Mathf.Clamp01(volume);
                Debug.Log("CHANGING VOLUME" + volume.ToString());
                BGMusic.GetComponent<AudioSource>().volume = volume;
            }*/
        }
     
}
