using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Allows the Developer to create a SongController using a menu at the top of the screen
	/// </summary>
	internal class MusicalGameplayMenu
	{

		[MenuItem("MusicalGameplay/Create Sound Manager")]
		private static void CreateSoundManager()
		{
			MonoBehaviour component;
			if (component = GameObject.FindObjectOfType<SongController>())
			{
				Selection.activeGameObject = component.gameObject;

				Debug.LogError("SongController already exists in the current scene");
				return;
			}

			GameObject gameObject = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("SongController")) as GameObject;
			Selection.activeGameObject = gameObject;

			Debug.Log("SongController object has been created.");
		}
	}
}