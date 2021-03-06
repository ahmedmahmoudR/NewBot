// <copyright file="SmeTicketCard.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.AskHR.Cards
{
    using System;
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using Microsoft.Teams.Apps.AskHR.Common.Models;
    using Microsoft.Teams.Apps.AskHR.Models;
    using Microsoft.Teams.Apps.AskHR.Properties;

    /// <summary>
    /// Represents an SME ticket used for both in place card update activity within SME channel
    /// when changing the ticket status and notification card when bot posts user question to SME channel.
    /// </summary>
    public class SmeTicketCard
    {
        private readonly TicketEntity ticket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmeTicketCard"/> class.
        /// </summary>
        /// <param name="ticket">The ticket model with the latest details.</param>
        public SmeTicketCard(TicketEntity ticket)
        {
            this.ticket = ticket;
        }

        /// <summary>
        /// Gets the ticket that is the basis for the information in this card
        /// </summary>
        protected TicketEntity Ticket => this.ticket;

        /// <summary>
        /// Returns an attachment based on the state and information of the ticket.
        /// </summary>
        /// <param name="localTimestamp">Local timestamp of the user activity.</param>
        /// <returns>Returns the attachment that will be sent in a message</returns>
        public Attachment ToAttachment(DateTimeOffset? localTimestamp)
        {
            var card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = this.Ticket.Title,
                        Size = AdaptiveTextSize.Large,
                        Weight = AdaptiveTextWeight.Bolder,
                        Wrap = true,
                    },
                    
                 
                    new AdaptiveTextBlock
                    {
                        Text = string.Format(Resource.QuestionForExpertSubHeaderText, this.Ticket.RequesterUserPrincipalName),
                        Wrap = true,
                    },
                    new AdaptiveFactSet
                    {
                        Facts = this.BuildFactSet(localTimestamp),
                    },
                },
                Actions = this.BuildActions(),
            };

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
        }

        /// <summary>
        /// Return the appropriate set of card actions based on the state and information in the ticket.
        /// </summary>
        /// <returns>Adaptive card actions.</returns>
        protected virtual List<AdaptiveAction> BuildActions()
        {
            List<AdaptiveAction> actionsList = new List<AdaptiveAction>();

            actionsList.Add(this.CreateChatWithUserAction());

          /// Change Status

            //actionsList.Add(new AdaptiveShowCardAction
            //{
            //    Title = Resource.ChangeStatusButtonText,
            //    Card = new AdaptiveCard("1.0")
            //    {
            //        Body = new List<AdaptiveElement>
            //        {
            //            this.GetAdaptiveChoiceSetInput(),
            //        },
            //        Actions = new List<AdaptiveAction>
            //        {
            //            new AdaptiveSubmitAction
            //            {
            //                Data = new ChangeTicketStatusPayload { TicketId = this.Ticket.TicketId }
            //            }
            //        },
            //    }
            //});



            if (!string.IsNullOrEmpty(this.Ticket.KnowledgeBaseAnswer))
            {
                actionsList.Add(new AdaptiveShowCardAction
                {
                    Title = Resource.ViewArticleButtonText,
                    Card = new AdaptiveCard("1.0")
                    {
                        Body = new List<AdaptiveElement>
                        {
                            new AdaptiveTextBlock
                            {
                                Text = CardHelper.TruncateStringIfLonger(this.Ticket.KnowledgeBaseAnswer, CardHelper.KnowledgeBaseAnswerMaxDisplayLength),
                                Wrap = true,
                            }
                        },
                    },
                });
            }

            return actionsList;
        }

        /// <summary>
        /// Create an adaptive card action that starts a chat with the user.
        /// </summary>
        /// <returns>Adaptive card action for starting chat with user</returns>
        protected AdaptiveAction CreateChatWithUserAction()
        {
            var messageToSend = string.Format(Resource.SMEUserChatMessage, this.Ticket.Title);
            var encodedMessage = Uri.EscapeDataString(messageToSend);

            return new AdaptiveOpenUrlAction
            {
                Title = string.Format(Resource.ChatTextButton, this.Ticket.RequesterName),
                Url = new Uri($"https://teams.microsoft.com/l/chat/0/0?users={Uri.EscapeDataString(this.Ticket.RequesterUserPrincipalName)}&message={encodedMessage}")
            };
        }


        /// <summary>
        /// Return the appropriate fact set based on the state and information in the ticket.
        /// </summary>
        /// <param name="localTimestamp">The current timestamp.</param>
        /// <returns>The fact set showing the necessary details.</returns>
        private List<AdaptiveFact> BuildFactSet(DateTimeOffset? localTimestamp)
        {
            List<AdaptiveFact> factList = new List<AdaptiveFact>();

            if (!string.IsNullOrEmpty(this.Ticket.Description))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = Resource.DescriptionFact,
                    Value = this.Ticket.Description,
                });
            }


             if (!string.IsNullOrEmpty(this.Ticket.Location))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = "IP address/computer account  :",
                    Value = this.Ticket.Location,
                });
            }

            if (!string.IsNullOrEmpty(this.Ticket.PhoneNumber))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = "Phone Number  :",
                    Value = this.Ticket.PhoneNumber,
                });
            }


            if (!string.IsNullOrEmpty(this.Ticket.Mail))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = "Mail  :",
                    Value = this.Ticket.Mail,
                });
            }

            if (!string.IsNullOrEmpty(this.Ticket.Choices))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = "Location: ",
                    Value = this.Ticket.Choices,
                });
            }


            if (!string.IsNullOrEmpty(this.Ticket.UserQuestion))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = Resource.QuestionAskedFactTitle,
                    Value = this.Ticket.UserQuestion
                });
            }

            //factList.Add(new AdaptiveFact
            //{
            //    Title = Resource.StatusFactTitle,
            //    Value = CardHelper.GetTicketDisplayStatusForSme(this.Ticket),4
            //});

            //if (this.Ticket.Status == (int)TicketState.Closed)
            //{
            //    factList.Add(new AdaptiveFact
            //    {
            //        Title = Resource.ClosedFactTitle,
            //        Value = CardHelper.GetFormattedDateInUserTimeZone(this.Ticket.DateClosed.Value, localTimestamp),
            //    });
            //}

            return factList;
        }

        /// <summary>
        /// Return the appropriate status choices based on the state and information in the ticket.
        /// </summary>
        /// <returns>An adaptive element which contains the dropdown choices.</returns>
        private AdaptiveChoiceSetInput GetAdaptiveChoiceSetInput()
        {
            AdaptiveChoiceSetInput choiceSet = new AdaptiveChoiceSetInput
            {
                Id = nameof(ChangeTicketStatusPayload.Action),
                IsMultiSelect = false,
                Style = AdaptiveChoiceInputStyle.Compact
            };

            if (this.Ticket.Status == (int)TicketState.Open)
            {
                if (!this.Ticket.IsAssigned())
                {
                    choiceSet.Value = ChangeTicketStatusPayload.AssignToSelfAction;
                    choiceSet.Choices = new List<AdaptiveChoice>
                    {
                        new AdaptiveChoice
                        {
                            Title = Resource.AssignToMeActionChoiceTitle,
                            Value = ChangeTicketStatusPayload.AssignToSelfAction,
                        },
                        new AdaptiveChoice
                        {
                            Title = Resource.CloseActionChoiceTitle,
                            Value = ChangeTicketStatusPayload.CloseAction,
                        },
                    };
                }
                else
                {
                    choiceSet.Value = ChangeTicketStatusPayload.CloseAction;
                    choiceSet.Choices = new List<AdaptiveChoice>
                    {
                        new AdaptiveChoice
                        {
                            Title = Resource.UnassignActionChoiceTitle,
                            Value = ChangeTicketStatusPayload.ReopenAction,
                        },
                        new AdaptiveChoice
                        {
                            Title = Resource.AssignToMeActionChoiceTitle,
                            Value = ChangeTicketStatusPayload.AssignToSelfAction,
                        },
                        new AdaptiveChoice
                        {
                            Title = Resource.CloseActionChoiceTitle,
                            Value = ChangeTicketStatusPayload.CloseAction,
                        },
                    };
                }
            }
            else if (this.Ticket.Status == (int)TicketState.Closed)
            {
                choiceSet.Value = ChangeTicketStatusPayload.ReopenAction;
                choiceSet.Choices = new List<AdaptiveChoice>
                {
                    new AdaptiveChoice
                    {
                        Title = Resource.ReopenActionChoiceTitle,
                        Value = ChangeTicketStatusPayload.ReopenAction,
                    },
                    new AdaptiveChoice
                    {
                        Title = Resource.ReopenAssignToMeActionChoiceTitle,
                        Value = ChangeTicketStatusPayload.AssignToSelfAction,
                    },
                };
            }

            return choiceSet;
        }
    }
}