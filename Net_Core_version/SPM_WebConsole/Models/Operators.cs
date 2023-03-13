using System.ComponentModel;

namespace SPM_WebConsole.Models
{
    public enum Operators
    {
        [Description(">")] Greather,
        [Description(">=")] Greather_Equals,
        [Description("<")] Less,
        [Description("<=")] Less_Equals,
        [Description("=")] Equals,
        [Description("<>")] Not_Equals,
        None
    }
}
