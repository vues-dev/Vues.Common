using System;
namespace Vues.Common.Models
{
    public class ValidationError
    {
        public required IDictionary<string, string[]> Errors { get; init; }
    }
}

