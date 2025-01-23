using DTOMaker.Models.MessagePack;
using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;

namespace DTOMakerV10.Models2
{
    [Entity]
    [EntityKey(1)]
    [Layout(LayoutMethod.Linear)] 
    public interface IPolygon { }

    [Entity]
    [EntityKey(2)]
    [Layout(LayoutMethod.Linear)] 
    public interface ITriangle : IPolygon { }

    [Entity]
    [EntityKey(3)]
    [Layout(LayoutMethod.Linear)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(4)]
    [Layout(LayoutMethod.Linear)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

    [Entity]
    [EntityKey(5)]
    [Layout(LayoutMethod.Linear)] 
    public interface IQuadrilateral : IPolygon { }

    [Entity]
    [EntityKey(6)]
    [Layout(LayoutMethod.Linear)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(7)]
    [Layout(LayoutMethod.Linear)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }
}
