using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameMapManager : MonoBehaviour
{
    public static MainGameMapManager instance;

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Entity player;
    [SerializeField] float spawnRadius = 30f;
    [SerializeField] float spawnRate = 2;
    [SerializeField] float spawnHeight = 10;
    [SerializeField] int maxEnemyInMap = 30;
    [SerializeField] List<Enemy> enemies;
    [SerializeField] GameObject loseBoard;

    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        instance = this;

        player.onDie.AddListener(LoseGame);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (maxEnemyInMap <= enemies.Count) continue;
            if(player == null) yield break;

            Vector2 dir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            Vector3 posH = player.transform.position + Vector3.up * spawnHeight + new Vector3(dir.x, 0, dir.y) * Random.Range(0, spawnRadius);
            Ray ray = new Ray(posH, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity)) continue;
            var e = SimplePool.Spawn(enemyPrefab, hit.point, Quaternion.identity);
            enemies.Add(e);
            e.onDie.AddListener(RemoveEnemyFromList);
            e.Init();
        }
    }

    private void RemoveEnemyFromList(Entity enemy)
    {
        enemy.onDie.RemoveListener(RemoveEnemyFromList);
        enemies.Remove((Enemy)enemy);

        score++;
        scoreText.text = score.ToString();
    }

    int score = 0;
    private void OnDestroy()
    {
        instance = null;
    }

    private void LoseGame(Entity player)
    {
        player.onDie.RemoveListener(LoseGame);
        Cursor.lockState = CursorLockMode.None;
        //print("Lose");
        loseBoard.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Sensitivity(float value)
    {
        GameSetting.Sensitivity = 20 * value;
    }

}
