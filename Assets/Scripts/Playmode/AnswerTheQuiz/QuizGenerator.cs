using TMPro;
using UnityEngine;

public class QuizGenerator : MonoBehaviour
{
    [SerializeField] private EyeController _eyeController;
    [SerializeField] private GameObject _question;
    [SerializeField] private GameObject _rightAns;
    [SerializeField] private GameObject _leftAns;
    [SerializeField] private GameObject _correctR;
    [SerializeField] private GameObject _wrongR;
    [SerializeField] private GameObject _correctL;
    [SerializeField] private GameObject _wrongL;

    private string[] _quizStrings = new string[]{"哪個比較大", "哪個比較小"};
    private string _quizString;

    private int _rightOperand1;
    private int _rightOperand2;
    private int _rightResult;

    private int _leftOperand1;
    private int _leftOperand2;
    private int _leftResult;

    private char[] _operators = new char[]{'+', '-', '×'};
    private char _rightOperator;
    private char _leftOperator;

    private bool _rightAnsCorrect = false;
    private bool _leftAnsCorrect = false;
    
    void Start()
    {
        _quizString = _quizStrings[Random.Range(0, _quizStrings.Length)];
        _question.GetComponent<TMP_Text>().text = _quizString;

        do
        {
            _rightOperand1 = Random.Range(1, 11);
            _rightOperand2 = Random.Range(1, 11);
            _rightOperator = _operators[Random.Range(0, _operators.Length)];
            if (_rightOperand2 > _rightOperand1)
            {
                int temp = _rightOperand1;
                _rightOperand1 = _rightOperand2;
                _rightOperand2 = temp;
            }
            _rightResult = CalculateTheAnswer(_rightOperand1, _rightOperand2, _rightOperator);
            _rightAns.GetComponent<TMP_Text>().text = _rightOperand1.ToString() + " " + _rightOperator + " " + _rightOperand2.ToString(); 

            _leftOperand1 = Random.Range(1, 11);
            _leftOperand2 = Random.Range(1, 11);
            _leftOperator = _operators[Random.Range(0, _operators.Length)];
            if (_leftOperand2 > _leftOperand1)
            {
                int temp = _leftOperand1;
                _leftOperand1 = _leftOperand2;
                _leftOperand2 = temp;
            }
            _leftResult = CalculateTheAnswer(_leftOperand1, _leftOperand2, _leftOperator);
            _leftAns.GetComponent<TMP_Text>().text = _leftOperand1.ToString() + " " + _leftOperator + " " + _leftOperand2.ToString();
        } while (_rightResult == _leftResult);

        if (_quizString == "哪個比較大")
        {
            _rightAnsCorrect = (_rightResult > _leftResult) ? true : false;
            _leftAnsCorrect = (_rightAnsCorrect == false) ? true : false;
        }
        else
        {
            _rightAnsCorrect = (_rightResult < _leftResult) ? true : false;
            _leftAnsCorrect = (_rightAnsCorrect == false) ? true : false;
        }
    }

    int CalculateTheAnswer(int Operand1, int Operand2, char inputOperator)
    {
        int result = 0;
        switch (inputOperator)
        {
            case '+':
                result = Operand1 + Operand2;
                break;

            case '-':
                result = Operand1 - Operand2;
                break;

            case '×':
                result = Operand1 * Operand2;
                break;
        }
        return result;
    }

    void CheckRightAnswer()
    {
        if (_rightAnsCorrect)
        {
            GameManager.Instance.MissionComplete();
            Debug.Log("Correct");
            StartCoroutine(TransformUtil.ScaleObject(_correctR, 0, 1, 1));
        }
        else
        {
            GameManager.Instance.MissionFailed();
            Debug.Log("Wrong");
            StartCoroutine(TransformUtil.ScaleObject(_wrongR, 0, 1, 1));
        }
    }

    void CheckLeftAnswer()
    {
        if (_leftAnsCorrect)
        {
            GameManager.Instance.MissionComplete();
            Debug.Log("Correct");
            StartCoroutine(TransformUtil.ScaleObject(_correctL, 0, 1, 1));
        }
        else
        {
            GameManager.Instance.MissionFailed();
            Debug.Log("Wrong");
            StartCoroutine(TransformUtil.ScaleObject(_wrongL, 0, 1, 1));
        }
    }

    void OnEnable()
    {
        _eyeController.OnEyeLookInRight += CheckRightAnswer;
        _eyeController.OnEyeLookInLeft += CheckLeftAnswer;
    }

    void OnDisable()
    {
        _eyeController.OnEyeLookInRight -= CheckRightAnswer;
        _eyeController.OnEyeLookInLeft -= CheckLeftAnswer;
    }
}
