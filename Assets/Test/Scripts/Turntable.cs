using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeatRider;
using Pixelplacement;

public class Turntable : MonoBehaviour {

    //[SerializeField] private List<UnlockableShip> m_unlockableShips = new List<UnlockableShip>();
    [SerializeField] private GameObject m_previewPrefab = null;
    [SerializeField] private float m_spawnDistance = 5;
    private float degBetweenShips = 0;
    [SerializeField] private float m_rotateTime = 0.23f;
    private float m_rotateCooldown = 0;
    [SerializeField] private TweenCurveHelper.CurveType CurveType;
    //[SerializeField] private GameObject m_cameraObject = null;

    private UnlockableManager m_unlockableManager = null;
    //private Button m_buyButton = null;
    [SerializeField] private Color m_unselectedColour = new Color(0.25f, 0.25f, 0.25f);

    private int m_currentShipNumber = 0;
    private int m_currentColourNumber = 0;
    private int m_currentNeonNumber = 0;


    [SerializeField] private Image m_colourImage = null;
    [SerializeField] private Image m_neonImage = null;

    // Currently Selected
    private Color m_selectedColour = Color.white;
    private Color m_selectedNeon = Color.white;
    private UnlockableShip m_selectedShip = null;

    [SerializeField] private Text m_colourPriceText = null;
    [SerializeField] private Text m_neonPriceText = null;
    [SerializeField] private Text m_shipPriceText = null;

    [SerializeField] private GameObject m_colourBuyGameObject = null;
    [SerializeField] private GameObject m_neonBuyGameObject = null;
    [SerializeField] private GameObject m_shipBuyGameObject = null;

    [SerializeField] private GameObject m_applyGameObject = null;
    [SerializeField] private List<ShipCustomiser> m_shipCustomiserList = new List<ShipCustomiser>();

    [SerializeField] private Text m_gemDustText = null;

    private int m_selectedPrice = 0;
    //[HideInInspector] public int m_shopManager.m_currentGemDust = 0;
    [SerializeField] private ShopManager m_shopManager = null;
    [SerializeField] private GameObject m_insufficientGemCanvas = null;


    // Use this for initialization
    void Start () {
        if (!m_previewPrefab.GetComponent<ShipPreview>())
            m_previewPrefab.AddComponent<ShipPreview>();
        if (!m_shopManager)
            m_shopManager = FindObjectOfType<ShopManager>();

        m_unlockableManager = FindObjectOfType<UnlockableManager>();
        //ShipCustomiser[] m_shipCustomiser = Find<ShipCustomiser>();

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
            Material matInstance = shipInstance.GetComponent<Renderer>().material = Instantiate(ship.m_material);
            //Material matInstance = Material.Instantiate(ship.m_material);
            matInstance.SetColor("_EmissionColor", m_unselectedColour);
            matInstance.mainTexture = null;
            i++;
        }

        m_selectedShip = m_unlockableManager.m_unlockableShips[m_currentShipNumber];

        //m_textComponent.text = m_unlockableShips[m_currentShipNumber].price.ToString();

        //LoadDust();

        m_shipPriceText.text = m_unlockableManager.m_unlockableShips[m_currentShipNumber].price.ToString();

        m_colourImage.color = m_unlockableManager.m_unlockableColours[m_currentColourNumber].m_colour;

