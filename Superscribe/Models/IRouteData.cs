namespace Superscribe.Models
{
    public interface IRouteData
    {
        dynamic Parameters { get; set; }

        object Response { get; set; }
    }
}