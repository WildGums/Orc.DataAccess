namespace Orc.DataAccess.Tests
{
    using System.Runtime.CompilerServices;
    using System.Xaml;
    using ApiApprover;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_DataAccess_HasNoBreakingChanges()
        {
            var assembly = typeof(ReaderBase).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_DataAccess_Xaml_HasNoBreakingChanges()
        {
            var assembly = typeof(XamlType).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}
