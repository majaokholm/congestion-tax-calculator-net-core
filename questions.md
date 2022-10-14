Question:
1. Can I assume that all data needed for the calculation is passed in the same request? Hence I do not need to cache each requests and check if a request has already been sent by the same
vehicle within the hour
2. Is there a reason the vechiele types have been implemented as different classes with a common interface? I have changed the signature of the GetTax() method to use an enum type instead,
as it is very problematic to use an interface in the requestbody object. Because I saw no need for it, I simply changed it instead of creating a mapping between enums and vechieles at the
controller level 
3. Is there anything that should be persisted to a database? 


