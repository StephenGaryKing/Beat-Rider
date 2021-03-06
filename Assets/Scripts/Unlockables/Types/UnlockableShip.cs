﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableShip", menuName = "Beat Rider/Unlockables/Ship", order = 1)]
public class UnlockableShip : Unlockable {

	public Mesh m_model;
    public Material m_material;
    public Texture m_metallicTexture;
}
