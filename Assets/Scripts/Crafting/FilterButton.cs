using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FilterButton : MonoBehaviour {

	Image m_image;
	public Sprite m_selectedImage;
	public Sprite m_deSelectedImage;
	public int m_filterNumber;

	private void Start()
	{
		m_image = GetComponent<Image>();
	}

	public void DeActivate()
	{
		m_image.sprite = m_deSelectedImage;
	}

	public void Activate()
	{
        if (!m_image)
            m_image = GetComponent<Image>();
        m_image.sprite = m_selectedImage;
	}
}
