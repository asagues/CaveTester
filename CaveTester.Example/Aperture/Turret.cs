using System.ComponentModel.DataAnnotations;

namespace CaveTester.Example.Aperture
{
    public class Turret
    {
        [Required] public int Id { get; set; }
        [Required] public bool IsDefective { get; set; }
        [Required] public string Name { get; set; }
    }
}