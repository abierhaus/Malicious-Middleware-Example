# Malicious Middleware Example
This example shows how easy it is to develop a middleware that can redirect all requests and responses of an application to an external server.  

Please read following [article](https://alexbierhaus.medium.com/malicious-middleware-in-net-example-8c4f524f4580) about possible threads when using external packages.

## Idea
External packages can be found in any software nowadays and it has become virtually impossible to develop software without external open source components. Thus the risk for attackers compromising this kind of software become a trend within the last couple of years. In this short full working code example, that is based on the article above, we show how a useful middleware component can be used to route all incoming and outgoing traffic to another server.

## Source Overview
The code is build up on 3 simple projects
1. Middleware class that contains the middleware component (based on https://github.com/abierhaus/httprequestresponse-logging-middleware-example) and the malicious piece of software. This could be used for creating a NuGet package
2. ASP.NET Web API project that acts as the client who uses the middleware
3. ASP.NET Web API that acts as the application which receives the traffic and logs it
