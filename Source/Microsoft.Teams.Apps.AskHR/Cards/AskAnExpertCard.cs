// <copyright file="AskAnExpertCard.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserve.
// </copyright>

namespace Microsoft.Teams.Apps.AskHR.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using Microsoft.Teams.Apps.AskHR.Models;
    using Microsoft.Teams.Apps.AskHR.Properties;

    /// <summary>
    ///  This class process Ask an expert function : A feature available in bot menu commands in 1:1 scope.
    /// </summary>
    public static class AskAnExpertCard
    {
        /// <summary>
        /// Text associated with ask an expert
        /// </summary>
        public const string AskAnExpertSubmitText = "QuestionForExpert";

        /// <summary>
        /// This method will construct the card for ask an expert, when invoked from the bot menu.
        /// </summary>
        /// <returns>Ask an expert card.</returns>
        public static Attachment GetCard()
        {
            return GetCard(false, new AskAnExpertCardPayload());
        }

        /// <summary>
        /// This method will construct the card for ask an expert, when invoked from the response card.
        /// </summary>
        /// <param name="payload">Payload from the response card.</param>
        /// <returns>Ask an expert card.</returns>
        public static Attachment GetCard(ResponseCardPayload payload)
        {
            var data = new AskAnExpertCardPayload
            {
                Description = payload.UserQuestion,     // Pre-populate the description with the user's question
                UserQuestion = payload.UserQuestion,
                KnowledgeBaseAnswer = payload.KnowledgeBaseAnswer,
            };
            return GetCard(false, data);
        }

        /// <summary>
        /// This method will construct the card for ask an expert, when invoked from the ask an expert card submit.
        /// </summary>
        /// <param name="payload">Payload from the ask an expert card.</param>
        /// <returns>Ask an expert card.</returns>
        public static Attachment GetCard(AskAnExpertCardPayload payload)
        {
            return GetCard(true, payload);
        }

        /// <summary>
        /// This method will construct the card for ask an expert bot menu.
        /// </summary>
        /// <param name="showValidationErrors">Determines whether we show validation errors.</param>
        /// <param name="data">Data from the ask an expert card.</param>
        /// <returns>Ask an expert card.</returns>
        private static Attachment GetCard(bool showValidationErrors, AskAnExpertCardPayload data)
        {
            AdaptiveCard askAnExpertCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Weight = AdaptiveTextWeight.Bolder,
                        Text = Resource.AskAnExpertText1,
                        Size = AdaptiveTextSize.Large,
                        Wrap = true
                    },

                    new AdaptiveTextBlock
                    {
                        Text = Resource.AskAnExpertSubheaderText,
                        Wrap = true
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = Resource.TitleRequiredText,
                                        Wrap = true
                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = (showValidationErrors && string.IsNullOrWhiteSpace(data.Title)) ? Resource.MandatoryTitleFieldText : string.Empty,
                                        Color = AdaptiveTextColor.Attention,
                                        HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                        Wrap = true
                                    }
                                }
                            }
                        },
                    },
                    new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Title),
                        Placeholder = Resource.ShowCardTitleText,
                        IsMultiline = false,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Title,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = "Issue description",
                        Wrap = true
                    },
                    new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Description),
                        Placeholder = Resource.AskAnExpertPlaceholderText,
                        IsMultiline = true,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Description,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = "IP address/computer account ",
                        Wrap = true
                    },
                    new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Location),
                        Placeholder = "Please Enter your IP Address ",
                        IsMultiline = false,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Location,
                    },


                     new AdaptiveTextBlock
                    {
                        Text = "Location",
                        Wrap = true
                    },


                   new AdaptiveChoiceSetInput
                    {
                        Type = AdaptiveChoiceSetInput.TypeName,
                        Id = "Choices",
                        IsMultiSelect = false,
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Value = "Choose a plan",

                        Choices = new List<AdaptiveChoice>
                        {
                            new AdaptiveChoice() { Title = "العباسية ", Value = "العباسية" },
                            new AdaptiveChoice() { Title = "اسيوط & منيا ", Value = "اسيوط & منيا" },
                            new AdaptiveChoice() { Title = "بني سويف ", Value = "بني سويف" },
                            new AdaptiveChoice() { Title = "بنها", Value = "بنها" },
                            new AdaptiveChoice() { Title = "كول سنتر ", Value = "كول سنتر " },
                            new AdaptiveChoice() { Title = "هليوپوليس ", Value = "هليوپوليس " },
                            new AdaptiveChoice() { Title = "Showroom ", Value = "Showroom " },
                            new AdaptiveChoice() { Title = "Small Branches & Service Center ", Value = "Small Branches & Service Center " },
                            new AdaptiveChoice() { Title = "بورسعيد -الاسكندرية-طنطا - المنصوره ", Value = "بورسعيد -الاسكندرية-طنطا - المنصوره" },
                            new AdaptiveChoice() { Title = "قليوبية", Value = "قليوبية" },
                            new AdaptiveChoice() { Title = "قويسنا ", Value = "قويسنا" },
                        },
                    }, 
                     new AdaptiveTextBlock
                    {
                        Text = "Mail ",
                        Wrap = true
                    },
                     new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Mail),
                        Placeholder = "Please Enter your Mail ",
                        IsMultiline = false,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Mail,
                    },
                      new AdaptiveTextBlock
                    {
                        Text = "Phone Number ",
                        Wrap = true
                    },

                    new AdaptiveTextInput
                    {
                    Id = nameof(AskAnExpertCardPayload.PhoneNumber),
                        Placeholder = "Please Enter your Phone Number ",
                        IsMultiline = false,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.PhoneNumber,
                    },
                },
               
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveSubmitAction
                    {
                        Title = Resource.AskAnExpertButtonText,
                        Data = new AskAnExpertCardPayload
                        {
                            MsTeams = new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                DisplayText = Resource.AskAnExpertDisplayText,
                                Text = AskAnExpertSubmitText,
                            },
                            UserQuestion = data.UserQuestion,
                            KnowledgeBaseAnswer = data.KnowledgeBaseAnswer,
                        },
                    }
                }
            };

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = askAnExpertCard,
            };
        }
    }
}