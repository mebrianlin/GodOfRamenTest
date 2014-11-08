using UnityEngine;
using System.Collections;

public class ClothRendererTest : MonoBehaviour
{

		Renderer _renderer;
		
		public GameObject topBar;
		public GameObject bottomBar;
		public float slope = 1f;
		public float maxNoodleScore;	

		private float topHeight;
		private float bottomHeight;
		private float maxRamenHeight;

		private float noodleScore;


		private bool reachedTopBuffer = false;
		private bool reachedBottomBuffer = false;
		// Use this for initialization
		void Start ()
		{
				_renderer = GetComponent<Renderer> ();
				topHeight = topBar.GetComponent<Transform> ().position.y;
				bottomHeight = bottomBar.GetComponent<Transform> ().position.y;
				noodleScore = 0f;
				slope = 1f;
				maxNoodleScore = 500f;
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				Debug.Log (noodleScore);

//				Debug.Log (string.Format ("({0},{1})", _renderer.bounds.center.y - _renderer.bounds.extents.y, _renderer.bounds.center.y + _renderer.bounds.extents.y));
//				Debug.Log (_renderer.bounds.center.y - _renderer.bounds.extents.y);
//				Debug.Log (string.Format ("({0},{1})", renderer.bounds.min.y, renderer.bounds.max.y));

				maxRamenHeight = _renderer.bounds.max.y;
				

				//checking to see if ramen is within range
				if (maxRamenHeight > bottomHeight && maxRamenHeight < topHeight) {
				
						Debug.Log ("I MADE IT");
						float distancePastBottomHeight = maxRamenHeight - bottomHeight;
						addNoodleScore (distancePastBottomHeight);

				}

				if (maxRamenHeight > topHeight) {
						this.transform.GetComponent<InteractiveCloth> ().tearFactor = 1f;
				} else {
			
				}

		
				if (noodleScore >= maxNoodleScore) {
						Debug.Log ("FINISHED ONE SET OF NOODLES");
						noodleScore = 0f;
				}

		
		
		}
	


		void setTopBufferFalse ()
		{
				reachedTopBuffer = false;
		}

		//add to ramen score as a linear function y=kx where y is addOnScore, and x is distance. 
		//addOnScore is then added onto the noodleScore
		void addNoodleScore (float distance)
		{
				float addOnScore = slope * Mathf.Pow (distance, 2f); 
				noodleScore += addOnScore;
		}
}