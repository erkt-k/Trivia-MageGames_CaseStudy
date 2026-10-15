using System.Collections.Generic;
using TMPro;
using TriviaGame.Questions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Linq;

public class QuestionUIBuilder : MonoBehaviour
{
    [Header("UI Elements")]
    [Header("Choices UI")]
    [SerializeField] private List<Button> m_choices;
    [SerializeField] private Color m_defaultChoiceColor = Color.white;
    [SerializeField] private Color m_wrongChoiceColor = Color.darkRed;
    [SerializeField] private Color m_correctChoiceColor = Color.lawnGreen;

    [Header("Question UI")]
    [SerializeField] private TextMeshProUGUI m_questionText;
    [SerializeField] private TextMeshProUGUI m_questionCategoryText;

    [Header("PopUps")]
    [SerializeField] private TextMeshProUGUI m_popUpText;

    public List<Button> Choices => m_choices;

    public UnityEvent OnAnimationComplete = new UnityEvent();

    public void BuildQuestionUI(Question question)
    {
        // Draw the question body.
        m_questionText.text = question.QuestionText;
        m_questionCategoryText.text = question.QuestionCategory;

        // Draw question choices.
        for (int i = 0; i < question.QuestionChoices.Count; i++)
        {
            m_choices[i].GetComponentInChildren<TextMeshProUGUI>().text = 
                question.QuestionChoices[i];
        }
    }

    public void ShowCorrectAnswer(string correctIndex)
    {
        // Get the choice to animate
        Choice chosenAnswer = GetChoiceByIndex(correctIndex);

        // Animate
        AnimateChoice(chosenAnswer, m_correctChoiceColor, true);
        ShowPopupText("Correct Answer!", m_correctChoiceColor);
    }

    public void ShowWrongAnswer(string correctIndex, string wrongIndex)
    {
        // Get the choices to animate
        Choice correctAnswer = GetChoiceByIndex(correctIndex);
        Choice wrongAnswer = GetChoiceByIndex(wrongIndex);

        // Animate
        AnimateChoice(correctAnswer, m_correctChoiceColor, false);
        AnimateChoice(wrongAnswer, m_wrongChoiceColor, true);
        ShowPopupText("Wrong Answer!", m_wrongChoiceColor);
    }

    /// <summary>
    /// Animates the given choice and Invokes the Animation Complete Event if necessary.
    /// </summary>
    /// <param name="choiceToAnimate"></param>
    /// <param name="color"></param>
    /// <param name="isTheChosenAnswer">Event and shake animation 
    /// only happens if that's the chosen choice so it doesn't unnecessarly shake or call the event twice.</param>
    private void AnimateChoice(Choice choiceToAnimate, Color color, bool isTheChosenAnswer)
    {
        // Set answers
        Image answerImage = choiceToAnimate.GetComponent<Image>();
        RectTransform answerTransform = choiceToAnimate.GetComponent<RectTransform>();

        // Animate answer
        answerImage.DOColor(color, 0.15f)
                    .OnKill(() =>
                    {
                        if (isTheChosenAnswer) OnAnimationComplete.Invoke();
                        answerImage.DOColor(m_defaultChoiceColor, 0.1f)
                                    .SetEase(Ease.OutSine);
                    })
                    .SetEase(Ease.InSine);
        
        if (isTheChosenAnswer)
        {
            answerTransform.DOKill();
            Vector3 initialScale = answerTransform.localScale;
            answerTransform.DOShakeScale(0.15f)
                            .OnKill(() =>
                            {
                                answerTransform.localScale = initialScale;
                            });
        }
    }

    /// <summary>
    /// Compares the choiceIndex with the choices' indexes and returns the choice that was found.
    /// </summary>
    /// <param name="choiceIndex"></param>
    /// <returns></returns>
    private Choice GetChoiceByIndex(string choiceIndex)
    {
        Choice choice = m_choices
            .Select(button => button.GetComponent<Choice>())
            .FirstOrDefault(choice => choice != null && choice.choiceIndex.ToLower() == choiceIndex.ToLower());

        return choice; 
    }

    private void ShowPopupText(string text, Color textColor)
    {
        // Set up text
        m_popUpText.text = text;
        m_popUpText.color = textColor;
        RectTransform textTransform = m_popUpText.rectTransform;
        float initialTextY = textTransform.anchoredPosition.y;
        float initialAlpha = m_popUpText.alpha;
        // Animate text
        textTransform.DOKill();
        m_popUpText.DOKill();

        m_popUpText.gameObject.SetActive(true);
        m_popUpText.DOFade(0f, 1.9f);
        textTransform.DOAnchorPosY(textTransform.anchoredPosition.y + 50, 2f)
                    .SetEase(Ease.InOutCubic)
                    .OnKill(() =>
                    {
                        // Put it in position again
                        m_popUpText.gameObject.SetActive(false);
                        textTransform.anchoredPosition = new Vector2(textTransform.anchoredPosition.x,
                                                            initialTextY);
                        m_popUpText.alpha = initialAlpha;
                    });
    }

    public void Reset()
    {
        foreach (Button choice in m_choices)
        {
            choice.image.color = m_defaultChoiceColor;
        }
    }
}