# NFL depth chart
dotnet minimal api with swagger

Build solution, can access endpoints at http://localhost:5010/swagger/index.html  -> use the actual port used during build.  
Note: 2 extra test endpoints (not in spec) to help setup data, MyInit to add some test data and MyClearAll to clear all data.




### Clean Architecture

```
src/    domain  
        application  
        infrastructure  
        api  

tests/  UnitTests  
        IntegrationTests
```
