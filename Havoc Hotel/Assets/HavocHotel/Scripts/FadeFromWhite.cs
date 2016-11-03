using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeFromWhite : MonoBehaviour
{

    public float FadeTime = 4;
    public float m_fTime = 4;
    public bool m_bStartFade;
    private bool m_bEndFade;
    public GameObject Player1;


    void Start()
    {
        m_bStartFade = true;
        m_fTime = 4;

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
            Debug.Log(m_fTime);
            m_fTime -= Time.deltaTime;
            GetComponent<Image>().color = new Color(1, 1, 1, (m_fTime / FadeTime));
            if (m_fTime <= 0)
            {
                m_bStartFade = false;
                this.enabled = false;
            }
        }

    }
}
