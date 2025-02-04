using DTOMaker.Models;
using DTOMaker.Models.MemBlocks;
using DTOMaker.Models.MessagePack;
using System;

namespace SampleDTO.Basic
{
    [Entity]
    [EntityKey(1)]
    [Id("MyDTP")]
    [Layout(LayoutMethod.Linear)]
    public interface IMyDTO
    {
        [Member(1)] bool Field01 { get; set; }
        [Member(2)][Endian(false)] double Field02LE { get; set; }
        [Member(3)][Endian(true)] double Field02BE { get; set; }
        [Member(4)] Guid Field03 { get; set; }
        [Member(5)] short Field05_Length { get; set; }
        [Member(6)][Capacity(128)] ReadOnlyMemory<byte> Field05_Data { get; set; }
        string? Field05 { get; set; }
    }
}
