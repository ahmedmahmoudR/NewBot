﻿// <copyright file="AskAnExpertCardPayload.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.AskHR.Models
{
    /// <summary>
    /// Represents the submit data associated with the Ask An Expert card.
    /// </summary>
    public class AskAnExpertCardPayload : TeamsAdaptiveSubmitActionData
    {
        /// <summary>
        /// Gets or sets the user title text for ask an expert button.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the question for the expert being asked by the user through bot command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the add Your physical location 
        /// </summary>
        public string Physicallocation  { get; set;}

        /// <summary>
        /// Gets or sets the add loction.
        /// </summary>
        public string Location { get; set;}
        

        /// <summary>
        /// Gets or sets the question for the expert being asked by the user through Response card-
        /// Response Card: Response generated by the bot to user question by calling QnA Maker service.
        /// </summary>
        public string UserQuestion { get; set; }

        /// <summary>
        /// Gets or sets the answer for the expert- Answer sent to the SME team along with feedback
        /// provided by the user on response given by bot calling QnA Maker service.
        /// </summary>
        public string KnowledgeBaseAnswer { get; set; }
    }
}