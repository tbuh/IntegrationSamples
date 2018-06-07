using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Infrastructure
{
    public class QuestionService
    {
        private Dictionary<string, string> _questions = new Dictionary<string, string>();

        public QuestionService()
        {
            _questions = new Dictionary<string, string>
            {
                { "q1", "answer1" },
                { "q2", "answer2" },
                { "q3", "answer3" },
                { "q4", "answer4" },
            };
        }

        public string GetAnswer(string question)
        {
            if (_questions.ContainsKey(question)) return _questions[question];
            return "Sorry, we will give you answer later today!";
        }
    }
}