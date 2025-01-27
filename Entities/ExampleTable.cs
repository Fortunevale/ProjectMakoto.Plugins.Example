// Project Makoto Example Plugin
// Copyright (C) 2023 Fortunevale
// This code is licensed under MIT license (see 'LICENSE'-file for details)

using Newtonsoft.Json;
using ProjectMakoto.Database;
using ProjectMakoto.Enums;

namespace ProjectMakoto.Plugins.Example.Entities;


[TableName("exampletable")]
public class ExampleTable : PluginDatabaseTable
{
    public ExampleTable(BasePlugin plugin, ulong identifierValue) : base(plugin, identifierValue)
    {
        this.Id = identifierValue; // Don't forget to initialize the identifier so the getters and setters can do their work.

        this.SubTable1 = new(this.Plugin.Bot, this);
    }

    [ColumnName("userid"), ColumnType(ColumnTypes.BigInt), Primary] // The identifier value is stored twice, once as key for indexing (access via List[id])
    internal ulong Id { get; init; }                                // and inside the value (this class) for easy actually communicating with the database

    [ColumnName("example_value1"), ColumnType(ColumnTypes.TinyInt), Default("1")]
    public bool ExampleValue1
    {
        get => this.GetValue<bool>(this.Id, "example_value1");  // This directly gets the value from the database. It can be frequently accessed, theres no need to cache the value.
                                                                // Makoto takes care of caching for you. Values are stored for a few seconds.

        set => this.SetValue(this.Id, "example_value1", value); // Setting the value removes the cached item. And refetches on next get.
    }

    [ColumnName("example_value2"), ColumnType(ColumnTypes.LongText), Default("[]")] // Make sure to minify the json if you want to store large amounts of data.
    public string[] ExampleValue2                                                   // LongText can store up to 4GiB of text.
    {                                                                               // Another important note: It's recommended to use arrays instead of
                                                                                    // lists as they do not fire the setters when adding/removing items.
        get => JsonConvert.DeserializeObject<string[]>(this.GetValue<string>(this.Id, "example_value2")) ?? []; // Storing more sophisticated data is usually done by converting it to json
                                                                                                                // and storing the resulting string in the database.

        set => this.SetValue(this.Id, "example_value2", JsonConvert.SerializeObject(value));
    }

    [ContainsValues]                                    // This attribute signals to Makoto to look for more columns inside this property.
    public ExampleSubTable1 SubTable1 { get; init; }    // This allows you to sort your table a little more so you don't get bombarded with
}                                                       // tons of properties everytime you want to access the table in your code.
