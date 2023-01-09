using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] float _maxAlpha = 1f;
    [SerializeField] float _minAlpha = 0.3f;
    [SerializeField] float _twinkleDelay = 0.5f;
    [SerializeField] float _twinkleDuration = 0.3f;

    private Coroutine _currentCoroutine;
    private float _desiredAlpha;
    private float _currentAlpha;
    private float _startAlpha;
    private float _timeStart;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;

        _startAlpha = 0f;
        _desiredAlpha = 0f;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(Twinkle());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }

    private void Update()
    {
        _currentAlpha = Mathf.Lerp(_startAlpha, _desiredAlpha, (Time.time - _timeStart) / _twinkleDuration);
        _canvasGroup.alpha = _currentAlpha;
    }

    private IEnumerator Twinkle()
    {
        while(gameObject.activeInHierarchy)
        {
            _desiredAlpha = _maxAlpha;
            _startAlpha = _currentAlpha;
            _timeStart = Time.time;

            yield return new WaitForSeconds(_twinkleDuration + _twinkleDelay);

            _desiredAlpha = _minAlpha;
            _startAlpha = _currentAlpha;
            _timeStart = Time.time;

            yield return new WaitForSeconds(_twinkleDuration);
        }
    }
}
