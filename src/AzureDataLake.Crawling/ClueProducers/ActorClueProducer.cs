using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CluedIn.Core.Data;
using CluedIn.Crawling.AzureDataLake.Core.Models;
using CluedIn.Crawling.AzureDataLake.Vocabularies;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.Helpers;
using Microsoft.Extensions.Logging;

namespace CluedIn.Crawling.AzureDataLake.ClueProducers
{
    public class ActorClueProducer : BaseClueProducer<Actor>
    {
        private readonly IClueFactory _factory;
        private readonly ILogger<ActorClueProducer> _log;

        public ActorClueProducer([NotNull] IClueFactory factory, ILogger<ActorClueProducer> _log)

        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this._log = _log;
        }

        protected override Clue MakeClueImpl(Actor input, Guid id)
        {
            var clue = _factory.Create("/Actor", input.email, id);
            var data = clue.Data.EntityData;

            var vocab = new ActorVocabulary();

            data.Codes.Add(new EntityCode("/Actor", "Global", input.email));

            if (!string.IsNullOrEmpty(input.first_name) && !string.IsNullOrEmpty(input.last_name))
            {
                data.Name = $"{input.first_name} {input.last_name}";
            }

            data.Properties[vocab.FirstName] = input.first_name.PrintIfAvailable();
            data.Properties[vocab.LastName] = input.last_name.PrintIfAvailable();
            data.Properties[vocab.Gender] = input.gender.PrintIfAvailable();
            data.Properties[vocab.Email] = input.email.PrintIfAvailable();
            data.Properties[vocab.DateOfBirth] = input.Birthday.PrintIfAvailable();

            if (!data.OutgoingEdges.Any())
            {
                _factory.CreateEntityRootReference(clue, EntityEdgeType.PartOf);
            }


            return clue;
        }
    }
}
