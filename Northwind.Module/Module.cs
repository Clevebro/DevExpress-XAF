using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;

namespace Northwind.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class NorthwindModule : ModuleBase {
    public NorthwindModule() {
		// 
		// NorthwindModule
		// 
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.ChartModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }
    public override void Setup(XafApplication application) {
        base.Setup(application);
        // Manage various aspects of the application UI and behavior at the module level.
    }
    public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
    }

    public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
    {
        base.ExtendModelInterfaces(extenders);
        extenders.Add<IModelApplication, IModelMyModelExtension>();
    }

    public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters)
    {
        base.AddGeneratorUpdaters(updaters);
        updaters.Add(new MyChildNodesUpdater1());
    }

}

[KeyProperty(nameof(Name))]
public interface IModelMyChildNode : IModelNode
{
    string Name { get; set; }
    [Localizable(true)]
    string MyStringProperty { get; set; }
    int MyIntegerProperty { get; set; }
}
public interface IModelMyNodeWithChildNode : IModelNode
{
    IModelMyChildNode MyChildNode { get; }
}

[ModelNodesGenerator(typeof(MyChildNodeGenerator))]
public interface IModelMyNodeWithChildNodes : IModelNode, IModelList<IModelMyChildNode>
{
    [DataSourceProperty("SourceListProperty")]
    String ListProperty { get; set; }

    [Browsable(false)]
    IEnumerable<String> SourceListProperty
    {
        get;
    }
}

[DomainLogic(typeof(IModelMyNodeWithChildNodes))]
public static class ModelMyNodeWithChildNodesLogic
{
    public static IEnumerable<string> Get_SourceListProperty(IModelMyNodeWithChildNodes myNodeWithChildNodes)
    {
        return new List<string>() { "Value1", "value2", "Value3" };
    }
}

public interface IModelMyModelExtension : IModelNode
{
    IModelMyNodeWithChildNode MyNodeWithOneChildNode { get; }
    IModelMyNodeWithChildNodes MyNodeWithSeveralChildNodes { get; }
}

public class MyChildNodeGenerator : ModelNodesGeneratorBase
{
    protected override void GenerateNodesCore(ModelNode node)
    {
        for (int i = 0; i < 10; i++)
        {
            string childNodeName = "MyChildNode " + i.ToString();
            node.AddNode<IModelMyChildNode>(childNodeName);
            node.GetNode(childNodeName).Index = i;
        }
    }
}

public class MyChildNodesUpdater1 : ModelNodesGeneratorUpdater<MyChildNodeGenerator>
{
    public override void UpdateNode(ModelNode node)
    {
        foreach (IModelMyChildNode childNode in ((IModelMyNodeWithChildNodes)node))
        {
            if (childNode.Index.HasValue)
            {
                childNode.MyIntegerProperty = (int)childNode.Index + 1;
            }
        }
    }
}