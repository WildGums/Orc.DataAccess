namespace Orc.DataAccess.Tests;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Controls;
using NUnit.Framework;
using PublicApiGenerator;
using VerifyNUnit;

[TestFixture]
public class PublicApiFacts
{
    [Test, MethodImpl(MethodImplOptions.NoInlining)]
    public async Task Orc_DataAccess_HasNoBreakingChanges_Async()
    {
        var assembly = typeof(ReaderBase).Assembly;

        await PublicApiApprover.ApprovePublicApiAsync(assembly);
    }

    [Test, MethodImpl(MethodImplOptions.NoInlining)]
    public async Task Orc_DataAccess_Xaml_HasNoBreakingChanges_Async()
    {
        var assembly = typeof(ConnectionStringBuilder).Assembly;

        await PublicApiApprover.ApprovePublicApiAsync(assembly);
    }

    internal static class PublicApiApprover
    {
        public static async Task ApprovePublicApiAsync(Assembly assembly)
        {
            var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
            await Verifier.Verify(publicApi);
        }
    }
}
