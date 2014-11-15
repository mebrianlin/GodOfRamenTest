using UnityEngine;
using System.Collections;

public class AttachNoodleScript : MonoBehaviour
{
		public bool attached = false;
		public int waveCount;
		public bool countAbility = false;
		int requireCount = 10;
		// Use this for initialization
		void Start ()
		{
			waveCount = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter (Collider col)
		{
            if(col.tag == "TableTrigger")
				attached = true;
		}

		void OnTriggerExit (Collider col)
		{

            if (col.tag == "TableTrigger")
				attached = false;
		}

		public void unAttach ()
		{
				attached = false;
		}

		public bool isAttached ()
		{
				return attached;
		}
		
		public void AddWaveCount(){
			if(countAbility){
				waveCount ++;
				Debug.Log("Current noodle wave count = " + waveCount);
				countAbility = false;
			}
		}

		public bool IsNoodleComplete() {
			return (waveCount>=requireCount);
		}

	
}
