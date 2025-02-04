using MemoryPack;
using System;

namespace SampleDTO.Basic.MemoryPack
{
    [MemoryPackable]
    public sealed partial class MemoryPackMyDTO : IMyDTO
    {
        public void Freeze() { }

        [MemoryPackInclude] public bool Field01 { get; set; }
        [MemoryPackInclude] public double Field02LE { get; set; }
        [MemoryPackInclude] public double Field02BE { get; set; }
        [MemoryPackInclude] public Guid Field03 { get; set; }
        [MemoryPackIgnore] public short Field05_Length { get; set; }
        [MemoryPackIgnore] public ReadOnlyMemory<byte> Field05_Data { get; set; }
        [MemoryPackInclude] public string? Field05 { get; set; }
    }

}
