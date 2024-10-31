using Hospital.Console.Configuration;

var config = Bootstrap.GetConfiguration();
var host = Bootstrap.GetHost(config, args);

Bootstrap.Run(config, host);