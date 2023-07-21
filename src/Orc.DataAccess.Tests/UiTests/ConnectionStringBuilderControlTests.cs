namespace Orc.DataAccess.Tests;

using Controls;
using NUnit.Framework;
using Orc.Automation;
using Orc.DataAccess.Automation.Controls;

[Explicit]
[TestFixture]
public class ConnectionStringBuilderControlTests : ConnectionStringBuilderControlTestsBase
{
    [Test]
    public void DoubleClick_On_ProviderPicker_Must_Not_Freeze_Application()
    {
        //Prepare
        var control = Target;

        using (StartProvider("PR_1"))
        using (StartProvider("PR_2"))
        {
            var edit = control.OpenEditWindow();

            //NOTE:Vladimir: If control doesn't match this MAP anymore - remove test
            var editMap = edit.Map<ConnectionStringEditWindowMap>();

            var providerPicker = editMap.ProviderPicker;
            var providersListWindow = providerPicker.ShowProviderListWindow();

            //NOTE:Vladimir: If control doesn't match this MAP anymore - remove test
            var providersListWindowMap = providersListWindow.Map<DbConnectionProviderListWindowMap>();

            var providersList = providersListWindowMap.ProvidersList;
            var item = providersList.Items[1];

            item.MouseClick();
            item.MouseClick();

            Wait.UntilInputProcessed(500);

            //Window should be closed
            Assert.That(providersListWindow.IsVisible(), Is.False);

            var provider = edit.SelectedProvider;
            Assert.That(provider, Is.EqualTo("PR_2"));

            edit.Close();
        }
    }

    [Test]
    public void Provider_Should_Be_Selected_Automatically_In_EditConnectionStringWindow_If_Its_Only_One_Matching_Provider_In_A_List()
    {
        //Prepare
        var control = Target;

        using (StartProvider("SingleProvider"))
        {
            var edit = control.OpenEditWindow();
            Assert.That(edit.SelectedProvider, Is.EqualTo("SingleProvider"));
            edit.Close();
            
            control.Current.DatabaseProvider = "no provider";

            using (StartProvider("OtherProvider"))
            {
                edit = control.OpenEditWindow();
                Assert.That(edit.SelectedProvider, Is.EqualTo(string.Empty));
                edit.Close();
            }
        }
    }

    [Test]
    public void AdvancedProperties_Filter_ShouldWorkCorrectly()
    {
        var control = Target;

        using (StartProvider(nameof(AdvancedProperties_Filter_ShouldWorkCorrectly)))
        {
            var connectionStringEditWindow = control.OpenEditWindow();
            var advPropertiesWindow = connectionStringEditWindow.OpenAdvancedProperties();

            //Check initial state
            var initialProperties = advPropertiesWindow.Properties;
            Assert.That(initialProperties.Count, Is.GreaterThan(3));

            //Set filter
            advPropertiesWindow.SetFilter("Adv");
            var propertiesAfterFilter = advPropertiesWindow.Properties;
            CollectionAssert.AreEquivalent(new[]
                {
                    "ADVPROPERTY_1",
                    "ADVPROPERTY_2",
                    "ADVPROPERTY_3"
                },
                propertiesAfterFilter);

            //Return to initial state
            advPropertiesWindow.SetFilter(string.Empty);
            var returnToInitialStateProperties = advPropertiesWindow.Properties;
            CollectionAssert.AreEquivalent(initialProperties, returnToInitialStateProperties);

            advPropertiesWindow.Close();
            connectionStringEditWindow.Close();
        }
    }

    [Test]
    public void Default_Properties_Should_Be_Set_In_Editor()
    {
        var control = Target;
        var controlModel = control.Current;

        using (StartProvider(nameof(Default_Properties_Should_Be_Set_In_Editor)))
        {
            controlModel.DefaultProperties = new DbConnectionPropertyDefinitionCollection
            {
                new ()
                {
                    Name = "ADVPROPERTY_1",
                    Value = "ADVPROPERTY_1_DEFAULT_VALUE_FROM_TEST"
                },
                new ()
                {
                    Name = "ADVPROPERTY_3",
                    Value = "ADVPROPERTY_3_DEFAULT_VALUE_FROM_TEST"
                }
            };

            var connectionStringEditWindow = control.OpenEditWindow();
            var advPropertiesWindow = connectionStringEditWindow.OpenAdvancedProperties();

            foreach (var property in controlModel.DefaultProperties)
            {
                var value = advPropertiesWindow.GetValue(property.Name);

                Assert.That(value, Is.EqualTo(advPropertiesWindow.GetValue(property.Name)));
            }

            advPropertiesWindow.Close();
            connectionStringEditWindow.Close();
        }
    }
}
