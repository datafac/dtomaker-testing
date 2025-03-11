using DTOMaker.Models.MessagePack;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace DTOMakerV10.Models2
{
    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IPolygon { }

    [Entity]
    [Id(2)]
    [Layout(LayoutMethod.Linear)]
    public interface ITriangle : IPolygon { }

    [Entity]
    [Id(3)]
    [Layout(LayoutMethod.Linear)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; }
    }

    [Entity]
    [Id(4)]
    [Layout(LayoutMethod.Linear)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }

    [Entity]
    [Id(5)]
    [Layout(LayoutMethod.Linear)]
    public interface IQuadrilateral : IPolygon { }

    [Entity]
    [Id(6)]
    [Layout(LayoutMethod.Linear)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; }
    }

    [Entity]
    [Id(7)]
    [Layout(LayoutMethod.Linear)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }
}
