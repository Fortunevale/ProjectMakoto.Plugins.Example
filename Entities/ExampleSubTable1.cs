using ProjectMakoto.Database;
using ProjectMakoto.Enums;

namespace ProjectMakoto.Plugins.Example.Entities;
public class ExampleSubTable1(Bot bot, ExampleTable parent) : RequiresParent<ExampleTable>(bot, parent) // You can easily add a Parent to a class by using
{                                                                                                       // RequiresParent, so theres no need for extensive
                                                                                                        // constructors.

    [ColumnName("example_value3"), ColumnType(ColumnTypes.TinyInt), Default("1")] // In the sub-table, you simply do the same as in the parent.
    public bool ExampleValue3
    {
        get => this.Parent.GetValue<bool>(this.Parent.Id, "example_value3");  // The only difference being that you need to access the Parent
        set => this.Parent.SetValue(this.Parent.Id, "example_value3", value); // for the Database Tools.
    }
}
