using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollectThemAllInTimeCore : MonoBehaviour 
{
    public Text CollectibleCounterText;
    public Text TimeText;
    private List<CollectibleBehaviour> m_allCollectibles;
    private int m_totalCollectibles;
    private float m_time = 0f;
    private bool m_isOver = false;
    private string m_sBestTime = "";

	void Start () 
    {
        m_allCollectibles = new List<CollectibleBehaviour>( FindObjectsOfType<CollectibleBehaviour>() );
        m_totalCollectibles = m_allCollectibles.Count;
        //PlayerPrefs.SetFloat("PlayerBestTime", 60*5);
        m_sBestTime = "\n<color=orange>" + ConvertTimeToStr(PlayerPrefs.GetFloat("PlayerBestTime", 60f * 5f)) + "</color>";
	}
	
	void Update () 
    {
        if (!m_isOver)
        {
            m_allCollectibles.RemoveAll(x => x == null);
            CollectibleCounterText.text = (m_totalCollectibles - m_allCollectibles.Count) + "/" + m_totalCollectibles;
            m_time += Time.deltaTime;
            TimeText.text = ConvertTimeToStr(m_time) + m_sBestTime;
            if (m_allCollectibles.Count == 0)
            {
                m_isOver = true;
                float bestTime = PlayerPrefs.GetFloat("PlayerBestTime", 60f*5f);
                if (m_time < bestTime)
                {
                    PlayerPrefs.SetFloat("PlayerBestTime", m_time);
                    m_sBestTime = "\n<color=orange>" + ConvertTimeToStr(m_time) + "</color>";
                    StartCoroutine(PlayWinAnim());
                }
            }
        }
        else
        {
            TimeText.text = ConvertTimeToStr(m_time) + m_sBestTime;
        }
	}

    IEnumerator PlayWinAnim()
    {
        for (float t = 0f; t < Mathf.PI; t += Time.deltaTime * Mathf.PI)
        {
            TimeText.transform.localScale = Vector2.one * (1f + Mathf.Sin(t));
            yield return null;
        }
        TimeText.transform.localScale = Vector2.one;
    }

    string ConvertTimeToStr(float time)
    {
        int totalSecs = (int)time;
        int h = totalSecs / 3600;
        int m = (totalSecs % 3600) / 60;
        int s = totalSecs % 60;
        return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
    }
}
