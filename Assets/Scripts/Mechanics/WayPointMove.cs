using UnityEngine;

public class LocalOffsetMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _localOffsetA = new Vector3(-2f, 0f, 0f); // �������� �����
    [SerializeField] private Vector3 _localOffsetB = new Vector3(2f, 0f, 0f);  // �������� ������
    [SerializeField] private float _speed = 1f;

    private Vector3 _startPosition;
    private float _progress = 0f;
    private bool _movingToB = true;

    private void Start()
    {
        _startPosition = transform.position; // ���������� ��������� �������
    }

    private void Update()
    {
        // ��������� ����������� ��������
        if (_movingToB)
        {
            _progress += _speed * Time.deltaTime;
            if (_progress >= 1f)
            {
                _progress = 1f;
                _movingToB = false;
            }
        }
        else
        {
            _progress -= _speed * Time.deltaTime;
            if (_progress <= 0f)
            {
                _progress = 0f;
                _movingToB = true;
            }
        }

        // ������������� ����� ����� ����������
        Vector3 currentOffset = Vector3.Lerp(_localOffsetA, _localOffsetB, _progress);

        // ��������� � ��������� �������
        transform.position = _startPosition + currentOffset;
    }

    // ������������ ����� � ��������� (Gizmos)
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_startPosition + _localOffsetA, 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_startPosition + _localOffsetB, 0.2f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + _localOffsetA, 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + _localOffsetB, 0.2f);
        }
    }
}