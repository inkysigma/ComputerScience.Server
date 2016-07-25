<h1>A Small Side Project</h1>
To get this running:
<ol>
	<li>Install ASP.NET Core</li>
	<li>Create a self signed certificate in ComputerScience.Server.Console</li>
	<li>Import necessary schema files into Postgres Server such as compsci.backup</li>
	<li>Create necessary configuration files according to ComputerScience.Server.Console/template.json.</li>
	<li>Go to web/ComputerScience.Server.Web.Business and run "dotnet ef --startup-project migrations add init".</li>
	<li>Run "dotnet ef --startup-project database update"</li>
	<li>Go to ComputerScience.Server.Console and run "dotnet run"</li>
</ol>