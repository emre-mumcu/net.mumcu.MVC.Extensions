# Create Project

```powershell
# .editorconfig eklemek için EXPLORER paneline sağ tık ve Generate .editorconfig seçin.

# Projenin oluşturulması
dotnet new sln
dotnet new classlib
dotnet sln add .
dotnet new gitignore
```

# 

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <!-- ASP.NET Core Framework Referansı -->
    <ItemGroup>
        <FrameworkReference Include="Microsoft.AspNetCore.App" />
    </ItemGroup>

</Project>
```

# Git

```powershell
# Git Repository
git init
git add .
git commit -m "First Commit"
git branch -M main
git remote add origin https://github.com/repo-name.git
git push -u origin main
```

# NuGet

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <ItemGroup>
        <FrameworkReference Include="Microsoft.AspNetCore.App" />
    </ItemGroup>

    <PropertyGroup>
        <!-- <PackageType>Template</PackageType> -->
        <PackageId>net.mumcu.MVC.Extensions</PackageId>
        <PackageVersion>1.0.0</PackageVersion>
        <TargetFramework>net10.0</TargetFramework>
        <IsPackable>true</IsPackable>
        <Title>net.mumcu.MVC.Extensions</Title>
        <Authors>Emre Mumcu</Authors>
        <Company>mumcu.net</Company>
        <Description>net.mumcu.MVC.Extensions</Description>
        <RepositoryUrl>https://github.com/emre-mumcu/net.mumcu.MVC.Extensions.git</RepositoryUrl>
        <PackageLicenseExpression>MIT</PackageLicenseExpression>
        <PackageTags>mumcu.net; aspnetcore; mvc; extensions;</PackageTags>
        <Copyright>Copyright © 2026</Copyright>
        <PackageProjectUrl>https://emre-mumcu.github.io</PackageProjectUrl>
        <PackageReadmeFile>readme.md</PackageReadmeFile>
        <PackageIcon>net.mumcu.png</PackageIcon>
        <IncludeContentInPack>true</IncludeContentInPack>
        <IncludeBuildOutput>false</IncludeBuildOutput>
        <EnableDefaultContentItems>false</EnableDefaultContentItems>
        <NoDefaultExcludes>true</NoDefaultExcludes>
    </PropertyGroup>

    <ItemGroup>
        <Content Include="**\**" Exclude="**\bin\**;**\obj\**;**\.vs\**;**\.git\**" />
        <Content Remove=".gitignore; .gitattributes; editorconfig;" />
        <None Include="net.mumcu.png" Pack="true" PackagePath="\" />
        <None Include="readme.md" Pack="true" PackagePath="\" />
    </ItemGroup>

</Project>
```

```powershell
dotnet pack -c Release
```