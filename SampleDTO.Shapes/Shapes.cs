﻿using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace SampleDTO.Shapes
{
    [Entity]
    [Id(3)]
    [Layout(LayoutMethod.Linear)]
    public interface IShape { }

    [Entity]
    [Id(4)]
    [Layout(LayoutMethod.Linear)]
    public interface ITriangle : IShape { }

    [Entity]
    [Id(5)]
    [Layout(LayoutMethod.Linear)]
    public interface IEquilateral : ITriangle
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [Id(6)]
    [Layout(LayoutMethod.Linear)]
    public interface IRightTriangle : ITriangle
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

    [Entity]
    [Id(7)]
    [Layout(LayoutMethod.Linear)]
    public interface IQuadrilateral : IShape { }

    [Entity]
    [Id(8)]
    [Layout(LayoutMethod.Linear)]
    public interface ISquare : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
    }

    [Entity]
    [Id(9)]
    [Layout(LayoutMethod.Linear)]
    public interface IRectangle : IQuadrilateral
    {
        [Member(1)] double Length { get; set; }
        [Member(2)] double Height { get; set; }
    }

}
