using DTOMaker.Models;

namespace DTOMakerV10.Models2
{
    [Entity(1, LayoutMethod.Linear)]
    public interface IPolygon : IEntityBase { }

    [Entity(2, LayoutMethod.Linear)]
    public interface ITriangle : IPolygon { }

    [Entity(3, LayoutMethod.Linear)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; }
    }

    [Entity(4, LayoutMethod.Linear)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }

    [Entity(5, LayoutMethod.Linear)]
    public interface IQuadrilateral : IPolygon { }

    [Entity(6, LayoutMethod.Linear)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; }
    }

    [Entity(7, LayoutMethod.Linear)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }
}
