// Project Makoto Example Plugin
// Copyright (C) 2023 Fortunevale
// This code is licensed under MIT license (see 'LICENSE'-file for details)

namespace ProjectMakoto.Plugins.Example;

public class ExamplePlugin : BasePlugin
{
    // Set the plugin's information here.

    // The plugin's name.
    public override string Name => "Example Plugin";

    // The plugin's description.
    public override string Description => "This is an example plugin.";

    // The current version of this plugin. Will be used for update checking.
    public override SemVer Version => new(1, 0, 0);

    // The Api Versions this Plugin supports.
    public override int[] SupportedPluginApis => [1];

    // Your name.
    public override string Author => "Mira";

    // Your Discord Id, if you want to provide it.
    public override ulong? AuthorId => 411950662662881290;

    // The repository url of your plugin. Used for automatic update checking.
    public override string UpdateUrl => "https://github.com/Fortunevale/ProjectMakoto.Plugins.Example";

    // Here you can provide github credentials for update checking, if your repository is private.
    public override Octokit.Credentials? UpdateUrlCredentials => base.UpdateUrlCredentials;


    // A reference to your own plugin accessible from anywhere inside of it.
    public static ExamplePlugin? Plugin { get; set; }

    // When trying to get an entry which does not already exist, SelfFillingDatabaseDictionary will create one.
    public SelfFillingDatabaseDictionary<ExampleTable>? UserData { get; set; }

    // You can directly interface with the already existing config.json of Project Makoto. This property allows you to.
    private PluginConfig LoadedConfig
    {
        get
        {
            if (!this.CheckIfConfigExists())
            {
                this._logger.LogDebug("Creating Plugin Config..");
                this.WriteConfig(new PluginConfig()); // You can use any class you choose, it gets saved with Newtonsoft.Json.
            }

            var v = this.GetConfig(); 

            if (v.GetType() == typeof(JObject))
            {
                this.WriteConfig(((JObject)v).ToObject<PluginConfig>() ?? new PluginConfig()); // Automatically converts the config you get to your PluginConfig.
                v = this.GetConfig();
            }

            return (PluginConfig)v;
        }
    }

    // Here you can do stuff when initializing your plugin. Happens on startup, before loading data.
    public override ExamplePlugin Initialize()
    {
        ExamplePlugin.Plugin = this; // Allows you to access your Main Class from anywhere within your project.

        this.Connected += (s, e) =>
        {
            this._logger.LogDebug("Hello from the Example Plugin!");

            // There's several different events that fire. Connected being one of them. It fires upon successful log in to discord.
            // The sender (s) is usually the 'Bot' Instance this plugin is loaded under, in this context also accessible via 'this.Bot'.

            // You don't need to register all events while initializing, you can register and unregister them at any time.
        };

        this.DatabaseInitialized += (s, e) => // The earliest a SelfFillingDatabaseDictionary can be initiated is after the Database has been initialized.
        {                                     // This event fires as soon as it is possible to initiate any database related list.
            this._logger.LogDebug("Creating SelfFillingDatabaseDictionaries for {PluginName}!", this.Name);

            this.UserData = new SelfFillingDatabaseDictionary<ExampleTable>(this, typeof(ExampleTable), (id) =>
            {
                return new ExampleTable(this, id);
            }); // The predicate is required for the self-filling capability.
        };      // There's also a DatabaseDictionary which does not have that.

        return this;
    }

    // This allows you to register commands, they'll be registered as prefix and application command. Happens right before the login to discord.
    public override async Task<IEnumerable<MakotoModule>> RegisterCommands()
    {
        await Task.Delay(0); // You can do things when registering commands.

        return
        [
            new("Example", // The name of this module. This will define in what group this command shows up in the help command.
            [
                new("single-example", "Example command from an example plugin for Project Makoto.", typeof(ExampleCommand),
                    new MakotoCommandOverload(typeof(string), "example_arg1", "Example Argument 1", true, false),
                    new MakotoCommandOverload(typeof(string), "example_arg2", "Example Argument 2", false, true)),

                new("group-example", "Example command from an example plugin for Project Makoto.",
                    new MakotoCommand("subexample1", "Example command from an example plugin for Project Makoto.", typeof(ExampleCommand)),
                    new MakotoCommand("subexample2", "Example command from an example plugin for Project Makoto.", typeof(ExampleCommand)))
            ])
        ];
    }

    // Allows you to define the tables you need to store user/guild/misc data. Any data that does not belong in the config.
    public override Task<IEnumerable<Type>?> RegisterTables()
    {
        return Task.FromResult<IEnumerable<Type>?>(
        [
            typeof(ExampleTable),
        ]);
    }

    // Here you can enable translations for your provided commands.
    public override (string? path, Type? type) LoadTranslations()
    {
        return ("Translations/strings.json", typeof(Entities.Translations)); // Make sure to include the strings.json in your build directory via the csproj.
    }                                                                        // The type has to inherit ITranslation and the CommandList must include *ALL* commands that
                                                                             // you registered in the RegisterCommands() Method, otherwise DisCatSharp will fail generating the
                                                                             // command payload.

    // Here you can do stuff before shutting down. Happens at shutdown of Project Makoto.
    public override Task Shutdown()
        => base.Shutdown();
}
