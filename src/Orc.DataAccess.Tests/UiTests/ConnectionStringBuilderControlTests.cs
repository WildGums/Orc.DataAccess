namespace Orc.DataAccess.Tests;

using System.Linq;
using Controls;
using NUnit.Framework;

[Explicit]
[TestFixture]
public class ConnectionStringBuilderControlTests : ConnectionStringBuilderControlTestsBase
{
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
