using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReadyGridAlphaChanges : MonoBehaviour
{
    public Color32 m_cDefaultColor;
    private Color32 m_cGridColour;
    public byte m_byTransitionSpeed;
    public byte m_byMinAlpha;
    public byte m_byMaxAlpha;

    private byte CurrentAlpha;

    public float FadeTime = 4;
    public float m_fTime = 4;
    public bool m_bStartFade;
    private bool m_bEndFade;

    private SpriteRenderer[] m_srGridBorders = new SpriteRenderer[3];

    private Color GridBorderColour;

    public Color32 GridColour { get { return m_cGridColour; } set { m_cGridColour = value; } }
    void Start()
    {
        m_srGridBorders[0] = (transform.FindChild("Ready_Zone_Grid_Front").GetComponent<SpriteRenderer>());
        m_srGridBorders[1] = (transform.FindChild("Ready_Zone_Grid_Right").GetComponent<SpriteRenderer>());
        m_srGridBorders[2] = (transform.FindChild("Ready_Zone_Grid_Left").GetComponent<SpriteRenderer>());
        GridBorderColour = m_srGridBorders[0].color;
        m_bStartFade = true;
        CurrentAlpha = m_byMinAlpha;
    }
    void OnEnable()
    {
        //m_fTime = 0;
    }

    //  Update is called once per frame
    void Update()
    {
        //FadeInOut();
        FadeBetweenAlphas();
    }

    void FadeInOut()
    {
        #region fading
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
        foreach (SpriteRenderer image in m_srGridBorders)
        {
            image.color = new Color(GridBorderColour.r , GridBorderColour.g , GridBorderColour.b , m_fTime / FadeTime);
        }
        #endregion
    }

    void FadeBetweenAlphas()
    {
        #region 
        //do the logic to change alphas/ colours;
        if (m_bStartFade)
        {
            CurrentAlpha += m_byTransitionSpeed;
            if (CurrentAlpha >= m_byMaxAlpha)
            {
                m_bStartFade = false;
                m_bEndFade = true;
            }
        }

        else if (m_bEndFade)
        {
            CurrentAlpha -= m_byTransitionSpeed;
            if (CurrentAlpha <= m_byMinAlpha)
            {
                m_bEndFade = false;
                m_bStartFade = true;
            }
        }



        //set its colours
        foreach (SpriteRenderer image in m_srGridBorders)
        {
            image.color = new Color32(GridColour.r , GridColour.g , GridColour.b , CurrentAlpha);
        }

        #endregion
    }
}
