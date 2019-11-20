using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeatRider;

public class Turntable : MonoBehaviour {

    [SerializeField] private List<UnlockableShip> m_unlockableShips = new List<UnlockableShip>();
    [SerializeField] private GameObject m_previewPrefab = null;
    [SerializeField] private float m_spawnDistance = 5;
    private float degBetweenShips = 0;
    [SerializeField] private Text m_textComponent = null;
    //[SerializeField] private GameObject m_cameraObject = null;

    private UnlockableShip m_selectedShip = null;
    private int m_currentShipNumber = 0;
    private UnlockableManager m_unlockableManager = null;
    private Button m_buyButton = null;
    private int m_selectedPrice = 0;
    [SerializeField] private Color m_unselectedColour = new Color(0.25f, 0.25f, 0.25f);


    // Money variables
    [SerializeField] private int m_initialGemDust = 10;
    private int m_currentGemDust = 0;

    // Use this for initialization
    void Start () {
        if (!m_previewPrefab.GetComponent<ShipPreview>())
            m_previewPrefab.AddComponent<ShipPreview>();

        m_textComponent.text = m_unlockableShips[m_currentShipNumber].price.ToString();

        m_unlockableManager = FindObjectOfType<UnlockableManager>();

        // Checks for null references and message developer
        //NullChecker();

        // Instantiate all the buyable ships
        degBetweenShips = 360.0f / m_unlockableManager.m_unlockableShips.Length;
        float radians = degBetweenShips * Mathf.Deg2Rad;

        int i = 0;
        
        foreach (UnlockableShip ship in m_unlockableManager.m_unlockableShips)
        {
            GameObject shipInstance = Instantiate(m_previewPrefab, transform.position + new Vector3(Mathf.Sin(radians * i), 0f, Mathf.Cos(radians * i)) * m_spawnDistance, Quaternion.identity, transform);
            shipInstance.GetComponent<MeshFilter>().mesh = ship.m_model;
            Material matInstance = shipInstance.GetComponent<Renderer>().material = ship.m_material;
            matInstance.SetColor("_EmissionColor", m_unselectedColour);
            i++;
        }

        m_selectedShip = m_unlockableShips[m_currentShipNumber];

        //m_textComponent.text = m_unlockableShips[m_currentShipNumber].price.ToString();

        m_currentGemDust = m_initialGemDust;

        //if (m_cameraObject)
        //    m
    }

    // Update is called once per frame
    void Update () {
		// Create a dynamic spawner
	}

    public void TurnLeft()
    {
        //Debug.Log(degBetweenShips);
        gameObject.transform.Rotate(0, degBetweenShips, 0);
        m_currentShipNumber--;
        if (m_currentShipNumber < 0)
            m_currentShipNumber = m_unlockableShips.Count-1;
        m_selectedPrice = m_unlockableShips[m_currentShipNumber].price;
        m_textComponent.text = m_selectedPrice.ToString();
    }
    public void TurnRight()
    {
        Debug.Log(degBetweenShips);
        gameObject.transform.Rotate(0, -degBetweenShips, 0);
        m_currentShipNumber++;
        if (m_currentShipNumber > m_unlockableShips.Count - 1)
            m_currentShipNumber = 0;
        m_selectedPrice = m_unlockableShips[m_currentShipNumber].price;
        m_textComponent.text = m_selectedPrice.ToString();

        //if (m_unlockableManager.)
    }

    public void BuySelected()
    {
        if (m_initialGemDust > m_selectedPrice)
            m_unlockableManager.UnlockShip(m_unlockableShips[m_currentShipNumber]);
    }

    public void ApplyTriggered()
    {

    }


    private void NullChecker()
    {
        // This part is used to show error dialog messages if there are parts of the script that have not been initialised properly
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            bool stopApplication = false;
            if (!m_previewPrefab)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a preview Prefab", "Exit");
                stopApplication = true;
            }
            if (!m_textComponent)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to a text component", "Exit");
                stopApplication = true;
            }
            if (!m_unlockableManager)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script can't find Unlockable Manager script in the scene. Make sure you have one", "Exit");
                stopApplication = true;
            }
            if (!m_buyButton)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to the buy button", "Exit");
                stopApplication = true;
            }
            if (stopApplication)
                UnityEditor.EditorApplication.isPlaying = false;

        }

#endif
    }
}
