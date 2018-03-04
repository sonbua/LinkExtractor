﻿using System.Threading.Tasks;
using R2;

namespace LinkExtractor.Instagram
{
    public class FakeCommandHandler : CommandHandler<FakeCommand>
    {
        protected override Task HandleCommandAsync(FakeCommand command)
        {
            // do something fake here

            return Task.CompletedTask;
        }
    }
}