        m_neonImage.color = m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].m_colour;

        //// This is to turn off the Apply button at the start but issue would be if the item is not unlocked yet
        //m_colourBuyGameObject.SetActive(!m_unlockableManager.m_unlockableColours[m_currentColourNumber].unlocked);
        //m_neonBuyGameObject.SetActive(!m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].unlocked);
        //m_shipBuyGameObject.SetActive(!m_unlockableManager.m_unlockableShips[m_currentShipNumber].unlocked);
        PriceDisplay("Colour");
        PriceDisplay("Neon");
        PriceDisplay("Ship");

        //if (m_cameraObject)
        //    m
    }

    // Update is called once per frame
    void Update () {
        // Create a dynamic spawner
        m_gemDustText.text = "Current Gem Dust: " + m_shopManager.m_currentGemDust;

        if (m_rotateCooldown > 0)
            m_rotateCooldown -= Time.deltaTime;

        if (!m_applyGameObject)
            return;
        if (m_unlockableManager.m_unlockableColours[m_currentColourNumber].unlocked &&
            m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].unlocked &&
            m_unlockableManager.m_unlockableShips[m_currentShipNumber].unlocked)
        {
            m_applyGameObject.SetActive(true);
        }
        else
        {
            m_applyGameObject.SetActive(false);
        }
    }

    public void TurnLeft(string unlockableType)
    {
        switch (unlockableType)
        {
            case ("Colour"):
                m_currentColourNumber--;
                if (m_currentColourNumber <= 0)
                    m_currentColourNumber = 0;
                PriceDisplay("Colour");
                break;
            case ("Neon"):
                m_currentNeonNumber--;
                if (m_currentNeonNumber <= 0)
                    m_currentNeonNumber = 0;
                PriceDisplay("Neon");
                break;
            case ("Ship"):
                if (m_rotateCooldown > 0)
                    break;

                //Debug.Log(degBetweenShips);
                m_rotateCooldown = m_rotateTime + 0.1f;
                //gameObject.transform.Rotate(0, degBetweenShips, 0);
                //Tween.LocalRotation(transform, transform.localEulerAngles + new Vector3(0, degBetweenShips, 0), m_rotateTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);
                Tween.Rotate(transform, new Vector3(0, degBetweenShips, 0), Space.Self, m_rotateTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);
                m_currentShipNumber--;
                if (m_currentShipNumber < 0)
                    m_currentShipNumber = m_unlockableManager.m_unlockableShips.Length - 1;
                PriceDisplay("Ship");
                break;
        }
    }
    public void TurnRight(string unlockableType)
    {
        switch (unlockableType)
        {
            case ("Colour"):
                m_currentColourNumber++;
                if (m_currentColourNumber >= m_unlockableManager.m_unlockableColours.Length)
                    m_currentColourNumber = m_unlockableManager.m_unlockableColours.Length-1;
                PriceDisplay("Colour");
                break;
            case ("Neon"):
                m_currentNeonNumber++;
                if (m_currentNeonNumber >= m_unlockableManager.m_unlockableHighlights.Length)
                    m_currentNeonNumber = m_unlockableManager.m_unlockableHighlights.Length-1;
                PriceDisplay("Neon");
                break;
            case ("Ship"):
                if (m_rotateCooldown > 0)
                    break;

                //Debug.Log(degBetweenShips);
                //gameObject.transform.Rotate(0, -degBetweenShips, 0);
                m_rotateCooldown = m_rotateTime + 0.1f;
                //Tween.LocalRotation(transform, transform.localEulerAngles + new Vector3(0, -degBetweenShips, 0), m_rotateTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);
                Tween.Rotate(transform, new Vector3(0, -degBetweenShips, 0), Space.Self, m_rotateTime, 0, TweenCurveHelper.GetCurve(CurveType), Tween.LoopType.None, null, null, false);
                m_currentShipNumber++;
                if (m_currentShipNumber > m_unlockableManager.m_unlockableShips.Length - 1)
                    m_currentShipNumber = 0;
                PriceDisplay("Ship");
                break;
        }
        Debug.Log(degBetweenShips);
        //if (m_unlockableManager.)
    }

                                  
    private void PriceDisplay(string unlockableType)
    {
        switch (unlockableType)
        {
            case ("Colour"):
                m_colourImage.color = m_unlockableManager.m_unlockableColours[m_currentColourNumber].m_colour;
                if (m_unlockableManager.m_unlockableColours[m_currentColourNumber].unlocked)
                {
                    m_colourBuyGameObject.SetActive(false);
                    return;
                }
                m_colourBuyGameObject.SetActive(true);
                m_colourPriceText.text = "Buy: " + m_unlockableManager.m_unlockableColours[m_currentColourNumber].price.ToString();
                break;
            case ("Neon"):
                m_neonImage.color = m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].m_colour;
                if (m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].unlocked)
                {
                    m_neonBuyGameObject.SetActive(false);
                    return;
                }
                m_neonBuyGameObject.SetActive(true);
                m_neonPriceText.text = "Buy: " + m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].price.ToString();
                break;
            case ("Ship"):
                if (m_unlockableManager.m_unlockableShips[m_currentShipNumber].unlocked)
                {
                    m_shipBuyGameObject.SetActive(false);
                    return;
                }
                m_shipBuyGameObject.SetActive(true);
                m_shipPriceText.text = "Buy: " + m_unlockableManager.m_unlockableShips[m_currentShipNumber].price.ToString();
                break;
        }
    }

    public void BuySelected(string unlockableType)
    {
        switch (unlockableType)
        {
            case ("Colour"):
                if (m_shopManager.m_currentGemDust < m_unlockableManager.m_unlockableColours[m_currentColourNumber].price)
                {
                    InsufficientFund();
                    return;
                }
                else
                {
                    m_shopManager.m_currentGemDust -= m_unlockableManager.m_unlockableColours[m_currentColourNumber].price;
                    m_unlockableManager.UnlockColour(m_unlockableManager.m_unlockableColours[m_currentColourNumber]);
                    m_colourBuyGameObject.SetActive(false);
                }
                break;
            case ("Neon"):
                if (m_shopManager.m_currentGemDust < m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].price)
                {
                    InsufficientFund();
                    return;
                }
                else
                {
                    m_shopManager.m_currentGemDust -= m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber].price;
                    m_unlockableManager.UnlockHighlight(m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber]);
                    m_neonBuyGameObject.SetActive(false);
                }
                break;
            case ("Ship"):
                if (m_shopManager.m_currentGemDust < m_unlockableManager.m_unlockableShips[m_currentShipNumber].price)
                {
                    InsufficientFund();
                    return;
                }
                else
                {
                    m_shopManager.m_currentGemDust -= m_unlockableManager.m_unlockableShips[m_currentShipNumber].price;
                    m_unlockableManager.UnlockShip(m_unlockableManager.m_unlockableShips[m_currentShipNumber]);
                    m_shipBuyGameObject.SetActive(false);
                }
                break;
        }
        m_gemDustText.text = "Current Gem Dust: " + m_shopManager.m_currentGemDust;
        //SaveDust();
    }

    public void ApplyTriggered()
    {
        foreach (ShipCustomiser customiser in m_shipCustomiserList)
        {
            customiser.CustomiseColour(m_unlockableManager.m_unlockableColours[m_currentColourNumber]);
            customiser.CustomiseHighlights(m_unlockableManager.m_unlockableHighlights[m_currentNeonNumber]);
            customiser.CustomiseShip(m_unlockableManager.m_unlockableShips[m_currentShipNumber]);
        }
        //Instantiate(m_unlockableShips[m_currentShipNumber]
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
            if (!m_shipPriceText)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to a text component", "Exit");
                stopApplication = true;
            }
            if (!m_unlockableManager)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script can't find Unlockable Manager script in the scene. Make sure you have one", "Exit");
                stopApplication = true;
            }
            if (m_shipCustomiserList.Count <= 0)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script's ship customiser list is empty. Make sure you have at least one", "Exit");
                stopApplication = true;
            }
            //if (!m_buyButton)
            //{
            //    UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to the buy button", "Exit");
            //    stopApplication = true;
            //}
            if (!m_colourImage)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to the colour box image", "Exit");
                stopApplication = true;
            }
            if (!m_neonImage)
            {
                UnityEditor.EditorUtility.DisplayDialog("Error", "Turntable script does not have a reference to the neon gem image", "Exit");
                stopApplication = true;
            }
            if (stopApplication)
                UnityEditor.EditorApplication.isPlaying = false;

        }

#endif
    }
    private void InsufficientFund()
    {
        // This part is used to show error dialog messages if there are parts of the script that have not been initialised properly
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
                UnityEditor.EditorUtility.DisplayDialog("Insufficient funds", "Play Free Flow to farm for more gem dust", "Exit");
        }

#endif
        if (m_insufficientGemCanvas)
            m_insufficientGemCanvas.SetActive(true);
    }

}
