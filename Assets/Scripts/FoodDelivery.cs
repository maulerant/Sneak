using UnityEngine;
using Random = System.Random;

public class FoodDelivery : MonoBehaviour
{
    [SerializeField] private Sneak _sneak;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _topBorder;
    [SerializeField] private Transform _bottomBorder;

    [Header("Food Configuration")]
    [SerializeField] private GameObject _foodPrefab;

    [SerializeField] private int _foodMaxCount;
    private int _deliveredFoodCount = 0;

    private void OnEnable()
    {
        _sneak.SnakeEatFood += OnFoodEaten;
    }

    private void OnDisable()
    {
        _sneak.SnakeEatFood -= OnFoodEaten;
    }

    private void OnFoodEaten()
    {
        _deliveredFoodCount -= 1;
    }

    private void Update()
    {
        if (_deliveredFoodCount < _foodMaxCount)
        {
            DeliverFood();
        }
    }

    private void DeliverFood()
    {
        float x = UnityEngine.Random.Range(_leftBorder.position.x + 2, _rightBorder.position.x - 2);
        float z = UnityEngine.Random.Range(_bottomBorder.position.z + 2, _topBorder.position.z - 2 );
        var food = Instantiate(_foodPrefab, new Vector3(x, 0, z), Quaternion.identity);
        food.tag = Tags.Food;
        _deliveredFoodCount += 1;
    }
}
