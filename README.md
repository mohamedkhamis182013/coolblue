# coolblue
Please Make sure to have productdata.api.dll up and running in http://localhost:5002/ before run the project
1-BugFix:
   i added a test case when productType can be Insured and
   one of the types that should be insured by 500 withit was failing then i seperated the condtion
   out side the insurance value conditions, and it worked fine. 

2-Refactoring:
   for the refactor part i applied implemented clean archticture and added 
	-Behaviors(LoggingBehaviour, UnhandledExceptionBehaviour, ValidationBehaviour).
	-exceptions handling(ForbiddenAccessException, NotFoundException, ValidationException).
	-I seperated the http calles to be in a service(IHttpRequestsService) 
		and IProductService for calculations.
	-I added chain of responsability for refactoring the if statments .
	-I added MediatR and CQRS for getting the product Insurance. 
	-for this part first i restructured the project and added the services
		then i added the test.
	-I changed the Xunit to MsTest as it is more readable and easier. 
	-I added swagger as well to can test the endpoints in an easy way 
		it will open by default when you run the project.   
3-Feature1:
	-I added EFW inmemory DB for that as a layer of caching for the products 
		and produt types it it  to not make an httpcall for each product
	    and to save the order details in it.
	-OrderVm that will return contains productsInsurance
4-Feature2:
		- i Edited the OrderVm to return "additionalInsurance" property 
			and a calculated property "totalInsurance"
Notes: 
	-for all features i used the inmemory Db instead of HttpCalls
		for performance purpose.
	-behaviors  and Exceptions and dbcontext are not coverd in unittest because of limitation of time.
	-time wasn't enough for me to implement feature3.
	-i intented to add Repositories to be as a layer of abstrction above the DbContext. but didn't have enaugh time to add it. 
