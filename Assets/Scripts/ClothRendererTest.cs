using UnityEngine;
using System.Collections;

public class ClothRendererTest : MonoBehaviour
{
		public event NoodleEventHandler OnNoodleReady;

		Renderer _renderer;
		
		public GameObject topBar;
		public GameObject bottomBar;
		public GameObject ramenCountText;
		public GameObject noodles;
		public GameObject leftHandle;
		public GameObject rightHandle;
		public GameObject table;
		public GameObject[] triggers;

		public float slope = 1f;

		private float topHeight;
		private float bottomHeight;
		private float maxRamenHeight;
		private float noodleScore;
		private float maxNoodleScore;	
		private int noodlesfinished;

		private bool attachedToHands = true;

		// Use this for initialization
		void Start ()
		{
				noodles = GameObject.FindGameObjectWithTag ("Noodles");
				table = GameObject.FindGameObjectWithTag ("Table");
				topBar = GameObject.FindGameObjectWithTag ("TopBar");
				bottomBar = GameObject.FindGameObjectWithTag ("BottomBar");
				leftHandle = GameObject.FindGameObjectWithTag ("LeftHandle");
				rightHandle = GameObject.FindGameObjectWithTag ("RightHandle");
				triggers = GameObject.FindGameObjectsWithTag ("TableTrigger");	

				_renderer = noodles.GetComponent<Renderer> ();
				topHeight = topBar.GetComponent<Transform> ().position.y;
				bottomHeight = bottomBar.GetComponent<Transform> ().position.y;
				noodleScore = 0f;
				slope = 1f;
				maxNoodleScore = 500f;
				noodlesfinished = 0;
				foreach (GameObject trigger in triggers) {
						trigger.SetActive (false);
				}
				noodles.GetComponent<InteractiveCloth> ().useGravity = true;
		}
	
		// Update is called once per frame
		void Update ()
		{
//				Debug.Log (string.Format ("({0},{1})", _renderer.bounds.center.y - _renderer.bounds.extents.y, _renderer.bounds.center.y + _renderer.bounds.extents.y));
				
				//check to see if both hands have grabbed the noodlesA
				if (leftHandle.GetComponent<AttachNoodleScript> ().isAttached () && rightHandle.GetComponent<AttachNoodleScript> ().isAttached ()) {
						noodles.GetComponent<InteractiveCloth> ().AttachToCollider (leftHandle.GetComponent<Collider> ().collider);
						noodles.GetComponent<InteractiveCloth> ().AttachToCollider (rightHandle.GetComponent<Collider> ().collider);
						attachedToHands = true;
						leftHandle.GetComponent<AttachNoodleScript> ().unAttach ();
						rightHandle.GetComponent<AttachNoodleScript> ().unAttach ();
						foreach (GameObject trigger in triggers) {
								trigger.SetActive (false);
						}
						noodles.GetComponent<InteractiveCloth> ().useGravity = true;
			
				}

				if (attachedToHands) {
						maxRamenHeight = _renderer.bounds.max.y;
				

						//checking to see if ramen is within range
						if (maxRamenHeight > bottomHeight && maxRamenHeight < topHeight) {
				
								float distancePastBottomHeight = maxRamenHeight - bottomHeight;
								addNoodleScore (distancePastBottomHeight);

						}

						// Noodles BREAAAKKKK
						if (maxRamenHeight > topHeight) {
								noodles.transform.GetComponent<InteractiveCloth> ().tearFactor = 1f;
								noodles.transform.GetComponent<Cloth> ().randomAcceleration = new Vector3 (0, 10, 0);
								noodleScore = 0f;
								Invoke ("resetNoodles", 2f);
								attachedToHands = false;
								
						}		
		
						if (noodleScore >= maxNoodleScore) {
								//Debug.Log ("FINISHED ONE SET OF NOODLES");

								if (OnNoodleReady != null)
										OnNoodleReady ();

								noodleScore = 0f;
								noodlesfinished++;
						}
				} else {
						
				}

				ramenCountText.GetComponent<TextMesh> ().text = "Score: " + noodleScore.ToString ();
		}
	
		void resetNoodles ()
		{
				
				Destroy (noodles.gameObject);

				foreach (GameObject trigger in triggers) {
						trigger.SetActive (true);
				}

				noodles = (GameObject)Instantiate (Resources.Load ("Prefabs/Noodles", typeof(GameObject)), new Vector3 (0, table.GetComponent<Transform> ().position.y + .1f, 0), Quaternion.identity);
				//noodles are reset so no longer attached to hands
//				leftHandle.GetComponent<AttachNoodleScript> ().unAttach ();
//				rightHandle.GetComponent<AttachNoodleScript> ().unAttach ();
				_renderer = noodles.GetComponent<Renderer> ();		
		}

		//add to ramen score as a linear function y=kx where y is addOnScore, and x is distance. 
		//addOnScore is then added onto the noodleScore
		void addNoodleScore (float distance)
		{
				float addOnScore = slope * Mathf.Pow (distance, 2f); 
				noodleScore += addOnScore;
		}

}