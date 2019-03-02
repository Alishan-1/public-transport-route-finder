# public-transport-route-finder

if you want to go from stop A to stop B but you do not know which routes to follow, the public-transport-route-finder will help you decide

## Live Demo
[See it here](http://34.80.23.62)

## Getting Started

[Clone](https://github.com/Alishan-1/public-transport-route-finder.git) the project and open the `BusApp.sln` in visual studio. attach the database files found in `App_Data` folder in SQL Server. Then update the `DefaultConnection` and `PublicTransportRouteFinderEntities` connection strings in `Web.config`.  

run the following command in Package Manager Console: [Source and reason](https://stackoverflow.com/questions/32780315/could-not-find-a-part-of-the-path-bin-roslyn-csc-exe)

```
Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```
 


then press F5 and you should see a running app

### Prerequisites

* SQL Server 
* Visual Studio 2015 or above



## Built With

* [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc) - A design pattern for achieving a clean separation of concerns
* [Entity Framework](https://docs.microsoft.com/en-us/ef/) - for data storage and retrival
* [jQuery](https://jquery.com/) - for ajax and dynamic behaviours
* [Google Maps Api](https://developers.google.com/maps/documentation/javascript/tutorial) for displaying routes

## Contributing

Please read [CONTRIBUTING.md]() for details on our code of conduct, and the process for submitting pull requests to us.


## Authors

* **Ali Shan** - *Initial work* - [Alishan-1](https://github.com/Alishan-1)
* **Khurram Shahzad** - *team member* -

See also the list of [contributors](https://github.com/our/project/contributors/will-be-added-here) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
