// Project Makoto Example Plugin
// Copyright (C) 2023 Fortunevale
// This code is licensed under MIT license (see 'LICENSE'-file for details)

using ProjectMakoto.Entities.Translation;
using ProjectMakoto.Plugins.Example;
using Translations = ProjectMakoto.Plugins.Example.Entities.Translations;

namespace ProjectMakoto.Commands;

internal class ExampleCommand : BaseCommand
{
    public override Task ExecuteCommand(SharedCommandContext ctx, Dictionary<string, object> arguments)
    {
        return Task.Run(async () =>
        {
            ExamplePlugin.Plugin!.UserData![ctx.User.Id].ExampleValue1 = !ExamplePlugin.Plugin.UserData![ctx.User.Id].ExampleValue1;

            _ = await this.RespondOrEdit(this.GetString(((Translations)ExamplePlugin.Plugin!.Translations).Commands.ValueSet, 
                new TVar("Value", ExamplePlugin.Plugin.UserData![ctx.User.Id].ExampleValue1)));
        });
    }
}