using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] GameObject[] lstLevel;
    [SerializeField] Transform LevelSpawn;
    [SerializeField] GameObject gameController;

    private void OnEnable()
    {
        lstLevel = Resources.LoadAll<GameObject>("Levels");
    }

    public void SelectLevel(int level)
    {
        PlayerDataManager.Instance.SetCurrentLevel(level);
    }

    public void LoadLevel(int level)
    {
        if (LevelSpawn.transform.childCount != 0)
            for (int i = 0; i < LevelSpawn.transform.childCount; i++)
            {
                Destroy(LevelSpawn.transform.GetChild(i).gameObject);
            }
        StartCoroutine(DelayEnableSpawnLevel(lstLevel[level], LevelSpawn));
    }

    IEnumerator DelayEnableSpawnLevel(GameObject g, Transform t)
    {
        yield return new WaitUntil(() => Instantiate(g, t));
        gameController.SetActive(true);
    }

}
