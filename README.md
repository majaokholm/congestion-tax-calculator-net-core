## Summary of changes made

I added a controller as a HTTP POST entrypoint for the application and configured swagger
I upgraded to .NET 6
I added a program.cs file and configures the app with dependency indjection 
I added a NUnit test project to test the CongestionTaxCalculator class. I am using Moq to mock the Paramaterservice class. The controller endpoint has been tested manually using Swagger and Postman, however I have not spent time creating tests for the controller (and have purposlly kept the controller as slim as possible, with all the calculations and logic moved into the service classes)
I several made changes to the CongestionTaxCalculator method:
- It can calculate tax for multiple dates
- I changed the way the single charge rule is implemented (there was a bug), so the datetime array recieved is first reformatted into 60 minutes chunkes and then each tax rate is calculated and the max is choocen
- I changed the vechile type to use an enum and not an interface as this does not work for the controller and I did not wish to add a mapping layer (see question file)
I added some structure to the project using folders: BO for business objects, Controlers for controllers, Models for model classes (please refere to the questions.md for my questions of how to further develop this), Paramaters for config files (the intention is to structure this per city), Services for classes with more (business) logic
I started to create a ParamaterService class with methods to desceralize the json config files for each city to allow the CongestionTaxCalculator to use dynamic rates, howeevre I did not finish this


## Things I didn't finish
1. The ParamaterService class is not fully working and I did not manage to add paramaters for the tax free dates and vechile types either
2. I extended the calculator to be able to handle request with multiple dates, however I did not have enough time to add several endpoints, but would have liked to have had a seperate endpoint for
timestamps for one day and one for several
3. The unit tests are not complete