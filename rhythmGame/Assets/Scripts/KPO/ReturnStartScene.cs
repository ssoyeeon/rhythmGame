using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ReturnStartScene : MonoBehaviour
{
    // UI �ؽ�Ʈ ��ҵ�
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI badText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI perfectText;

    // �ִϸ��̼� ���� ������
    public float animationDuration = 2f; // �ִϸ��̼� ���� �ð� (��)
    public AnimationCurve animationCurve; // �ִϸ��̼� � (�����Ϳ��� ���� ����)

    public bool isAnimateNumber = true;

    private void Start()
    {
        // ���� ���۽� ��� �ִϸ��̼� �ڷ�ƾ ����
        if(isAnimateNumber)
        StartCoroutine(AnimateStats());
    }

    private IEnumerator AnimateStats()
    {
        // GameManager���� ���� ��谪���� ������
        int targetScore = GameManager.Instance.score;
        int targetMiss = GameManager.Instance.MissCount;
        int targetBad = GameManager.Instance.BadCount;
        int targetGood = GameManager.Instance.GoodCount;
        int targetGreat = GameManager.Instance.GreatCount;
        int targetPerfect = GameManager.Instance.PerfectCount;

        float elapsedTime = 0f;

        // �ִϸ��̼� ���� �ð����� �ݺ�
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float curvedT = animationCurve.Evaluate(t); // �ִϸ��̼� � ����

            // �� ��迡 ���� ���� ���� ����ϰ� UI�� ������Ʈ
            UpdateText(scoreText, 0, targetScore, curvedT);
            UpdateText(missText, 0, targetMiss, curvedT);
            UpdateText(badText, 0, targetBad, curvedT);
            UpdateText(goodText, 0, targetGood, curvedT);
            UpdateText(greatText, 0, targetGreat, curvedT);
            UpdateText(perfectText, 0, targetPerfect, curvedT);

            yield return null; // ���� �����ӱ��� ���
        }

        // �ִϸ��̼� ���� �� ��Ȯ�� ���������� ����
        SetFinalValue(scoreText, targetScore);
        SetFinalValue(missText, targetMiss);
        SetFinalValue(badText, targetBad);
        SetFinalValue(goodText, targetGood);
        SetFinalValue(greatText, targetGreat);
        SetFinalValue(perfectText, targetPerfect);
    }

    // �ؽ�Ʈ UI�� ���� �ִϸ��̼� ���൵�� ���� ������Ʈ
    private void UpdateText(TextMeshProUGUI text, int start, int end, float t)
    {
        if (text != null)
        {
            int current = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
            text.text = current.ToString();
        }
    }

    // �ؽ�Ʈ UI�� ������ ����
    private void SetFinalValue(TextMeshProUGUI text, int value)
    {
        if (text != null)
        {
            text.text = value.ToString();
        }
    }

    void Update()
    {
        // �����̽��ٸ� ������ ���� ������ ���ư�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}