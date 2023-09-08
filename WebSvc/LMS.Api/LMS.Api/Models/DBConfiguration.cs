using System.Diagnostics.CodeAnalysis;

namespace LMS.Api.Models
{
    [ExcludeFromCodeCoverage]

    public class DBConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
