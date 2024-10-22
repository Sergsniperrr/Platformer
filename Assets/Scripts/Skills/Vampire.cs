using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Vampire : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _slider;
    [SerializeField] private VampiricAura _aura;
    [SerializeField] private float _actionTime = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private int _healthPerSecond = 5;

    private Coroutine _coroutineOfUseSkill;
    private Coroutine _coroutineOfRegenerateSkill;
    private PlayerInput _input;

    public bool _isWorking { get; private set; }
    public bool _isReady { get; private set; } = true;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void Update()
    {
        if (_input.IsVampirismKeyPress && _isReady)
        {
            if (_coroutineOfRegenerateSkill != null)
                StopCoroutine(_coroutineOfRegenerateSkill);

            _coroutineOfUseSkill = StartCoroutine(UseSkill());
        }
    }

    private void StealHealth(Enemy enemy, int stealedHealth)
    {
        enemy.TakeDamageFast(stealedHealth);
        _health.IncreaseFast(stealedHealth);
    }

    private IEnumerator UseSkill()
    {
        int hitPoint = 1;
        float second = 1f;
        float counter = 0f;
        float timeForOneHitPoint = second / _healthPerSecond;
        Enemy enemy;

        _isReady = false;
        _isWorking = true;
        _slider.maxValue = _actionTime;
        _slider.value = _actionTime;
        _aura.Enable();

        while (_slider.value > 0)
        {
            enemy = _aura.SearchTarget();

            if (enemy != null)
            {
                counter += Time.deltaTime;

                if (counter >= timeForOneHitPoint)
                {
                    StealHealth(enemy, hitPoint);

                    counter = 0f;
                }
            }

            _slider.value -= Time.deltaTime;

            yield return null;
        }

        _coroutineOfRegenerateSkill = StartCoroutine(RegenerateSkill());
    }

    private IEnumerator RegenerateSkill()
    {
        StopCoroutine(_coroutineOfUseSkill);

        _isWorking = false;
        _slider.maxValue = _cooldown;
        _aura.Disable();

        while (_slider.value < _cooldown)
        {
            _slider.value += Time.deltaTime;

            yield return null;
        }

        _isReady = true;
    }
}
