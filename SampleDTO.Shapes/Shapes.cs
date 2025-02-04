using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace SampleDTO.Shapes
{
    [Entity]
    [EntityKey(3)]
    [Layout(LayoutMethod.Linear)]
    public interface IShape { }

    [Entity]
    [EntityKey(4)]
    [Layout(LayoutMethod.Linear)]
    public interface ITriangle : IShape { }

    [Entity]
    [EntityKey(5)]
    [Layout(LayoutMethod.Linear)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(6)]
    [Layout(LayoutMethod.Linear)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

    [Entity]
    [EntityKey(7)]
    [Layout(LayoutMethod.Linear)]
    public interface IQuadrilateral : IShape { }

    [Entity]
    [EntityKey(8)]
    [Layout(LayoutMethod.Linear)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(9)]
    [Layout(LayoutMethod.Linear)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

}
