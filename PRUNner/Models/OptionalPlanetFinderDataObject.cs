using ReactiveUI;

namespace PRUNner.Models
{
    public class OptionalPlanetFinderDataObject : ReactiveObject
    {
        public SystemTextBox ExtraSystem { get; } = new();
        
    }
}