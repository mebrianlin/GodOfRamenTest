using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
		public static SoundManager instance { get; private set; }
		public AudioClip[] clips;
		private AudioSource source;


        AudioSource _ramenAudio;
        
        string _ramenPath =  "Sounds/RamenFeedback/";

        string[] _upSounds = { "Up1", "Up2", "Up3", "Up4", };
        string[] _downSounds = { "Down1", "Down2", "Down3", "Down4", };
        string _finishSound = "RamenFinish";

		// Use this for initialization
		void Start ()
		{
				instance = this;
				source = this.GetComponent<AudioSource> ();

                _ramenAudio = transform.Find("RamenFeedback").gameObject.GetComponent<AudioSource>();
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

    /*
        IEnumerator fadingBGM()
        {
            int count = 20;
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                float volume = BGMusic.GetComponent<AudioSource>().volume;
                volume -= 1f / (float)count;
                volume = Mathf.Clamp01(volume);
                BGMusic.GetComponent<AudioSource>().volume = volume;
            }

            if (currentFading < 0)
            {
                return false;
            }

            AudioClip newClip = (AudioClip)Resources.Load(string.Concat(musicPath, music[currentFading]), typeof(AudioClip));
            BGMusic.GetComponent<AudioSource>().clip = newClip;
            BGMusic.GetComponent<AudioSource>().Play();
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                float volume = BGMusic.GetComponent<AudioSource>().volume;
                volume += 1f / (float)count;
                volume = Mathf.Clamp01(volume);
                BGMusic.GetComponent<AudioSource>().volume = volume;
            }
        }
     */
}
