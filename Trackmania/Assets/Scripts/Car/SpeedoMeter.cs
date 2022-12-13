using UnityEngine;

namespace Car
{

    public class SpeedoMeter : MonoBehaviour
    {
        [Header("Car rigibody")]
        [SerializeField] private Rigidbody _carRigibody;

        [Header("Delay")]
        [SerializeField] private float _delayRefresh = 0.1f;

        [Header("Visible value")]
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 999f;

        private float _value;
        private bool _isRunning = false;
        private TextMesh _textMesh;
        // Start is called before the first frame update
        private void Awake()
        {
            _textMesh = GetComponent<TextMesh>();
            _textMesh.text = "000";
        }

        public void Launch()
        {
            _isRunning = true;
            StartCoroutine(ChangeText());
        }

        public void Stop()
        {
            _isRunning = false;
            StopCoroutine(ChangeText());
        }

        public void Pause()
        {
            _isRunning = false;
        }

        public void UnPause()
        {
            _isRunning = true;
        }

        System.Collections.IEnumerator ChangeText()
        {
            while (true)
            {
                if (_isRunning)
                {
                    _value = Mathf.Floor(_carRigibody.velocity.magnitude);
                    _value = Mathf.Clamp(_value, _minValue, _maxValue);
                    _textMesh.text = ((int)_value).ToString("D3"); // Show value as "###"
                }

                yield return new WaitForSeconds(_delayRefresh);
            }
        }
    }

}


