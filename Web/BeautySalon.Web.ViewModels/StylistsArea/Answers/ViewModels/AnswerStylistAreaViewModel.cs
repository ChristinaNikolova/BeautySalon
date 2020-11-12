﻿using BeautySalon.Data.Models;
using BeautySalon.Services.Mapping;
using System;

namespace BeautySalon.Web.ViewModels.StylistsArea.Answers.ViewModels
{
    public class AnswerStylistAreaViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionClientUsername { get; set; }

        public DateTime QuestionCreatedOn { get; set; }

        public string FormattedQuestionCreatedOn
            => this.QuestionCreatedOn.ToString("dd.MM.yyyy hh:mm:ss");
    }
}
