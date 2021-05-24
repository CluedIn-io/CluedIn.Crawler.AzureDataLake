using System;
using System.Collections.Generic;
using System.Text;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.AzureDataLake.Vocabularies
{
    public class ActorVocabulary : SimpleVocabulary
    {
        public ActorVocabulary()
        {
            VocabularyName = "Azure Data Lake Actors"; // TODO: Set value
            KeyPrefix = "azureDataLake.Actor"; // TODO: Set value
            KeySeparator = ".";
            Grouping = "/Actor"; // TODO: Check value

            AddGroup("Incident Details", group =>
            {
                FirstName = group.Add(new VocabularyKey("FirstName", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Name"));
                LastName = group.Add(new VocabularyKey("LastName", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Name"));
                Age = group.Add(new VocabularyKey("Age", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Name"));
                DateOfBirth = group.Add(new VocabularyKey("DateOfBirth", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Name"));
                Gender = group.Add(new VocabularyKey("Gender", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Gender"));
                FavouriteTShirt = group.Add(new VocabularyKey("FavouriteTShirt", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Name"));
                Email = group.Add(new VocabularyKey("Email", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible).WithDisplayName("Email"));
            });
        }

        public VocabularyKey FirstName { get; internal set; }
        public VocabularyKey LastName { get; internal set; }

        public VocabularyKey Email { get; internal set; }

        public VocabularyKey Age { get; internal set; }
        public VocabularyKey DateOfBirth { get; internal set; }

        public VocabularyKey Gender { get; internal set; }

        public VocabularyKey FavouriteTShirt { get; internal set; }

    }
}
