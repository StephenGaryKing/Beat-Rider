using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;

namespace MusicalGameplayMechanics
{
	/// <summary>
	/// Used for manipulating the appearance of AudioReactors in the editor
	/// </summary>
	[CustomEditor(typeof(AudioReactor))]
	public class AudioReactorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			AudioReactor serializedAudioReactor = (AudioReactor)target;

			serializedObject.Update();
			bool reactionSelected = false;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_shouldAlwaysReact"), new GUIContent("Always React"), new GUILayoutOption[0]);
			if (!serializedAudioReactor.m_shouldAlwaysReact)
			{
				EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_shouldReactToPitch"), new GUIContent("Pitch"), new GUILayoutOption[0]);
				if (serializedAudioReactor.m_shouldReactToPitch)
				{
					if (reactionSelected)
						EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AndPitch"), new GUIContent("And"), new GUILayoutOption[0]);
					reactionSelected = true;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_pitch"), new GUIContent("pitch"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_pitchLeniency"), new GUIContent("leniency"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_lessThanPitch"), new GUIContent("less than"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_equalToPitch"), new GUIContent("equal to"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_moreThanPitch"), new GUIContent("more than"), new GUILayoutOption[0]);
				}

				EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_shouldReactToDb"), new GUIContent("React to decibels"), new GUILayoutOption[0]);
				if (serializedAudioReactor.m_shouldReactToDb)
				{
					if (reactionSelected)
						EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AndDB"), new GUIContent("And"), new GUILayoutOption[0]);
					reactionSelected = true;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_db"), new GUIContent("decibels"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_dbLeniency"), new GUIContent("leniency"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_lessThanDb"), new GUIContent("less than"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_equalToDb"), new GUIContent("equal to"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_moreThanDb"), new GUIContent("more than"), new GUILayoutOption[0]);
				}

				EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_shouldReactToBeatDensity"), new GUIContent("React to beat density"), new GUILayoutOption[0]);
				if (serializedAudioReactor.m_shouldReactToBeatDensity)
				{
					if (reactionSelected)
						EditorGUILayout.PropertyField(serializedObject.FindProperty("m_AndBeatDensity"), new GUIContent("And"), new GUILayoutOption[0]);
					reactionSelected = true;
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_density"), new GUIContent("beat density"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_densityLeniency"), new GUIContent("leniency"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_lessThanDensity"), new GUIContent("less than"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_equalToDensity"), new GUIContent("equal to"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("m_moreThanDensity"), new GUIContent("more than"), new GUILayoutOption[0]);
				}
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}