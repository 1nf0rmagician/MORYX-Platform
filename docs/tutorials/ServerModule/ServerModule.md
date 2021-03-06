---
uid: GettingStarted.ServerModule
---
# ServerModule

The ServerModule is the place where you have access to [level 1 and the level 2 components](xref:ComponentComposition) of your module.
So this is the place where you have to link the different components and make them work together.

This document describes how to build (the basis for) a new ServerModule from scratch and step by step.

Add a new project to your solution. The name of the project should be the name of your new ServerModule. (In the following examples the name "Execution" is chosen.)

Your new ServerModule-Project gets at least one folder: "ModuleController". The "ModuleController" consists at least of three files. To get detailed information about the architecture of the ModuleController click [here](xref:ComponentComposition#runtime-ServerModuleArchitecture). 

For implementation details click on the file name:

    * [ModuleController.cs](#moduleControllerFile)
    * [ModuleConfig.cs](#moduleConfigFile)
    * [ModuleConsole.cs](#moduleConsoleFile)

## The ModuleController.cs - File <a id="moduleControllerFile" />

The ModuleController.cs-File is the key point of your module. Here all the components of your module came together and you are responsible to initialize, start and stop them in the right way. Because every ServerModule can have its own architecture there is not _the correct way_ to do so, but there are some _usually points_ a ModuleController.cs-File covers. These points are:

1. Import the global components your ServerModule needs
2. Register imported _global_ components to the internal container
3. Resolve the desired components from the container and **start** them
4. Stop the started components when the ServerModule is stopped
5. (Export and Import facades -> this topic is covered in [this guide](xref:FacadeGuide))

Now we will look at examples for these points. But first create your your class with the ServerModuleAttribute.cs and the ServerModuleBase.cs.
For the following properties and attributes reference these files:

* Moryx.dll
* Moryx.Runtime.dll

````cs
[ServerModule(ModuleName)]
public class ModuleController : ServerModuleBase<ModuleConfig>
{
    internal const string ModuleName = "ExampleName";
    /// <summary>
    /// Name of this module
    /// </summary>
    public override string Name
    {
        get { return ModuleName; }
    }
.
.
.   
````

As example for the first point we import the ResourceManagement and the ProductManagement. We do so by simply write them as public properties, the global DI container will do the rest. (The RequiredModuleApi-Attribute is described [here](xref:FacadeGuide))

````cs
[RequiredModuleApi(IsStartDependency = true, IsOptional = false)]
public IResourceManagement ResourceManagement { get; set; }

[RequiredModuleApi(IsStartDependency = true, IsOptional = false)]
public IProductManagement ProductManagement { get; set; }
````

Now we will register the global components to the internal container of our module. We do this in the _OnInitialize_ method we must override form our base class:

````cs
/// <summary>
/// Code executed on start up and after service was stopped and should be started again
/// </summary>
protected override void OnInitialize()
{
    // TODO: Check config if necessary!

    // Register all imported components
    Container.SetInstances(ResourceManagement, ProductManagement);
}
````

After the initialization we have to start the custom plugins of our ServerModule activate facades and in many cases we also have to start WebServices. We do so in the derived _OnStart_ method.

````cs
protected override void OnStart()
{
    // Activate facades
    ActivateFacade(_playGroundExecution);

    // Start wcf services
    var factory = Container.Resolve<IConfiguredHostFactory>();
    var host = factory.CreateHost<IExecutionWeb>(Config.ExecutionWebHostConfig);
    host.Start();
    _hosts.Add(host);
}
````

Even the greatest ServerModule must be stopped from time to time. We must override the _OnStop_ method to clean up our ServerModule and we must put it in a state in which it can be reinitialized. This includes for example to dispose and clear WCF hosts and deactivate facades: 

````cs
/// <summary>
/// Code executed when service is stopped
/// </summary>
protected override void OnStop()
{
    // Stop wcf hosts 
    foreach (var host in _hosts)
        host.Dispose();

    _hosts.Clear();

    // Deactivate facades
    DeactivateFacade(_playGroundExecution);

}
````

## The ModuleConfig.cs - File <a id="moduleConfigFile" />

The _ModuleConfig.cs_-file is the place where you can define the data fields which are needed to configure your ServerModule. During the _build_ process a xml configuration file is automatically created for each _ModuleConfig.cs_, here you can set the configuration values for your ServerModule. Furthermore you can use the maintenance website to edit the values of the different data fields.

For this to work, the following points must be considered:

* Your ModuleConfig class must derive form ConfigBase.cs
* You must add the _DataContract_ attribute to your class
* You must add the _DataMember_ attribute for each of the data fields
* (Beyond this you can use the _DefaultValue_ attribute to add a default value to your data fields)

The following code is an example for a ModuleConfig.cs:

````cs
[DataContract]
public class ModuleConfig : ConfigBase
{
    public ModuleConfig()
    {
        ExecutionWebHostConfig = new HostConfig
        {
            Endpoint = "ExecutionWeb",
            BindingType = ServiceBindingType.WebHttp,
            MetadataEnabled = true,
            HelpEnabled = true
        };
    }
    [DataMember]
    [DefaultValue(42)]
    public int InstanceCount { get; set; }

    [DataMember]
    [DefaultValue("Hello World")]
    public string WcfMessage { get; set; }

    [DataMember]
    public HostConfig ExecutionWebHostConfig { get; set; }

}
````

## The ModuleConsole.cs - File <a id="moduleConsoleFile" />

The module console provides a command line interface to interact with the server module without any custom client. It can be used for initial testing, debugging or 'admin access'-features. It can be used for initial testing, debugging or 'admin access'-features. As a starting point for this feature you can create ModuleConsole.cs file in your _ModuleController_ folder and copy the following code to it:

````cs
[ServerModuleConsole]
internal class ModuleConsole : IServerModuleConsole
{
    public string ExportDescription(DescriptionExportFormat format)
    {
        switch (format)
        {
            case DescriptionExportFormat.Console:
                return ExportConsoleDescription();
            case DescriptionExportFormat.Documentation:
                return ExportHtmlDescription();
        }
        return string.Empty;
    }

    // Export your description for the developer console here
    // This should represent the current state
    private string ExportConsoleDescription()
    {
        var manPage = @"Little module for little test";
        return manPage;
    }

    // Export your description for the supervisor or maintenance
    // This should be a static explanation of the plugin
    private string ExportHtmlDescription()
    {
        var manPage = @"Little module for little test";
        return manPage;
    }

    public void ExecuteCommand(string[] args, Action<string> outputStream)
    {
        if (!args.Any())
            outputStream("Execution console requires arguments");

        switch (args[0])
        {
            // Handle commands
            case "testAdminCommand":
                outputStream("Your method is executed");
                break;
        }
    }
}
````

Most of this code should be self explanatory and the most interesting part happens in the _ExecuteCommand_ method. Here you can add your own _command words_ which will be used to execute your custom testing or admin methods. These methods can be defined right below the _ExecuteCommand_ method and have access to both, the global and the local DI container. The following pictures show this functions for the example code above.

* Type "enter _ServerModule-Name_" in the runtime console you get access to your custom _ServerModule-Console_.
![](images/enterServerModule.png)
* In the _ServerModule-Console_ you can enter your command words to execute the defined functions.
![](images/executeServerModuleCommand.png)
* Type "bye" to exit the _ServerModule-Console_.
![](images/exitServerModuleConsole.png)