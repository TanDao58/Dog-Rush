using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLevel : MonoBehaviour
{
    [SerializeField] Transform LevelTrans;
    [SerializeField] GameObject LevelPrefab;
    [SerializeField] ButtonManager uiButton;
    bool isSetup = false;
    private void OnEnable()
    {
        if (!isSetup)
        {
            for (int i = 0; i < 30; i++)
            {
                int temp = i;
                GameObject g = Instantiate(LevelPrefab, LevelTrans);
                g.GetComponent<Button>().onClick.AddListener(() =>
                {
                    uiButton.ClickChangeLevel(temp);
                    this.gameObject.SetActive(false);
                });
                g.transform.GetChild(0).GetComponent<Text>().text = "Level " + (i + 1).ToString();
            }
            isSetup = true;
        }
    }

}
