using DTOMaker.Models;

namespace DTOMakerV10.Models2
{
    [Entity(1)]
    public interface IPolygon : IEntityBase { }

    [Entity(2)]
    public interface ITriangle : IPolygon { }

    [Entity(3)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; }
    }

    [Entity(4)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }

    [Entity(5)]
    public interface IQuadrilateral : IPolygon { }

    [Entity(6)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; }
    }

    [Entity(7)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; }
        [Member(2)] double Height { get; }
    }
}
