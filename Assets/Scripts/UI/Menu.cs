using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Entities;
using Unity.Collections;

namespace DefenseGame
{
    public class Menu : MonoBehaviour
    {
        public static Menu Instance;

        [SerializeField] private TextMeshProUGUI gameOverSummarySubtitle;
        [SerializeField] private CanvasGroup gameOverGroup;

        private bool initialized = false;
        private bool isGameOverDisplayed = false;
        private EntityManager entityManager;
        private Entity playerEntity;

        public void Initialize()
        {
            initialized = false;
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var inputsDataQuery = entityManager.CreateEntityQuery(typeof(InputsData));
            var entities = inputsDataQuery.ToEntityArray(Allocator.TempJob);

            if (entities.Length > 0)
            {
                playerEntity = entities[0];
            }
            else
            {
                Debug.LogError($"No entities of {nameof(InputsData)} have been found! Canceling {nameof(Menu)} initialization!");
                entities.Dispose();
                return;
            }

            initialized = true;
            entities.Dispose();
            Debug.Log("Initialized HUD Controller");
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ToggleGameOverScreen(bool value, float experienceGained, int levelReached)
        {
            if (isGameOverDisplayed == value)
            {
                Debug.Log($"Gameover screen toggle value is already at state {value}");
            }

            ToggleGameOverScreenInternal(value, experienceGained, levelReached);
        }

        private void ToggleGameOverScreenInternal(bool value, float experienceGained, int levelReached)
        {
            isGameOverDisplayed = value;
            float desiredAlphaValue = value ? 1f : 0f;
            gameOverSummarySubtitle.text = $"You've scored {experienceGained} experience and reached {levelReached} level";
            gameOverGroup.alpha = desiredAlphaValue;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (initialized && !isGameOverDisplayed)
            {
                if (entityManager.HasComponent<GameOverTag>(playerEntity))
                {
                    Debug.Log("Game over!!!");
                    var gameOverTag = entityManager.GetComponentData<GameOverTag>(playerEntity);
                    ToggleGameOverScreen(true, gameOverTag.experienceGained, gameOverTag.levelReached);
                }
            }
        }
    }
}