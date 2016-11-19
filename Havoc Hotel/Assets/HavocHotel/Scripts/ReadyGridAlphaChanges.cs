using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReadyGridAlphaChanges : MonoBehaviour
{

    public float FadeTime = 4;
    public float m_fTime = 4;
    public bool m_bStartFade;
    private bool m_bEndFade;

    private List<SpriteRenderer> m_iGridBorders = new List<SpriteRenderer>();

    private Color GridBorderColour;
    void Start()
    {
        m_iGridBorders.Add(transform.FindChild("Ready_Zone_Grid_Front").GetComponent<SpriteRenderer>());
        m_iGridBorders.Add(transform.FindChild("Ready_Zone_Grid_Right").GetComponent<SpriteRenderer>());
        m_iGridBorders.Add(transform.FindChild("Ready_Zone_Grid_Left").GetComponent<SpriteRenderer>());
        GridBorderColour = m_iGridBorders[0].color;
        m_bStartFade = true;
    }
    void OnEnable()
    {
        //m_fTime = 0;
    }

    //  Update is called once per frame
    void Update()
    {
        if (m_bStartFade)
        {
            m_fTime -= Time.deltaTime;
            if (m_fTime <= FadeTime * 0.33f)
            {
                m_bEndFade = true;
                m_bStartFade = false;
            }
        }
        else if (m_bEndFade)
        {
            m_fTime += Time.deltaTime;
            if (m_fTime >= FadeTime)
            {
                m_bEndFade = false;
                m_bStartFade = true;
            }

        }
        foreach (SpriteRenderer image in m_iGridBorders)
        {
            image.color = new Color(GridBorderColour.r , GridBorderColour.g , GridBorderColour.b , m_fTime / FadeTime);
        }
    }
}
