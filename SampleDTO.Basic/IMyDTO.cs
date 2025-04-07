using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace SampleDTO.Basic
{
    [Entity]
    [Id(1)]
    [Layout(LayoutMethod.Linear)]
    public interface IMyDTO
    {
        [Member(1)] bool Field01 { get; }
        [Member(2)][Endian(false)] double Field02LE { get; }
        [Member(3)][Endian(true)] double Field02BE { get; }
        [Member(4)] Guid Field03 { get; }
        [Member(5)] short Field05_Length { get; }
        [Member(6)][Capacity(128)] ReadOnlyMemory<byte> Field05_Data { get; }
        string? Field05 { get; set; }
    }
}
