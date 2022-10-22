﻿namespace Orc.DataAccess.Tests.ViewModels
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Services;
    using Moq;
    using NUnit.Framework;
    using Orc.DataAccess.Controls;
    using Orc.DataAccess.Database;

    [TestFixture]
    public class ConnectionStringEditViewModelTextFixture
    {
        public class TheConnectionStringEditViewModelProperties
        {
            [TestCase("Server=mycomputer.test.xxx.com,8888;Database=DataAcess;User Id=myUsername;Password=myPassword;", null, "mycomputer.test.xxx.com,8888",
                "DataAcess", false, "myPassword", "myUsername")]
            public async Task ArePropertiesReturnsValuesFromConnectionStringAsync(string connectionString, string expectedPort, string expectedDataSource,
                string expectedInitialCatalog, bool expectedIntegratedSecurity, string expectedPassword, string expectedUserId)
            {
                Assert.IsNotNull(connectionString);
                Assert.IsNotEmpty(connectionString);

                DbProviderFactories.RegisterFactory("System.Data.SqlClient", typeof(System.Data.SqlClient.SqlClientFactory));

                var sqLiteProviderInfo = new DbProviderInfo(
                    "Sql Client Data Provider",
                    "System.Data.SqlClient",
                    ".NET Framework Data Provider for Sql Client",
                    "System.Data.SqlClient.SqlClientFactory, System.Data.SqlClient, Version=4.6.1.3, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");       

                var messageService = new Mock<IMessageService>().Object;
                var uiVisualizerService = new Mock<IUIVisualizerService>().Object;
                var typeFactory = TypeFactory.Default;
                var dispatcherServiceMock = new Mock<IDispatcherService>();
                dispatcherServiceMock.Setup(x => x.Invoke(It.IsAny<Action>(), It.IsAny<bool>()))
                    .Callback((Action action, bool onlyInvokeWhenNoAccess) => action.Invoke());
                var dispatcherService = dispatcherServiceMock.Object;


                var vm = new ConnectionStringEditViewModel(connectionString, sqLiteProviderInfo, messageService, uiVisualizerService, typeFactory, dispatcherService);
                await vm.InitializeViewModelAsync();

                // Wait for timer
                await Task.Delay(1000);

                // Attention null can come here if port parsed as part of data source
                Assert.AreEqual(expectedPort, vm.Port);
                Assert.AreEqual(expectedDataSource, vm.DataSource.Value);
                Assert.AreEqual(expectedInitialCatalog, vm.InitialCatalog.Value);
                Assert.AreEqual(expectedIntegratedSecurity, vm.IntegratedSecurity.Value);
                Assert.AreEqual(expectedPassword, vm.Password.Value);
                Assert.AreEqual(expectedUserId, vm.UserId.Value);
            }
        }
    }
}
