using Hospital.Console.Configuration;

var config = Bootstrap.GetConfiguration();
var host = Bootstrap.GetHost(config, args);
var serviceProvider = Bootstrap.GetServices();

await Bootstrap.Run(config, host, serviceProvider);