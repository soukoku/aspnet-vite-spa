dotnet pack -c Release /p:ContinuousIntegrationBuild=true -o ./ Soukoku.AspNetCore.ViteIntegration/Soukoku.AspNetCore.ViteIntegration.csproj
dotnet pack -c Release /p:ContinuousIntegrationBuild=true -o ./ Soukoku.AspNet.Mvc.ViteIntegration/Soukoku.AspNet.Mvc.ViteIntegration.csproj
pause