# Rainbow.Tfs

A Team Foundation Server plug-in for the [Rainbow](https://github.com/kamsar/Rainbow) serialization provider used by [Unicorn](https://github.com/kamsar/Unicorn). 

This library aims to solve file system access denied errors when using Unicorn under TFS source control for versions 2010 and older (or Server Workspaces in 2012 and later). Solutions using such versions of TFS, requiring TFS checkout actions prior to editing files on the file system, are candidates for inclusion of this library.

## Configuration Notes
### Local TFS Cache

TFS uses local cache to maintain the local workspace and keep track of files and related changesets. In order to access the appropriate set of TFS cache, IIS must be configured to allow for ASP.NET Impersonation such that the application pool assumes the identity of the developer and not the local system account.

Note: configuring the application pool for ASP.NET Impersonation should only be set on developer workstations. Do not make this change to environments outside of development. 

### 32-Bit Dependencies

The library dependencies for access via the TFS API are built for 32-bit support. Application pools must support 32-bit applications to properly communicate with TFS.

## Configuration Steps
### Installation
[Install NuGet package](https://www.nuget.org/packages/Rainbow.Tfs)

### IIS

1. In IIS, select your application pool and click on _Advanced Settings_
2. Change _Enable 32-Bit Applications_ from _false_ to _true_
![step 2](https://raw.github.com/PetersonDave/Rainbow.Tfs/master/Documentation/32bit.png)
3. In IIS, select your site
4. Under the _IIS_ section, click on _Authentication_
![step 4](https://raw.github.com/PetersonDave/Rainbow.Tfs/master/Documentation/Authentication.png)
5. Right-click on ASP.NET Impersonation and choose _Enable_
6. Right-Click on ASP.NET Impersonation and choose _Edit_
7. Choose _Specific User_ and enter your user name (domain\username) and password.
![step 7](https://raw.github.com/PetersonDave/Rainbow.Tfs/master/Documentation/Impersonation.png)

### Rainbow

For integration with Rainbow, the default file sync configuration will be replaced with a reference to this library. Included will be your user name, password and domain for access to your TFS server. Update the Rainbow.Tfs configuration patch under _\App_Config\Include\Unicorn.Tfs.config_.

```
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
	<sitecore>
		<unicorn>
			<defaults>
				<targetDataStore type="Rainbow.Tfs.Storage.TfsSerializationFileSystemDataStore, Rainbow.Tfs" physicalRootPath="$(dataFolder)\Unicorn\$(configurationName)" useDataCache="false" singleInstance="true" patch:instead="targetDataStore[@type='Rainbow.Storage.SerializationFileSystemDataStore, Rainbow']" />
			</defaults>
		</unicorn>

		<!-- add your TFS creds here -->
		<settings>
			<setting name="Rainbow.Tfs.Login" value="" />
			<setting name="Rainbow.Tfs.Password" value="" />
			<setting name="Rainbow.Tfs.Domain" value="" />
		</settings>
	</sitecore>
</configuration>
```
