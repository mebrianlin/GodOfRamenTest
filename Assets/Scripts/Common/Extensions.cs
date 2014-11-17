using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
		public static class Extensions
		{

				public static bool Empty<T> (this Queue<T> queue)
				{
						return queue.Count == 0;
				}

				public static bool Empty<K, V> (this Dictionary<K, V> container)
				{
						return container.Count == 0;
				}

				public static GameObject[] FindObjectsWithTagInChildren (this GameObject obj, string tag)
				{
						// cannot find disabled children
						return obj.transform.Cast<Transform> ()
                .Select (x => x.gameObject)
                .Where (x => x.tag == tag)
                .ToArray ();
				}

				public static GameObject FindObjectWithTagInChildren (this GameObject obj, string tag)
				{
						// cannot find disabled children
						GameObject[] array = obj.transform.Cast<Transform> ()
				.Select (x => x.gameObject)
				.Where (x => x.tag == tag)
				.ToArray ();
						if (array.Length == 0)
								return null;
						else
								return array [0];

				}

				public static GameObject[] FindObjectsWithTagInChildrenRecursive (this GameObject obj, string tag)
				{
						return GetAllChildren (obj)
				.Where (x => x.tag == tag)
				.ToArray ();
				}

				public static List<GameObject> GetAllChildren (this GameObject obj)
				{
						List<GameObject> allChildren = new List<GameObject> ();
						/*
			GameObject[][] array = obj.transform.Cast<Transform>()
				.Select(x => GetAllChilds(x.gameObject))
				.ToArray();
			*/
						foreach (Transform trans in obj.transform) {
								allChildren.AddRange (GetAllChildren (trans.gameObject));
								allChildren.Add (trans.gameObject);
						}        
						return allChildren;
				}
		}   
}
