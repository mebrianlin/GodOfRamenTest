using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
		public static SoundManager instance { get; private set; }
		public AudioClip[] clips;
		private AudioSource source;
		// Use this for initialization
		void Start ()
		{
				instance = this;
				source = this.GetComponent<AudioSource> ();
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
		
		
}
