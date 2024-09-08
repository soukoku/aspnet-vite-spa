# Soukoku.AspNetCore.ViteIntegration

Lib for integrating with a Vite app in an aspnet core app. 
Dotnet app provides the backend while Vite app provides the frontend.

## Usage

In the typical **Program.cs** file, register it with

```cs
var builder = WebApplication.CreateBuilder(args);

// depends on the actual dev server url
builder.Services.AddViteManifest("https://localhost:3000");

```

Run the typical `npm run dev` in your *vite-app* folder, then debug in aspnet as usual.


## Example

A more complete example is in the repo's `aspnet-vite-spa` project.
