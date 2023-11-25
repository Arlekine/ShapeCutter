using DG.Tweening;
using UnityEngine;

namespace DoodleJump
{
    public class StateAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _firstKeyPosition;
        [SerializeField] private Vector3 _secondKeyPosition;

        [Space]
        [SerializeField] private Vector3 _firstKeyRotation;
        [SerializeField] private Vector3 _secondKeyRotation;

        [Space]
        [SerializeField] private float _loopDuration;
        [SerializeField] private Ease _ease = Ease.Linear;

        private Sequence _sequence;

        private void OnEnable()
        {
            transform.localEulerAngles = _firstKeyRotation; 
            transform.localPosition = _firstKeyPosition; 

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOLocalRotate(_secondKeyRotation, _loopDuration * 0.5f).SetEase(Ease.Linear));
            _sequence.Join(GetComponent<RectTransform>().DOAnchorPos(_secondKeyPosition, _loopDuration * 0.5f).SetEase(Ease.Linear));
            _sequence.Append(transform.DOLocalRotate(_firstKeyRotation, _loopDuration * 0.5f).SetEase(Ease.Linear));
            _sequence.Join(GetComponent<RectTransform>().DOAnchorPos(_firstKeyPosition, _loopDuration * 0.5f).SetEase(Ease.Linear));
            _sequence.SetLoops(-1);
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }

        [EditorButton]
        private void AddSmallMove()
        {
            _firstKeyPosition = GetComponent<RectTransform>().anchoredPosition;
            _secondKeyPosition = GetComponent<RectTransform>().anchoredPosition + Random.insideUnitCircle * 30f;
        }

        [EditorButton]
        private void AddRandomRot()
        {
            _firstKeyRotation.z = transform.localEulerAngles.z;
            _secondKeyRotation.z = Random.Range(transform.localEulerAngles.z - 20f, transform.localEulerAngles.z + 20f);
        }

        [EditorButton]
        private void SetRandomTime()
        {
            _loopDuration = Random.Range(4f, 6f);
        }
    }
}