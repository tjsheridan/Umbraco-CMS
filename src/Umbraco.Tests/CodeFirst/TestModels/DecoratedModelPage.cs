﻿using System;
using Umbraco.Tests.CodeFirst.Attributes;
using umbraco.editorControls.textfield;

namespace Umbraco.Tests.CodeFirst.TestModels
{
    [ContentType("modelPage", 
        AllowedChildContentTypes = new[] { typeof(ContentPage) }, 
        AllowedTemplates = new[]{"umbMaster"})]
    public class DecoratedModelPage
    {
        [PropertyType(typeof(TextFieldDataType), PropertyGroup = "Content")]
        [SortOrder(0)]
        public string Author { get; set; }

        [PropertyType(typeof(TextFieldDataType), PropertyGroup = "Content")]
        [SortOrder(1)]
        public string Title { get; set; }

        [Richtext(PropertyGroup = "Content")]
        [SortOrder(2)]
        [Description("Richtext field to enter the main content of the page")]
        public string BodyContent { get; set; }

        [SortOrder(3)]
        [Alias("publishedDate", Name = "Publish Date")]
        public DateTime PublishDate { get; set; } 
    }
}