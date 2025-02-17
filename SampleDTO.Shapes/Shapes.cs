using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace SampleDTO.Shapes
{
    [Entity]
    [EntityKey(3)]
    [Layout(LayoutMethod.Linear)]
    [Id("0ba78cf0-cdbe-48c8-92cc-4ce568bea544")]
    public interface IShape { }

    [Entity]
    [EntityKey(4)]
    [Layout(LayoutMethod.Linear)]
    [Id("329bc5d5-359e-41b8-b220-205006c21e56")]
    public interface ITriangle : IShape { }

    [Entity]
    [EntityKey(5)]
    [Layout(LayoutMethod.Linear)]
    [Id("075ce763-6b54-4338-82d7-e6c1780a74d2")]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(6)]
    [Layout(LayoutMethod.Linear)]
    [Id("5a99dcfe-3530-4e0f-8ee1-2037a3e1e7b6")]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

    [Entity]
    [EntityKey(7)]
    [Layout(LayoutMethod.Linear)]
    [Id("bb485d4e-7ad2-4509-917a-4730ddf0bfde")]
    public interface IQuadrilateral : IShape { }

    [Entity]
    [EntityKey(8)]
    [Layout(LayoutMethod.Linear)]
    [Id("4dbd294d-2eef-4e80-a423-6d4d8e13b66a")]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [EntityKey(9)]
    [Layout(LayoutMethod.Linear)]
    [Id("db2f772a-8cb4-4304-8ddb-0ef5202900a3")]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

}
