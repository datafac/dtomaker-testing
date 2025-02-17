using DTOMaker.Models.MessagePack;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace DTOMakerV10.Models2
{
    [Entity]
    [EntityKey(1)]
    [Layout(LayoutMethod.Linear)]
    [Id("62bd8eb5-df6b-4134-bb66-53f73ece4773")]
    public interface IPolygon { }

    [Entity]
    [EntityKey(2)]
    [Layout(LayoutMethod.Linear)]
    [Id("973e7c6f-b1ee-4367-a2f4-9f71eb3484d0")]
    public interface ITriangle : IPolygon { }

    [Entity]
    [EntityKey(3)]
    [Layout(LayoutMethod.Linear)]
    [Id("55f76099-ea92-4efc-b509-fab17e30cdf7")]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(4)]
    [Layout(LayoutMethod.Linear)]
    [Id("00982c25-df99-464b-b5c4-41e43a3833be")]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

    [Entity]
    [EntityKey(5)]
    [Layout(LayoutMethod.Linear)]
    [Id("dbaf15c9-fd08-4c0a-ad2a-738fafbf3288")]
    public interface IQuadrilateral : IPolygon { }

    [Entity]
    [EntityKey(6)]
    [Layout(LayoutMethod.Linear)]
    [Id("3c5c6e3a-3e1e-4db5-8994-90ffb50083b5")]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(7)]
    [Layout(LayoutMethod.Linear)]
    [Id("d4c3bb72-664b-4a4e-98fe-53c72d3c0995")]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }
}
