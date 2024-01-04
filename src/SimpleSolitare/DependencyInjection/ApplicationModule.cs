﻿using Microsoft.Extensions.DependencyInjection;
using SimpleSolitare.CommandLine;
using SimpleSolitare.Wraps;

namespace SimpleSolitare.DependencyInjection
{
    public class ApplicationModule : Module
    {
        protected override void Load(IServiceCollection services, object[]? parameters = null)
        {
            // Wrap factories
            services.AddTransient<IStreamWriterWrapFactory, StreamWriterWrapFactory>();

            // Application
            services.AddTransient<ICommandLineProcessor, CommandLineProcessor>();
            services.AddTransient<IDeckProvider, DeckProvider>();
            services.AddTransient<IRngFactory, RngFactory>();
            services.AddTransient<IDeckShuffler, DeckShuffler>();
            services.AddTransient<IPlayer, Player>();
            services.AddTransient<IGameRunner, GameRunner>();
            services.AddTransient<IGameResultWriter, GameResultWriter>();
        }
    }
}
