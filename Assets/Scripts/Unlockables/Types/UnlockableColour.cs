using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableColour", menuName = "Beat Rider/Unlockables/Colour", order = 1)]
public class UnlockableColour : Unlockable {

	public Color m_colour = Color.white;
    public bool m_isMetallic = false;
}
