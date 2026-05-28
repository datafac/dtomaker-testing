
namespace System.Runtime.CompilerServices
{

    /// <summary>
    /// Adding this fixes CS0518 errors.
    /// </summary>
    internal static class IsExternalInit { }

    // CompatAttributes.cs
    // Add only when your target/framework doesn't already provide these attributes.

    /// <summary>
    /// Used by the compiler for C# 'required' support.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class RequiredMemberAttribute : Attribute
    {
        public RequiredMemberAttribute() { }
    }

    /// <summary>
    /// Used by the compiler to annotate required language features.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        public string Feature { get; }
        public CompilerFeatureRequiredAttribute(string feature) => Feature = feature;
        // compiler may emit either overload; provide both
        public CompilerFeatureRequiredAttribute(string feature, bool isOptional) => Feature = feature;
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Used by the compiler for 'SetsRequiredMembers' annotation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public sealed class SetsRequiredMembersAttribute : Attribute
    {
        public SetsRequiredMembersAttribute() { }
    }
}