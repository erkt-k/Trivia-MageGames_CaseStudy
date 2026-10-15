using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TriviaGame.Questions
{
    [Serializable]
    public class Question
    {
        private QuestionUIBuilder m_uiBuilder;
        [SerializeField] private string category;
        [SerializeField] private string question;
        [SerializeField] private List<string> choices;
        [SerializeField] private string answer;

        public UnityEvent<QuestionResult> OnQuestionAnswered = new UnityEvent<QuestionResult>();
        public string QuestionText => question;
        public List<string> QuestionChoices => choices;
        public string CorrectAnswer => answer;

        public string QuestionCategory
        {
            get
            {
                // Take the category and split it if it has an hypen. And rejoin it
                string categoryToReturn = string.Join(" ", category.Split("-"));

                // make sure the category has Title Case.
                TextInfo tf = CultureInfo.CurrentCulture.TextInfo;
                return tf.ToTitleCase(categoryToReturn);
            }
        }

        public void Initialize(QuestionUIBuilder questionUIBuilder)
        {
            m_uiBuilder = questionUIBuilder;
            // Build the question UI
            m_uiBuilder.BuildQuestionUI(this);

            // Connect Check Answer to choices
            foreach (Button choice in m_uiBuilder.Choices)
            {
                choice.GetComponent<Choice>().OnChoiceSelected.AddListener(CheckAnswer);
            }
        }

        public void CheckAnswer(string choiceIndex)
        {
            if (choiceIndex.ToLower() == CorrectAnswer.ToLower())
            {
                m_uiBuilder.ShowCorrectAnswer(choiceIndex);
                m_uiBuilder.OnAnimationComplete.AddListener(() =>
                {
                    OnQuestionAnswered.Invoke(QuestionResult.Success);
                });
            }
            else
            {
                m_uiBuilder.ShowWrongAnswer(answer, choiceIndex);
                m_uiBuilder.OnAnimationComplete.AddListener(() =>
                {
                    OnQuestionAnswered.Invoke(QuestionResult.Fail);
                });
            }
        }

        public void Unload()
        {
            if (m_uiBuilder != null)
            {
                foreach(Button choice in m_uiBuilder.Choices)
                {
                    choice.GetComponent<Choice>().OnChoiceSelected.RemoveListener(CheckAnswer);
                }
            }            

            m_uiBuilder.Reset();
            m_uiBuilder = null;
        }
    }
}