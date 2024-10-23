using System.Collections;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private VampireBar _progressBar;
    [SerializeField] private VampiricAura _aura;
    [SerializeField] private float _actionTime = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private int _healthPerSecond = 5;

    private PlayerInput _input;
    private bool _isReady = true;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void Update()
    {
        if (_input.IsVampirismKeyPress && _isReady)
            StartCoroutine(UseSkill());
    }

    private void StealHealth(Enemy enemy, float stealedHealth)
    {
        stealedHealth = enemy.TransferHealth(stealedHealth);
        _health.IncreaseFast(stealedHealth);
    }

    private IEnumerator UseSkill()
    {
        float counter = 0f;
        Enemy enemy;

        _isReady = false;
        _aura.Enable();

        while (counter < _actionTime)
        {
            enemy = _aura.SearchTarget();

            if (enemy != null)
                StealHealth(enemy, _healthPerSecond * Time.deltaTime);

            counter += Time.deltaTime;

            _progressBar.Decrease(_progressBar.MaxValue * Time.deltaTime / _actionTime);

            yield return null;
        }

        StartCoroutine(RegenerateSkill());
    }

    private IEnumerator RegenerateSkill()
    {
        float counter = 0;

        _aura.Disable();

        while (counter < _cooldown)
        {
            counter += Time.deltaTime;

            _progressBar.Increase(_progressBar.MaxValue * Time.deltaTime / _cooldown);

            yield return null;
        }

        _isReady = true;
    }
}
